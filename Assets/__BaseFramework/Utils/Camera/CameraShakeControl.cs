using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraShakeControl : MonoBehaviour
{
    public static CameraShakeControl Instance;
    public float timeShake = 0.5f;
    public float forceUnit = 0.05f;
    public float speedCam = 0.5f;
    public float MaxRicher = 0.25f;
    float speed = 0;
    float richter = 0;
    float time = 0;
    bool stop = false;
    public Vector3 beginPos;
	public Vector3 targetTo = Vector3.zero;

    void Awake()
    {
        Instance = this;
		beginPos = transform.localPosition;
    }
    void Update()
    {
        if( Input.GetKeyDown( KeyCode.Q))
        {
            Shake();
        }

        if (time > 0)
        {
            time -= Time.deltaTime;
			if (Vector3.Distance(transform.localPosition, targetTo+beginPos) < 0.02f)
            {
                //targetTo.x = Random.Range(-richter, richter);
				targetTo.y =  targetTo.y > 0 ? -richter : richter;
				//targetTo.x =  targetTo.x > 0 ? Random.Range(-richter, -richter/2) : Random.Range(richter / 2, richter) ;
            }
        }
        else
        {

            targetTo = Vector3.zero;
        }
        if (richter != 0)
            richter = Mathf.Lerp(richter, 0, 0.1f);
        if (!stop)
        {
			Vector3 dis = targetTo + beginPos;
			if (transform.localPosition != dis )
				transform.localPosition = Vector3.Lerp(transform.localPosition,  dis, speedCam);
        }
    }

    public void Shake(float scale = 1)
    {
        if (time < 0)
            time = 0;
        richter += forceUnit * scale;
        richter = Mathf.Min(MaxRicher, richter);
            speed += forceUnit * scale;
        time += timeShake * scale;
        time = Mathf.Max(0.1f, time);
    }


    public void Enable()
    {
        stop = false;        
    }
    public void Disable()
    {
        stop = true;

    }
}
