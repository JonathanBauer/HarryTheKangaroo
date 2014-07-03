using UnityEngine;
using System.Collections;

public class SimpleMouseRotate : MonoBehaviour {

	public float trackingSpeed = 1f;
	

	private Vector3 targetRotation = new Vector3(0,0,0);

	private Vector2 previousMousePos = new Vector2(0,0);
	private Vector2 currentMousePos = new Vector2(0,0);
	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


		currentMousePos = Input.mousePosition;

		// The Y rotation of the Orbit Center controls the longitudinal orbit of the camera
		targetRotation.y = targetRotation.y + (previousMousePos.x - currentMousePos.x);
		targetRotation.y = Mathf.Clamp(targetRotation.y, -30, 30);

		// The X rotation of the Orbit Center controls the latitudinal orbit of the camera
		targetRotation.x = targetRotation.x - (previousMousePos.y - currentMousePos.y);
		targetRotation.x = Mathf.Clamp(targetRotation.x, -10, 30);

		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetRotation), trackingSpeed); 

		previousMousePos = currentMousePos;

	
	}
}
