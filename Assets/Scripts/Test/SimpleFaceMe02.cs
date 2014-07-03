using UnityEngine;
using System.Collections;

public class SimpleFaceMe02 : MonoBehaviour {

	public Camera cameraTarget;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {

		transform.rotation = cameraTarget.transform.rotation;
	
	}
}
