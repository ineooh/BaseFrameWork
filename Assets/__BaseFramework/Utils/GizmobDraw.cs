using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GizmobDraw : MonoBehaviour {
    public bool autolink = false;
	public bool EnableDraw = true;

    public Color color;
    public Color colorDir;
    public bool Mode_Connect = false;

    public GizmobCell[] pathArray;
    void OnDrawGizmos()
    {
		if(autolink)
		{
            autolink = false;
            GizmobCell[] arr = transform.GetComponentsInChildren<GizmobCell>();
			for (int i = 0; i < arr.Length-1; i++)
			{
                arr[i].connectList = new List<GizmobCell>();
                arr[i].connectList.Add(arr[i + 1]);

            }
            pathArray = arr; 
        }

		if (!EnableDraw) return;

		GizmobCell[] list = transform.GetComponentsInChildren<GizmobCell>();
		//for (int i = 0; i < list.Length; i++)// Transform i in list)
		//{
		//	//list[i].name = list[i].name.Replace("waypoint" , "wp");
		//	//list[i].name = list[i].name.Replace('(' ,'');
		//	//list[i].name = list[i].name.Replace(')' ,'');
		//}
		if (Mode_Connect)
        {
           
            for (int i = 0; i < list.Length; i++)// Transform i in list)
            {
                Vector3 pos1 = list[i].transform.position;
#if UNITY_EDITOR
				Handles.color = Color.yellow;
				Handles.Label(pos1, list[i].name);
#endif
                foreach (GizmobCell to in list[i].connectList )
                {
                    if (to != null)
                    {
                        Vector3 pos2 = to.transform.position;
                        Gizmos.color = color;
                        Gizmos.DrawLine(pos1, pos2);
                        Gizmos.color = colorDir;
                        Gizmos.DrawLine(pos1, pos1+(pos2-pos1).normalized*3 );

                    }
                }
            }
        }
        else
        {
			Gizmos.color = color;
			for (int i = 0; i < list.Length - 1; i++)// Transform i in list)
            {
                Vector3 pos1 = list[i].transform.position;
                Vector3 pos2 = list[i + 1].transform.position;
				Gizmos.color = color;
				Gizmos.DrawLine(pos1, pos2);
#if UNITY_EDITOR
				Handles.color = Color.yellow;
				Handles.Label(pos1, list[i].name);
                Handles.Label(pos2, list[i + 1].name);
#endif
            }
        }
    }

	public void DrawGizmos()
    {

		//if (delayTimeDraw != Time.frameCount)
		//{

		//	delayTimeDraw = Time.frameCount;
		//}
		//else
		//	return;
		OnDrawGizmos();
    }
    public GizmobCell[] GetPath()
    {
        GizmobCell[] list = transform.GetComponentsInChildren<GizmobCell>(true);
        return list;
    }


}
