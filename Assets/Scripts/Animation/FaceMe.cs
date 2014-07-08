using UnityEngine;
using System.Collections;

public class FaceMe : MonoBehaviour {

	public Camera cameraTarget;

	// Use this for initialization
	void Start () {
		if (!cameraTarget)
			Debug.Log(this.name + " is using FaceMe without a camera.");
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (cameraTarget)
			transform.rotation = cameraTarget.transform.rotation;
	
	}
}
