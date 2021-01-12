using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepRotation : MonoBehaviour
{
	// Use this for initialization
	Quaternion beginRotation;

	public bool isUseConst = false;
	public Quaternion constRotation;

	private void OnDrawGizmosSelected()
	{
		constRotation = transform.rotation;
	}
	void Awake () 
	{
		beginRotation = transform.rotation ;
	}
	
	// Update is called once per frame
	void LateUpdate ()
    {
		if( !isUseConst)
			transform.rotation = beginRotation;
		else
			transform.rotation = constRotation;

	}
}
