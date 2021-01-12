using UnityEngine;
using System.Collections;

public class SelfDestroy : MonoBehaviour
{

    public float lifeTime = 2;
	
    // Use this for initialization
	void Awake ()
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
    public void selfDestroy( float seconds)
    {
        Invoke("destroy", seconds);
    }
}
