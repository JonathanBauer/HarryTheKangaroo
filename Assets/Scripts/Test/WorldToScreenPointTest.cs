using UnityEngine;
using System.Collections;

public class WorldToScreenPointTest : MonoBehaviour {

	public Camera perspectiveCamera;
	
	public Transform firstTarget;

	private Vector2 firstTargetScreenPos = new Vector2 (0,0);

	public Transform secondTarget;

	private Vector2 secondTargetScreenPos = new Vector2 (0,0);

	public float trackingSpeed = 0.2f;

	private float tracking = 0f;

	public bool isReturning = false;

	public float distanceBack = 15f;

	// Use this for initialization
	void Start () {

	
	
	}
	
	// Update is called once per frame
	void Update () {

		// Calculate targets' screen co-ordinates from world co-ordinates
		firstTargetScreenPos = perspectiveCamera.WorldToScreenPoint(firstTarget.transform.position);
		
		secondTargetScreenPos = perspectiveCamera.WorldToScreenPoint(secondTarget.transform.position);


		Vector3 objectPosition = new Vector3 (0,0,0);

		// Calculate moving object's screen co-ordinates from target screen co-ordinates
		objectPosition.x = Mathf.Lerp(firstTargetScreenPos.x, secondTargetScreenPos.x, tracking);
		objectPosition.y = Mathf.Lerp(firstTargetScreenPos.y, secondTargetScreenPos.y, tracking);
		objectPosition.z = distanceBack;
			
		//trackingObject.transform.position = orthogonalCamera.ScreenToWorldPoint(objectPosition);

		// Calculate moving object's world co-ordinates from calculated screen co-ordinates
		transform.position = perspectiveCamera.ScreenToWorldPoint(objectPosition);

		if (isReturning == false)
		{
			Debug.Log(tracking);
			tracking += trackingSpeed;

			if (tracking > 1)
				isReturning = true;
		} else {

			tracking -= trackingSpeed;
			
			if (tracking < 0)
				isReturning = false;

		}



	
	}
}
