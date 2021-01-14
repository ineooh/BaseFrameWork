using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : PoolObject {

    public int color;

    public float tMultiplier;

    public Transform transf;

    public void Throw(Vector3 v0) {
        if (!gameObject.activeInHierarchy)
            Debug.Log(name);
        StartCoroutine(IThrow(v0));
    }

    private IEnumerator IThrow(Vector3 v0) {
        float x, y, z, t, t0;
        Vector3 p0 = transf.position;
        y = p0.y;
        t0 = Time.time;
        while (y > LevelBuilder.Instance.y - 10) {
            t = (Time.time - t0) * tMultiplier;
            x = p0.x + v0.x * t;
            z = p0.z + v0.z * t;
            y = p0.y + (v0.y + v0.y - Player.Instance.g * t) / 2 * t;
            transf.position = new Vector3(x, y, z);
            yield return new WaitForEndOfFrame();
        }
        Release();
        yield break;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Ground") {
            Vector3 p = new Vector3(transf.position.x, LevelBuilder.Instance.y, transf.position.z);
            GameManager.Instance.CreateColorPool(color, p);
            Release();
        }
        else if (other.tag == "Box") {
            other.transform.GetChild(0).GetComponent<MeshRenderer>().material = (color == 1) ? FightManager.Instance.red : FightManager.Instance.green;

            int count = FightManager.Instance.fighters.Count;
            Fighter f;
            Vector3 p = transf.position;
            for (int i = 0; i < count; i++) {
                f = FightManager.Instance.fighters[i];
                if (f.color == 0 && f.walkingOnBoxes) {
                    if (Mathf.Abs(f.transf.position.x - p.x) < LevelBuilder.Instance.boxHeight / 2 && Mathf.Abs(f.transf.position.z - p.z) < LevelBuilder.Instance.boxHeight)
                        f.GetColored(color);
                }
            }

            Release();
        }
    }

}
