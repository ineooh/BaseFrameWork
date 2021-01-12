#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class JSMenuItem : MonoBehaviour
{
    [MenuItem("Tools/ChangeScreen/MainMenu #1")]
    static void OpenMainMenu()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Scene_Main.unity");
    }

    [MenuItem("Tools/ChangeScreen/MainGame #2")]
    static void OpenMainGame()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Scene_Play.unity");
    }

    [MenuItem("Tools/ChangeScreen/Demo #3")]
    static void OpenDemo()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Scene_Editor.unity"); 
    }
    [MenuItem("Tools/ChangeScreen/ToolScene #4")]
    static void OpenTool()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Scene_Tool.unity");
    }
}
#endif