using UnityEngine;
using System.Collections;

public class GyroAidControl : MonoBehaviour {
	
	public float rotX = 0.0f;
	public float rotY = 0.0f;
	public Vector2 rotationRate= new Vector2(30,30);
	Quaternion targetRotation = Quaternion.identity;
	
	public float trackingSpeed = 10.0f;
	
	private bool onGyro;
	private bool onRightMouseButton = false;
	
	public Transform gyroAidYaw;
	public Transform gyroAidPitch;
	
	private Vector3 gyroAidYawControl;		
	private Vector3 gyroAidPitchControl;
	
	private float gyroAidYawOffset = 0.0f;
	private float gyroAidPitchOffset = 0.0f;
	
	private Vector2 previousMousePos = new Vector2(0,0);
	private Vector2 currentMousePos = new Vector2(0,0);
	private Vector2 gimbalRotationAmount = new Vector2(0,0);
	
	public Vector2 mouseRotSpeed = new Vector2(0.1f,0.1f);

	
	void Start () {
		onGyro = SystemInfo.supportsGyroscope;
		previousMousePos.x = Screen.width / 2;
		previousMousePos.y = Screen.height / 2;
		
		//Input.gyro.enabled = true;
	
	}
	
	// Update is called once per frame
	void Update () {
		if (onGyro)
		{
		
			//targetRotation = ConvertRotation(Quaternion.Euler (0,0,-90) * Input.gyro.attitude);
			
			//targetRotation = ConvertRotation(Input.gyro.attitude);
			targetRotation = ConvertRotation(Quaternion.Euler (-90,0,0) * Input.gyro.attitude);
		}
		
		onRightMouseButton = Input.GetMouseButton (1);
		
		if (onRightMouseButton)
		{
			currentMousePos = Input.mousePosition;
			gimbalRotationAmount.x = (currentMousePos.x - previousMousePos.x) * mouseRotSpeed.x;
			gimbalRotationAmount.y = (currentMousePos.y - previousMousePos.y) * mouseRotSpeed.y;
			
			//Reverse the rotation direction of X axis
			gimbalRotationAmount.y = - gimbalRotationAmount.y;
			gimbalRotationAmount.x = - gimbalRotationAmount.x;
			
			rotX = rotX + gimbalRotationAmount.x;
			rotY = rotY + gimbalRotationAmount.y;
			
			previousMousePos = currentMousePos;
			
			targetRotation = Quaternion.Euler(rotY,rotX, 0);
			
		
			
			//gyroAidPitch.transform.localRotation
		}
		
		// X and Y must be swapped for Quaternion calculation (No idea why)
		
		
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, trackingSpeed); 
		
		gyroAidYawControl = (transform.localRotation).eulerAngles;
		
		gyroAidPitchControl = (transform.localRotation).eulerAngles;
		
		gyroAidYawControl.x = 0;
		
		gyroAidYawControl.z = 0;
		
		gyroAidYawControl.y -= gyroAidYawOffset;
		//gyroAidYawControl.y = 90;
		
		gyroAidYaw.transform.localRotation = Quaternion.Euler(gyroAidYawControl);
		
		//gyroAidPitchControl.z = gyroAidPitchControl.x;
		
		gyroAidPitchControl.z = 0;
		
		gyroAidPitchControl.y = 0;
		
		gyroAidPitchControl.x -= gyroAidPitchOffset;
		
		gyroAidPitch.transform.localRotation = Quaternion.Euler(gyroAidPitchControl);
		
		
	
	}
	
	void OnGUI () {
		
		GUILayout.Label("On Gyro? " + onGyro);
		GUILayout.Label("Gyro Enabled? " + Input.gyro.enabled);
		GUILayout.Label("Gyro Attitude " + Input.gyro.attitude);
		GUILayout.Label("Target Rotation " + targetRotation);
		GUILayout.Label("RotX: " + rotX);
		GUILayout.Label("RotY: " + rotY);
		GUILayout.Label("Transform.rotation (euler): " + transform.rotation.eulerAngles);
		GUILayout.Label("gyroAidYawControl.y " + gyroAidYawControl.y);
		GUILayout.Label("gyroAidPitchControl.x " + gyroAidPitchControl.x);
		GUILayout.Label("gyroAidYawOffset " + gyroAidYawOffset);
		GUILayout.Label("gyroAidPitchOffset " + gyroAidPitchOffset);
		
		if (GUILayout.Button("Calibate Pitch and Yaw", GUILayout.Height(80)))
		{
			gyroAidYawOffset = gyroAidYawControl.y;
			
			gyroAidYaw.transform.localRotation = Quaternion.Euler(gyroAidYawControl);
			
			gyroAidPitchOffset = gyroAidPitchControl.x;
			
			gyroAidPitch.transform.localRotation = Quaternion.Euler(gyroAidPitchControl);
		}
		
		
		
	}
	
	
	static Quaternion ConvertRotation(Quaternion q)
	{
		return new Quaternion(q.x, q.y, -q.z, -q.w);	
	}
}
