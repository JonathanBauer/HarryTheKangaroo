using UnityEngine;
using System.Collections;

public class TouchActivate : MonoBehaviour {
	
	
	public bool touched = false;

	// This script needs to be attached to an activation mesh
	// It is a glow effect modelled around a specific detail on the screen
	// The renderable mesh is also the collision
	
	public void HasBeenTouched (){
		
		touched = true;
		
		Debug.Log (transform+" has been touched");
		renderer.enabled = true;
	
	}
}
