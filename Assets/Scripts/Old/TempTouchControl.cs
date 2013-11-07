using UnityEngine;
using System.Collections;

public class TempTouchControl : MonoBehaviour {
	

	
	enum ControlState
	{
		WaitingForFirstTouch,
		WaitingForDrag,
		DragBegins
	
	}
	
	private int state = (int)ControlState.WaitingForFirstTouch;
	private float firstTouchTime = 0.0f;
	private bool zeroTouchPromptGiven = false;
	private bool oneTouchPromptGiven = false;
	//private bool isDragging = false;
	
	public float minimumTimeUntilDrag = 0.25f;
	public float minimumDragDistance = 300f;
	
	private float rayCastX;
	private float rayCastY;
	
	public GameObject cameraObject;
	public GameObject cameraPivot;
	//private Transform thisTransform;
	private CameraGravityTest camGravTest;
	
	private Vector2 fingerStartPosition = new Vector2( 0, 0 );
	private Vector2 fingerRecordedPosition = new Vector2( 0, 0 );
	
	
	private Camera cam;
	

	// Use this for initialization
	void Start () {
		Debug.Log ("Screen Width:" + Screen.width);
		Debug.Log ("Screen Height:" + Screen.height);
		
		camGravTest = (CameraGravityTest)cameraPivot.GetComponent( typeof(CameraGravityTest) );
		cam = cameraObject.camera;
		//thisTransform = transform;
		
		ResetControlState();
	
	}
	
	void ResetControlState()
	{
		state = (int)ControlState.WaitingForFirstTouch;	
		
		if (!zeroTouchPromptGiven)
		{
			//Debug.Log("Waiting for first touch");
			
			zeroTouchPromptGiven = true;
			oneTouchPromptGiven = false;
			
		
		}
	}
	
	void InteractionControl()
	{
		int count = Input.touchCount;
		
		bool mouseDown = Input.GetMouseButtonDown(0);
		
		if ( (count == 1 && state == (int)ControlState.WaitingForDrag) && (!oneTouchPromptGiven)|| mouseDown)
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
			
			RaycastHit hit;
			Ray ray = cam.ScreenPointToRay( new Vector3 (rayCastX, rayCastY ) );
			if ( Physics.Raycast ( ray, out hit) )
			{
				Debug.Log("Ray Cast Hit on Object");
				oneTouchPromptGiven = true;
			}
			else
			{
				Debug.Log("Ray Cast Miss");
				oneTouchPromptGiven = true;
			}
		}
		
		
		
		if (Input.GetKeyDown (KeyCode.Alpha1))
		{
				PageTurnPrevious ();
		}
		if (Input.GetKeyDown (KeyCode.Alpha2))
		{
				PageTurnNext ();
		}
		
		if (Input.GetKeyDown (KeyCode.UpArrow))
		{
			camGravTest.RotateUp ();
		}
		
		if (Input.GetKeyDown (KeyCode.DownArrow))
		{
			camGravTest.RotateDown ();
		}
		
		if (Input.GetKeyDown (KeyCode.LeftArrow))
		{
			camGravTest.RotateLeft ();
		}
		
		if (Input.GetKeyDown (KeyCode.RightArrow))
		{
			camGravTest.RotateRight ();
		}
		
		
	}
	
	void PageTurnPrevious ()
	{
		Debug.Log("Page has turned to Previous"); 	
	}
	
	void PageTurnNext ()
	{
		Debug.Log("Page has turned to Next"); 	
	}
	
	// Update is called once per frame
	void Update () 
	{
		Debug.Log(state);
		
		int touchCount = Input.touchCount;
		
		if (touchCount == 0) 
		{
			if (state == (int)ControlState.DragBegins)
			{
				float dragDistance = fingerStartPosition.x - fingerRecordedPosition.x;
				
				if (Mathf.Abs(dragDistance) > minimumDragDistance)
				{
					if (dragDistance > 0)
					{
						PageTurnPrevious ();
					}
					else if (dragDistance < 0)
					{
						PageTurnNext ();
					}
					
					
				}
				
			}
			
			
			ResetControlState ();
			
		}
		

		else
		{
			firstTouchTime = 0.0f;
			//isDragging = false;
			
			Touch touch;
			Touch[] touches = Input.touches;
			if (state == (int)ControlState.WaitingForFirstTouch )
			{
				touch = touches[0];
				
				if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled )
				{
					state = (int)ControlState.WaitingForDrag;
					firstTouchTime = Time.time;
					
				}
			}
			
			if (state == (int)ControlState.WaitingForDrag )
			{
				
				touch = touches[0];
			
				if (touch.phase != TouchPhase.Canceled )
				{
					if (Time.time > firstTouchTime + minimumTimeUntilDrag) 
					{
						state = (int)ControlState.DragBegins;
						fingerStartPosition = touch.position;
						fingerRecordedPosition = fingerStartPosition;
						//isDragging = true;
						
					}
				}
			}
			if (state == (int)ControlState.DragBegins )
			{
				touch = touches[0];
				if (touch.phase != TouchPhase.Canceled )
				{
					fingerRecordedPosition = touch.position;
				}
			}

		}
		
		InteractionControl();
					
	}
}
