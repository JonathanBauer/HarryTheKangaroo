using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextControl : MonoBehaviour {

	public bool debugMode = false;

	public ParagraphText[] paragraphTexts;		// All the GUIText objects that are children of the text control
	
	public string textOutput = "";				// Not implemented - The string that all the paragraphs will display

	private Vector2 currentScreenResolution = new Vector2 (0,0);

	public Vector2 textPositionMultiplier = new Vector2 (0,0);

	// Use this for initialization
	void Start () {

		currentScreenResolution = new Vector2 ( Screen.width, Screen.height);

		// The Text Setup Resolution on PageManager will always be 640 x 426. The GUI Text is positioned based on this.
		// If the resolution is larger, how much does it need to be multiplied by on each axis?

		textPositionMultiplier.x = currentScreenResolution.x / PageManager.Instance.textSetupResolution.x;
		textPositionMultiplier.y = currentScreenResolution.y / PageManager.Instance.textSetupResolution.y;

		for (int i=0; i < paragraphTexts.Length; i++)
		{
			// Each Paragraph text will multiply their pixel offsets and font sizes based on this
			paragraphTexts[i].MultiplyText ( textPositionMultiplier );
		}

		paragraphTexts = this.GetComponentsInChildren<ParagraphText>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void EnableText () {

		for (int i=0; i < paragraphTexts.Length; i++)
		{
			if (debugMode)
				Debug.Log(paragraphTexts[i].name + " text is enabled.");

			paragraphTexts[i].guiText.enabled = true;


		}
	
		
	}

	public void DisableText () {

		for (int i=0; i < paragraphTexts.Length; i++)
		{
			if (debugMode)
				Debug.Log(paragraphTexts[i].name + " text is disabled.");

			paragraphTexts[i].guiText.enabled = false;
		}
		
	
		
	}
}
