using UnityEngine;
using System.Collections;

public class WorldGameManager : MonoBehaviour {

	/* A singleton is a design pattern that restricts the instantiation of a class to one object. 
	 * 
	 * 
	 */
	
	static WorldGameManager myInstance;
	static int instances = 0;
	
	public static WorldGameManager Instance
	{
		get
		{
			if (myInstance == null)
				myInstance = FindObjectOfType (typeof(WorldGameManager)) as WorldGameManager;
			
			return myInstance;
		}
	}

	// Use this for initialization
	void Start () {

		instances++;
		
		if (instances > 1)
			Debug.Log ("There is more than one World Game Manager in the level");
		else
			myInstance = this;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
