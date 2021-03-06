using UnityEngine;
using System.Collections;

public class PageManagerOld : MonoBehaviour {
	
	/* A singleton is a design pattern that restricts the instantiation of a class to one object. 
	 * 
	 * 
	 */
	
	static PageManagerOld myInstance;
	static int instances = 0;
	private GimbalGyroAidControl gimbalGyroAidControl;
	
	public static PageManagerOld Instance
	{
		get
		{
			if (myInstance == null)
				myInstance = FindObjectOfType (typeof(PageManagerOld)) as PageManagerOld;
			
			return myInstance;
		}
	}
	
	// PageManagerOld automatically checks how many page prefabs are in the Pages object and
	// places them within arrays to move them
	
	
	private GameObject pageRoot;				//for the Pages Object
	public PageControlOld[] ebookPage;
	
	private GameObject pageCoverRoot;			//for the Page Screen Covers Object
	private Renderer[] pageCoverRenderer;
	
	private float[] pageCoverInitialXPos;
	
	
	private bool canProgress = true;

	private int currentPage = 0;
	private int turnToPage = 0;
	private int lastPage = 0;					// the number of the last page 
	
	public float pageTurnSpeed = 1.0f;
	public float distanceBetweenPages = 120f;	// how far apart are the page prefabs spaced
	
	private float pageCoverMovement = 0f;
	private float pageCoverTravel = 0f;
	
	private Vector3 pagePosition = new Vector3 (0,0,0);
	private Vector3 pageCoverPosition = new Vector3 (0,0,0);
	
	private float pageTurnDestination = 0f;
	private float pageCoverDestination = 0f;
		
	
	//private Vector2 screenSize = new Vector2(0,0);
	
	public float cameraOrthographicSize = 34f;
	public float screenCoverScale = 1f;
	public float touchActivateWaitTime = 0.5f;
	
	private float activationWaitTimer = -1;
	
	private Vector3 screenCoverSize = new Vector3(1,1,1);
	

