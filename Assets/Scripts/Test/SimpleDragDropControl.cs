using UnityEngine;
using System.Collections;

public class SimpleDragDropControl : MonoBehaviour {

	public bool debugMode = false;

	public Camera cam;

	public Transform shoeDragPickUp;	// The shoe on the ground

	public Transform shoeDragged;		// The shoe that moves during drag, disabled on start

	public Transform shoeDragDrop;		// The hook that the shoe goes to

	private SimpleDragged shoeDraggedState;		// The control state of the dragged object

	private SimpleDragDrop shoeDragDropState;	// The control state of the hook

	private TouchManager touchManager;

	public float enableDistance = 5f;	// How close the shoe must be to the hook to be collected

	// Use this for initialization
	void Start () {

		if (shoeDragged)
		{
			shoeDraggedState = shoeDragged.GetComponent<SimpleDragged>();

			shoeDragged.gameObject.SetActive(false);

		} else {

			Debug.Log ("There is no shoe drag object");
		}

		if (shoeDragDrop)
		{
			shoeDragDropState = shoeDragDrop.GetComponent<SimpleDragDrop>();

			
		} else {
			
			Debug.Log ("There is no shoe drag drop object");
		}

		GameObject eBookManager = GameObject.Find("eBookManager");

		if(!eBookManager)
		{
			Debug.Log("There is no EbookManager in the scene");
		} else {

			touchManager = eBookManager.GetComponent<TouchManager>();

		}

		if(!touchManager)
		{
			Debug.Log("There is no EbookManager with a TouchManager component in the scene");
		}


	}
	
	// Update is called once per frame
	void Update () {

		if (shoeDragPickUp.renderer.enabled == false && shoeDraggedState.thisDragState == SimpleDragged.dragState.InActive)
		{
			if (debugMode)
				Debug.Log("Shoe returned");
				
			shoeDragPickUp.renderer.enabled = true;

			shoeDragged.gameObject.SetActive(false);
				
		}

		float dist = Vector3.Distance(shoeDragged.transform.position, shoeDragDrop.transform.position);
		
		if (dist < enableDistance && shoeDragDropState.thisConnectState == SimpleDragDrop.connectState.NotConnected)
		{
			shoeDragDropState.thisConnectState = SimpleDragDrop.connectState.ConnectionMade;

			shoeDragged.gameObject.SetActive(false);
		}
	
	}

	// LiftUpDragObject is called by TouchManager
	public void LiftUpDragObject ( GameObject target )
	{
		Debug.Log("Object Lifted");

		shoeDragged.transform.position = shoeDragPickUp.transform.position;
		shoeDraggedState.initialPosition = shoeDragPickUp.transform.position;

		shoeDraggedState.thisDragState = SimpleDragged.dragState.FollowingFinger;
		
		shoeDragged.gameObject.SetActive(true);
		
		shoeDragPickUp.renderer.enabled = false;

	}

	// DropDragObject is called by TouchManager
	public void DropDragObject ( GameObject target )
	{
		Debug.Log("Object Dropped");
		
		shoeDraggedState.thisDragState = SimpleDragged.dragState.IsReleased;
	}
}
