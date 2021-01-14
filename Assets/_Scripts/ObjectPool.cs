using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : SingletonMono<ObjectPool> {

    [SerializeField]
    private GameObject[] objectPrefabs;

    [SerializeField]
    private List<GameObject> pool;

    private void Start() {
        pool = new List<GameObject>();
    }

    public GameObject GetObject(string type, Transform transf = null) {
        foreach (GameObject obj in pool) {
            if (obj.name == type & !obj.activeInHierarchy) {
                obj.SetActive(true);
                obj.transform.SetParent(transf);
                return obj;
            }
        }
        foreach (GameObject obj in objectPrefabs) {
            if (obj.name == type) {
                GameObject newObject = Instantiate(obj, transf);
                newObject.name = type;
                pool.Add(newObject);
                return newObject;
            }
        }
        Debug.Log("Cannot find pool object");
        return null;
    }

    public void ReleaseObject(GameObject obj) {
        obj.GetComponent<PoolObject>().Release();
    }

    public void ReleaseObject(GameObject obj, float t) {
        StartCoroutine(IReleaseObject(obj, t));
    }

    private IEnumerator IReleaseObject(GameObject obj, float t) {
        yield return new WaitForSeconds(t);
        ReleaseObject(obj);
    }

    public void StartLevel() {
        foreach (GameObject obj in pool) {
            ReleaseObject(obj);
        }
    }

}
