using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class GizmobCell : MonoBehaviour
{
	
    public List<GizmobCell> connectList;
    public bool lookingForLink;
    public float radiusForLink =  1;
    public bool registerWhenEnable = false;


	public bool isLinkToOther = false;
	public static float RangeLookForLink = 10;
	public bool isGetLinkFromOther = false;

	private void Start()
	{
		Destroy(GetComponent<MeshRenderer>());
		Destroy(GetComponent<MeshFilter>());
	}
	//private void OnEnable()
 //   {
 //       if (registerWhenEnable )
 //           MapControl.Instance.RegisterNode(this);
 //   }
 //   void OnDisable()
 //   {
 //       if (registerWhenEnable)
 //           MapControl.Instance.UnRegisterNode(this);
 //   }
 //   void OnDestroy()
 //   {
 //       if (registerWhenEnable)
 //           MapControl.Instance.UnRegisterNode(this);
 //   }

    void OnDrawGizmosSelected()
    {
        if (lookingForLink)
        {
            connectList.Clear();
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, radiusForLink);
            GizmobCell[] list = transform.parent.GetComponent<GizmobDraw>().GetPath();
            foreach (GizmobCell item in list)
            {
                if( item != this)
                {
                    float distance = Vector3.Distance(transform.position, item.transform.position);
                    if( distance <= radiusForLink)
                    {
                        connectList.Add(item);
                    }
                }
            }
		}

#if UNITY_EDITOR

		//UnityEngine.Object SelectedObject = Selection.activeObject;
		//GizmobCell c = ((GameObject)SelectedObject).GetComponent<GizmobCell>();
		//for (int i = 0; i < connectList.Count; i++)
		//{
		//	if (connectList[i] == null)
		//	{
		//		connectList.RemoveAt(i);
		//		break;
		//	}

		//}
		////if( connectList.Count == 0)
		////	Debug.Log(name);
		//UnityEngine.Object[] SelectedObjects = Selection.objects;
		//if (SelectedObjects.Length == 2)
		//{
		//	foreach (UnityEngine.Object obj in SelectedObjects)
		//	{

		//		GizmobCell gizmobcell = ((GameObject)obj).GetComponent<GizmobCell>();
		//		if (gizmobcell != null && gizmobcell != c && !gizmobcell.connectList.Contains(c))
		//		{

		//			gizmobcell.connectList.Add(c);
		//		}
		//	}
		//}
#endif
	}

}
