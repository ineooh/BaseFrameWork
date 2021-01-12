using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFollow : MonoBehaviour {

	public Transform targetFollow;
	void Update()
	{
		if (targetFollow != null)
		{
			transform.position = targetFollow.position;
		}
	}

}
