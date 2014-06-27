using UnityEngine;
using System.Collections;

public class SimpleDragDrop : MonoBehaviour {

	public Camera cam;

	public Transform test_shoedragged;

	private Transform clone;

	private float rayCastX;
	private float rayCastY;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

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

					clone = Instantiate(test_shoedragged, transform.position, transform.rotation) as Transform;
				}
				
			}
			else
			{
				Debug.Log("Ray Cast Miss");

			}

		}
	}
}
