using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TiltManager : MonoBehaviour {

	public bool debugMode = false;
	
	static TiltManager myInstance;
	static int instances = 0;
	
	public static TiltManager Instance
	{
		get
		{
			if (myInstance == null)
				myInstance = FindObjectOfType (typeof(TiltManager)) as TiltManager;
			
			return myInstance;
		}
	}

	public bool iPhoneDevice = false;

	public float mouseDragAdjust = 0.5f;

	public float iphoneTiltAdjust = 0.5f;

	public Vector2 tiltAngle = new Vector2( 0, 0);


	// Use this for initialization
	void Start () {

		instances++;
		
		if (instances > 1)
			Debug.Log ("There is more than one Tilt Manager in the level");
		else
			myInstance = this;

		#if !UNITY_EDITOR && !UNITY_WEBPLAYER

			iPhoneDevice = true;

		#else

			iPhoneDevice = false;
		
		#endif

		if (!TouchManager.Instance)
		{
			if (debugMode)
				Debug.Log("Touch Manager not found");
		} else {
			if (debugMode)
				Debug.Log("Touch Manager found");
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (iPhoneDevice)
		{
			/* tiltViewAngle returns a negative angle when the device is tilted to the left and a positive to the right. It calculates this from
			 * the angle between the gyro.gravity x and z vectors. This gives a reasonably reliable tilt value even when the device is upright in
			 * landscape left */
			tiltAngle.x = (Mathf.Rad2Deg * Mathf.Atan(Input.gyro.gravity.x / Input.gyro.gravity.z)) * iphoneTiltAdjust;

			tiltAngle.y = (Mathf.Rad2Deg * Mathf.Atan(Input.gyro.gravity.y / Input.gyro.gravity.z)) * iphoneTiltAdjust;


		} else {

			tiltAngle.x = TouchManager.Instance.draggedViewAngle.x * mouseDragAdjust;
			tiltAngle.y = TouchManager.Instance.draggedViewAngle.y * mouseDragAdjust;


		}

	
	}

}
