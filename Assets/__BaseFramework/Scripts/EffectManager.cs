using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : EffectManagerBase
{
	//static public void CreateEffectForPush(Vector3 wPos, Vector3 mainPos, Quaternion rotation)
	//{
	//    CreateWithRotation("exp", wPos, mainPos, rotation);
	//}
	//static public void CreateEffecPush(Vector3 wPos, Vector3 mainPos)
	//{
	//    Create("carhit", wPos, mainPos);
	//}
	//static public void CreateEffecColl(Vector3 wPos, Vector3 mainPos)
	//{
	//    Create("unlockParticle", wPos, mainPos);
	//}
	static public void CreateEffec_MathchingStar(Vector3 wPos)
	{
		EffectManagerBase.Create("matchingStar", wPos );
	}
	static public void CreateEffec_MathchingAffer(Vector3 wPos)
	{
		EffectManagerBase.Create("matchingAfter", wPos);
	}
	static public void CreateEffec_Missile(Vector3 wPos, float delay = 0)
	{
		if (delay == 0)
			EffectManagerBase.Create("Missile", wPos);
		else
			Instance.StartCoroutine(IE_CreateEffec_Missile(wPos, delay));
	}
	public static IEnumerator IE_CreateEffec_Missile(Vector3 wPos, float delay = 0)
	{
		yield return new WaitForSeconds(delay);
		EffectManagerBase.Create("Missile", wPos);

	}

	static public void CreateEffec_Boom(Vector3 wPos, float delay = 0)
	{
		if (delay == 0)
			EffectManagerBase.Create("Boom", wPos);
		else
			Instance.StartCoroutine(IE_CreateEffec_Boom(wPos, delay));
	}
	public static IEnumerator IE_CreateEffec_Boom(Vector3 wPos, float delay = 0)
	{
		yield return new WaitForSeconds(delay);
		EffectManagerBase.Create("Boom", wPos);

	}

}
