using UnityEngine;
using System.Collections;

public class PageCover : MonoBehaviour {
	
	private Vector2 screenSize = new Vector2(0,0);
	private Vector3 screenCoverSize = new Vector3(1,1,1);
	private Transform thisTransform;
	private float aspectRatio; 
	
	public float cameraOrthographicSize = 34f;
	public float screenCoverScale = 1f;
	
	public Texture2D screenCoverTexture;
	

	// Use this for initialization
	void Start () {
		
		thisTransform = transform;
		
		screenSize.x = Screen.width;
		screenSize.y = Screen.height;
		//Debug.Log ("Screen.width" +screenSize.x);
		//Debug.Log ("Screen.height" +screenSize.y);
		aspectRatio = screenSize.x / screenSize.y;
		//Debug.Log ("Aspect Ratio" +aspectRatio);
		
		screenCoverSize.y = (cameraOrthographicSize * 2)* screenCoverScale ;
		screenCoverSize.x = (screenCoverSize.y * aspectRatio) * screenCoverScale;  

		thisTransform.transform.localScale = screenCoverSize;
		Debug.Log ("ScreenCoverSize" +screenCoverSize.x);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
