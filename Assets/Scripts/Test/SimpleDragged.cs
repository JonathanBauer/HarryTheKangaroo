using UnityEngine;
using System.Collections;

public class SimpleDragged : MonoBehaviour {

	public enum dragState
	{
		FollowingFinger,
		IsReleased,
		IsReturning
			
	}

	public dragState thisDragState;

	public Vector3 initialPosition = new Vector3 (0,0,0);

	public Vector3 returnVector = new Vector3 (0,0,0);

	public float distanceBack = 15f;

	public float returnSpeed = 0.01f;

	public float destroyDistance = 0.3f;
	

	public Camera cam;

	public SimpleDragDropControl dragDropControl;

	void Awake () {

		dragDropControl = FindObjectOfType(typeof(SimpleDragDropControl)) as SimpleDragDropControl;

		if (dragDropControl)
		{
			cam = dragDropControl.cam;
		
		}



	}
	

	// Use this for initialization
	void Start () {

		initialPosition = transform.position;

		thisDragState = dragState.FollowingFinger;

	
	}
	
	// Update is called once per frame
	void Update () {

		if (thisDragState == dragState.IsReleased)
		{
			returnVector = transform.position - initialPosition;

			returnVector = returnVector.normalized;

			thisDragState = dragState.IsReturning;

			Debug.Log (this + " is released");

		}

		if (thisDragState == dragState.IsReturning)
		{
			transform.position = transform.position - (returnVector *  returnSpeed);

			float dist = Vector3.Distance(initialPosition, transform.position);

			Debug.Log (dist);

			if (dist < destroyDistance)
			{
				Debug.Log (this + " is destroyed");

				Destroy (gameObject);

			}
			
		}

		if (thisDragState == dragState.FollowingFinger)
		{

			Vector3 objectPosition = new Vector3 (0,0,0);

			objectPosition.x = Input.mousePosition.x;
			objectPosition.y = Input.mousePosition.y;
			objectPosition.z = distanceBack;
			
			transform.position = cam.ScreenToWorldPoint(objectPosition);

		}

	

	}


}
