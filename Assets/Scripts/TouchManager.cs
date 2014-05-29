using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour {

	/* A singleton is a design pattern that restricts the instantiation of a class to one object. 
	 * 
	 * 
	 */
	
	static TouchManager myInstance;
	static int instances = 0;
	
	public static TouchManager Instance
	{
		get
		{
			if (myInstance == null)
				myInstance = FindObjectOfType (typeof(TouchManager)) as TouchManager;
			
			return myInstance;
		}
	}

	public bool debugMode = true;
	public Camera cam;

	private float rayCastX;
	private float rayCastY;

	//private TouchActivate touchActivated;

	private bool oneTouchPromptGiven = false;


	// Use this for initialization
	void Start () {

		instances++;
		
		if (instances > 1)
			Debug.Log ("There is more than one Touch Manager in the level");
		else
			myInstance = this;
	
	}
	
	// Update is called once per frame
	void Update () {

		int count = Input.touchCount;
		
		bool mouseDown = Input.GetMouseButtonDown(0);
		
		if ( count == 1 && (!oneTouchPromptGiven)|| mouseDown)
		{
			if (count == 1)
			{
				Touch touch = Input.GetTouch(0);
				rayCastX = touch.position.x;
				rayCastY = touch.position.y;
			}
			else
			{
				Vector3 cursorScreenPosition = Input.mousePosition;
				
				rayCastX = cursorScreenPosition.x;
				rayCastY = cursorScreenPosition.y;
			}
			
			// RaycastHit is the structure used to get information back from a raycast
			RaycastHit hit;
			
			// A ray is an infinite line starting at origin and going in some direction
			Ray ray = cam.ScreenPointToRay( new Vector3 (rayCastX, rayCastY ) );
			
			
			if ( Physics.Raycast ( ray, out hit) )
			{
				Debug.Log("Ray Cast Hit on Object");
				oneTouchPromptGiven = true;
				
				if (hit.collider != null) {
					
					//touchActivated = hit.collider.GetComponent<TouchActivateO>();

					
					//touchActivated.HasBeenTouched ();

					Collider target = hit.collider;
					PageManager.Instance.TriggerMeshTouched ( target );
				}
				
			}
			else
			{
				Debug.Log("Ray Cast Miss");
				oneTouchPromptGiven = true;
			}
		}
		
		/*
		
		if (Input.GetKeyDown (KeyCode.Alpha1))
		{
			PageManagerOld.Instance.PageTurnPrevious();
		}
		if (Input.GetKeyDown (KeyCode.Alpha2))
		{
			PageManagerOld.Instance.PageTurnNext();
		}
		


		if (Input.GetKeyDown (KeyCode.Alpha3))
		{
				PageManagerOld.Instance.ScreenCap();
		}
		*/
		
		

	
	}
}
