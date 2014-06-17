using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextControl : MonoBehaviour {

	public List<string> textInput = new List<string>();

	public string textOutput = "";

	// Use this for initialization
	void Start () {

		for (int i=0; i < textInput.Count; i++)
		{
			textOutput += textInput[i];
			textOutput +="\n";
		}

		guiText.text = textOutput;

		guiText.enabled = false;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void EnableText () {
		
		guiText.enabled = true;
		
	}

	public void DisableText () {
		
		guiText.enabled = false;
		
	}
}
