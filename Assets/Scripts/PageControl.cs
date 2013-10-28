using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PageControl : MonoBehaviour {
		

	
	private Vector3 gimbalSquared = Vector3.up;
	
	private Vector2 pageRotation = new Vector2(0,0);
	public Vector2 pageRotationLimit = new Vector2(30,20);
	private Vector3 elementPos = Vector3.up;
	
	public Vector2 dampenFactor = new Vector2(0.4f,0.4f);
	
	
	//public List<GameObject> pictureObjects;
	
	public List<GameObject> pictureElement = new List<GameObject>();
	public List<float> pictureRate = new List<float>();

	
	public GimbalGyroAidControl gimbal;
	
	

	
	// Update is called once per frame
	void Update () {
		
		pageRotation.y = gimbal.reportedRotation.y;
		
		pageRotation.x = gimbal.reportedRotation.x;
		
		if (pageRotation.y > pageRotationLimit.y)
		{
			pageRotation.y = pageRotationLimit.y;
		}
		if (pageRotation.y < -(pageRotationLimit.y))
		{
			pageRotation.y = -(pageRotationLimit.y);
		}
		
		
		if (pageRotation.x > pageRotationLimit.x)
		{
			pageRotation.x = pageRotationLimit.x;
		}
		if (pageRotation.x < -(pageRotationLimit.x))
		{
			pageRotation.x = -(pageRotationLimit.x);
		}
		
		
		
		
		float v = pictureElement.Count;
		
		gimbalSquared.x = pageRotation.x * Mathf.Abs(pageRotation.x);
		gimbalSquared.y = pageRotation.y * Mathf.Abs(pageRotation.y);
		
		for (int i = 0; i < v; i ++)
		{
			elementPos.x = gimbalSquared.x * pictureRate[i]* dampenFactor.x;
			elementPos.y = - gimbalSquared.y * pictureRate[i]* dampenFactor.y;
			pictureElement[i].transform.localPosition = elementPos;
			
			
		}
		
	
	}
	
	void OnGUI () {
		
		//GUILayout.Label("RotX: " + pictureElement.Count);
		//GUILayout.Label("RotY: " + i);
		//GUILayout.Label("RotY: " + gimbal.rotY);
	}
	
	
	
}
