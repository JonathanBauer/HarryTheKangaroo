using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextControl : MonoBehaviour {

	public bool debugMode = false;

	public ParagraphText[] paragraphTexts;		// All the GUIText objects that are children of the text control
	
	public string textOutput = "";				// Not implemented - The string that all the paragraphs will display


	// Use this for initialization
	void Start () {

		paragraphTexts = this.GetComponentsInChildren<ParagraphText>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void EnableText () {

		for (int i=0; i < paragraphTexts.Length; i++)
		{
			if (debugMode)
				Debug.Log(this.name + " text is enabled.");

			paragraphTexts[i].guiText.enabled = true;
		}
	
		
	}

	public void DisableText () {

		for (int i=0; i < paragraphTexts.Length; i++)
		{
			if (debugMode)
				Debug.Log(this.name + " text is disabled.");

			paragraphTexts[i].guiText.enabled = false;
		}
		
	
		
	}
}
