using UnityEngine;
using System.Collections;

public class FaceMe : MonoBehaviour {

	public Camera cameraTarget;

	public bool turnAroundFaceMe = false;

	// Use this for initialization
	void Start () {
		if (!cameraTarget)
			Debug.Log(this.name + " is using FaceMe without a camera.");
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (cameraTarget)
			transform.rotation = cameraTarget.transform.rotation;

		if (turnAroundFaceMe)
		{
			Vector2 pos = new Vector2(0,0);

			if (TiltManager.Instance.tiltAngle.x < -15)
			{
				pos.x = -0.25f;
			} else if (TiltManager.Instance.tiltAngle.x > 15)
			{
				pos.x = 0.25f;
			} else 
			{
				pos.x = 0f;
			}
			renderer.material.mainTextureOffset = pos;
		}
	
	}
}
