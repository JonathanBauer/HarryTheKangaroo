using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextTextureOffset : MonoBehaviour {
	
	public float sentenceTime = 0.0f;	// The  calculated length of time the sentence takes to play
	
	public float travelTime = 0.0f;		// The specified time a sentence takes to appear on screen
	public float endSentencePause = 0.5f;	// The specified time a sentence waits once it's finished
	
	private float textStartTime = -1.0f;
	private bool inPlace = false;
	
	private Vector3 startPosition;
	private Vector2 startOffset;
	private Vector2 startScale;
	
	public List<float> wordTime = new List<float>();
	public List<float> offsetForWord = new List<float>();
	public List<float> scaleForWord = new List<float>();
	private bool[] isPlayed;
	
	private Vector2 textureOffset = new Vector2 (0,0);
	private Vector2 textureScale = new Vector2 (0,0);
	
	void Start () {
		
		//Find the last offset entry and record its time as the sentence time
		//Which will be passed to PageControl
		
		int v = wordTime.Count;	
			
		if (v > 0) {
			
			// Unlike .Length, .Count will include entry [0] so 1 must be subtracted to get the last entry
			
			sentenceTime = wordTime[v-1];
			
		}
		
		// Get the starting positions of the object
		
		startPosition = transform.localPosition;
		
		startOffset = renderer.material.GetTextureOffset ("_MultiTex");
		
		startScale = renderer.material.GetTextureScale ("_MultiTex");
		
		// isPlayed array defined here, entries are added in ResetText ()
		
		isPlayed = new bool[wordTime.Count];
			
		
		
		ResetText ();	
		
	}
	
	
	
	
	// Update is called once per frame
	void Update () {
		
		// For testing TextTextureOffset
		/*
		if (Input.GetKeyDown (KeyCode.Alpha9))
		{
				StartText ();
		}
		
		if (Input.GetKeyDown (KeyCode.Alpha0))
		{
				ResetText ();
		}
		*/
		
		// If the text is active
		
		if (textStartTime > -1)
		{
			if (Time.time > (textStartTime + sentenceTime + endSentencePause + travelTime) && inPlace ) {
				
				//Debug.Log("Reseting");
				
				ResetText ();
				
				
			}
			
			// If time has passed for the sentence to play, it's time to pull the text object back to start
			// A Mathf lerp is a nice way to make something travel over a specific time
			
			else if (Time.time > (textStartTime + sentenceTime + endSentencePause) && inPlace ) {
				
				//Debug.Log("Moving out");
				
				transform.localPosition += (startPosition / travelTime) * Time.deltaTime;
				
				
			}
			// Otherwise, once Time.time has passed textStartTime, the travel Time is over. The transform
			// must be locked to 0,0,0
			else if (Time.time > textStartTime && !inPlace) {
				
				//Debug.Log("In Place");
				
				transform.localPosition = new Vector3(0,0,0);
				
				inPlace = true;
				
			}
			
			// Otherwise, the text object must still be moving outwards
		
			else if (Time.time > (textStartTime - travelTime) && !inPlace) {
				
				//Debug.Log("Moving in");
				
				transform.localPosition -= (startPosition / travelTime) * Time.deltaTime;
				
				 
			}
			
			
				
			float v = wordTime.Count;
			
			
			if (v > 0) {
			
				for (int i = 0; i < v; i ++)
				{
					if (Time.time > (wordTime[i] + textStartTime) && isPlayed[i] != true)  {
					//if ((Time.time > (wordTime[i] + textStartTime)) && wordTime[i] != -1)  {
						textureOffset.x = offsetForWord[i];
						//textureOffset.x = textureOffset.x + offsetForWord[i];
						renderer.material.SetTextureOffset ("_MultiTex", textureOffset);
						
						textureScale.x = scaleForWord[i];
						renderer.material.SetTextureScale ("_MultiTex", textureScale);
						isPlayed[i] = true;
						
					}
				}
	
			}
		}
		
		
	
	}
	
	public void StartText (){
		
		// When a start time is given, some time must be allowed for the text to travel into the picture
		
		textStartTime = Time.time + travelTime;
	}
	
	
	public void ResetText () {
		
		
		textStartTime = -1;
		
		float v = wordTime.Count;
			
			
		if (v > 0) {
				
			for (int i = 0; i < v; i ++)
			{
				isPlayed[i] = false;
					
			}
			
		}
		
		renderer.material.SetTextureOffset ("_MultiTex", startOffset);
		
		renderer.material.SetTextureScale ("_MultiTex", startScale);
			
		transform.localPosition = startPosition;
		
		inPlace = false;
	}
	
	
}
