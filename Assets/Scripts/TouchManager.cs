using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour {
	
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
		
	private Vector2 fingerStartPosition = new Vector2( 0, 0 );
	private Vector2 fingerRecordedPosition = new Vector2( 0, 0 );
	
	
	private Camera cam;
	

	// Use this for initialization
	void Start () {
		
		instances++;
		
		if (instances > 1)
			Debug.Log ("There is more than one Touch Manager in the level");
		else
			myInstance = this;
		
		cam = gameObject.GetComponentInChildren<Camera>();
		//Debug.Log ("Screen Width:" + Screen.width);
		//Debug.Log ("Screen Height:" + Screen.height);
		//Debug.Log (cam);
		
		EbookEventManager.EbookStart += EbookStart;
		EbookEventManager.EbookBackToMenu += EbookBackToMenu;

		
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
				PageManager.Instance.PageTurnPrevious();
		}
		if (Input.GetKeyDown (KeyCode.Alpha2))
		{
				PageManager.Instance.PageTurnNext();
		}
		/*
		if (Input.GetKeyDown (KeyCode.Alpha3))
		{
				PageManager.Instance.ScreenCap();
		}
		*/

		
		
	}
	

	
	// Update is called once per frame
	void Update () 
	{
		//Debug.Log(state);
		
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
						PageManager.Instance.PageTurnPrevious ();
					}
					else if (dragDistance < 0)
					{
						PageManager.Instance.PageTurnNext ();
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
	
	private void EbookStart () {
	}
	
	private void EbookBackToMenu () {
	}
}
