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
	private int currentPage = 0;
	private int lastPage = 0;

	public Transform cameraParent;
	public float distanceBetweenPages = 200f;




	// Use this for initialization
	void Start () {

		instances++;
		
		if (instances > 1)
			Debug.Log ("There is more than one Page Manager in the level");
		else
			myInstance = this;

		pageRoot = GameObject.Find("eBookPages");
		eBookPage = pageRoot.GetComponentsInChildren<PageControl>();

		lastPage = eBookPage.Length - 1;

		eBookPage[currentPage].StartAnimations ();


	
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

	public void TriggerMeshTouched ( Collider hit ) {

		int currentPageTriggerMeshCount = eBookPage[currentPage].triggerMesh.Count;

		for (int target = 0; target < currentPageTriggerMeshCount; target++)
		{
			if (hit == eBookPage[currentPage].triggerMesh[target])
			{
				if (debugMode)
					Debug.Log ("The hit object is " + hit + ".It's index is "+target);

				eBookPage[currentPage].TriggerAnimation(target);
			}
		}
	}
}