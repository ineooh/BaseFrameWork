using UnityEngine;
using System.Collections;

public class SingletonMonoAutoCreate<T>: MonoBehaviour where T : Component
{
	private static T _instance;

	public static T Instance {
		get {
			if (_instance == null) {
				Debug.Log ("Don't have " + typeof(T).Name + " on Scene");
				var objs = FindObjectsOfType (typeof(T)) as T[];
				if (objs.Length > 0)
					_instance = objs [0];
				if (objs.Length > 1) {
					Debug.LogError ("There is more than one " + typeof(T).Name + " in the scene.");
				}
				if (_instance == null) {
					GameObject obj = new GameObject ();
					obj.name = typeof(T).Name;
//					obj.hideFlags = HideFlags.HideAndDontSave;
					_instance = obj.AddComponent<T> ();
				}
			}
			return _instance;
		}
	}

	/// <summary>
	/// When Unity quits, it destroys objects in a random order.
	/// In principle, a Singleton is only destroyed when application quits.
	/// If any script calls Instance after it have been destroyed, 
	///   it will create a buggy ghost object that will stay on the Editor scene
	///   even after stopping playing the Application. Really bad!
	/// So, this was made to be sure we're not creating that buggy ghost object.
	/// </summary>
	protected virtual void OnDestroy ()
	{
		_instance = null;
	}

	protected virtual void OnApplicationQuit ()
	{
		_instance = null;
	}
}

public class Singleton<T> where T : new()
{
	static T _instance = default(T);

	public static T Instance {
		get {
			if (_instance == null)
				_instance = new T ();
			return _instance;
		}
	}
}

public class SingletonMonoPersistent<T> : MonoBehaviour
    where T : Component
{
	public static T Instance { get; private set; }

	public virtual void Awake ()
	{
		if (Instance == null) {
			Instance = this as T;
			DontDestroyOnLoad (this);
		} else {
			Destroy (gameObject);
		}
	}
}

public class SingletonPersistentAutoCreate<T> : MonoBehaviour
	where T : Component
{
	private static T _instance;

	public static T Instance {
		get {
			if (_instance == null) {
				Debug.Log ("Don't have " + typeof(T).Name + " on Scene");
				var objs = FindObjectsOfType (typeof(T)) as T[];
				if (objs.Length > 0)
					_instance = objs [0];
				if (objs.Length > 1) {
					Debug.LogError ("There is more than one " + typeof(T).Name + " in the scene.");
				}
				if (_instance == null) {
					GameObject obj = new GameObject ();
					obj.name = typeof(T).Name;
//					obj.hideFlags = HideFlags.HideAndDontSave;
					_instance = obj.AddComponent<T> ();
				}
				DontDestroyOnLoad (_instance.gameObject);
			}
			return _instance;
		}
	}

	public virtual void Awake ()
	{
		if (_instance != null) {
			Destroy (gameObject);
		}
	}
}

public class SingletonMono<T> : MonoBehaviour where T : Component
{
	public static T Instance { get; private set; }

	public virtual void Awake ()
	{
		if (Instance == null) {
			Instance = this as T;
		} else {
			Destroy (gameObject);
		}
	}
}
//public class SingletonMonoObjectGame<T> : MonoObjectGame where T : Component
//{
//	public static T Instance { get; private set; }

//	public virtual void Awake()
//	{
//		if (Instance == null)
//		{
//			Instance = this as T;
//		}
//		else
//		{
//			Destroy(gameObject);
//		}
//	}
//}
