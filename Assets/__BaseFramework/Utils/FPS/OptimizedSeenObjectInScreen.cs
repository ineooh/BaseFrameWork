using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptimizedSeenObjectInScreen : MonoBehaviour
{
    public int NumberFrameEachCheck = 5;
    public float ExtraZone = 0.1f;
    public SpriteRenderer spriteRenderer;
    public GameObject child;

    void Awake()
    {
        Check();
    }
    void Update()
    {
        if (Time.frameCount % NumberFrameEachCheck == 0)
        {

            Check();

        }
    }
    void Check()
    {
        //if (MyUtils.CheckInScreen(transform.position, MapControl.Instance.mainChar.transform.position, ExtraZone))
        //{
        //    if (spriteRenderer != null)
        //    {
        //        spriteRenderer.enabled = true;
        //    }
        //    if (child != null)
        //        child.gameObject.SetActive(true);

        //}
        //else
        //{
        //    if (spriteRenderer != null)
        //        spriteRenderer.enabled = false;
        //    if (child != null)
        //        child.gameObject.SetActive(false);

        //}
    }
    void OnDrawGizmosSelected()
    {

        if (spriteRenderer == null)
            spriteRenderer = transform.GetComponent<SpriteRenderer>();

    }

}
