using UnityEngine;
using System.Collections;

public class GimbalControl : MonoBehaviour {
	
	public float rotX = 0.0f;
	public float rotY = 0.0f;
	public Vector2 rotationRate= new Vector2(30,30);
	Quaternion targetRotation = Quaternion.identity;
	
	public float trackingSpeed = 10.0f;
	
	private bool onGyro;
	private bool onRightMouseButton = false;
	
	private Vector2 previousMousePos = new Vector2(0,0);
	private Vector2 currentMousePos = new Vector2(0,0);
	private Vector2 gimbalRotationAmount = new Vector2(0,0);
	
	public Vector2 mouseRotSpeed = new Vector2(0.1f,0.1f);

	
	void Start () {
		onGyro = SystemInfo.supportsGyroscope;
		previousMousePos.x = Screen.width / 2;
		previousMousePos.y = Screen.height / 2;
	
	}
	
	// Update is called once per frame
	void Update () {
		if (onGyro)
		{
			rotX = Input.gyro.gravity.x * rotationRate.x;
			rotY = Input.gyro.gravity.y * rotationRate.y;
			
			// Y reversed to make gimbal act like a spirit level
			rotY = -rotY;
		}
		
		onRightMouseButton = Input.GetMouseButton (1);
		
		if (onRightMouseButton)
		{
			currentMousePos = Input.mousePosition;
			gimbalRotationAmount.x = (currentMousePos.x - previousMousePos.x) * mouseRotSpeed.x;
			gimbalRotationAmount.y = (currentMousePos.y - previousMousePos.y) * mouseRotSpeed.y;
			
			//Reverse the rotation direction of X axis
			gimbalRotationAmount.y = - gimbalRotationAmount.y;
			
			rotX = rotX + gimbalRotationAmount.x;
			rotY = rotY + gimbalRotationAmount.y;
			
			previousMousePos = currentMousePos;
		}
		
		// X and Y must be swapped for Quaternion calculation (No idea why)
		targetRotation = Quaternion.Euler(rotY,rotX, 0);
		
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, trackingSpeed); 
	
	}
	/*
	void OnGUI () {
		
		GUILayout.Label("RotX: " + rotX);
		GUILayout.Label("RotY: " + rotY);
		GUILayout.Label("Transform.rotation (euler): " + transform.rotation.eulerAngles);
		GUILayout.Label("Right Mouse Button is down: " + onRightMouseButton);
		GUILayout.Label("Mouse Position: " + Input.mousePosition);
		
		
	}
	*/
}
