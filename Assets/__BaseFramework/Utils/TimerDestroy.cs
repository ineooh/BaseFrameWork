using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerDestroy : MonoBehaviour {

	// Use this for initialization
	public void DestroyIn(float	lifeTime)
	{
		if (lifeTime > 0)
			Invoke("destroy", lifeTime);
		else
			destroy();


	}
	public void destroy()
	{
		Destroy(gameObject);
	}
	
}
