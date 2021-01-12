using UnityEngine;
using System.Collections.Generic;

public class ParticleControl : MonoBehaviour {

    public ParticleSystem[] particles;
    Dictionary<ParticleSystem, float> dic = new Dictionary<ParticleSystem, float>();
	// Use this for initialization
	void Awake ()
    {
        if (particles.Length == 0)
            particles = GetComponentsInChildren<ParticleSystem>();

        foreach(ParticleSystem i in particles)
        {
            dic.Add(i, i.emissionRate);
        }
    }
    public void Set(string name, int order)
    {
        Renderer[] rdererList = GetComponentsInChildren<Renderer>();
        foreach (Renderer rderer in rdererList)
            if (rderer != null)
            {
                rderer.sortingLayerName = name;
                rderer.sortingOrder = order;
            }
    }

    public void StopEmission()
    {
        foreach (ParticleSystem i in particles)
        {
            i.emissionRate = 0;
        }
    }
    public void PlayEmission()
    {
        foreach (ParticleSystem i in particles)
        {
            i.emissionRate = dic[i];
        }
    }
}
    