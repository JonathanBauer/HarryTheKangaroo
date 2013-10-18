using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {
		
	public Transform farBackGroundObject;
	public Transform midBackGroundObject;
	
	private Vector3 farBackPos = Vector3.up;
	private Vector3 midBackPos = Vector3.up;
	
	public Vector2 dampenRegionMin = new Vector2(-12,-12);
	public Vector2 dampenRegionMax = new Vector2(12,12);
	
	public float dampenFactor = 1f;
	
	public float farBackGroundMovementRate = 0.1f;
	public float midBackGroundMovementRate = 0.2f;
	
	public GimbalControl gimbal;
	
	

	
	// Update is called once per frame
	void Update () {
		
		if (gimbal.rotX > dampenRegionMin.x && gimbal.rotX < dampenRegionMax.x) {
			farBackPos.x = gimbal.rotX * farBackGroundMovementRate * dampenFactor;
			midBackPos.x = gimbal.rotX * midBackGroundMovementRate * dampenFactor;
		}
		else {
			farBackPos.x = gimbal.rotX * farBackGroundMovementRate;
			midBackPos.x = gimbal.rotX * midBackGroundMovementRate;
		}
		
		if (gimbal.rotY > dampenRegionMin.y && gimbal.rotY < dampenRegionMax.y) {
			farBackPos.y = - gimbal.rotY * farBackGroundMovementRate * dampenFactor;
			midBackPos.y = - gimbal.rotY * midBackGroundMovementRate * dampenFactor;
		}
		else {
			farBackPos.y = - gimbal.rotY * farBackGroundMovementRate;
			midBackPos.y = - gimbal.rotY * midBackGroundMovementRate;
		}
				
		
		farBackGroundObject.transform.localPosition = farBackPos;
		
		
		
		
		midBackGroundObject.transform.localPosition = midBackPos;

	
	}
	void OnGUI () {
		
		GUILayout.Label("RotX: " + gimbal.rotX);
		GUILayout.Label("RotY: " + gimbal.rotY);
	}
	
}
