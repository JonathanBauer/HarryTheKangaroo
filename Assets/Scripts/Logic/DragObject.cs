using UnityEngine;
using System.Collections;

public class DragObject : MonoBehaviour {

	// All public variables are defined by the controller except returnRange and debugMode

	public bool debugMode = false;

	public enum dragState
	{
		InActive,
		FollowingFinger,
		IsReleased,
		IsReturning,
		Returned,
		Collected

	}
	public dragState thisDragState = dragState.InActive;		// The behaviour of the dragged object is defined by states

	public Camera cam;						// The perspective or orthographic camera used to determine the object's position when dragging

	public Vector3 initialPosition = new Vector3 (0,0,0);		// Where the dragged object was activated. Also where it returns

	public Vector3 destinationPosition = new Vector3 (0,0,0);	// Where the dragged object needs to go to become "Collected"

	public float distanceFromCamera = 1f;		// How far the dragged object moves in front of the camera.

	public float returnSpeed = 4f;			// How fast the dragged object returns to initial position when released.

	public float returnRange = 5f;			// The distance the dragged object must get inside for it to become "Returned"
	
	
	void Update () {

		if (thisDragState == dragState.IsReleased)
		{

			thisDragState = dragState.IsReturning;

			if (debugMode)
				Debug.Log (this + " is released");

		}

		if (thisDragState == dragState.IsReturning)
		{
			// The dragged object needs to maintain distance distanceFromCamera from the camera the entire time it is active.
			// Convert initialPosition from world position to 2D screen position.
			Vector2 position2D = cam.WorldToScreenPoint(initialPosition);

			//Convert to 3D again using distanceFromCamera
			Vector3 originOnScreen3D = cam.ScreenToWorldPoint(new Vector3 (position2D.x, position2D.y, distanceFromCamera));

			//The transform is already distanceFromCamera distance from the camera, so calculate the vector that returns it to 
			// initial position
			Vector3 returnVector = transform.position - originOnScreen3D;

			returnVector = returnVector.normalized;

			// shift the transform back towards initial position using the vector generated from this camera angle
			transform.position = transform.position - (returnVector *  returnSpeed);

			if (debugMode)
				Debug.Log("Object Returning via "+ returnVector);

			float dist = Vector3.Distance(originOnScreen3D, transform.position);

			if (dist < returnRange)
			{
				thisDragState = dragState.Returned;

				if (debugMode)
					Debug.Log("Object Returned");

			}
			
		}

		if (thisDragState == dragState.FollowingFinger)
		{
			// The dragged object must have NO COLLISION for this physics raycast to work
			Vector3 objectPosition = new Vector3 (0,0,0);

			objectPosition.x = TouchManager.Instance.fingerRecordedPosition.x;
			objectPosition.y = TouchManager.Instance.fingerRecordedPosition.y;
			objectPosition.z = distanceFromCamera;

			RaycastHit hit;
			
			Ray ray = cam.ScreenPointToRay( new Vector3 (objectPosition.x, objectPosition.y, 0 ) );
			
			
			if ( Physics.Raycast ( ray, out hit) )
			{
				if (debugMode)
					Debug.Log("Ray Cast Hit Object on Object");
				
				// Is this object a collision object?
				if (hit.collider != null) {

					Collider target = hit.collider;
					
					GameObjectMode targetMode;
					Debug.Log(target);
					targetMode = target.GetComponent<GameObjectMode>();
					
					if (targetMode)
					{
						if (targetMode.gameObjectMode == GameObjectMode.ModeList.DragObjectTarget)
						{
							thisDragState = dragState.Collected;

							if (debugMode)
								Debug.Log("Object Collected");
							
						}
					}	
				}	
			}

			transform.position = cam.ScreenToWorldPoint(objectPosition);

			Vector2 position2D = cam.WorldToScreenPoint(destinationPosition);

			Vector3 destinationOnScreen3D = cam.ScreenToWorldPoint(new Vector3 (position2D.x, position2D.y, distanceFromCamera));

			if (debugMode)
				Debug.Log("Object Dragged");

		}
	}
}
