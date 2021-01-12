using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
public abstract class GUIManagerBase : SingletonMonoPersistent <GUIManagerBase>
{
    public Camera guiCamera = null;

    public EventSystem eventSystem;

    public List<GUIHandlerBase> listHandler = new List<GUIHandlerBase>();

	//[HideInInspector]
	//public string prefabPath = "GUI";

	[HideInInspector]
	public string sourcePath = "_Scripts/GUI";

	[HideInInspector]
	public string newGuiName = "";

	[HideInInspector]
	public string destPath = "_Scripts";

	[HideInInspector]
	public string destName = "GUIName.cs";

#if UNITY_EDITOR
    //Create GUI with template
    [HideInInspector]
    public bool createWithTemplate = false;

    [HideInInspector]
    public GUIBase template = null;
#endif

    public T GetHandler<T>(int index) where T : GUIHandlerBase
    {
        if (listHandler.Count <= index || index < 0)
            return null;
        if (listHandler[index] is T)
            return (T)listHandler[index];
        return null;
    }

    public T GetGUI<T>(int index) where T : GUIBase
    {
        if (listHandler.Count <= index || index < 0)
            return null;
        if (listHandler[index] == null)
            return null;
        return listHandler[index].GetGUI<T>();
    }

    public void ShowGUI(int index, params object[] @parameter)
    {
        if (listHandler.Count <= index || index < 0)
            return;
        if (listHandler[index] == null)
            return;

        listHandler[index].Show(@parameter);
    }
    public void ShowGUI_NoAnim(int index, params object[] @parameter)
    {
        if (listHandler.Count <= index || index < 0)
            return;
        if (listHandler[index] == null)
            return;

        listHandler[index].Show_NoAnim(@parameter);
    }

    public void HideGUI(int index, params object[] @parameter)
    {
        if (listHandler.Count <= index || index < 0)
            return;
        if (listHandler[index] == null)
            return;

        listHandler[index].Hide(@parameter);
    }

    public bool IsShowed(int index)
    {
        if (listHandler.Count <= index || index < 0)
            return false;
        if (listHandler[index] == null)
            return false;
        return listHandler[index].IsShowed();
    }

    public bool IsGUI(int index, GUIBase gUIBase)
    {
        if (listHandler.Count <= index || index < 0)
            return false;
        if (listHandler[index] == null)
            return false;
        return listHandler[index] == gUIBase.handler;
    }
}
