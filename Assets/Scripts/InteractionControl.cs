using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteractionControl : MonoBehaviour {

	public bool debugMode = false;

	public int eBookPageNumber = 2;		// Which eBook page this InteractionControl is intended for
	
	public Camera interactionCamera;	// The camera the InteractionControl will use

	public List<GameObjectMode> dragObjectOrigins = new List<GameObjectMode>();	// All the Drag Object Origins used by this InteractionControl

	public List<DragObject> dragObjects = new List<DragObject>();	// All Drag Objects found as children of the Drag Object Origins

	public GameObjectMode dragObjectTarget;			// The Drag Object Target that all Drag Objects can be dragged into
	
	public float distanceFromCamera = 1f;			// How far all Drag Objects will be from the camera when they are dragged
	
	public float returnSpeed = 4f;					// The speed that drag objects move to return to their origins
	
	// Use this for initialization
	void Start () {

		if (interactionCamera)
		{
			interactionCamera.enabled = false;		// The main orthographic camera will only work when all other cameras are disabled
		} 

		if (dragObjectOrigins.Count == 0)
		{
			if (debugMode)
				Debug.Log (this.name + " has no drag object origins");
		} 

		for (int i=0; i < dragObjectOrigins.Count; i++)
		{
			if (dragObjectOrigins[i])
			{

				if (dragObjectOrigins[i].gameObjectMode == GameObjectMode.ModeList.DragObjectOrigin)
				{
					// Set the ConnectState to the initial state for Drag Object Origins
					dragObjectOrigins[i].thisConnectState = GameObjectMode.ConnectState.DragObjOriginPresent;

					// Check if any children have the Drag Object component attached
					DragObject dragObj = dragObjectOrigins[i].GetComponentInChildren<DragObject>();

					if (dragObj)
					{
						// Collision on Drag Objects prevents the physics raycast striking the Drag Object Origin
						if (dragObj.collider)
						{
							if (debugMode)
								Debug.Log (dragObj + " has collision attached. Remove to allow correct detection.");
						}
						// if this had a Drag Object component, add it to the Drag Object list for this Drag Object Origin
						dragObjects.Add(dragObj);

						// disable this Drag Object to be activated on drag
						dragObjects[i].gameObject.SetActive(false);

					} else {

						if (debugMode)
							Debug.Log (this.dragObjectOrigins[i] + " has no DragObject child");

					}

					
				} else {

					if (debugMode)
						Debug.Log (this.name + " has a GameObjectMode that is not DragObjectOrigin");

				}


			} else {

				if (debugMode)
					Debug.Log (this.name + " has an invalid entry at index "+i);
				dragObjectOrigins.RemoveAt(i);
			}
		}
		

		
		if (dragObjectTarget)
		{
			if (dragObjectTarget.gameObjectMode == GameObjectMode.ModeList.DragObjectTarget)
			{
				dragObjectTarget.thisConnectState = GameObjectMode.ConnectState.DragObjTargetNotConnected;

			} else {

				if (debugMode)
					Debug.Log (this.name + " has a GameObjectMode that is not DragObjectTarget");
			}

		} else {

			if (debugMode)
				Debug.Log (this.name + " has no drag target");
		}

		
	}
	
	// Update is called once per frame
	void Update () {

		for (int i=0; i < dragObjects.Count; i++)
		{
			if (dragObjects[i].thisDragState == DragObject.dragState.Returned)
			{
				if (debugMode)
					Debug.Log(dragObjectOrigins[i] + " returned");
				
				dragObjectOrigins[i].thisConnectState = GameObjectMode.ConnectState.DragObjOriginReturned;
				
				dragObjects[i].thisDragState = DragObject.dragState.InActive;
				
				dragObjects[i].gameObject.SetActive(false);
				
			}

			if (dragObjects[i].thisDragState == DragObject.dragState.Collected)
			{
				if (debugMode)
					Debug.Log(dragObjectOrigins[i] + " collected");

				// Conditions for other connection states for dragObjectTarget go here

				dragObjectTarget.thisConnectState = GameObjectMode.ConnectState.DragObjTargetConnectionMade;
				
				dragObjects[i].thisDragState = DragObject.dragState.InActive;
				
				dragObjects[i].gameObject.SetActive(false);
				
			}
		}


	}
	
	// LiftUpDragObject is called by TouchManager
	public void LiftUpDragObject ( GameObject target )
	{

		for (int i=0; i < dragObjectOrigins.Count; i++)
		{
			if (dragObjectOrigins[i].gameObject == target)
			{
				if (debugMode)
					Debug.Log(dragObjectOrigins[i] + " has been Lifted");

				dragObjects[i].transform.position = dragObjectOrigins[i].transform.position;

				dragObjects[i].thisDragState = DragObject.dragState.FollowingFinger;

				dragObjects[i].initialPosition = dragObjectOrigins[i].transform.position;

				dragObjects[i].destinationPosition = dragObjectTarget.transform.position;

				dragObjects[i].cam = interactionCamera;

				dragObjects[i].distanceFromCamera = distanceFromCamera;

				dragObjects[i].returnSpeed = returnSpeed;

				dragObjectOrigins[i].thisConnectState = GameObjectMode.ConnectState.DragObjOriginLifted;

				dragObjects[i].gameObject.SetActive(true);

			}
		}

	}
	
	// DropDragObject is called by TouchManager
	public void DropDragObject ( GameObject target )
	{
		for (int i=0; i < dragObjectOrigins.Count; i++)
		{
			if (dragObjectOrigins[i].gameObject == target)
			{
				if (dragObjects[i].thisDragState == DragObject.dragState.FollowingFinger)
				{
					if (debugMode)
						Debug.Log("Object Dropped");
					
					dragObjects[i].thisDragState = DragObject.dragState.IsReleased;
				}
			}
		}

	}

	public void ResetPuzzle () {

		for (int i=0; i < dragObjectOrigins.Count; i++)
		{
			dragObjectOrigins[i].thisConnectState = GameObjectMode.ConnectState.DragObjOriginReturned;
		}

		if (dragObjectTarget)
			dragObjectTarget.thisConnectState = GameObjectMode.ConnectState.DragObjTargetNotConnected;

	}
}
