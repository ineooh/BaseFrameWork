using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaticleSystem_AutoOff : MonoObjectGame {

	public ParticleSystem particle;


	private void Update()
	{
		if (particle.isStopped)
		{

			gameObject.SetActive(false);
		}
	}

}
