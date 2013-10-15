using UnityEngine;
using System.Collections;



public class VectorTest : MonoBehaviour {
	
	public Vector3 myVector = Vector3.up;
	
	
void OnGUI()
	{
		GUILayout.Label("My Vector: " + myVector);
		
	}
	
	
	// Update is called once per frame
	void Update () {
		

	}
}
