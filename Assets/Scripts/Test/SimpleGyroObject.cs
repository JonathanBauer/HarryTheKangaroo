using UnityEngine;
using System.Collections;

public class SimpleGyroObject : MonoBehaviour {

	private bool onGyro = true;
	private bool adjust = false;

	private bool lookingAround = false;
	
	private Quaternion targetRotation;

	public GUIText lookingAroundPrompt;
	public GUITexture lookAroundTexture;
	public GUITexture centerTexture;

	private Quaternion adjustedRotation = Quaternion.identity;
	private Quaternion adjustmentConstant = Quaternion.identity;


	public enum GyroList
	{
		PressToLook,
		PressToCalibrate
	}

	public GyroList gyroMode = GyroList.PressToLook;


	// Use this for initialization
	void Start () {

		onGyro = SystemInfo.supportsGyroscope;
		lookingAroundPrompt.guiText.enabled = false;
		centerTexture.guiTexture.enabled = false;



	
	}
	
	// Update is called once per frame
	void Update () {

		if (onGyro)
		{
			/* If the camera with this script applied has 0,0,0 rotation, the camera when the app runs on the iphone will
			 * have the same orientation when the device starts placed flat on a tabletop */
			targetRotation = ConvertRotation (Input.gyro.attitude);


			transform.rotation = targetRotation * adjustedRotation * adjustmentConstant;
		}
	
	}

	void OnGUI () {

		if (GUILayout.Button ("Adjustment Constant "+adjust, GUILayout.Height (80)))
		{
			if (adjust)
			{
				adjustmentConstant = Quaternion.identity;
				adjust = false;


			} else {

				adjustmentConstant = Quaternion.Euler (0,0,-90);
				adjust = true;

			}

		
		}

		if (GUILayout.Button ("Gyro Mode "+gyroMode, GUILayout.Height (80)))
		{
		
			if (gyroMode == GyroList.PressToLook)
			{
				gyroMode = GyroList.PressToCalibrate;
				lookAroundTexture.guiTexture.enabled = false;
				centerTexture.guiTexture.enabled = true;

			} else {

				gyroMode = GyroList.PressToLook;
				lookAroundTexture.guiTexture.enabled = true;
				centerTexture.guiTexture.enabled = false;
			}

		

		}

		if (gyroMode == GyroList.PressToLook)
		{

			if (Input.touchCount > 0  && lookAroundTexture.HitTest(new Vector2 (Input.GetTouch(0).position.x,Input.GetTouch(0).position.y)))
			{
				lookingAroundPrompt.guiText.enabled = true;
				
			} else {
				
				lookingAroundPrompt.guiText.enabled = false;
				
			}
	

			if (Input.GetMouseButton(0) && lookAroundTexture.HitTest(new Vector2 (Input.mousePosition.x,Input.mousePosition.y)))
			{
				lookingAroundPrompt.guiText.enabled = true;

			} else {

				lookingAroundPrompt.guiText.enabled = false;

			}
		
		}

	

		if (gyroMode == GyroList.PressToCalibrate)
		{

			if (Input.touchCount > 0  && centerTexture.HitTest(new Vector2 (Input.GetTouch(0).position.x,Input.GetTouch(0).position.y)))
			{
				adjustedRotation = Quaternion.Inverse(targetRotation);
				
			} 
		}

	}



	static Quaternion ConvertRotation(Quaternion q)
	{
		/* The basic way to see what Input.gyro.attitude does is to attach this script to a camera in a full 3D environment
		 * transform.rotation = Input.gyro.attitude
		 * This on it's own will make the camera rotate in the wrong direction to how the device is being rotated.
		 * To fix this, the ConvertRotation function is applied */
		return new Quaternion (q.x, q.y, -q.z, -q.w);
	}
}
