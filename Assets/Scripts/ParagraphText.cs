using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParagraphText : MonoBehaviour {

	/* ParagraphText allows a GUIText object print text on more that one line
	 * This is only visible when the script starts */

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

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
