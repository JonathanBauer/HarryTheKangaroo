using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextControl : MonoBehaviour {

	public ParagraphText[] paragraphTexts;



	public string textOutput = "";

	// Use this for initialization
	void Start () {

		paragraphTexts = this.GetComponentsInChildren<ParagraphText>();

		DisableText ();

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void EnableText () {

		for (int i=0; i < paragraphTexts.Length; i++)
		{
			paragraphTexts[i].guiText.enabled = true;
		}
	
		
	}

	public void DisableText () {

		for (int i=0; i < paragraphTexts.Length; i++)
		{
			paragraphTexts[i].guiText.enabled = false;
		}
		
	
		
	}
}
