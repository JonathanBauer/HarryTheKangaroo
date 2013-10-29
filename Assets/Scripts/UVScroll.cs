using UnityEngine;
using System.Collections;

public class UVScroll : MonoBehaviour {
	
	
	
	public Vector2 scrollSpeed = new Vector2(0,0);
	
	private Vector2 uvPosition;

	
	// Update is called once per frame
	void Update () {
		
		uvPosition = Time.time * scrollSpeed;
		renderer.material.mainTextureOffset = uvPosition;
	
	}
}
