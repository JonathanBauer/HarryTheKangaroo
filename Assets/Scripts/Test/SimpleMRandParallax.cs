using UnityEngine;
using System.Collections;

public class SimpleMRandParallax : MonoBehaviour {

	public Transform foreGroundObject;
	public Transform backGroundObject;

	private Vector3 currentFGPos = new Vector3(0,0);
	private Vector3 currentBGPos = new Vector3(0,0);

	public Vector2 foreGroundSpeed = new Vector3(1,1);
	public Vector2 backGroundSpeed = new Vector3(1,1);

	private Vector3 previousFGPos = new Vector3(0,0);
	private Vector3 previousBGPos = new Vector3(0,0);
	
	public float trackingSpeed = 1f;
	
	
	private Vector3 targetRotation = new Vector3(0,0,0);
	private Vector3 trueRotation = new Vector3(0,0,0);
	
	private Vector2 previousMousePos = new Vector2(0,0);
	private Vector2 currentMousePos = new Vector2(0,0);
	


	// Use this for initialization
	void Start () {

		currentFGPos = foreGroundObject.position;
		currentBGPos = backGroundObject.position;
		previousFGPos = foreGroundObject.position;
		previousBGPos = backGroundObject.position;
	
	}
	
	// Update is called once per frame
	void Update () {

		
		currentMousePos = Input.mousePosition;
		
		// The Y rotation of the Orbit Center controls the longitudinal orbit of the camera
		targetRotation.y = targetRotation.y + (previousMousePos.x - currentMousePos.x);

		targetRotation.y = Mathf.Clamp(targetRotation.y, -30, 30);

	
		
		// The X rotation of the Orbit Center controls the latitudinal orbit of the camera
		targetRotation.x = targetRotation.x - (previousMousePos.y - currentMousePos.y);
		targetRotation.x = Mathf.Clamp(targetRotation.x, -10, 0);

		currentFGPos.x = previousFGPos.x + (targetRotation.y * foreGroundSpeed.x);
		currentFGPos.y = previousFGPos.y - (targetRotation.x * foreGroundSpeed.y);

		foreGroundObject.localPosition = currentFGPos;

		currentBGPos.x = previousBGPos.x + (targetRotation.y * backGroundSpeed.x);
		currentBGPos.y = previousBGPos.y - (targetRotation.x * backGroundSpeed.y);
		
		backGroundObject.localPosition = currentBGPos;

		
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetRotation), trackingSpeed); 
		
		previousMousePos = currentMousePos;
		

	
	}
}
