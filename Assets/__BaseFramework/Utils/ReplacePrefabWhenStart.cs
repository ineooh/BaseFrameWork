using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplacePrefabWhenStart : MonoBehaviour
{
    public GameObject prefab;
    public bool enable = true;
    void Start()
    {
        if (enable && prefab!=null)
        {
            Instantiate(prefab, transform.position, transform.rotation, transform.parent);
            Destroy(gameObject);
        }
    }
  
}
