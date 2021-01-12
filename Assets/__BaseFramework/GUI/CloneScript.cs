using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CloneScript : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField]
    public MonoScript script;
#endif

    [SerializeField]
    public string newName;

    [SerializeField]
    public string newPath = "Scripts/GUI";
}   