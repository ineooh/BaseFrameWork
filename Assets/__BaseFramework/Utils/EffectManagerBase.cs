using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PoolInfo
{
	[SerializeField]
	public GameObject item;
	[SerializeField]
	public int poolCount;
}
public class EffectManagerBase : SingletonMono<EffectManagerBase>
{
	public PoolInfo[] infoPoolList;
//    public GameObject[] listParticle;
//    public int NumObjectInPool = 100;

    Dictionary<string, Queue<PoolParticle>> dicPool = new Dictionary<string, Queue<PoolParticle>>();

    void Start ()
    {
        CreateAllPoolObject();
    }

	void CreateAllPoolObject()
	{
		foreach (PoolInfo info in infoPoolList)
		{
			info.item.SetActive(false);
			Queue<PoolParticle> queue = new Queue<PoolParticle>();
			dicPool.Add(info.item.name, queue);
			for (int i = 0; i < info.poolCount; i++)
			{
				GameObject o = Instantiate(info.item, transform) as GameObject;
				PoolParticle c = o.GetComponent<PoolParticle>();
				c.nameprefab = info.item.name;
				queue.Enqueue(c);
			}
		}
	}

	int indexParticleUpdate = -1;
	public List<PoolParticle> listParticleUpdate = new List<PoolParticle>();
	public void RegisterForUpdating( PoolParticle par )
	{
		if ( par!=null && !listParticleUpdate.Contains (par))
		{
			listParticleUpdate.Add (par);
		}
	}
	public void UnRegisterForUpdating( PoolParticle par )
	{
		if ( par!=null && listParticleUpdate.Contains (par))
		{
			listParticleUpdate.Remove(par);
		}
	}
	void Update()
	{
		if (listParticleUpdate.Count > 0)
		{
			if (indexParticleUpdate == -1 || indexParticleUpdate > listParticleUpdate.Count - 1)
			{
				indexParticleUpdate = listParticleUpdate.Count - 1;
			}
			listParticleUpdate[indexParticleUpdate].UpdateGame();
			indexParticleUpdate--;
		}
	}

   
    static public void PushIntoPool(PoolParticle p)
    {
        p.gameObject.SetActive(false);
		p.transform.parent = Instance.transform;
        Instance.dicPool[p.nameprefab].Enqueue(p);

    }
//    PoolParticle GetPoolParticle(int index)
//    {
//        return GetPoolParticle(listParticle[index].name);
//    }
    PoolParticle GetPoolParticle(GameObject pf)
    {
        return GetPoolParticle(pf.name);

    }
    PoolParticle GetPoolParticle(string name)
    {
        Queue<PoolParticle> queue = dicPool[name];
        if (queue.Count > 0)
        {
            PoolParticle p = queue.Dequeue();
            p.Play();
            return p;
        }
		else
		{
			foreach (PoolInfo info in infoPoolList)
			{
				if (name == info.item.name)
				{
					GameObject o = Instantiate(info.item, transform) as GameObject;
					PoolParticle c = o.GetComponent<PoolParticle>();
					c.nameprefab = info.item.name;
					return c;
				}
			}
		}
        return null;

    }
	static public PoolParticle Create( string name , Vector3 wPos  , Transform followTarget = null  , Transform parent = null)
    {
        //if (MyUtils.CheckInScreen(wPos, mainPos ))
        {
            PoolParticle p = Instance.GetPoolParticle(name);
            if (p != null)
            {
                p.transform.position = wPos;
                p.transform.rotation = Quaternion.identity;
				p.followTarget = followTarget;
				if (parent != null) {
					p.transform.parent = parent;
				}
            }
			
			return p;
        }
		return null;
    }
    static public void CreateWithRotation(string name, Vector3 wPos, Vector3 mainPos, Quaternion rotation)
    {
		if (MyUtils.CheckInScreen(wPos, mainPos))
		{
			PoolParticle p = Instance.GetPoolParticle(name);
			if (p != null)
			{
				p.transform.position = wPos;
				p.transform.rotation = rotation;
				
			}
		}
	}
	static public PoolParticle CreateWithBurst(string name, Vector3 wPos, Vector3 mainPos, int amount)
	{
		if (MyUtils.CheckInScreen(wPos, mainPos))
		{
			PoolParticle p = Instance.GetPoolParticle(name);
			if (p != null)
			{
				p.transform.position = wPos;
				p.transform.rotation = Quaternion.identity;
				ParticleSystem.EmissionModule emission = p.particle.emission;
				ParticleSystem.Burst burst = emission.GetBurst(0);
				burst.minCount = (short)amount;
				burst.maxCount = (short)(amount+3);
				emission.SetBurst(0 , burst);

			}

			return p;
		}
		return null;
	}

	

}
