using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolParticle : MonoBehaviour
{
    public ParticleSystem particle;
    [HideInInspector]
    public string nameprefab = "";
	public Transform followTarget;

	public float lifeTime = 0;
	float lifeTimeCur = 0;

	private void OnEnable()
	{
		EffectManagerBase.Instance?.RegisterForUpdating(this);
		
	}
	void OnDisable()
	{
//		if (registerWhenEnable)
		EffectManagerBase.Instance?.UnRegisterForUpdating(this);
//		if (isBackToPoolWhenDisable) {
//			EffectManager.PushIntoPool(this);
//		}
	}
	void OnDestroy()
	{
//		if (registerWhenEnable)
		EffectManagerBase.Instance.UnRegisterForUpdating(this);
	} 
    public void Play()
    {
        gameObject.SetActive(true);

		if (lifeTime > 0)
		{
			lifeTimeCur = Time.realtimeSinceStartup; 
		}
		if(particle !=null)
			particle?.Play(true);

	}
	public void Stop()
	{
		//gameObject.SetActive(true);
		particle.Stop(true);

	}

	public void UpdateGame()
    {
		if( lifeTime > 0)
		{
			if(Time.realtimeSinceStartup - lifeTimeCur >=  lifeTime)
			{
				EffectManagerBase.PushIntoPool(this);
			}
		}
		else if (particle!=null && particle.isStopped)
		{
			EffectManagerBase.PushIntoPool(this);
		}
		else
		{
			EffectManagerBase.PushIntoPool(this);
		}
	}
	void Update()
	{
		if (followTarget != null) 
		{
			transform.position = followTarget.position;
		}
	}

    private void OnDrawGizmosSelected()
    {
        if (particle == null)
            particle = GetComponent<ParticleSystem>();
    }
}
