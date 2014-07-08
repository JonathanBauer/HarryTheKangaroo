using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PageManager : MonoBehaviour {

	/* A singleton is a design pattern that restricts the instantiation of a class to one object. 
	 * 
	 * 
	 */
	
	static PageManager myInstance;
	static int instances = 0;

	public static PageManager Instance
	{
		get
		{
			if (myInstance == null)
				myInstance = FindObjectOfType (typeof(PageManager)) as PageManager;
			
			return myInstance;
		}
	}

	public bool debugMode = true;					// set to true in inspector to see debug messages

	private GameObject pageRoot;					// The eBookPages object
	public PageControl[] eBookPage;					// Array for each eBook page
	public int startingPage = 0;					// Page to start with, chosen in inspector
	public int currentPage = 0;						
	public int lastPage = 0;
	public int lastText = 0;

	public Transform cameraParent;
	public Camera mainOrthoCamera;
	public float distanceBetweenPages = 200f;		// How far does the camera need to move to find the next page

	private GameObject textRoot;
	public TextControl[] eBookText;					// Array for the Text Control on each eBookPage




	// Use this for initialization
	void Start () {

		instances++;
		
		if (instances > 1)
			Debug.Log ("There is more than one Page Manager in the level");
		else
			myInstance = this;

		pageRoot = GameObject.Find("eBookPages");
		eBookPage = pageRoot.GetComponentsInChildren<PageControl>();

		textRoot = GameObject.Find("eBookTexts");
		eBookText = textRoot.GetComponentsInChildren<TextControl>();

		currentPage = startingPage;

		lastPage = eBookPage.Length - 1;	// The first entry in an array is always zero, so the last page's ID is 1 short of the length
		lastText = eBookText.Length - 1;

		if (cameraParent)
		{
			mainOrthoCamera = cameraParent.GetComponentInChildren<Camera>();

			if (debugMode)
				Debug.Log(mainOrthoCamera);

		} else {

			if (debugMode)
				Debug.Log("There is no camera parent object.");

		}


		if (lastPage > lastText)
		{
			if (debugMode)
				Debug.Log("There are more Page Controls than Text Controls. Reducing Page Controls.");
			lastPage = lastText;
		}

		if (lastText > lastPage)
		{
			if (debugMode)
				Debug.Log("There are more Text Controls than Page Controls. Reducing Text Controls.");
			lastText = lastPage;
		}

		for (int i=0; i < eBookText.Length; i++)		
		{
			if (i == currentPage)
			{
				eBookText[i].EnableText ();

				if (debugMode)
					Debug.Log(eBookText[i].name + " text is enabled.");

			} else {

				eBookText[i].DisableText ();

				if (debugMode)
					Debug.Log(eBookText[i].name + " text is disabled.");
			}
		}

		eBookPage[currentPage].StartAnimations ();



	
	}
	
	// Update is called once per frame
	void Update () {


	}


	public void TriggerStoryAnimation ( Collider hit ) {

		// count the animation meshes on this page
		int currentPageAnimationCount = eBookPage[currentPage].animationTarget.Count - 1;

		// for each target number
		for (int target = 0; target < currentPageAnimationCount; target++)
		{
			// if the collider is on this target number
			if (hit == eBookPage[currentPage].animationTarget[target].collider)
			{

				if (debugMode)
					Debug.Log ("The hit object is " + hit + ".It's index is "+target);
				// trigger the animation for that target number	
				eBookPage[currentPage].TriggerAnimation(target);

			}
		}
	}


	public void PageTurnNext () {

		if (debugMode)
			Debug.Log ("PageTurnNext");
		
		if (currentPage == lastPage) {
			if (debugMode)
				Debug.Log ("You're on the last page");
		} else {

			InteractionControl iac = eBookPage[currentPage].GetComponent<InteractionControl>();

			if (iac)
			{
				iac.interactionCamera.enabled = false;
				mainOrthoCamera.enabled = true;
				TouchManager.Instance.cam = mainOrthoCamera;
			}

			eBookPage[currentPage].StopAnimations ();
			eBookText[currentPage].DisableText ();

			currentPage ++;

			iac = eBookPage[currentPage].GetComponent<InteractionControl>();
			
			if (iac)
			{
				// Find out if the eBook Page Number in the inspector corresponds with the current eBook page
				// because PageManager only counts PageControls, not InteractionControls, and each 
				// InteractionControl must be correctly numbered
				if (iac.eBookPageNumber != currentPage)
				{
					if (debugMode)
						Debug.Log ("Interaction eBook Page Number of "+iac.eBookPageNumber + " does not match Pagemanager Ebook control of "+currentPage);
					
				} else {
					
					// Does the InteractionControl have a camera attached?
					if (iac.interactionCamera)
					{
						// Turn off the main Camera and enable the InteractionControl's
						mainOrthoCamera.enabled = false;
						iac.interactionCamera.enabled = true;
						TouchManager.Instance.cam = iac.interactionCamera;
						
					} else {
						
						if (debugMode)
							Debug.Log(iac.name + " has no camera.");
						
					}
					
				}
				
				
			} else {
				
				if (debugMode)
					Debug.Log (iac + " does not have an Interaction Control");
				
			}

			eBookPage[currentPage].StartAnimations ();
			eBookText[currentPage].EnableText ();
			
			Vector3 position = cameraParent.transform.position;
			position.x += distanceBetweenPages;
			cameraParent.transform.position = position;
			
		}

	}

	public void PageTurnPrevious () {
		
		if (debugMode)
			Debug.Log ("PageTurnPrevious");
		
		if (currentPage == 0) {
			if (debugMode)
				Debug.Log ("You're on the first page");
		} else {

			InteractionControl iac = eBookPage[currentPage].GetComponent<InteractionControl>();
			
			if (iac)
			{
				iac.interactionCamera.enabled = false;
				mainOrthoCamera.enabled = true;
				TouchManager.Instance.cam = mainOrthoCamera;
			}
			
			eBookPage[currentPage].StopAnimations ();
			eBookText[currentPage].DisableText ();

			currentPage --;

			iac = eBookPage[currentPage].GetComponent<InteractionControl>();
			
			if (iac)
			{
				// Find out if the eBook Page Number in the inspector corresponds with the current eBook page
				// because PageManager only counts PageControls, not InteractionControls, and each 
				// InteractionControl must be correctly numbered
				if (iac.eBookPageNumber != currentPage)
				{
					if (debugMode)
						Debug.Log ("Interaction eBook Page Number of "+iac.eBookPageNumber + " does not match Pagemanager Ebook control of "+currentPage);
					
				} else {
					
					// Does the InteractionControl have a camera attached?
					if (iac.interactionCamera)
					{
						// Turn off the main Camera and enable the InteractionControl's
						mainOrthoCamera.enabled = false;
						iac.interactionCamera.enabled = true;
						TouchManager.Instance.cam = iac.interactionCamera;
						
					} else {
						
						if (debugMode)
							Debug.Log(iac.name + " has no camera.");
						
					}
					
				}
				
				
			} else {
				
				if (debugMode)
					Debug.Log (iac + " does not have an Interaction Control");
				
			}

			eBookPage[currentPage].StartAnimations ();
			eBookText[currentPage].EnableText ();
			
			Vector3 position = cameraParent.transform.position;
			position.x -= distanceBetweenPages;
			cameraParent.transform.position = position;
			
		}
		
	}



}