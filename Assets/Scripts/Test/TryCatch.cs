using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TryCatch : MonoBehaviour {

	//public List<GameObject> thisList = new List<GameObject>();
	public GameObject[] thisList;

	// Use this for initialization
	void Start () {

		int i;
		int a = 0;
		for ( i = 0; i < thisList.Length; i++ )
		{
			a++;
		}
		Debug.Log(a);
		


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
