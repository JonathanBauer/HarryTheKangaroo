using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PageControl : MonoBehaviour {

	public bool debugMode = false;									// Toggle debug messages

	
	public List<Animation> animationTarget = new List<Animation>();	// All plugged-in animating objects
	private List<GameObjectMode> targetMode = new List<GameObjectMode>();
	public List<float> storyAnimationStartTime = new List<float>();	// The delay before the story-oriented play animation starts
	
	private List<bool> hasIdleAnimation = new List<bool>();			// Does this object have an idle animation that loops on start

	private enum StoryEvent
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

	private List<StoryEvent> storyEvent = new List<StoryEvent>();
	
	private List<float> triggerAnimationTime = new List<float>();

	private bool isValid = true;						// Ensures there are no empty entries before running
	private float startPageTime = -1f;					// Records when the page's StartAnimations() began

	// = new List<float>() etc are assigned to avoid CS0414 warnings. They aren't needed for public lists, but if it might prevent crashes...
	
	void Start () {

		AnimationCheck();

	}
	
	// Update is called once per frame
	void Update () {

		if (startPageTime > -1)
		{
			// Go through each animationTarget
			for (int i=0; i < animationTarget.Count; i++)
			{

				if (storyEvent[i] == StoryEvent.WaitingForTrigger)
				{
					if (Time.time > storyAnimationStartTime[i] + startPageTime)
					{
						animationTarget[i].Stop(); 
						animationTarget[i].Play ("play");
						storyEvent[i] = StoryEvent.TriggeredForStory;

						if (debugMode)
							Debug.Log(animationTarget[i].name + " has begun playing 'play' as a start event");

					}
					
				}

				if (storyEvent[i] == StoryEvent.TriggeredForStory)
				{
					if (Time.time > storyAnimationStartTime[i] + startPageTime + animationTarget[i].animation["play"].length)
					{
						if (debugMode)
							Debug.Log(animationTarget[i].name + " has finished playing 'play' as a start event");
							
						storyEvent[i] = StoryEvent.StoryPlayFinished;
							
						// Restart the idle animation if the animationTarget has one
						if (hasIdleAnimation[i])
							animationTarget[i].Play ("idle");

					}
				}

				if (storyEvent[i] == StoryEvent.TriggeredByUser)
				{
					animationTarget[i].Stop(); 
					animationTarget[i].Play ("play");
					storyEvent[i] = StoryEvent.PlayingForUser;
					triggerAnimationTime[i] = Time.time;

					if (debugMode)
						Debug.Log(animationTarget[i].name + " has begun playing 'play' as a trigger event");											
				}

				if (storyEvent[i] == StoryEvent.PlayingForUser)
				{
					if (Time.time > triggerAnimationTime[i] + startPageTime + animationTarget[i].animation["play"].length)
					{
						if (debugMode)
							Debug.Log(animationTarget[i].name + " has finished playing 'play' as a trigger event");
							
						storyEvent[i] = StoryEvent.UserPlayFinished;
							
						// Restart the idle animation if the animationTarget has one
						if (hasIdleAnimation[i])
							animationTarget[i].Play ("idle");
							
					}

				}

			}
		}



	}

	public void StartAnimations () {

		startPageTime = Time.time;
		for (int i=0; i < animationTarget.Count; i++)
		{
			animationTarget[i].Stop();


			if (storyEvent[i] != StoryEvent.BackgroundAnimation)
				storyEvent[i] = StoryEvent.WaitingForTrigger;


			if (hasIdleAnimation[i])
				animationTarget[i].Play ("idle");

		}

	}

	public void StopAnimations () {

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

	public void TriggerAnimation ( int target) {
		if (isValid)
		{
			if (startPageTime != -1)
			{
				storyEvent[target] = StoryEvent.TriggeredByUser;
			}
		}
	}
	

	public void AnimationCheck () {

		if (storyAnimationStartTime.Count != animationTarget.Count)
		{
			if (debugMode)
				Debug.Log("Animation Target Count is differnet from Story Animation Start Time Count in "+this);
		} 

		for (int i=0; i < animationTarget.Count; i++)
		{

			targetMode.Add(animationTarget[i].GetComponent<GameObjectMode>());

			if (!targetMode[i])
			{
				if (debugMode)
					Debug.Log(animationTarget[i]+" has no GameObjectMode.");

			} else {

				if (targetMode[i].gameObjectMode == GameObjectMode.ModeList.PageTurnNext || 
				    targetMode[i].gameObjectMode == GameObjectMode.ModeList.PageTurnPrevious)
				{
					if (debugMode)
						Debug.Log(animationTarget[i] + "is a PageTurnNext.");

					hasIdleAnimation.Add(false);
					animationTarget[i].animation["play"].wrapMode = WrapMode.Once;
					storyEvent.Add(StoryEvent.WaitingForTrigger);
					triggerAnimationTime.Add(-1f);

				} else if (targetMode[i].gameObjectMode == GameObjectMode.ModeList.BackGroundAnimation)
				{
					if (debugMode)
						Debug.Log(animationTarget[i] + "is a BackGround Animation.");

					if (!animationTarget[i].animation["idle"])
					{
						if (debugMode)
							Debug.Log(animationTarget[i] + "is a BackGround Animation without an idle.");

						hasIdleAnimation.Add(false);
						storyEvent.Add(StoryEvent.Ignore);
						triggerAnimationTime.Add(-1f);

					} else {

						hasIdleAnimation.Add(true);
						animationTarget[i].animation["idle"].wrapMode = WrapMode.Loop;

						storyEvent.Add(StoryEvent.BackgroundAnimation);
						triggerAnimationTime.Add(-1f);

					}
				} else if (targetMode[i].gameObjectMode == GameObjectMode.ModeList.StoryAnimation)
				{
					if (debugMode)
						Debug.Log(animationTarget[i] + "is a Story Animation.");

					if (!animationTarget[i].animation["play"])
					{
						if (debugMode)
							Debug.Log(animationTarget[i] + "is a Story Animation without a play animation.");

						hasIdleAnimation.Add(false);
						storyEvent.Add(StoryEvent.Ignore);
						triggerAnimationTime.Add(-1f);
						
					} else {

						animationTarget[i].animation["play"].wrapMode = WrapMode.Once;

						if (!animationTarget[i].animation["idle"])
						{
							if (debugMode)
								Debug.Log(animationTarget[i] + "is a Story Animation without an idle.");
							hasIdleAnimation.Add(false);

							storyEvent.Add(StoryEvent.WaitingForTrigger);
							triggerAnimationTime.Add(-1f);

							
						} else {
							
							hasIdleAnimation.Add(true);
							animationTarget[i].animation["idle"].wrapMode = WrapMode.Loop;

							storyEvent.Add(StoryEvent.WaitingForTrigger);
							triggerAnimationTime.Add(-1f);
							
						}
					}

				} else {

					if (debugMode)
						Debug.Log(animationTarget[i] + "uses an unrecognized game object mode "+targetMode[i].gameObjectMode);
					hasIdleAnimation.Add(false);
					
					storyEvent.Add(StoryEvent.Ignore);
					triggerAnimationTime.Add(-1f);

				}

			}

		}
	}
	/*
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
	*/
}
