using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Background : MonoBehaviour {
		

	
	private Vector3 gimbalSquared = Vector3.up;

	
	private Vector3 elementPos = Vector3.up;
	
	public float dampenFactor = 1f;
	
	
	//public List<GameObject> pictureObjects;
	
	public List<GameObject> pictureElement = new List<GameObject>();
	public List<float> pictureRate = new List<float>();

	
	public GimbalControl gimbal;
	
	

	
	// Update is called once per frame
	void Update () {
		
		float v = pictureElement.Count;
		
		gimbalSquared.x = gimbal.rotX * Mathf.Abs(gimbal.rotX);
		gimbalSquared.y = gimbal.rotY * Mathf.Abs(gimbal.rotY);
		
		for (int i = 0; i < v; i ++)
		{
			elementPos.x = gimbalSquared.x * pictureRate[i]* dampenFactor;
			elementPos.y = - gimbalSquared.y * pictureRate[i]* dampenFactor;
			pictureElement[i].transform.localPosition = elementPos;
			
			
		}
		
	
	}
	
	void OnGUI () {
		
		//GUILayout.Label("RotX: " + pictureElement.Count);
		//GUILayout.Label("RotY: " + i);
		//GUILayout.Label("RotY: " + gimbal.rotY);
	}
	
}
