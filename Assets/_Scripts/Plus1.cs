using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plus1 : PoolObject {

    [SerializeField]
    private Vector2 offset;
    [SerializeField]
    private float time;

    private Transform transf;

    public void MoveUp(Vector2 start) {
        transf = transform;
        StartCoroutine(IMoveUp(start));
    }
    private IEnumerator IMoveUp(Vector2 start) {
        float elapsedTime = 0;
        while (elapsedTime < time) {
            transf.position = start + offset * elapsedTime / time;
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        this.Release();
        yield break;
    }

}
