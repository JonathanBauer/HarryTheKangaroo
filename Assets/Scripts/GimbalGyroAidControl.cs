using UnityEngine;
using System.Collections;

public class GimbalGyroAidControl : MonoBehaviour {
	
	
	public float trackingSpeed = 0.75f;
	
	public Transform gimbal;
	public Transform gyroAidYaw;
	public Transform gyroAidPitch;
	
	public bool calibrationSlerp = true;
	public float calibrationSlerpSpeed = 0.1f;
	
	public Vector2 mouseRotSpeed = new Vector2(0.1f,0.1f);
	
	

	private Quaternion targetRotation = Quaternion.identity;
	private bool onGyro;
	private bool debug = true;
	
	private Vector3 gyroAidPosition;
	private Vector3 gyroAidOffset = new Vector3(0,0,0);
	
	private Vector2 previousMousePos = new Vector2(0,0);
	private Vector2 currentMousePos = new Vector2(0,0);
	private Vector2 gimbalRotationAmount = new Vector2(0,0);
	private bool onRightMouseButton = false;
	private float rotX = 0.0f;
	private float rotY = 0.0f;
	
	
	void Start () {
		onGyro = SystemInfo.supportsGyroscope;
		previousMousePos.x = Screen.width / 2;
		previousMousePos.y = Screen.height / 2;
		
	
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
			
			// X and Y must be swapped for Quaternion calculation (No idea why)
			targetRotation = Quaternion.Euler(rotY,rotX, 0);
			
		
		}
		
		
		
		
		gimbal.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, trackingSpeed); 
				
		// gyroAidPosition reports the orientation of the iPhone gyroscope
		// but is also controlled by the mouse
		
		gyroAidPosition = (gimbal.transform.localRotation).eulerAngles;
				
		// To use the iPhone gyroscope, the 'virtual' gyro needs to be calibrated so that
		// the direction the device is pointing when the app starts has x = 0 and y = 0.
		// This is accomplished by applying an offset to the virtual gyro
		
		gyroAidPosition.x += gyroAidOffset.x;
		
		gyroAidPosition.y +=gyroAidOffset.y;
		
		// The virtual gyro objects then display gyroAidPosition as Yaw and Pitch
		// If calibrationSlerp is on, the gyro will move toward 0,0,0 instead of snapping to it
		
		if (calibrationSlerp)
		{
			gyroAidYaw.transform.localRotation = Quaternion.Slerp(gyroAidYaw.transform.localRotation,
													Quaternion.Euler(0,gyroAidPosition.y,0),
													calibrationSlerpSpeed);
			
			gyroAidPitch.transform.localRotation = Quaternion.Slerp(gyroAidPitch.transform.localRotation,
													Quaternion.Euler(gyroAidPosition.x,0,0),
													calibrationSlerpSpeed);
		}
		else
		{
		
			gyroAidYaw.transform.localRotation = Quaternion.Euler(0,gyroAidPosition.y,0);
			
			gyroAidPitch.transform.localRotation = Quaternion.Euler(gyroAidPosition.x,0,0);
		}

	
	}
	
	void OnGUI () {
		
		if (debug)
		{
	
			GUILayout.Label("Gyro Attitude " + Input.gyro.attitude);
			GUILayout.Label("Target Rotation " + targetRotation);
			GUILayout.Label("RotX: " + rotX);
			GUILayout.Label("RotY: " + rotY);
			GUILayout.Label("Transform.rotation (euler): " + transform.rotation.eulerAngles);
			GUILayout.Label("gyroAidPosition .y " + gyroAidPosition .y);
			GUILayout.Label("gyroAidPosition .x " + gyroAidPosition .x);
			GUILayout.Label("gyroAidOffset.y " + gyroAidOffset.y);
			GUILayout.Label("gyroAidOffset.x " + gyroAidOffset.x);
			
			if (GUILayout.Button("Calibate Pitch and Yaw", GUILayout.Height(80)))
			{
				
				CalibrateGyro();
				
			}
			
			if (GUILayout.Button("Calibration Lerp " + calibrationSlerp, GUILayout.Height(80)))
			{
				
				ToggleCalibrationSlerp();
				
			}
		
		}
		
		
		
	}
	
	
	static Quaternion ConvertRotation(Quaternion q)
	{
		return new Quaternion(q.x, q.y, -q.z, -q.w);	
	}
	
	private void CalibrateGyro()
	{
		// Reseting the gyro to face forwards involves changing the offset value.
		// Through long trials and error I worked out
		// it's the difference between the current rotation and the offset
		// changed to a negative
		
		gyroAidOffset.y -= gyroAidPosition.y;
			
		gyroAidOffset.x -= gyroAidPosition.x;
	}
	
	private void ToggleCalibrationSlerp()
	{
		calibrationSlerp = !calibrationSlerp;
	}
}
