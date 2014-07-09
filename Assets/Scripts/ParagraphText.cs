using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParagraphText : MonoBehaviour {

	/* ParagraphText allows a GUIText object print text on more that one line
	 * This is only visible when the script starts */

	public List<string> textInput = new List<string>();

	public string textOutput = "";

	public Vector2 textPositionMultiplier = new Vector2 (0,0);

	public Vector2 textPosition = new Vector2 (0,0);



	// Use this for initialization
	void Start () {

		//textPosition = guiText.pixelOffset;

		//textPositionX = guiText.pixelOffset.x * textPositionMultiplier.x;
		//textPositionY = guiText.pixelOffset.y * textPositionMultiplier.y;

		//guiText.pixelOffset = textPosition;

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

	public void MultiplyText ( Vector2 multiplier) {

		textPosition.x = guiText.pixelOffset.x * multiplier.x;
		textPosition.y = guiText.pixelOffset.y * multiplier.y;

		guiText.pixelOffset = textPosition;	// update pixelOffset with multiplied values

		float fontMultiplier = Mathf.Lerp(multiplier.x, multiplier.y, 0.5f); // find the middle of multiplier.x and multiplier.y

		guiText.fontSize = Mathf.FloorToInt( guiText.fontSize * fontMultiplier ); // round the multiplied result down to an integer
		
	}


}
