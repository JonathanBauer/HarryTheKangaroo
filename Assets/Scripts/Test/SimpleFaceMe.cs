using UnityEngine;
using System.Collections;

public class SimpleFaceMe : MonoBehaviour {

	public Transform faceMeTarget;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.LookAt(faceMeTarget, Vector3.up);

	
	}
}
