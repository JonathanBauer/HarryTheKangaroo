using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PageControl : MonoBehaviour {
	
	public bool isCurrent = false;
	
	public Vector3 textTravelDistance = new Vector2(200,0);
	public float textTravelSpeed = 1.0f;
		
	private TextTextureOffset[] ebookText;
	private float[] textStartTime;
	
	private float startPageTextTime = -1.0f;
	private int currentTextObject = -1;
	
	private Vector3 gimbalSquared = Vector3.up;
	
	private Vector2 pageRotation = new Vector2(0,0);
	public Vector2 pageRotationLimit = new Vector2(30,20);
	private Vector3 elementPos = Vector3.up;
	
	public Vector2 dampenFactor = new Vector2(0.4f,0.4f);
	
	
	//public List<GameObject> pictureObjects;
	
	public List<GameObject> pictureElement = new List<GameObject>();
	public List<float> pictureRate = new List<float>();

	
	public GimbalGyroAidControl gimbal;
	
	void Start (){
		
		ebookText = GetComponentsInChildren<TextTextureOffset>();
		
		int textObjectCount;
		
		textObjectCount = ebookText.Length;
		
		textStartTime = new float[textObjectCount];
		
		float sumOfTextTimes = 0f;
		
		if (textObjectCount > 0) {
			
			for (int i = 0; i < textObjectCount; i ++) {
				
				textStartTime[i] = sumOfTextTimes;
				
				sumOfTextTimes += ebookText[i].sentenceTime + (ebookText[i].travelTime *2) + ebookText[i].endSentencePause;
				Debug.Log("Start time for Text Object "+i+" "+ebookText[i]+" is "+textStartTime[i]);
			}
		}

	}
	
	
	// Update is called once per frame
	void Update () {
		
		
		if (Input.GetKeyDown (KeyCode.Alpha9))
		{
				StartPageText ();
		}
		
		if (Input.GetKeyDown (KeyCode.Alpha0))
		{
				ResetPageText ();
		}
		
		
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
		
		
		// if there's a time given to start the text on this page
		if (startPageTextTime > 0)
		{
			
			int textObjectCount;
			
			// Count the text objects (
			textObjectCount = ebookText.Length;
			
			
			// if there are text objects under this page control
			if (textObjectCount > 0) {
				
				
				for (int i = 0; i < textObjectCount; i ++)
				{
					float t = startPageTextTime + textStartTime[i];
		
					// currentTextObject begins at -1 so that the below statement can be true when i is 0			
		
					if (Time.time > (startPageTextTime + textStartTime[i]) && currentTextObject < i)
					{
						currentTextObject = i;
							
						Debug.Log ("TIME to Start Text Object "+ currentTextObject);
						
						ebookText[i].StartText ();
							
					}
					
				}
				
			}
			
		}
			
		
			
		
		/*
		
		if (isCurrent)
		{
			float startTime = Time.time;
			
			for( int i = 0; i < ebookText.Length; i++) 
			{
				Vector3 startPosition = ebookText[i].transform.localPosition;
				
				Vector3 position = new Vector3 (0,0,0);
				
				position.x = Mathf.Lerp (startPosition.x,
								startPosition.x + textTravelDistance.x,
								(Time.time - startTime) * textTravelSpeed  );
				
				position.y = Mathf.Lerp (startPosition.x,
								startPosition.x + textTravelDistance.x,
								(Time.time - startTime) * textTravelSpeed  );
				
				
				ebookText[i].transform.localPosition = position;
			}
			
			isCurrent = false;
		}
		*/
	
	}
	
	public void StartPageText () {
		
		startPageTextTime = Time.time;
		
		Debug.Log ("Page Begun");

	}
	
	public void ResetPageText () {
		
		startPageTextTime = -1;

	}
	
	
	
}
