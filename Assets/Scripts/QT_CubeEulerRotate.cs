using UnityEngine;
using System.Collections;



public class QT_CubeEulerRotate : MonoBehaviour {
	
	public float horizontalSpeed = 2.0f;
	public float verticalSpeed = 2.0f;
	public Transform fromToPointer;
	public Vector3 myFirstVector = Vector3.up;
	public Vector3 mySecondVector = Vector3.forward;
	public Vector3 myFirstAngle = new Vector3(90,0,0);
	public Vector3 mySecondAngle = new Vector3(30,0,0);
	
void OnGUI()
	{
		GUILayout.Label("Mouse X Axis: " + Input.GetAxis("Mouse X"));
		GUILayout.Label("Mouse Y Axis: " + Input.GetAxis("Mouse Y"));
		GUILayout.Label("My First Vector * Quat(MyFirstAngle): " + (Quaternion.Euler (myFirstAngle)) * myFirstVector);
		GUILayout.Label("My First Vector FromToRotation my Second Vector in Euler: " + 
			(Quaternion.FromToRotation(myFirstVector, mySecondVector)).eulerAngles);
		GUILayout.Label("My First Vector FromToRotation this object in Euler: " + Quaternion.FromToRotation(myFirstVector, (transform.rotation * Vector3.up)).eulerAngles);
		GUILayout.Label("Quat(MyFirstAngle) * Quat(MySecondAngle) in Euler: " + ((Quaternion.Euler (myFirstAngle)) * (Quaternion.Euler (mySecondAngle))).eulerAngles);
		GUILayout.Label("The inverse of Quaternion Indentity in Euler is:" +(Quaternion.Inverse(Quaternion.identity)).eulerAngles);
	}
	
	
	// Update is called once per frame
	void Update () {
		
		float h = horizontalSpeed * Input.GetAxis("Mouse X");
		float v = verticalSpeed * Input.GetAxis("Mouse Y");
		transform.Rotate (v,0,h);
	
	}
}
