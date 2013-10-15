// ***********************************************************
// Written by Heyworks Unity Studio http://unity.heyworks.com/
// ***********************************************************
using UnityEngine;

/// <summary>
/// Gyroscope controller that works with any device orientation.
/// </summary>
public class GyroController_new : MonoBehaviour 
{
	

	bool gyroEnabled = true;
	const float lowPassFilterFactor = 0.2f;

	Quaternion baseIdentity =  Quaternion.Euler(90, 0, 0);
	Quaternion landscapeRight =  Quaternion.Euler(0, 0, 90);
	Quaternion landscapeLeft =  Quaternion.Euler(0, 0, -90);
	Quaternion upsideDown =  Quaternion.Euler(0, 0, 180);
	
	Quaternion cameraBase =  Quaternion.identity;
	Quaternion calibration =  Quaternion.identity;
	Quaternion baseOrientation =  Quaternion.Euler(90, 0, 0);
	Quaternion baseOrientationRotationFix =  Quaternion.identity;

	Quaternion referanceRotation = Quaternion.identity;
	bool debug = true;

	
	void Start () 
	{
		AttachGyro();
	}

	void Update() 
	{
		
		if (!gyroEnabled)
			return;
		transform.rotation = Quaternion.Slerp(transform.rotation,
			cameraBase * ( ConvertRotation(referanceRotation * Input.gyro.attitude) * GetRotFix()), lowPassFilterFactor);
	}

	void OnGUI()
	{
		if (!debug)
			return;

		GUILayout.Label("Orientation: " + Screen.orientation);
		GUILayout.Label("Calibration: " + calibration);
		GUILayout.Label("Camera base: " + cameraBase);
		GUILayout.Label("input.gyro.attitude: " + Input.gyro.attitude);
		GUILayout.Label("transform.rotation: " + transform.rotation);

		if (GUILayout.Button("On/off gyro: " + Input.gyro.enabled, GUILayout.Height(100)))
		{
			Input.gyro.enabled = !Input.gyro.enabled;
		}

		if (GUILayout.Button("On/off gyro controller: " + gyroEnabled, GUILayout.Height(100)))
		{
			if (gyroEnabled)
			{
				DetachGyro();
			}
			else
			{
				AttachGyro();
			}
		}

		if (GUILayout.Button("Update gyro calibration (Horizontal only)", GUILayout.Height(80)))
		{
			UpdateCalibration(true);
		}

		if (GUILayout.Button("Update camera base rotation (Horizontal only)", GUILayout.Height(80)))
		{
			UpdateCameraBaseRotation(true);
		}

		if (GUILayout.Button("Reset base orientation", GUILayout.Height(80)))
		{
			ResetBaseOrientation();
		}

		if (GUILayout.Button("Reset camera rotation", GUILayout.Height(80)))
		{
			transform.rotation = Quaternion.identity;
		}
	}

	



	/// <summary>
	/// Attaches gyro controller to the transform.
	/// </summary>
	void AttachGyro()
	{
		gyroEnabled = true;
		ResetBaseOrientation();
		UpdateCalibration(true);
		UpdateCameraBaseRotation(true);
		RecalculateReferenceRotation();
	}

	/// <summary>
	/// Detaches gyro controller from the transform
	/// </summary>
	void DetachGyro()
	{
		gyroEnabled = false;
	}

	

	

	/// <summary>
	/// Update the gyro calibration.
	/// </summary>
	void UpdateCalibration(bool onlyHorizontal)
	{
		if (onlyHorizontal)
		{
			var fw = (Input.gyro.attitude) * (-Vector3.forward);
			fw.z = 0;
			if (fw == Vector3.zero)
			{
				calibration = Quaternion.identity;
			}
			else
			{
				calibration = (Quaternion.FromToRotation(baseOrientationRotationFix * Vector3.up, fw));
			}
		}
		else
		{
			calibration = Input.gyro.attitude;
		}
	}
	
	/// <summary>
	/// Update the camera base rotation.
	/// </summary>
	/// <param name='onlyHorizontal'>
	/// Only y rotation.
	/// </param>
	void UpdateCameraBaseRotation(bool onlyHorizontal)
	{
		if (onlyHorizontal)
		{
			var fw = transform.forward;
			fw.y = 0;
			if (fw == Vector3.zero)
			{
				cameraBase = Quaternion.identity;
			}
			else
			{
				cameraBase = Quaternion.FromToRotation(Vector3.forward, fw);
			}
		}
		else
		{
			cameraBase = transform.rotation;
		}
	}
	
	/// <summary>
	/// Converts the rotation from right handed to left handed.
	/// </summary>
	/// <returns>
	/// The result rotation.
	/// </returns>
	/// <param name='q'>
	/// The rotation to convert.
	/// </param>
	static Quaternion ConvertRotation(Quaternion q)
	{
		return new Quaternion(q.x, q.y, -q.z, -q.w);	
	}
	
	/// <summary>
	/// Gets the rot fix for different orientations.
	/// </summary>
	/// <returns>
	/// The rot fix.
	/// </returns>
	 Quaternion GetRotFix()
	{

		return Quaternion.identity;
	}
	
	/// <summary>
	/// Recalculates reference system.
	/// </summary>
	void ResetBaseOrientation()
	{
		baseOrientationRotationFix = GetRotFix();
		baseOrientation = baseOrientationRotationFix * baseIdentity;
	}

	/// <summary>
	/// Recalculates reference rotation.
	/// </summary>
	void RecalculateReferenceRotation()
	{
		referanceRotation = Quaternion.Inverse(baseOrientation)*Quaternion.Inverse(calibration);
	}

	
}
