using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnumTest : MonoBehaviour {

	public enum StoryEvent
	{
		Ignore,
		BackgroundAnimation,
		WaitingForTrigger,
		TriggeredForStory,
		StoryPlayFinished,
		TriggeredByUser,
		PlayingForUser,
		UserPlayFinished
		
		
	}

	public List<StoryEvent> storyEvent = new List<StoryEvent>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
