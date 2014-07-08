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
	public Vector2 iphoneTiltLimit = new Vector2 (45,0);
	public Vector2 mouseTiltLimit = new Vector2 (45,0);

	public bool iPhoneDevice = false;

	public float iPhoneTiltAdjust = 0.5f;

	public float mouseDragAdjust = 0.5f;

	public float tiltViewAngle = 0f;

	public bool useIncrements = false;

	public float incrementalAmount = 10f;

	public float incrementalThreshold = 2f;

	public float incrementalSpeed = 1f;
	
	private List<float> increment = new List<float>();

	private float incrementNumber = 0f;

	private float angleTarget = 0f;

	private float travel;

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

		if (useIncrements)
		{

			float lowestValue;

			if (iPhoneDevice)
			{

				incrementNumber = (Mathf.Floor(iphoneTiltLimit.x / incrementalAmount)* 2) + 1;
				lowestValue = - (Mathf.Floor(iphoneTiltLimit.x / incrementalAmount) * incrementalAmount);


			} else {

				incrementNumber = (Mathf.Floor(mouseTiltLimit.x / incrementalAmount)* 2) + 1;
				lowestValue = - (Mathf.Floor(mouseTiltLimit.x / incrementalAmount) * incrementalAmount);

			}

			for (int t = 0; t < incrementNumber; t++)
			{
				increment.Add(lowestValue + (incrementalAmount * t));
				Debug.Log(increment[t]);
			}



		}
		//Debug.Log (Mathf.Tan(1.6f));



	
	}
	
	// Update is called once per frame
	void Update () {

		if (iPhoneDevice)
		{
			/* tiltViewAngle returns a negative angle when the device is tilted to the left and a positive to the right. It calculates this from
			 * the angle between the gyro.gravity x and z vectors. This gives a reasonably reliable tilt value even when the device is upright in
			 * landscape left */
			float angle = (Mathf.Rad2Deg * Mathf.Atan(Input.gyro.gravity.x / Input.gyro.gravity.z)) * iPhoneTiltAdjust;

			angle = Mathf.Clamp( angle, -iphoneTiltLimit.x, iphoneTiltLimit.x);

			if (useIncrements)
			{
				
				
				for (int i = 0; i < incrementNumber; i++)
				{
					if (angle > (increment[i] - incrementalThreshold) && angle < (increment[i] + incrementalThreshold))
					{
						if (angleTarget != increment[i])
							angleTarget = increment[i];
						
						travel = 0;
						
					}
					
				}
				
				tiltViewAngle = Mathf.Lerp (angle, angleTarget, travel);
				
				travel += incrementalSpeed;
				
				
				
			} else {
				
				tiltViewAngle = angle;
				
			}
		} else {

			float angle = TouchManager.Instance.draggedViewAngle * mouseDragAdjust;

			angle = Mathf.Clamp( angle, -mouseTiltLimit.x, mouseTiltLimit.x);

			if (useIncrements)
			{


				for (int i = 0; i < incrementNumber; i++)
				{
					if (angle > (increment[i] - incrementalThreshold) && angle < (increment[i] + incrementalThreshold))
					{
						if (angleTarget != increment[i])
							angleTarget = increment[i];

						travel = 0;

					}

				}

				tiltViewAngle = Mathf.Lerp (angle, angleTarget, travel);

				travel += incrementalSpeed;



			} else {

			tiltViewAngle = angle;

			}
		}

	
	}

	void OnGUI (){

		GUILayout.Label("Harry Left/Right LookAngle: " + tiltViewAngle);
	}
}
