using UnityEngine;
using System.Collections;



public class QT_CubeQuatRotate : MonoBehaviour {
	
	public float smooth = 2.0f;
	public float tiltAngle = 30.0f;
	public Vector3 myFirstAngle = new Vector3(30,20,10);
	public Vector3 mySecondAngle = new Vector3(60,90,30);
	private Quaternion myFirstQuaternion;
	//private Quaternion mySecondQuaternion;
	public Vector3 myVector;
	
	void OnGUI()
	{
		GUILayout.Label("Mouse X Axis: " + Input.GetAxis("Mouse X"));
		GUILayout.Label("Mouse y Axis: " + Input.GetAxis("Mouse Y"));
		GUILayout.Label("My First Angle in Quaternions: " + Quaternion.Euler(myFirstAngle));
		GUILayout.Label("My Second Angle in Quaternions: " + Quaternion.Euler(mySecondAngle));
		myFirstQuaternion = Quaternion.Euler(myFirstAngle);
		//mySecondQuaternion = Quaternion.Euler(mySecondAngle);
		
		GUILayout.Label("My First Quaternion in Euler Angles: " + myFirstQuaternion.eulerAngles);
		GUILayout.Label("My First Quaternion x Quaternion Identity: " + (myFirstQuaternion * Quaternion.identity));
		myVector = myFirstQuaternion * -Vector3.forward;
		
		GUILayout.Label("My First Angle x Vector3.back " + myVector);
		GUILayout.Label("My First Angle FromToRotation My SecondAngle " + (Quaternion.FromToRotation ( myFirstAngle, mySecondAngle )).eulerAngles);
	}
	
	
	// Update is called once per frame
	void Update () {
		
		float tiltAroundZ = Input.GetAxis("Mouse X") * tiltAngle;
		float tiltAroundX = Input.GetAxis("Mouse Y") * tiltAngle;
		Quaternion target = Quaternion.Euler (tiltAroundX, 0, tiltAroundZ);
		
		
		transform.rotation =  Quaternion.Slerp (transform.rotation, target, Time.deltaTime * smooth);
	
	}
}
