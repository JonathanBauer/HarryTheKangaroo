using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TiltControl : MonoBehaviour {

	public bool isPerspective = false;			
	public bool isValid = false;
	public bool debugMode = false;
	/* isPerspective defines whether this page is using a perspective or orthographic camera
	 * Once defined, CameraCheck () finds out if the perspective camera is attached to an orbit pivot.
	 * If the settings are incorrect, isValid stays false and this component is disabled. */

	public bool iPhoneDevice = false;

	public Vector2 iphoneTiltLimit = new Vector2 (45,0);
	public Vector2 mouseTiltLimit = new Vector2 (45,0);

	public Transform cameraPivot;
	public Camera tiltCamera;

	public float tiltViewAngle = 0f;

	private Vector3 cameraPivotRotation = new Vector3 (0,0,0);

	public bool useIncrements = false;
	
	public float incrementalAmount = 10f;
	
	public float incrementalThreshold = 2f;
	
	public float incrementalSpeed = 1f;
	
	private List<float> increment = new List<float>();
	
	private float incrementNumber = 0f;
	
	private float angle = 0f;
	
	private float travelAngle;
	
	private float travelSpeed;
	
	private int currentIncrement = 0;
	
	private int targetIncrement = 0;

	// Use this for initialization
	void Start () {

		CameraCheck ();			// Check that either a Perspective with Camera Pivot is selected or an Orthographic Camera

		if (isPerspective)
			cameraPivotRotation = cameraPivot.localEulerAngles;

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
			}
			
			
			
		}

	}
	
	// Update is called once per frame
	void Update () {

		angle = TiltManager.Instance.angle;

		if (iPhoneDevice)
		{
			angle = Mathf.Clamp( angle, -iphoneTiltLimit.x, iphoneTiltLimit.x);

			if (useIncrements)
			{
				IncrementalTilting ();
				
			} else {
				
				tiltViewAngle = angle;
				
			}
		} else {

			angle = Mathf.Clamp( angle, -mouseTiltLimit.x, mouseTiltLimit.x);

			if (useIncrements)
			{
				IncrementalTilting ();
				
			} else {
				
				tiltViewAngle = angle;
				
			}
		}

		if (isPerspective)		// If this is a perspective camera, the pivot rotation is based on the TiltManager's tiltViewAngle
		{
			cameraPivotRotation.y = tiltViewAngle;


			cameraPivot.localEulerAngles = cameraPivotRotation;

		}
	
	}

	void IncrementalTilting () {
		
		for (int i = 0; i < incrementNumber; i++)
		{
			if (angle > (increment[i] - incrementalThreshold) && angle < (increment[i] + incrementalThreshold))
			{
				if (i != currentIncrement)
				{
					targetIncrement = i;
					
					if (i > currentIncrement)
					{
						travelSpeed = incrementalSpeed;
						
					} else {
						
						travelSpeed = - incrementalSpeed;
						
					}
					
				} 
				
			}
			
		}
		
		travelAngle += travelSpeed;
		
		if (travelSpeed > 0 && travelAngle > increment[targetIncrement])
		{
			travelAngle = increment[targetIncrement];
			currentIncrement = targetIncrement;
		}
		
		if (travelSpeed < 0 && travelAngle < increment[targetIncrement])
		{
			travelAngle = increment[targetIncrement];
			currentIncrement = targetIncrement;
		}
		
		tiltViewAngle = travelAngle;
		
	}

	private void CameraCheck ()
	{
		
		if (isPerspective)
		{
			if (cameraPivot)
			{
				tiltCamera = GetComponentInChildren<Camera>();
				
				if (tiltCamera)
				{
					if (!tiltCamera.orthographic)
						isValid = true;
				} 
			} 
		} else {
			if (tiltCamera)
			{
				if (tiltCamera.orthographic)
				{
					if (!isPerspective)
						isValid = true;
				}
			}
		}
		
		if (!isValid)
		{
			if (debugMode)
				Debug.Log (this.name + "is an invalid Tilt Control. Disabled.");
			this.enabled = false;
		} else {
			if (debugMode)
				Debug.Log (this.name + " is a valid Tilt Control.");
		}
		

	}
}
