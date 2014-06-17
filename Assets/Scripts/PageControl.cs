using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PageControl : MonoBehaviour {

	public bool debugMode = false;									// Toggle debug messages

	
	public List<Animation> animationTarget = new List<Animation>();	// All plugged-in animating objects	
	public List<float> storyAnimationStartTime = new List<float>();	// The delay before the story-oriented play animation starts
	public List<MeshCollider> triggerMesh = new List<MeshCollider>();	// All plugged-in animating objects
	
	public List<bool> hasIdleAnimation = new List<bool>();			// Does this object have an idle animation that loops on start
	
	public enum StoryEvent
	{
		NoPlayAnimation,
		WaitingForTrigger,
		TriggeredForStory,
		StoryPlayFinished,
		TriggeredByUser,
		PlayingForUser,
		UserPlayFinished


	}

	public List<int> storyEvent = new List<int>();
	
	public List<float> triggerAnimationTime = new List<float>();

	private bool isValid = true;						// Ensures there are no empty entries before running
	public float startPageTime = -1f;					// Records when the page's StartAnimations() began

	// = new List<float>() etc are assigned to avoid CS0414 warnings. They aren't needed for public lists, but if it might prevent crashes...
	
	void Start () {

		ValidEntryCheck();
	
		if (isValid)
		{
			AnimationCheck();

			TriggerMeshCheck();
		
		}

	}
	
	// Update is called once per frame
	void Update () {

		if (isValid) 
		{

			if (startPageTime > -1)
			{
				// Go through each animationTarget
				for (int i=0; i < animationTarget.Count; i++)
				{

					if (storyEvent[i] == (int)StoryEvent.WaitingForTrigger)
					{
						if (Time.time > storyAnimationStartTime[i] + startPageTime)
						{
							animationTarget[i].Stop(); 
							animationTarget[i].Play ("play");
							storyEvent[i] = (int)StoryEvent.TriggeredForStory;

							if (debugMode)
								Debug.Log(animationTarget[i].name + " has begun playing 'play' as a start event");

						}
					
					}

					if (storyEvent[i] == (int)StoryEvent.TriggeredForStory)
					{
						if (Time.time > storyAnimationStartTime[i] + startPageTime + animationTarget[i].animation["play"].length)
						{
							if (debugMode)
								Debug.Log(animationTarget[i].name + " has finished playing 'play' as a start event");
							
							storyEvent[i] = (int)StoryEvent.StoryPlayFinished;
							
							// Restart the idle animation if the animationTarget has one
							if (hasIdleAnimation[i])
								animationTarget[i].Play ("idle");

						}
					}

					if (storyEvent[i] == (int)StoryEvent.TriggeredByUser)
					{
						animationTarget[i].Stop(); 
						animationTarget[i].Play ("play");
						storyEvent[i] = (int)StoryEvent.PlayingForUser;
						triggerAnimationTime[i] = Time.time;

						if (debugMode)
							Debug.Log(animationTarget[i].name + " has begun playing 'play' as a trigger event");											
					}

					if (storyEvent[i] == (int)StoryEvent.PlayingForUser)
					{
						if (Time.time > triggerAnimationTime[i] + startPageTime + animationTarget[i].animation["play"].length)
						{
							if (debugMode)
								Debug.Log(animationTarget[i].name + " has finished playing 'play' as a trigger event");
							
							storyEvent[i] = (int)StoryEvent.UserPlayFinished;
							
							// Restart the idle animation if the animationTarget has one
							if (hasIdleAnimation[i])
								animationTarget[i].Play ("idle");
							
						}

					}

				}
			}
		}


	}

	public void StartAnimations () {
		if (isValid)
		{
			startPageTime = Time.time;
			for (int i=0; i < animationTarget.Count; i++)
			{
				animationTarget[i].Stop();


				if (storyEvent[i] != (int)StoryEvent.NoPlayAnimation)
					storyEvent[i] = (int)StoryEvent.WaitingForTrigger;


				if (hasIdleAnimation[i])
					animationTarget[i].Play ("idle");

			}
		}
	}

	public void StopAnimations () {
		if (isValid)
		{
			startPageTime = -1f;
			for (int i=0; i < (animationTarget.Count); i++)
			{
				// If this target has a play animation, set it's time to 0, enable the play animation, set the play animation weight to full
				// and then sample this frame.
				// This basically resets the rig to the first frame of the play animation, which is necessary if there's no idle animation
				// such as in page turn icons
				if (animationTarget[i].animation["play"])
				{
					animationTarget[i].animation["play"].normalizedTime = 0f;
					animationTarget[i].animation["play"].enabled = true;
					animationTarget[i].animation["play"].weight = 1;
					animationTarget[i].Sample();
				}

				animationTarget[i].Stop(); 


			

				
			}
		}
	}

	public void TriggerAnimation ( int target) {
		if (isValid)
		{
			if (startPageTime != -1)
			{
				storyEvent[target] = (int)StoryEvent.TriggeredByUser;
			}
		}
	}

	public void ValidEntryCheck () {

		// Validation check to ensure every entry in animationTarget is an Animation
		for (int i=0; i < animationTarget.Count; i++)
		{
			if (!animationTarget[i])
			{
				if (debugMode)
					Debug.Log(this.name + " has a null entry at index "+i+". This page is invalid.");
				isValid = false;
				
			}
			
		}
	
	}

	public void AnimationCheck () {

		for (int i=0; i < animationTarget.Count; i++)
		{
			
			// Check that every animationTarget has an idle animation and make it's boolean true if it does
			if (!animationTarget[i].animation["idle"])
			{
				if (debugMode)
					Debug.Log(animationTarget[i].name + " has no idle animation.");
				
				hasIdleAnimation.Add(false);
				
			} else {
				
				// There's an idle animation. Set the wrapmode to Loop.
				if (debugMode)
					Debug.Log(animationTarget[i].name + " has a idle animation.");

				hasIdleAnimation.Add(true);
				animationTarget[i].animation["idle"].wrapMode = WrapMode.Loop;
				
			}
			
			// Check that every animationTarget has a play animation.
			
			if (!animationTarget[i].animation["play"])
			{
				
				// If it doesn't, set it's storyEvent enum to No Play Animation so that it never plays
				int thisEvent = (int)StoryEvent.NoPlayAnimation;
				storyEvent.Add(thisEvent);
				
				// animationStartTime.Count must have 1 subtracted, because the first array entry starts at 0
				// If there isn't a start time entry, create one with -1 in it to prevent playing "play"
				// If the is a start time entry. Change it to -1.
				
				if (i > (storyAnimationStartTime.Count - 1))
				{
					if (debugMode)
						Debug.Log(animationTarget[i].name + " has no play animation. Adding -1 Start Time");
					
					storyAnimationStartTime.Add(-1f);
					
				} else {

					if (debugMode)
						Debug.Log(animationTarget[i].name + " has no play animation. Setting Start Time to -1");
					
					storyAnimationStartTime[i] = -1f;
					
				}
				
			} else {
				
				// If it does, set the wrapmode to once.
				animationTarget[i].animation["play"].wrapMode = WrapMode.Once;
				
				// set it's storyEvent to Waiting for Trigger so that it's ready to play
				int thisEvent = (int)StoryEvent.WaitingForTrigger;
				storyEvent.Add(thisEvent);
				
				if (debugMode)
					Debug.Log(animationTarget[i].name+ " has a play animation. It is "+animationTarget[i].animation["play"].length+" seconds long");
				
				if (i > (storyAnimationStartTime.Count - 1))
				{
					// If there isn't a start time entry, create one with 2 in it so that the "play" animation is seen.
					storyAnimationStartTime.Add(2);

						
					if (debugMode)
						Debug.Log(animationTarget[i].name+ " has a play animation but no start time entry. Creating 2 second entry.");		
				} 	
			}
		}
	}

	public void TriggerMeshCheck () {

		for (int i=0; i < animationTarget.Count; i++)
		{
			
			triggerAnimationTime.Add(-1);
			
			// Perform a check on each triggerMesh to find out if there is a triggerMesh present
			if (i > (triggerMesh.Count - 1))
			{
				if (debugMode)
					Debug.Log(this.name + " has a no MeshCollider entry at index "+i+". Creating a null entry");
				triggerMesh.Add(null);
				
			} else if (triggerMesh[i] == null)
			{
				
				if (debugMode)
					Debug.Log(this.name + " has a null MeshCollider entry in index "+i+".");
				
			}
			
		}
		
	}
}
