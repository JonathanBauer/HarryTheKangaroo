using UnityEngine;
using System.Collections;

public class CameraGravityTest : MonoBehaviour {
	
	public Quaternion baseRotation = Quaternion.identity;
	Quaternion targetRotation = Quaternion.identity;
	public Vector2 rotationSpeed = new Vector2(90,90);
	public Vector2 rotationOffset = new Vector2(0,0);
	public Vector2 minRotationBounds = new Vector2(-90,-90);
	public Vector2 maxRotationBounds = new Vector2(90,90);
	public Vector2 keyboardRotSpeed = new Vector2(5,5);
	
	public float trackingSpeed = 1.0f;
	
	private float rotX = 0.0f;
	private float rotY = 0.0f;
	
	private bool onGyro;
	
	void Start () {
		onGyro = SystemInfo.supportsGyroscope;
	}


	
	// Update is called once per frame
	void Update () {
		if (onGyro)
		{
			rotY = Input.gyro.gravity.y * rotationSpeed.y;
			rotX = Input.gyro.gravity.x * rotationSpeed.x;
		}
		
		if (rotX < minRotationBounds.x) {
			rotX = minRotationBounds.x;
		}
		if (rotX > maxRotationBounds.x) {
			rotX = maxRotationBounds.x;
		}
		
		if (rotY < minRotationBounds.y) {
			rotY = minRotationBounds.y;
		}
		if (rotY > maxRotationBounds.y) {
			rotY = maxRotationBounds.y;
		}
		
		if (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown){

			rotationOffset.y = 45f;
			rotationOffset.x = 0f;
			minRotationBounds.y = -65f;
			minRotationBounds.x = -20f;
			maxRotationBounds.y = -25f;
			maxRotationBounds.x = 20f;
		}
		else
		{
			rotationOffset.y = 10f;
			rotationOffset.x = 0f;
			minRotationBounds.y = -30f;
			minRotationBounds.x = -20f;
			maxRotationBounds.y = 10f;
			maxRotationBounds.x = 20f;
			
		}
		
		
		targetRotation = Quaternion.Euler((rotY + rotationOffset.y),(rotX + rotationOffset.x), 0);
		
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, trackingSpeed); 
		
		//Debug.Log ((transform.rotation).eulerAngles);
		//Debug.Log (Screen.orientation);
	
	}
	
	public void RotateUp ()
	{
		Debug.Log("Rotating Up");
		rotY = rotY - keyboardRotSpeed.y;
	}
	
	public void RotateDown ()
	{
		Debug.Log("Rotating Down");
		rotY = rotY + keyboardRotSpeed.y;
	}

	public void RotateLeft ()
	{
		Debug.Log("Rotating Left");
		rotX = rotX + keyboardRotSpeed.x;
	}
	
	public void RotateRight ()
	{
		Debug.Log("Rotating Right");
		rotX = rotX - keyboardRotSpeed.x;
	}
}
