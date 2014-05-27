using UnityEngine;
using System.Collections;

public class TestText : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		/*
		public static Rect FormatGuiTextArea(GUIText guiText, float maxAreaWidth)
		{
			string[] words = guiText.text.Split(' ');

			// Split separates strings. This makes a string array called words and separates using spaces

			string result = "";
			Rect textArea = new Rect();
			
			for(int i = 0; i < words.Length; i++)
			{
				// set the gui text to the current string including new word
				guiText.text = (result + words[i] + " ");
				// measure it
				textArea = guiText.GetScreenRect();
				// if it didn't fit, put word onto next line, otherwise keep it
				if(textArea.width > maxAreaWidth)
				{
					result += ("\n" + words[i] + " ");
				}
				else
				{
					result = guiText.text;
				}
			}
			return textArea;
		}
		*/

		// So using this kind of code, I can measure the width of each word and declare regions that can be touched.
	
	}
}
