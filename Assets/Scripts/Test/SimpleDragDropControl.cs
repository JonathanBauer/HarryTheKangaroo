using UnityEngine;
using System.Collections;

public class SimpleDragDropControl : MonoBehaviour {

	public Camera cam;

	public Vector3 objectPosition = new Vector3 (0,0,0);
	
	public Transform shoeDragPickUp;

	public Transform shoeDragged;

	//public Transform shoeDragDrop;

	public Transform clone;

	//public SimpleDragged cloneDrag;

	public bool isPickedUp = false;
	
	private float rayCastX;
	private float rayCastY;

	public float distanceBack = 15f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (isPickedUp)
		{
			bool mouseUp = Input.GetMouseButtonUp(0);

			if (mouseUp)
			{
				SimpleDragged cloneDrag = clone.GetComponent<SimpleDragged>();
				cloneDrag.thisDragState = SimpleDragged.dragState.IsReleased;
				isPickedUp = false;

			} else {

				/*
				objectPosition.x = Input.mousePosition.x;
				objectPosition.y = Input.mousePosition.y;
				objectPosition.z = distanceBack;

				clone.transform.position = cam.ScreenToWorldPoint(objectPosition);
				*/
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
				Debug.Log("Ray Cast Hit on Object");
				
				
				if (hit.collider != null) {
					
					//touchActivated = hit.collider.GetComponent<TouchActivateO>();
					
					
					//touchActivated.HasBeenTouched ();
					
					Collider target = hit.collider;
					Debug.Log( target );
					
					clone = Instantiate(shoeDragged, shoeDragPickUp.transform.position, shoeDragPickUp.transform.rotation) as Transform;

					isPickedUp = true;

					shoeDragPickUp.renderer.enabled = false;
				}
				
			}
			else
			{
				Debug.Log("Ray Cast Miss");
				
			}
			
		}
	
	}
}
