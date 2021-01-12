using UnityEngine;


public class GUIManager : GUIManagerBase
{

    //// Use this for initialization
    void Start()
    {
#if UNITY_EDITOR

        //UnLoadAll();
#endif
    }
    public bool loadAll = false;
    public bool unloadAll = false;
#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
	{
        if(loadAll)
		{
            loadAll = false;
            GUIHandlerBase[] collection = GetComponentsInChildren<GUIHandlerBase>();
			foreach (var item in collection)
			{
                item.Load();
			}

        }
        if (unloadAll)
        {
            unloadAll = false;
            UnLoadAll();

        }

    }
    void UnLoadAll()
    {
        GUIHandlerBase[] collection = GetComponentsInChildren<GUIHandlerBase>();
        foreach (var item in collection)
        {
            item.UnLoad();
        }
    }
#endif


}
