using UnityEngine;
using System.Collections;

public class SimpleAcceleroControl : MonoBehaviour {

	private float tiltAngle;
	private float lookAngle;
	private float mysteryAngle;

	private Vector3 badAngleA = new Vector3(-0.7f,-0.7f,0f);
	private Vector3 badAngleB = new Vector3(0.7f,-0.7f,0f);
	//private float angleClamp = 
	private Vector3 gyroAngle;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


	
	}

	void OnGUI ()
	{
		tiltAngle = Mathf.Rad2Deg * Mathf.Atan(Input.gyro.gravity.y / Input.gyro.gravity.z);
		lookAngle = Mathf.Rad2Deg * Mathf.Atan(Input.gyro.gravity.x / Input.gyro.gravity.z);
		mysteryAngle = Mathf.Rad2Deg * Mathf.Atan(Input.gyro.gravity.x / Input.gyro.gravity.y);

		GUILayout.Label("Accelerometer: " + Input.gyro.gravity);
		GUILayout.Label("Harry Up/Down LookAngle: " + tiltAngle);
		GUILayout.Label("Harry Left/Right LookAngle: " + lookAngle);
		GUILayout.Label("Mystery: " + mysteryAngle);
		GUILayout.Label("Upright Factor: " + (mysteryAngle/lookAngle));
		gyroAngle = Input.gyro.gravity;

		GUILayout.Label("To Bad angle: " + (Vector3.Dot (gyroAngle, badAngleA)));

		// No-one is going to care if you can tilt to look
	}
}
