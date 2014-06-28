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

	public bool debugMode = true;

	private GameObject pageRoot;
	public PageControl[] eBookPage;
	public int currentPage = 0;
	public int lastPage = 0;

	public Transform cameraParent;
	public float distanceBetweenPages = 200f;

	private GameObject textRoot;
	public TextControl[] eBookText;




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

		lastPage = eBookPage.Length - 1;

		eBookPage[currentPage].StartAnimations ();
		eBookText[currentPage].EnableText ();


	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Alpha4))
		{
			if (debugMode)
				Debug.Log ("PageTurnPrevious");

			if (currentPage == 0) {
				if (debugMode)
					Debug.Log ("You're on the first page");
			} else {

				eBookPage[currentPage].StopAnimations ();
				currentPage --;
				eBookPage[currentPage].StartAnimations ();

				Vector3 position = cameraParent.transform.position;
				position.x -= distanceBetweenPages;
				cameraParent.transform.position = position;
			}

			




		}

		if (Input.GetKeyDown (KeyCode.Alpha5))
		{
			if (debugMode)
				Debug.Log ("PageTurnNext");

			if (currentPage == lastPage) {
				if (debugMode)
					Debug.Log ("You're on the last page");
			} else {

				eBookPage[currentPage].StopAnimations ();
				currentPage ++;
				eBookPage[currentPage].StartAnimations ();

				Vector3 position = cameraParent.transform.position;
				position.x += distanceBetweenPages;
				cameraParent.transform.position = position;

			}



			
		}
	
	
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
			
			eBookPage[currentPage].StopAnimations ();
			eBookText[currentPage].DisableText ();

			currentPage ++;
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
			
			eBookPage[currentPage].StopAnimations ();
			eBookText[currentPage].DisableText ();
			currentPage --;
			eBookPage[currentPage].StartAnimations ();
			eBookText[currentPage].EnableText ();
			
			Vector3 position = cameraParent.transform.position;
			position.x -= distanceBetweenPages;
			cameraParent.transform.position = position;
			
		}
		
	}
}