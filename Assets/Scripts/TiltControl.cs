using UnityEngine;
using System.Collections;

public class TiltControl : MonoBehaviour {

	public bool isPerspective = false;
	public bool isValid = false;

	public Transform cameraPivot;
	public Camera tiltCamera;

	private Vector3 cameraPivotRotation = new Vector3 (0,0,0);

	// Use this for initialization
	void Start () {

		CameraCheck ();			// Check that either a Perspective with Camera Pivot is selected or an Orthographic Camera

		if (isPerspective)
			cameraPivotRotation = cameraPivot.localEulerAngles;

	}
	
	// Update is called once per frame
	void Update () {

		if (isPerspective)
		{
			cameraPivotRotation.y = TiltManager.Instance.tiltViewAngle;
			cameraPivot.localEulerAngles = cameraPivotRotation;

		}
	
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
			Debug.Log (this.name + "is an invalid Tilt Control. Disabled.");
			this.enabled = false;
		} else {
			Debug.Log (this.name + " is a valid Tilt Control.");
		}
		

	}
}
