﻿using UnityEngine;
using System.Collections.Generic;

namespace PaintIn3D
{
	/// <summary>This base class allows you to easily create components that can have their paint points connected together to form lines.</summary>
	public abstract class P3dConnectablePoints : MonoBehaviour
	{
		class Link
		{
			public object  Owner;
			public Vector3 Position;
			public float   Age;
			public bool    Preview;
			public Vector3    LastPosition;
			public float      LastPressure;
			public int        LastPriority;
			public Quaternion LastRotation;
		}

		/// <summary>The world space distance between each paint point.
		/// 0 = No spacing.</summary>
		public float HitSpacing { set { hitSpacing = value; } get { return hitSpacing; } } [SerializeField] private float hitSpacing;

		/// <summary>When using <b>HitSpacing</b>, this prevents scenarios where something goes wrong and you attempt to paint too many times per frame.</summary>
		public int HitLimit { set { hitLimit = value; } get { return hitLimit; } } [SerializeField] private int hitLimit = 30;

		/// <summary>If you enable this then the hit points generated by this component will be connected into lines, allowing you to paint continuously.</summary>
		public bool ConnectHits { set { connectHits = value; } get { return connectHits; } } [SerializeField] protected bool connectHits;

		[System.NonSerialized]
		private List<Link> links = new List<Link>();

		[System.NonSerialized]
		private static Stack<Link> linkPool = new Stack<Link>();

		[System.NonSerialized]
		protected P3dHitCache hitCache = new P3dHitCache();

		public P3dHitCache HitCache
		{
			get
			{
				return hitCache;
			}
		}

		/// <summary>This component sends hit events to a cached list of components that can receive them. If this list changes then you must manually call this method.</summary>
		[ContextMenu("Clear Hit Cache")]
		public void ClearHitCache()
		{
			hitCache.Clear();
		}

		/// <summary>If this GameObject has teleported and you have <b>ConnectHits</b> or <b>HitSpacing</b> enabled, then you can call this to prevent a line being drawn between the previous and current points.</summary>
		[ContextMenu("Reset Connections")]
		public void ResetConnections()
		{
			for (var i = links.Count - 1; i >= 0; i--)
			{
				linkPool.Push(links[i]);
			}

			links.Clear();
		}

		protected void BreakHits(object owner)
		{
			for (var i = links.Count - 1; i >= 0; i--)
			{
				var link = links[i];

				if (link.Owner == owner)
				{
					links.RemoveAt(i);

					linkPool.Push(link);

					return;
				}
			}
		}

		protected void SubmitLastPoint(bool preview, object owner)
		{
			if (owner != null)
			{
				var link = default(Link);

				if (TryGetLink(owner, ref link) == true)
				{
					if (link.Preview == preview && preview == false)
					{
						if (hitSpacing > 0.0f)
						{
							var currentPosition = link.Position;
							var distance        = Vector3.Distance(link.Position, link.LastPosition);
							var steps           = Mathf.FloorToInt(distance / hitSpacing);

							if (steps > hitLimit)
							{
								steps = hitLimit;
							}

							for (var i = 0; i < steps; i++)
							{
								currentPosition = Vector3.MoveTowards(currentPosition, link.LastPosition, hitSpacing);

								if (connectHits == true)
								{
									hitCache.InvokeLine(gameObject, preview, link.LastPriority, link.LastPressure, link.Position, currentPosition, link.LastRotation);
								}
								else
								{
									hitCache.InvokePoint(gameObject, preview, link.LastPriority, link.LastPressure, currentPosition, link.LastRotation);
								}

								link.Position = currentPosition;
							}
						}
					}
				}
			}
		}

		protected void SubmitPoint(bool preview, int priority, float pressure, Vector3 position, Quaternion rotation, object owner)
		{
			if (owner != null)
			{
				var setPositionAndPreview = true;
				var link                  = default(Link);

				if (TryGetLink(owner, ref link) == true)
				{
					if (link.Preview == preview && preview == false)
					{
						if (hitSpacing > 0.0f)
						{
							var currentPosition = link.Position;
							var distance        = Vector3.Distance(link.Position, position);
							var steps           = Mathf.FloorToInt(distance / hitSpacing);

							if (steps > hitLimit)
							{
								steps = hitLimit;
							}

							for (var i = 0; i < steps; i++)
							{
								currentPosition = Vector3.MoveTowards(currentPosition, position, hitSpacing);

								if (connectHits == true)
								{
									hitCache.InvokeLine(gameObject, preview, priority, pressure, link.Position, currentPosition, rotation);
								}
								else
								{
									hitCache.InvokePoint(gameObject, preview, priority, pressure, currentPosition, rotation);
								}

								link.Position = currentPosition;
							}

							setPositionAndPreview = false;
						}
						else if (connectHits == true)
						{
							hitCache.InvokeLine(gameObject, preview, priority, pressure, link.Position, position, rotation);
						}
						else
						{
							hitCache.InvokePoint(gameObject, preview, priority, pressure, position, rotation);
						}
					}
					else
					{
						hitCache.InvokePoint(gameObject, preview, priority, pressure, position, rotation);
					}
				}
				else
				{
					link = linkPool.Count > 0 ? linkPool.Pop() : new Link();

					link.Owner = owner;

					links.Add(link);

					hitCache.InvokePoint(gameObject, preview, priority, pressure, position, rotation);
				}

				if (setPositionAndPreview == true)
				{
					link.Position = position;
					link.Preview  = preview;
				}

				link.LastPosition = position;
				link.LastPressure = pressure;
				link.LastPriority = priority;
				link.LastRotation = rotation;
			}
			else
			{
				hitCache.InvokePoint(gameObject, preview, priority, pressure, position, rotation);
			}
		}

		protected virtual void OnEnable()
		{
			ResetConnections();
		}

		protected virtual void Update()
		{
			for (var i = links.Count - 1; i >= 0; i--)
			{
				var link = links[i];

				link.Age += Time.deltaTime;

				if (link.Age > 1.0f)
				{
					link.Age = 0.0f;

					links.RemoveAt(i);

					linkPool.Push(link);
				}
			}
		}

		private bool TryGetLink(object owner, ref Link link)
		{
			for (var i = links.Count - 1; i >= 0; i--)
			{
				link = links[i];

				link.Age = 0.0f;

				if (link.Owner == owner)
				{
					return true;
				}
			}

			return false;
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;

	public class P3dConnectablePoints_Editor<T> : P3dEditor<T>
		where T : P3dConnectablePoints
	{
		protected override void OnInspector()
		{
			Draw("hitSpacing", "The world space distance between each hit point.\n\n0 = No spacing.");
			Draw("hitLimit", "When using HitSpacing, this prevents scenarios where something goes wrong and you attempt to paint too many times per frame.");
			Draw("connectHits", "If you enable this then the hit points generated by this component will be connected into lines, allowing you to paint continuously.");
		}
	}
}
#endif