using UnityEngine;
using System.Collections;

public class InteractionControl : MonoBehaviour {

	public bool debugMode = false;

	public int eBookPageNumber = 2;
	
	public Camera interactionCamera;
	
	public Transform shoeDragPickUp;	// The shoe on the ground
	
	public Transform shoeDragged;		// The shoe that moves during drag, disabled on start
	
	public Transform shoeDragDrop;		// The hook that the shoe goes to
	
	private GameObjectMode shoeOriginState;	// The control state of the shoe on the ground
	
	private SimpleDragged shoeDraggedState;		// The control state of the dragged object
	
	private GameObjectMode shoeDragDropState;	// The control state of the hook
	
	private TouchManager touchManager;
	
	public float distanceFromCamera = 1f;
	
	public float returnSpeed = 4f;
	
	// Use this for initialization
	void Start () {

		if (interactionCamera)
		{
			interactionCamera.enabled = false;
		} 
		
		if (shoeDragged)
		{
			shoeDraggedState = shoeDragged.GetComponent<SimpleDragged>();
			
			shoeDragged.gameObject.SetActive(false);
			
			
		} else {
			
			Debug.Log ("There is no shoe drag object");
		}
		
		if (shoeDragDrop)
		{
			shoeDragDropState = shoeDragDrop.GetComponent<GameObjectMode>();
			
			
		} else {
			
			Debug.Log ("There is no shoe drag drop object");
		}
		
		if (shoeDragPickUp)
		{
			shoeOriginState = shoeDragPickUp.GetComponent<GameObjectMode>();
			
			
		} else {
			
			Debug.Log ("There is no shoe origin object");
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

		/*
		
		if (shoeDraggedState.thisDragState == SimpleDragged.dragState.Returned)
		{
			if (debugMode)
				Debug.Log("Shoe returned");
			
			shoeOriginState.thisConnectState = GameObjectMode.ConnectState.DragObjOriginReturned;
			
			//shoeDragPickUp.renderer.enabled = true;
			
			shoeDraggedState.thisDragState = SimpleDragged.dragState.InActive;
			
			shoeDragged.gameObject.SetActive(false);
			
		}
		
		
		if (shoeDraggedState.thisDragState == SimpleDragged.dragState.Collected)
		{
			shoeDragDropState.thisConnectState = GameObjectMode.ConnectState.DragObjTargetConnectionMade;
			
			shoeDraggedState.thisDragState = SimpleDragged.dragState.InActive;
			
			shoeDragged.gameObject.SetActive(false);
			
		}
		*/
		
	}
	
	// LiftUpDragObject is called by TouchManager
	public void LiftUpDragObject ( GameObject target )
	{
		Debug.Log("Object Lifted");
		
		shoeDragged.transform.position = shoeDragPickUp.transform.position;
		
		shoeDraggedState.thisDragState = SimpleDragged.dragState.FollowingFinger;
		
		shoeDraggedState.initialPosition = shoeDragPickUp.transform.position;
		
		shoeDraggedState.destinationPosition = shoeDragDropState.transform.position;
		
		shoeDraggedState.cam = interactionCamera;
		
		shoeDraggedState.distanceFromCamera = distanceFromCamera;
		
		shoeDraggedState.returnSpeed = returnSpeed;
		
		shoeOriginState.thisConnectState = GameObjectMode.ConnectState.DragObjOriginLifted;
		
		//shoeOriginState.thisConnectState = GameObjectMode.ConnectState.DragObjOriginReturned;
		
		shoeDragged.gameObject.SetActive(true);
		
		//shoeDragPickUp.renderer.enabled = false;
		
	}
	
	// DropDragObject is called by TouchManager
	public void DropDragObject ( GameObject target )
	{
		if (shoeDraggedState.thisDragState == SimpleDragged.dragState.FollowingFinger)
		{
			Debug.Log("Object Dropped");
			
			shoeDraggedState.thisDragState = SimpleDragged.dragState.IsReleased;
		}
	}
}
