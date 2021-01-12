using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasAutoLinkMainCamera : MonoBehaviour
{
    public static Camera mainCamera;
    public Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        if( canvas == null)
		{
            //if (mainCamera == null)
                mainCamera = Camera.main;

            canvas.worldCamera = mainCamera;
		}

    }

}
