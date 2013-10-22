using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextureOffset : MonoBehaviour {
	
	public List<float> wordTime = new List<float>();
	public List<float> offsetForWord = new List<float>();
	public List<float> scaleForWord = new List<float>();
	
	private Vector2 textureOffset = new Vector2 (0,0);
	private Vector2 textureScale = new Vector2 (0,0);
	
	
	// Update is called once per frame
	void Update () {
		
		
		float v = wordTime.Count;
		
		if (v > 0) {
		
			for (int i = 0; i < v; i ++)
			{
	
				if (Time.time > wordTime[i] && wordTime[i] != -1)  {
					textureOffset.x = offsetForWord[i];
					//textureOffset.x = textureOffset.x + offsetForWord[i];
					renderer.material.SetTextureOffset ("_MultiTex", textureOffset);
					
					textureScale.x = scaleForWord[i];
					renderer.material.SetTextureScale ("_MultiTex", textureScale);
					wordTime[i] = -1;
					
				}
			}
		}
		
		
	
	}
	
}
