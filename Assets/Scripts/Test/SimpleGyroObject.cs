using UnityEngine;
using System.Collections;

public class SimpleGyroObject : MonoBehaviour {

	private bool onGyro = true;

	// Use this for initialization
	void Start () {

		onGyro = SystemInfo.supportsGyroscope;
	
	}
	
	// Update is called once per frame
	void Update () {

		if (onGyro)
		{
			transform.rotation = Input.gyro.attitude;
		}
	
	}
}
