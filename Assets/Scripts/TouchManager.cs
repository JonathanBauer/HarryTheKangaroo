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

	public bool debugMode = false;
	public Camera cam;

	public GameObject webPlayerIcon;
	public GameObject iPhoneIcon;
	public GameObject waitingForFirstTouchIcon;
	public GameObject dragBeginsIcon;

	public bool iPhoneDevice = false;

	public Vector2 rayCast = new Vector2 (-1,-1);

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

	public GameObject latchedObject;

	public float draggedViewAngle = 0f;

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

	// Update is called once per frame
	void Update () 
	{
		//Debug.Log(rayCast);

		if (iPhoneDevice)
		{
			IPhoneTouchControl();
		}
		
		if (!iPhoneDevice)
		{	
			WebPlayerMouseControl();
		}
			
	}


	void RayCastCheck ()
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

					}  else if (targetMode.gameObjectMode == GameObjectMode.ModeList.StoryAnimation)
					{
						PageManager.Instance.TriggerStoryAnimation ( target );
						
					} else if (targetMode.gameObjectMode == GameObjectMode.ModeList.DragObjectOrigin)
					{
						state = ControlState.WaitingToDragObject;
						dragBeginTime = Time.time;
						if (debugMode)
							Debug.Log("Waiting to drag object");

						latchedObject = target.gameObject;

						//FindSimpleDragDropControl (latchedObject);

						
					} else {
						
						if (debugMode)
							Debug.Log(target+ "has an unrecognized GameObjectMode script "+targetMode.gameObjectMode);
						
					}
					
				} else {
					if (debugMode)
						Debug.Log(target+ "does not have a GameObjectMode script");
				}

			} 
		}
		else
		{
			if (debugMode)
				Debug.Log("Ray Cast Miss");

		}
	}

	// These next two functions are temporary. They should be called in PageManager, but it's still debug for drag objects
	void FindSimpleDragDropControl ( GameObject dragObject)
	{
		GameObject thisDDcontrol = GameObject.Find("DragDropTest");
		SimpleDragDropControl sDDControl = thisDDcontrol.GetComponent<SimpleDragDropControl>();
		sDDControl.LiftUpDragObject( dragObject );

	}

	void FindSimpleDragDropControl02 ( GameObject dragObject)
	{
		GameObject thisDDcontrol = GameObject.Find("DragDropTest");
		SimpleDragDropControl sDDControl = thisDDcontrol.GetComponent<SimpleDragDropControl>();
		sDDControl.DropDragObject( dragObject );
		
	}

	void IPhoneTouchControl ()
	{
		/*  IPHONE CONTROLS IPHONE CONTROLS IPHONE CONTROLS IPHONE CONTROLS IPHONE CONTROLS IPHONE CONTROLS 
		 *  if there are no touches and the state was a drag beginning on open space, finish the drag */

		int touchCount = Input.touchCount;
		if (touchCount == 0) 
		{
			if (state == ControlState.DragBegins)
			{
				if (debugMode)
					Debug.Log("IPHONE:Drag Finished");
			}
			if (state == ControlState.DraggingObjectBegins)
			{
				if (debugMode)
					Debug.Log("IPHONE: Drag Object Finished");
				
				FindSimpleDragDropControl02 (latchedObject);
			}
			ResetControlState ();
		} else {
			firstTouchTime = 0.0f;
			Touch touch;
			Touch[] touches = Input.touches;
			if (state == ControlState.WaitingForFirstTouch )
			{
				touch = touches[0];
				if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled )
				{
					state = ControlState.WaitingForDrag;
					firstTouchTime = Time.time;				
					if (debugMode)
						Debug.Log("IPHONE:Waiting for drag");
					int count = Input.touchCount;
					if (count == 1) 
					{
						rayCast = touch.position;
					}
					RayCastCheck();
				}
			}
			if (state == ControlState.WaitingForDrag )
			{
				touch = touches[0];
				if (touch.phase != TouchPhase.Canceled )
				{
					if (Time.time > firstTouchTime + minimumTimeUntilDrag) 
					{
						state = ControlState.DragBegins;
						fingerStartPosition = touch.position;
						fingerRecordedPosition = fingerStartPosition;
						if (debugMode)
							Debug.Log("IPHONE:Drag Begins");
					}
				}
			}
			if (state == ControlState.WaitingToDragObject )
			{
				touch = touches[0];
				if (touch.phase != TouchPhase.Canceled )
				{
					if (Time.time > dragBeginTime + minimumTimeUntilDrag) 
					{
						state = ControlState.DraggingObjectBegins;
						fingerStartPosition = touch.position;
						fingerRecordedPosition = fingerStartPosition;
						if (debugMode)
							Debug.Log("IPHONE: Drag Object Begins");
					
						FindSimpleDragDropControl (latchedObject);
					}
				}	
			}
			if (state == ControlState.DragBegins )
			{
				touch = touches[0];
				if (touch.phase != TouchPhase.Canceled )
				{
					fingerRecordedPosition = touch.position;
					if (debugMode)
					{
						Debug.Log("IPHONE:Drag Recording");
						if (waitingForFirstTouchIcon)
							waitingForFirstTouchIcon.SetActive(false);
						if (dragBeginsIcon)
							dragBeginsIcon.SetActive(true);	
					}
				}
			}
			if (state == ControlState.DraggingObjectBegins )
			{
				touch = touches[0];
				if (touch.phase != TouchPhase.Canceled )
				{
					fingerRecordedPosition = touch.position;
					if (debugMode)
					{
						Debug.Log("IPHONE: Drag Object Recording");
						if (waitingForFirstTouchIcon)
							waitingForFirstTouchIcon.SetActive(false);	
						if (dragBeginsIcon)
							dragBeginsIcon.SetActive(true);	
					}
				}
			}

			
		}
	}

	void WebPlayerMouseControl ()
	{
		/* PC CONTROLS PC CONTROLS PC CONTROLS PC CONTROLS PC CONTROLS PC CONTROLS 
		 * since touches and mousedowns will never happen together
		 * GetMouseButton returns true while the mouse is held */
		
		bool mouseDown = Input.GetMouseButton(0);
		if (mouseDown == false)
		{
			if (state == ControlState.DragBegins)
			{
				if (debugMode)
					Debug.Log("WEBPLAYER: Drag Finished");
			}
			if (state == ControlState.DraggingObjectBegins)
			{
				if (debugMode)
					Debug.Log("WEBPLAYER: Drag Object Finished");

				FindSimpleDragDropControl02 (latchedObject);
			}
			ResetControlState ();
		} else {
			if (state == ControlState.WaitingForFirstTouch )
			{
				state = ControlState.WaitingForDrag;
				firstTouchTime = Time.time;
				if (debugMode)
					Debug.Log("WEBPLAYER: Waiting for drag");
				rayCast = Input.mousePosition;
				RayCastCheck();
			}
			if (state == ControlState.WaitingForDrag )
			{
				if (Time.time > firstTouchTime + minimumTimeUntilDrag) 
				{
					state = ControlState.DragBegins;
					fingerStartPosition = Input.mousePosition;
					fingerRecordedPosition = fingerStartPosition;
					if (debugMode)
						Debug.Log("WEBPLAYER: Drag Begins");
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
						Debug.Log("WEBPLAYER: Drag Object Begins");

					FindSimpleDragDropControl (latchedObject);
				}	
			}
			if (state == ControlState.DragBegins )
			{
				fingerRecordedPosition = Input.mousePosition;

				if (debugMode)
					Debug.Log("WEBPLAYER: Drag Recording");
				if (waitingForFirstTouchIcon)
					waitingForFirstTouchIcon.SetActive(false);
				if (dragBeginsIcon)
					dragBeginsIcon.SetActive(true);

				draggedViewAngle = fingerRecordedPosition.x - fingerStartPosition.x;
				if (debugMode)
					Debug.Log("WEBPLAYER: Dragged View Angle: "+draggedViewAngle);

			}
			if (state == ControlState.DraggingObjectBegins )
			{
				fingerRecordedPosition = Input.mousePosition;

				if (debugMode)
					Debug.Log("WEBPLAYER: Drag Object Recording");
				if (waitingForFirstTouchIcon)
					waitingForFirstTouchIcon.SetActive(false);	
				if (dragBeginsIcon)
					dragBeginsIcon.SetActive(true);	

			}
			
		}
	}
	




}
