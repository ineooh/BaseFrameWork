using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GSystem : GSystemBase
{
	public static string userDataKey = "UserData_baseframework";

	public override void Start()
	{
		_userDataKey = userDataKey;
		LoadUserData();

		StartCoroutine(IEnumerator_EntryPoint());

	}

	public override IEnumerator IEnumerator_EntryPoint()
	{
		//Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");

		StartCoroutine(base.IEnumerator_EntryPoint());
		//loading
		yield return new WaitUntil(() => isInitGameService);

		//do your logic here
		//Debug.Log("BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB");
	}
}


