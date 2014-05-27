using UnityEngine;
using System.Collections;

public class GimbalGyroAidControl : MonoBehaviour {
	
	
	public float trackingSpeed = 1f;
	
	public Transform gimbal;
	public Transform gyroAidYaw;
	public Transform gyroAidPitch;
	
	public bool calibrationSlerp = true;
	public float calibrationSlerpSpeed = 0.1f;
	
	public Vector2 mouseRotSpeed = new Vector2(0.1f,0.1f);
	
	private bool useGyro = true;
	public Vector2 accelerometerRotRate= new Vector2(60,60);

	private Quaternion targetRotation = Quaternion.identity;
	private bool onGyro;
	//private bool debug = true;
	private bool invertRotation = false;
	
	public Vector3 gyroAid = new Vector3(0,0,0);			// This is used by the pages
	private Vector3 gyroAidOffset = new Vector3(0,0,0);
	
	public Vector3 reportedRotation = new Vector3(0,0,0);
	
	
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
			if(useGyro)
			{
		
			targetRotation = ConvertRotation(Quaternion.Euler (-90,0,0) * Input.gyro.attitude);
				
				
			}
			else
				{
			rotX = Input.gyro.gravity.x * accelerometerRotRate.x;
			rotY = Input.gyro.gravity.y * accelerometerRotRate.y;
			
			// Y reversed to make gimbal act like a spirit level
			rotY = -rotY;
			targetRotation = Quaternion.Euler(rotY,rotX, 0);
			}
				
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
				
		// gyroAid reports the orientation of the iPhone gyroscope
		// but is also controlled by the mouse
		
		gyroAid = (gimbal.transform.localRotation).eulerAngles;
				
		// To use the iPhone gyroscope, the 'virtual' gyro needs to be calibrated so that
		// the direction the device is pointing when the app starts has x = 0 and y = 0.
		// This is accomplished by applying an offset to the virtual gyro
		
		if ((gyroAid.x + gyroAidOffset.x) > 180)
		{
			gyroAid.x =  -(360 - (gyroAid.x + gyroAidOffset.x));
		}
		else if ((gyroAid.x + gyroAidOffset.x) < -180)
		{
			gyroAid.x =  360 + (gyroAid.x + gyroAidOffset.x);
		}
		else
		{
			gyroAid.x = gyroAid.x + gyroAidOffset.x;	
		}
			
		if ((gyroAid.y + gyroAidOffset.y) > 180)
		{
			gyroAid.y =  -(360 - (gyroAid.y + gyroAidOffset.y));
		}
		else if ((gyroAid.y + gyroAidOffset.y) < -180)
		{
			gyroAid.y =  360 + (gyroAid.y + gyroAidOffset.y);
		}
		else
		{
			gyroAid.y = gyroAid.y + gyroAidOffset.y;	
		}
		
		
		if (invertRotation)
		{
		reportedRotation.y  =  - gyroAid.x;	
		reportedRotation.x  =  - gyroAid.y;	
		}
		else
		{
		reportedRotation.y  =  gyroAid.x;	
		reportedRotation.x  =  gyroAid.y;	
		}
		
		// The virtual gyro objects then display gyroAid as Yaw and Pitch
		// If calibrationSlerp is on, the gyro will move toward 0,0,0 instead of snapping to it
		
		if (calibrationSlerp)
		{
			gyroAidYaw.transform.localRotation = Quaternion.Slerp(gyroAidYaw.transform.localRotation,
													Quaternion.Euler(0,gyroAid.y,0),
													calibrationSlerpSpeed);
			
			gyroAidPitch.transform.localRotation = Quaternion.Slerp(gyroAidPitch.transform.localRotation,
													Quaternion.Euler(gyroAid.x,0,0),
													calibrationSlerpSpeed);
		}
		else
		{
		
			gyroAidYaw.transform.localRotation = Quaternion.Euler(0,gyroAid.y,0);
			
			gyroAidPitch.transform.localRotation = Quaternion.Euler(gyroAid.x,0,0);
		}
		
		if (!TouchManager.Instance.debugMode)
		{
			gimbal.renderer.enabled = false;
			gyroAidYaw.renderer.enabled = false;
			gyroAidPitch.renderer.enabled = false;
			
			
		}
		else
		{
			gimbal.renderer.enabled = true;
			gyroAidYaw.renderer.enabled = true;
			gyroAidPitch.renderer.enabled = true;
			
		}
		
		

	
	}
	
	void OnGUI () { 
		
		if (TouchManager.Instance.debugMode)
		{
	
			GUILayout.Label("Gyro Attitude " + Input.gyro.attitude);
			GUILayout.Label("Target Rotation " + targetRotation);
			GUILayout.Label("RotX: " + rotX);
			GUILayout.Label("RotY: " + rotY);
			GUILayout.Label("gyroAid.y " + gyroAid .y);
			GUILayout.Label("gyroAid.x " + gyroAid .x);
			GUILayout.Label("gyroAidOffset.y " + gyroAidOffset.y);
			GUILayout.Label("gyroAidOffset.x " + gyroAidOffset.x);
			GUILayout.Label("reportedRotation.y " + reportedRotation.y);
			GUILayout.Label("reportedRotation.x " + reportedRotation.x);
			
			if (GUILayout.Button("Gyro over Accelerometer? " + useGyro, GUILayout.Height(80)))
			{
				
				ToggleUseGyro();
				
			}
			
			if (GUILayout.Button("Calibate Pitch and Yaw", GUILayout.Height(80)))
			{
				
				CalibrateGyro();
				
			}
			
			if (GUILayout.Button("Calibration Lerp " + calibrationSlerp, GUILayout.Height(80)))
			{
				
				ToggleCalibrationSlerp();
				
			}
			
			if (GUILayout.Button("Invert Rotation " + invertRotation, GUILayout.Height(80)))
			{
				
				ToggleInvertRotation();
				
			}
			
		
		}
		
		
		
	}
	
	
	static Quaternion ConvertRotation(Quaternion q)
	{
		return new Quaternion(q.x, q.y, -q.z, -q.w);	
	}
	
	public void CalibrateGyro()
	{
		// Reseting the gyro to face forwards involves changing the offset value.
		// Through long trials and error I worked out
		// it's the difference between the current rotation and the offset
		// changed to a negative
		
		gyroAidOffset.y -= gyroAid.y;
			
		gyroAidOffset.x -= gyroAid.x;
	}
	
	private void ToggleCalibrationSlerp()
	{
		calibrationSlerp = !calibrationSlerp;
	}
	
	private void ToggleUseGyro()
	{
		useGyro = !useGyro;
	}
	
	private void ToggleInvertRotation()
	{
		invertRotation = !invertRotation;
	}
}
