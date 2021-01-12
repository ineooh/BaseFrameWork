using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public enum ItemType
{
	Cash,
	Gold,
	Car,
	Gun,
	Skill,
}


public enum ChallengeType
{
	Buy_Car,
	Destroy_Car,
	Destroy_Car_In_One_Run,
	Destroy_Cop,
	Destroy_Cop_In_One_Run,
	Destroy_Prop,
	Destroy_Prop_In_One_Run,
	Reach_CheckPoint,
	Reach_CheckPoint_In_A_Play,
	Invite_Friend,
	Kill_Boss,
	Die_Count,
}

[System.Serializable]
public class Item_Amount
{
	public ItemType type;
	public int id = 0;
	public int amount = 0;
}

[System.Serializable]
public class ChallengeInfo
{
	public string name;
	public ChallengeType type;
	public int level = 0;
	public string des;
	public ChallengeLevel[] level_list;

}
[System.Serializable]
public class ChallengeLevel
{
	public string nameID;
	public int amount;
	public Item_Amount[] reward_list;

}
[System.Serializable]
public class ChallengeDataUser
{
	public ChallengeType type;
	public int level = 0;
	public int count = 0;
 	public bool isClaim = false;
	public ChallengeDataUser()
	{
		level = 0;
		count = 0;
		isClaim = false;
	}
}



[System.Serializable]
public class CarInfo
{
	public int id = 0;
	public string name = "Main Car";
	public float hp = 1000;
	public float maxspeed = 25;
	public float speed = 20;
	public float mass = 500;
	public float horsePower = 20;
	public float handling = 20;

	public int price = 5000; // 1: like facebook / 2:
	public bool isown = false;
	public bool isInvitePrice = false;
	public GunInfo gunInfo;
}
public enum GUNTYPE
{
	PISTOL = 0,
	SHOTGUN,
	UZI,
	SMG,
	ASSAULT_RIFLES,
	SNIPER_RIFLES,
	DESERT_EAGLE,
	MINI_GUN,
	RPG,
}
public enum BULLETTYPE
{
	STRAIGHT,
	RPG
}
[System.Serializable]
public class GunInfo
{
	public int id;
	public GUNTYPE type = GUNTYPE.PISTOL;
	public string name = "PISTOL";
	public float power = 50;
	public float rate = 0.5f;
	public float range = 15;
	public float radiusImpact = 0;
	public float speedBullet = 0;

	public int clip = 1;
	public float clip_rate = 1f;



	public float getDPS()
	{
		float dps = (power * clip) / (rate + clip_rate * clip);
		Debug.Log("getDPS " + dps);
		return dps;

	}
}