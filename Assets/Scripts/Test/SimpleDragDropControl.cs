using UnityEngine;
using System.Collections;

public class SimpleDragDropControl : MonoBehaviour {

	public bool debugMode = false;

	public Camera cam;

	//public Vector3 objectPosition = new Vector3 (0,0,0);
	
	public Transform shoeDragPickUp;

	public Transform shoeDragged;

	public Transform shoeDragDrop;

	private SimpleDragged shoeDraggedState;

	private SimpleDragDrop shoeDragDropState;

	//public Transform shoeDragDrop;

	//public Transform clone;

	//public SimpleDragged shoeDraggedState;

	//public bool isPickedUp = false;
	
	private float rayCastX;
	private float rayCastY;

	//public float distanceBack = 15f;

	public float enableDistance = 5f;

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


	
	}
	
	// Update is called once per frame
	void Update () {


		bool mouseUp = Input.GetMouseButtonUp(0);

		if (mouseUp) 
		{
			if (shoeDraggedState.thisDragState == SimpleDragged.dragState.FollowingFinger)
			{

				shoeDraggedState.thisDragState = SimpleDragged.dragState.IsReleased;

			}

		} 
			
		bool mouseDown = Input.GetMouseButtonDown(0);
		
		if (mouseDown)
		{
			
			Vector3 cursorScreenPosition = Input.mousePosition;
			
			rayCastX = cursorScreenPosition.x;
			rayCastY = cursorScreenPosition.y;
			
			RaycastHit hit;
			
			Ray ray = cam.ScreenPointToRay( new Vector3 (rayCastX, rayCastY ) );
			
			if ( Physics.Raycast ( ray, out hit) )
			{
				if (debugMode)
					Debug.Log("Ray Cast Hit on Object");
				
				
				if (hit.collider != null) {
				
					shoeDragged.transform.position = shoeDragPickUp.transform.position;
					shoeDraggedState.initialPosition = shoeDragPickUp.transform.position;
					//shoeDragged.transform.rotation = shoeDragPickUp.transform.rotation;

					shoeDraggedState.thisDragState = SimpleDragged.dragState.FollowingFinger;

					shoeDragged.gameObject.SetActive(true);

					shoeDragPickUp.renderer.enabled = false;
				}
				
			}
			else
			{
				if (debugMode)
					Debug.Log("Ray Cast Miss");
				
			}
			
		}

		if (shoeDragPickUp.renderer.enabled == false)
		{
			if (shoeDraggedState.thisDragState == SimpleDragged.dragState.InActive)
			{
				if (debugMode)
					Debug.Log("Shoe returned");
				
				shoeDragPickUp.renderer.enabled = true;

				shoeDragged.gameObject.SetActive(false);
				
			}
			
		}

		float dist = Vector3.Distance(shoeDragged.transform.position, shoeDragDrop.transform.position);
		
		if (dist < enableDistance)
		{
			if (shoeDragDropState.thisConnectState == SimpleDragDrop.connectState.NotConnected)
			{

				shoeDragDropState.thisConnectState = SimpleDragDrop.connectState.ConnectionMade;

				shoeDragged.gameObject.SetActive(false);

			}
		}
	
	}
}
