using UnityEngine;
using UnityEditor;
public class EditorUtils  {

    [MenuItem("EditorUtils/Clear Cache")]
    public static void getHash()
    {
        Caching.ClearCache();
    }

    [MenuItem("EditorUtils/Clear Data")]
    public static void clearData()
    {
        PlayerPrefs.DeleteAll();
    }

    

}
