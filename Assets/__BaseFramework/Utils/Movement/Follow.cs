using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {


    public GameObject target;
    public bool FollowRotation;
    public Vector3 offset;
    public float TimeDestroy = 0;
    bool isDestroy = false;
    
    
	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update () {
        if (target != null)
        {
            transform.position = target.transform.position + offset;
            if (FollowRotation)
                transform.rotation = target.transform.rotation;
        }
        else if (isDestroy == false)
        {
            TimeDestroy -= Time.deltaTime;
            if (TimeDestroy <= 0)
                Destroy();
        }
	}

    void Destroy()
    {
        Destroy(gameObject);
    }
}
