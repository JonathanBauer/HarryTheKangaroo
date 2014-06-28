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

	public enum ControlState
	{
		WaitingForFirstTouch,
		WaitingForDrag,
		WaitingToDragObject,
		DragBegins,
		DraggingObjectBegins
		
	}

	public ControlState state = ControlState.WaitingForFirstTouch;

	public bool debugMode = true;
	public Camera cam;

	public GameObject webPlayerIcon;
	public GameObject iPhoneIcon;
	public GameObject waitingForFirstTouchIcon;
	public GameObject dragBeginsIcon;

	public bool iPhoneDevice = false;

	private Vector2 rayCast = new Vector2 (-1,-1);

	//private float rayCastX;
	//private float rayCastY;

	private float firstTouchTime = 0.0f;
	private float dragBeginTime = 0.0f;
	
	//private bool zeroTouchPromptGiven = false;
	//private bool oneTouchPromptGiven = false;

	public float minimumTimeUntilDrag = 0.25f;
	public float minimumDragDistance = 300f;

	private Vector2 fingerStartPosition = new Vector2( 0, 0 );
	public Vector2 fingerRecordedPosition = new Vector2( 0, 0 );

	// Use this for initialization
	void Start () {

		instances++;
		
		if (instances > 1)
			Debug.Log ("There is more than one Touch Manager in the level");
		else
			myInstance = this;

		webPlayerIcon = GameObject.Find("WebPlayerIcon");
		iPhoneIcon = GameObject.Find("iPhoneIcon");
		waitingForFirstTouchIcon = GameObject.Find("WaitingForFirstTouch");
		dragBeginsIcon = GameObject.Find("DragBegins");

		if (waitingForFirstTouchIcon)
			waitingForFirstTouchIcon.SetActive(false);
		if (dragBeginsIcon)
			dragBeginsIcon.SetActive(false);

		#if !UNITY_EDITOR && !UNITY_WEBPLAYER
		if (iPhoneIcon)
		{
			if(webPlayerIcon)
				webPlayerIcon.SetActive(false);
			iPhoneDevice = true;
		}
				
		#else
		if (webPlayerIcon)
		{
			if(iPhoneIcon)
				iPhoneIcon.SetActive(false);
			iPhoneDevice = false;
		}

		#endif

	
	}

	void ResetControlState()
	{
		state = ControlState.WaitingForFirstTouch;	

		if (debugMode)
		{
			//Debug.Log("Waiting for first touch");

			if (waitingForFirstTouchIcon)
				waitingForFirstTouchIcon.SetActive(true);
			
			if (dragBeginsIcon)
				dragBeginsIcon.SetActive(false);
		}
			

	}

	// InteractionControl() is called within Update ()
	void InteractionControl()
	{
		// if this is the iPhone, check for touches
		if (iPhoneDevice)
		{
			int count = Input.touchCount;

			if (count == 1) 
			{
				Touch touch = Input.GetTouch(0);
				rayCast = touch.position;
			}
		
		// otherwise, if this is the webplayer/editor check if the mouse is down
		} else if (!iPhoneDevice)
		{
			bool mouseDown = Input.GetMouseButtonDown(0);

			if (mouseDown) 
			{
				rayCast = Input.mousePosition;
			}

		}

		if (rayCast.x > -1) 
		{
			RaycastHit hit;

			Ray ray = cam.ScreenPointToRay( new Vector3 (rayCast.x, rayCast.y, 0 ) );
			
			
			if ( Physics.Raycast ( ray, out hit) )
			{
				if (debugMode)
					Debug.Log("Ray Cast Hit on Object");

				// Is this object a collision object?
				if (hit.collider != null) {
					
					// the types of hit colliders will need to expand to include the game manager
					// instead of just the page manager
					Collider target = hit.collider;

					GameObjectMode targetMode;
					targetMode = target.GetComponent<GameObjectMode>();

					if (targetMode)
					{

						if (targetMode.gameObjectMode == GameObjectMode.ModeList.PageTurnNext)
						{
							if (debugMode)
								Debug.Log(target+ "has a PageTurnNext script");

							PageManager.Instance.PageTurnNext ();

						} else if (targetMode.gameObjectMode == GameObjectMode.ModeList.PageTurnPrevious)
						{
							if (debugMode)
								Debug.Log(target+ "has a PageTurnPrevious script");

							PageManager.Instance.PageTurnPrevious ();

						} else if (targetMode.gameObjectMode == GameObjectMode.ModeList.StoryAnimation)
						{
							PageManager.Instance.TriggerStoryAnimation ( target );

						} else if (targetMode.gameObjectMode == GameObjectMode.ModeList.DragObject)
						{
							state = ControlState.WaitingToDragObject;
							dragBeginTime = Time.time;
							if (debugMode)
								Debug.Log("Waiting to drag object");

						} else {

							if (debugMode)
								Debug.Log(target+ "has an unrecognized GameObjectMode script "+targetMode.gameObjectMode);

						}

					} else {
						if (debugMode)
							Debug.Log(target+ "does not have a GameObjectMode script");
					}

					//PageManager.Instance.TriggerMeshTouched ( target );
					rayCast = new Vector2(-1,-1);
				} 
			}
			else
			{
				if (debugMode)
					Debug.Log("Ray Cast Miss");

				rayCast = new Vector2(-1,-1);
			}
		}

	}


	
	// Update is called once per frame
	void Update () 
	{
		if (iPhoneDevice)
		{
			int touchCount = Input.touchCount;

			// if there are no touches 
			
			if (touchCount == 0) 
			{
				// and the finger was beginning a drag

				if (state == ControlState.DragBegins)
				{
					if (debugMode)
						Debug.Log("Drag Finished");
					// This is where a finished dragging gesture would finish.
					// We currently don't use dragging gestures so this is empty.

					
				}

				// There was a touch, but it wasn't held long enough to enter DragBegins state
				
				ResetControlState ();
			
				
			}
			
			// otherwise, if there are one or more touches 

			else
			{
				firstTouchTime = 0.0f;

				// new touch variable
				Touch touch;
				// new array variable for touches. Store all the touches in it.
				Touch[] touches = Input.touches;
				if (state == ControlState.WaitingForFirstTouch )
				{
					// the touch variable is the first touch
					touch = touches[0];

					// if the device hasn't give late Ended AND it hasn't given a late drag
					if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled )
					{
						// we COULD be pressing once, but this might be a drag
						state = ControlState.WaitingForDrag;
						firstTouchTime = Time.time;

						if (debugMode)
							Debug.Log("Waiting for drag");
						
					}
				}
				
				if (state == ControlState.WaitingForDrag )
				{
					
					touch = touches[0];
					
					if (touch.phase != TouchPhase.Canceled )
					{
						// if the time passed since the first touch is longer than the minimum wait time
						if (Time.time > firstTouchTime + minimumTimeUntilDrag) 
						{
							// then a drag has begun
							state = ControlState.DragBegins;
							// put the game dragging controls here
							// maybe a special designation for the touch position
							// maybe some public cursor coordinates
							// and the mouse detection will need to be here as well
							fingerStartPosition = touch.position;
							fingerRecordedPosition = fingerStartPosition;
							if (debugMode)
								Debug.Log("Drag Begins");
							
						}
					}
				}

				// now that the player is dragging
				if (state == ControlState.DragBegins )
				{
					touch = touches[0];
					if (touch.phase != TouchPhase.Canceled )
					{
						// since they haven't let go, keep recording

						// dragdrop needs TouchManager's controlstate and fingerRecordedPosition

						fingerRecordedPosition = touch.position;
						if (debugMode)
						{
							Debug.Log("Drag Recording");
							if (waitingForFirstTouchIcon)
								waitingForFirstTouchIcon.SetActive(false);

							if (dragBeginsIcon)
								dragBeginsIcon.SetActive(true);

						}
					}
				}
				
			}

		}

		if (!iPhoneDevice)
		{
	
			// PC Controls, since touches and mousedowns will never happen together
			// GetMouseButton returns true while the mouse is held

			bool mouseDown = Input.GetMouseButton(0);


			if (mouseDown == false)
			{

				if (state == ControlState.DragBegins)
				{
					if (debugMode)
						Debug.Log("Drag Finished");
					// This is where a finished dragging gesture would finish.
					// We currently don't use dragging gestures so this is empty.
					
					
				}

				if (state == ControlState.DraggingObjectBegins)
				{
					if (debugMode)
						Debug.Log("Drag Object Finished");
					// This is where a finished dragging gesture would finish.
					// We currently don't use dragging gestures so this is empty.
					
					
				}

				ResetControlState ();


			} else {
			

				if (state == ControlState.WaitingForFirstTouch )
				{
					state = ControlState.WaitingForDrag;
					firstTouchTime = Time.time;
					if (debugMode)
						Debug.Log("Waiting for drag");
					/*
					rayCast = Input.mousePosition;

					RaycastHit hit;
					
					Ray ray = cam.ScreenPointToRay( new Vector3 (rayCast.x, rayCast.y, 0 ) );
					
					
					if ( Physics.Raycast ( ray, out hit) )
					{
						if (debugMode)
							Debug.Log("Ray Cast Hit on Object");
						
						// Is this object a collision object?
						if (hit.collider != null) {
							
							// the types of hit colliders will need to expand to include the game manager
							// instead of just the page manager
							Collider target = hit.collider;
							//PageManager.Instance.TriggerMeshTouched ( target );
							// Go to the collision check function with the collider

							// if it is a draggable object, state is WaitingToDragObject


						}
						
					}
					else
					{
						if (debugMode)
							Debug.Log("Ray Cast Miss");
						
						rayCast = new Vector2(-1,-1);
					}
					*/

				}

				if (state == ControlState.WaitingForDrag )
				{
					
					if (Time.time > firstTouchTime + minimumTimeUntilDrag) 
					{

						state = ControlState.DragBegins;
						fingerStartPosition = Input.mousePosition;
						fingerRecordedPosition = fingerStartPosition;
						if (debugMode)
							Debug.Log("Drag Begins");
						
					}	
				}

				if (state == ControlState.WaitingToDragObject )
				{
					
					if (Time.time > dragBeginTime + minimumTimeUntilDrag) 
					{

						state = ControlState.DraggingObjectBegins;
						fingerStartPosition = Input.mousePosition;
						fingerRecordedPosition = fingerStartPosition;
						if (debugMode)
							Debug.Log("Drag Object Begins");
						
					}	
				}

				if (state == ControlState.DragBegins )
				{
					fingerRecordedPosition = Input.mousePosition;

					{
						Debug.Log("Drag Recording");
						if (waitingForFirstTouchIcon)
							waitingForFirstTouchIcon.SetActive(false);
						
						if (dragBeginsIcon)
							dragBeginsIcon.SetActive(true);
						
					}
				}

				if (state == ControlState.DraggingObjectBegins )
				{
					fingerRecordedPosition = Input.mousePosition;
					
					{
						Debug.Log("Drag Object Recording");
						if (waitingForFirstTouchIcon)
							waitingForFirstTouchIcon.SetActive(false);
						
						if (dragBeginsIcon)
							dragBeginsIcon.SetActive(true);
						
					}
				}

			}
		}
		
		InteractionControl();
		
	}



}
