
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DebugGUI : SingletonMono<DebugGUI>
{
    public string log = "";
    void OnGUI()
    {
#if UNITY_EDITOR
		GUI.color = Color.black;
        if (log != "")
            GUI.Label(new Rect(10, Screen.height - 50, Screen.width-20, 50), Instance.log);
#endif
    }
    public static void Log(string value)
    {
        Instance. log = value;
        //Debug.Log(Instance.log);
    }

}
