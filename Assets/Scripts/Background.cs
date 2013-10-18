using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {
		
	public Transform farBackGroundObject;
	public Transform midBackGroundObject;
	
	private Vector3 gimbalSquared = Vector3.up;
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
		
		gimbalSquared.x = gimbal.rotX * Mathf.Abs(gimbal.rotX);
		farBackPos.x = gimbalSquared.x * farBackGroundMovementRate * dampenFactor;
		midBackPos.x = gimbalSquared.x * midBackGroundMovementRate * dampenFactor;
	
		
		gimbalSquared.y = gimbal.rotY * Mathf.Abs(gimbal.rotY);
		farBackPos.y = - gimbalSquared.y * farBackGroundMovementRate * dampenFactor;
		midBackPos.y = - gimbalSquared.y * midBackGroundMovementRate * dampenFactor;


				
		
		farBackGroundObject.transform.localPosition = farBackPos;
		
		
		
		
		midBackGroundObject.transform.localPosition = midBackPos;

	
	}
	void OnGUI () {
		
		GUILayout.Label("RotX: " + gimbal.rotX);
		GUILayout.Label("RotY: " + gimbal.rotY);
		GUILayout.Label("RotY: " + gimbal.rotY);
	}
	
}