	// Use this for initialization
	void Start () 
	{
		instances++;
		
		if (instances > 1)
			Debug.Log ("There is more than one Page Manager in the level");
		else
			myInstance = this;
		
		
		EbookEventManagerOld.EbookStart += EbookStart;
		EbookEventManagerOld.EbookBackToMenu += EbookBackToMenu;
		
		gimbalGyroAidControl = GetComponentInChildren<GimbalGyroAidControl>();
		
		// Find the "Pages" object and place all the PageControlOlds into the ebookPage array
		pageRoot = GameObject.Find("Pages");
		ebookPage = pageRoot.GetComponentsInChildren<PageControlOld>();
		
		// Find the "PageScreenCovers" object and place all the Renderables into the pageCoverRenderer array
		pageCoverRoot = GameObject.Find("PageScreenCovers");
		pageCoverRenderer = pageCoverRoot.GetComponentsInChildren<Renderer>();
		
		// Arrays start at zero, so the last entry is always the count minus one
		lastPage = ebookPage.Length - 1;
		//Debug.Log ("eBook Page Length = " + ebookPage.Length);
		//Debug.Log ("Page 1 = " + ebookPage[0]);
		// Total pages are known, so the initial X position array for the page covers matches this
		pageCoverInitialXPos = new float[ebookPage.Length];
		Debug.Log ("Page Cover Initial Pos X Length = " + pageCoverInitialXPos.Length);
		Debug.Log ("Page Cover 1 = " + pageCoverRenderer[0]);
			
		// The page covers cover the entire screen corner to corner to hide the transition of the page prefabs
		//Vector2 screenSize = new Vector2(0,0);
		
		//screenSize.x = Screen.width;
		//screenSize.y = Screen.height;
		
		float aspectRatio = Screen.width / Screen.height;
		
		screenCoverSize.y = (cameraOrthographicSize * 2)* screenCoverScale ;
		screenCoverSize.x = (screenCoverSize.y * aspectRatio) * screenCoverScale;
		
		int i;
		
		for( i = 0; i < (lastPage + 1); i++) 
		{
			// All page covers are turned off	
			pageCoverRenderer[i].enabled = false;
			
			// All page covers are scaled to each cover the entire screen
			pageCoverRenderer[i].transform.localScale = screenCoverSize;
			
			// Based on the x scale, all page covers other than the first are positioned edge to edge
			if (i > 0) 
			{
				pageCoverPosition = pageCoverRenderer[i].transform.localPosition;
				
				pageCoverPosition.x -= (screenCoverSize.x * i);
				pageCoverRenderer[i].transform.localPosition = pageCoverPosition;
			}
			// To ensure accurate placement after animation, the initial x position of the pages covers
			// is recorded in an array
			pageCoverPosition = pageCoverRenderer[i].transform.localPosition;
			pageCoverInitialXPos[i] = pageCoverPosition.x;
			
		}	
		
		ebookPage[currentPage].StartPageText ();
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Check to see if this page has a TouchActivateOld object
		TouchActivateOld hasTouchActivateOld = ebookPage[currentPage].touchActivateObject;
		

		// if it has one
		if (hasTouchActivateOld != null) {
			// and it has just been activated
			if (canProgress != hasTouchActivateOld.touched) {
				
				// activate the timer
				
				activationWaitTimer = Time.time;
				
				canProgress = hasTouchActivateOld.touched;
				
				
			}
	
		}
		
		if (activationWaitTimer > 0) {
			
			if (Time.time > (activationWaitTimer + touchActivateWaitTime)) {
				
				PageTurnNext ();
				activationWaitTimer = -1f;
				
			}
		}
		

			
		
		if (Input.GetKeyDown (KeyCode.Alpha0))
		{	
			int i;
		
			for( i = 0; i < (lastPage + 1); i++) 
			{
				Debug.Log ("Page "+i+" is "+ebookPage[i]);
			}
			Debug.Log ("Current Page is "+ebookPage[currentPage]);
			
			
		}
				
		/*
		
		if (Input.GetKeyDown (KeyCode.Alpha9))
		{
				
				ebookPage[currentPage].StartPageText ();
		}
		
		if (Input.GetKeyDown (KeyCode.Alpha0))
		{
				ebookPage[currentPage].ResetPageText ();
		}
		*/
		
		// if the page covers are meant to be moving..	
		if (pageCoverMovement != 0) 
		{  
			
			if (pageCoverTravel == 0)
			{
				//Debug.Log("Page Cover Travel Begun");
				
				int i;
			
				// for each page and page cover
				for( i = 0; i < (lastPage + 1 ); i++ )
				{
					// turn page cover on
					pageCoverRenderer[i].enabled = true;
					
					// move the page to it's destination
					pagePosition = ebookPage[i].transform.localPosition;
				
					pagePosition.x += pageTurnDestination;
				
					ebookPage[i].transform.localPosition = pagePosition;
				
					
					// record page cover initial position
					pageCoverPosition = pageCoverRenderer[i].transform.localPosition;
					
					pageCoverInitialXPos[i] = pageCoverPosition.x;
					
				}
				// start the travel of the page cover
				pageCoverTravel += pageCoverMovement;
				
				
			}
			
			// if the recorded travelling distance is greater than the destination distance,
			// the page cover has reached its destination
			else if (Mathf.Abs(pageCoverTravel) > Mathf.Abs(pageCoverDestination)) 
			{
				//Debug.Log("Page Cover Travel Ended");
				
				int i;
			
				
				for( i = 0; i < (lastPage + 1 ); i++ )
				{
					// make sure the page cover is locked to initial + destination distance
					pageCoverPosition = pageCoverRenderer[i].transform.localPosition;
					
					pageCoverPosition.x = pageCoverInitialXPos[i] + pageCoverDestination;
				
					pageCoverRenderer[i].transform.localPosition = pageCoverPosition;
					
					// turn the cover off
				
					pageCoverRenderer[i].enabled = false;	
				}
				
				pageCoverMovement = 0;
					
				pageCoverTravel = 0;
					
				pageCoverDestination = 0;
				
				// turned page is now the current page
				
				currentPage = turnToPage;	
				
				ebookPage[currentPage].StartPageText ();
				
				
				
			}
			
			else 
				
			{
				//Debug.Log("Page Cover Travelling");
				
				// pageCoverMovement can be positive or negative.
				// Because multiple pages and page covers are being moved,
				// a master travelling counter is used called pageCoverTravel
				
				pageCoverTravel += pageCoverMovement;
			
				int i;
			
				for( i = 0; i < (lastPage + 1 ); i++ ) 
				{
				
					pageCoverPosition = pageCoverRenderer[i].transform.localPosition;
				
					pageCoverPosition.x += pageCoverMovement;;
				
					pageCoverRenderer[i].transform.localPosition = pageCoverPosition;	
			
				}
			}	
			
		}
	
	}
	
	public void PageTurnPrevious ()
	{
		
		if (currentPage <= 0) {
			Debug.Log("You're on the first page");
		}
		else
		{
			turnToPage = currentPage -1;
			Debug.Log("Turning down to Page " +turnToPage );
			pageCoverMovement = - (pageTurnSpeed);
			pageTurnDestination = - (distanceBetweenPages);
			pageCoverDestination = - (screenCoverSize.x);
			
			gimbalGyroAidControl.CalibrateGyro ();
			
			ebookPage[currentPage].ResetPageText ();
		}
	}
	
	public void PageTurnNext ()
	{
		if (!canProgress){
			Debug.Log("You need to touch an object first");
		}		
		else if (currentPage >= lastPage) {
			Debug.Log("You're on the last page");
		}
		
		else 
		{
			turnToPage = currentPage +1;
			Debug.Log("Turning up to Page " +turnToPage );
			pageCoverMovement = pageTurnSpeed;
			pageTurnDestination = distanceBetweenPages;
			pageCoverDestination = screenCoverSize.x;
			
			gimbalGyroAidControl.CalibrateGyro ();
			
			ebookPage[currentPage].ResetPageText ();
		}
	}
	
	private void EbookStart () {
	}
	
	private void EbookBackToMenu () {
	}
	
	/*
	public void ScreenCap() {
		
		Rect screenRegion = new Rect(0,0,Screen.width,Screen.height);
		Debug.Log (screenRegion);
		Texture2D result;
		result = new Texture2D (Screen.width,Screen.height);
		result.ReadPixels(screenRegion,0,0,false);
		result.Apply ();
		
	}
	*/
	
}
