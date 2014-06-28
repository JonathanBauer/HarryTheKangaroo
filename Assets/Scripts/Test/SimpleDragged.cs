using UnityEngine;
using System.Collections;

public class SimpleDragged : MonoBehaviour {

	public enum dragState
	{
		InActive,
		FollowingFinger,
		IsReleased,
		IsReturning

	}

	public dragState thisDragState = dragState.InActive;

	public Vector3 initialPosition = new Vector3 (0,0,0);

	private Vector3 returnVector = new Vector3 (0,0,0);

	public float distanceBack = 15f;

	public float returnSpeed = 4f;

	public float disableDistance = 5f;

	private Camera cam;

	private SimpleDragDropControl dragDropControl;

	
	// Use this for initialization
	void Start () {

		dragDropControl = FindObjectOfType(typeof(SimpleDragDropControl)) as SimpleDragDropControl;
		
		if (dragDropControl)
		{
			cam = dragDropControl.cam;
			
		}

		initialPosition = transform.position;

		thisDragState = dragState.InActive;

	
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

			if (dist < disableDistance)
			{
				thisDragState = dragState.InActive;

			}
			
		}

		if (thisDragState == dragState.FollowingFinger)
		{

			Vector3 objectPosition = new Vector3 (0,0,0);

			objectPosition.x = TouchManager.Instance.fingerRecordedPosition.x;
			objectPosition.y = TouchManager.Instance.fingerRecordedPosition.y;
			objectPosition.z = distanceBack;

			transform.position = cam.ScreenToWorldPoint(objectPosition);

		}

	

	}


}
