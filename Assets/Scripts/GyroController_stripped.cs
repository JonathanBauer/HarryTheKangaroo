// ***********************************************************
// Written by Heyworks Unity Studio http://unity.heyworks.com/
// ***********************************************************
using UnityEngine;

/// <summary>
/// Gyroscope controller that works with any device orientation.
/// </summary>
public class GyroController_stripped : MonoBehaviour 
{
	



	//Quaternion landscapeRight =  Quaternion.Euler(0, 0, 90);
	//Quaternion landscapeLeft =  Quaternion.Euler(0, 0, -90);
	//Quaternion upsideDown =  Quaternion.Euler(0, 0, 180);
	

	
	

	
	bool debug = true;

	

	void Update() 
	{

		transform.rotation = Quaternion.Slerp(transform.rotation,ConvertRotation(Quaternion.Euler (-90,0,0) * Input.gyro.attitude), 0.2f);
	}

	void OnGUI()
	{
		if (!debug)
			return;

		GUILayout.Label("Orientation: " + Screen.orientation);
		GUILayout.Label("input.gyro.attitude: " + Input.gyro.attitude);
		GUILayout.Label("transform.rotation: " + transform.rotation);
	
	}

	


	static Quaternion ConvertRotation(Quaternion q)
	{
		return new Quaternion(q.x, q.y, -q.z, -q.w);	
	}
	
	
	
}
