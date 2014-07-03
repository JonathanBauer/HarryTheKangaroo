using UnityEngine;
using System.Collections;

public class SimpleMouseRotateAndShift : MonoBehaviour {

	public Camera cameraToShift;

	public Vector2 minRotate = new Vector2(0,0);
	public Vector2 maxRotate = new Vector2(0,0);
	public Vector2 minShift = new Vector2(0,0);
	public Vector2 maxShift = new Vector2(0,0);
	public float rotatePower = 0.1f;

	private float shiftZPosition = 0f;

	private Vector3 minRotation = new Vector3(0,0,0);
	private Vector3 maxRotation = new Vector3(0,0,0);

	private Vector3 minPosition = new Vector3(0,0,0);
	private Vector3 maxPosition = new Vector3(0,0,0);

	private Vector2 previousMousePos = new Vector2(0,0);
	private Vector2 currentMousePos = new Vector2(0,0);

	private Vector3 rotationLerp = new Vector3(0.5f,0.5f,0.5f);
	//private Vector3 positionLerp = new Vector3(0.5f,0.5f,0.5f);

	private Vector3 targetRotation = new Vector3(0,0,0);
	private Vector3 targetPosition = new Vector3(0,0,0);

	// Use this for initialization
	void Start () {

		// rotation
		minRotation = transform.localEulerAngles;
		minRotation.x = minRotation.x + minRotate.x;
		minRotation.y = minRotation.y + minRotate.y;

		maxRotation = transform.localEulerAngles;
		maxRotation.x = maxRotation.x + maxRotate.x;
		maxRotation.y = maxRotation.y + maxRotate.y;

		//position
		minPosition = cameraToShift.transform.localPosition;
		minPosition.x = minPosition.x + minShift.x;
		minPosition.z = minPosition.z + minShift.y;

		maxPosition = cameraToShift.transform.localPosition;
		maxPosition.x = maxPosition.x + maxShift.x;
		maxPosition.z = maxPosition.z + maxShift.y;

		//shiftZPosition = cameraToShift.transform.localPosition.z;

		currentMousePos = Input.mousePosition;
		previousMousePos = currentMousePos;

		//initialCameraPos = cameraToShift.transform.position;

		//targetPosition = initialCameraPos;
	
	}
	
	// Update is called once per frame
	void Update () {
		currentMousePos = Input.mousePosition;
		
		rotationLerp.x = rotationLerp.x + ((previousMousePos.x - currentMousePos.x) * rotatePower);
		rotationLerp.y = rotationLerp.y + ((previousMousePos.y - currentMousePos.y) * rotatePower);
		targetRotation.y = Mathf.Lerp(minRotation.x, maxRotation.x, rotationLerp.x); 
		targetRotation.x = Mathf.Lerp(minRotation.y, maxRotation.y, rotationLerp.y); 

		targetPosition.x = Mathf.Lerp(minPosition.x, maxPosition.x, rotationLerp.x); 
		targetPosition.z = Mathf.Lerp(minPosition.z, maxPosition.z, rotationLerp.x); 
		//targetPosition.z = shiftZPosition;


		transform.localEulerAngles = targetRotation; 
		cameraToShift.transform.localPosition = targetPosition;
		
		previousMousePos = currentMousePos;

		Debug.Log (rotationLerp.x);
	}
}
