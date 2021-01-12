using UnityEngine;
using System.Collections;


public class SmoothFollow : MonoBehaviour
{
	public Transform target;
	public float smoothDampTime = 0.2f;
	[HideInInspector]
	public new Transform transform;
	public Vector3 offset;
	public bool useFixedUpdate = false;
	
	//private CharacterController2D _playerController;
	private Vector3 _smoothDampVelocity;
	private float lastZCam;

    bool isMoving = false;
    Vector3 move_posfrom;
    Vector3 move_posto;
    float move_atTime =0 ;
    float move_duration = 1f;
    public void MoveTo(Vector3 wpos)
    {
        isMoving = true;
        move_atTime = Time.realtimeSinceStartup;
        move_posfrom = transform.position;
        move_posto = wpos;

    }

	
	void Awake()
	{
		transform = gameObject.transform;
		//lastZCam = transform.position.z;
      //  if( target != null)
		    //_playerController = target.GetComponent<CharacterController2D>();
	}
	
	
	void LateUpdate()
	{

		if( !useFixedUpdate )
			updateCameraPosition();
	}


	void FixedUpdate()
	{
		if( useFixedUpdate )
			updateCameraPosition();
	}


	void updateCameraPosition()
	{
        //if(isMoving)
        //{
        //    transform.position = Vector3.Lerp(move_posfrom, move_posto, (Time.realtimeSinceStartup - move_atTime)/ move_duration);
        //    if (Time.realtimeSinceStartup - move_atTime > move_duration)
        //        isMoving = false;
        //    return;
        //}

        
        if (target == null  ) return;

  //      if ( _playerController == null )
		//{
		//	transform.position = Vector3.SmoothDamp( transform.position, target.position - cameraOffset, ref _smoothDampVelocity, smoothDampTime );
		//	return;
		//}
		
		//if( _playerController.velocity.x > 0 )
		//{
		//	transform.position = Vector3.SmoothDamp( transform.position, target.position - cameraOffset, ref _smoothDampVelocity, smoothDampTime );
		//}
		else
		{
			//var leftOffset = cameraOffset;
			//leftOffset.x *= -1;
			transform.position = Vector3.SmoothDamp( transform.position, target.position + offset/* - leftOffset*/, ref _smoothDampVelocity, smoothDampTime );
		}

		//Vector3 fixZ = transform.position;
		//fixZ = new Vector3(transform.position.x, transform.position.y,  lastZCam);
		//transform.position = fixZ;

	}
	
}
