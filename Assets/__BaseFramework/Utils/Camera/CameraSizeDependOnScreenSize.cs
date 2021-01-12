using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSizeDependOnScreenSize : MonoBehaviour
{
	public Camera cam;
	public float minSize;

	private void Awake()
	{
		Refesh();
	}


	void Refesh()
	{
		float cameraSize1 = 41;
		float w1 = 900f;
		float h1 = 1600f;

		float w2 = Screen.width;
		float h2 = Screen.height;

		float ratio1 = w1 / h1;
		float ratio2 = w2 / h2;

		float cameraSize2 = cameraSize1 * ratio1 / ratio2;
		if (cameraSize2 < minSize)
			cameraSize2 = minSize;
		cam.orthographicSize = cameraSize2;
	}

#if UNITY_EDITOR
	private void Update()
	{
		Refesh();
	}
#endif
}
