using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PageControl : MonoBehaviour {

	public bool debugMode = false;									// Toggle debug messages

	
	public List<Animation> animationTarget = new List<Animation>();	// All plugged-in animating objects
	public List<float> animationStartTime = new List<float>();		// The delay before the story-oriented play animation starts
	public List<MeshCollider> triggerMesh = new List<MeshCollider>();	// All plugged-in animating objects

	private List<bool> hasIdleAnimation = new List<bool>();			// Does this object have an idle animation that loops on start
	private List<bool> playAnimationStarted = new List<bool>();		// Check for the play animation starting due to animationStartTime
	private List<bool> playAnimationFinished = new List<bool>();		// Check for the play animation finishing due to animationStartTime
	private List<bool> triggeredAnimationStarted = new List<bool>();	// Toggle the animation starting due to user interaction.

	private bool isValid = true;						// Ensures there are no empty entries before running
	private float startPageTime = -1f;					// Records when the page's StartAnimations() began

	// = new List<bool>() etc are assigned to avoid CS0414 warnings. They aren't needed for public lists, but if it might prevent crashes...

	// Use this for initialization
	void Start () {

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
	
		if (isValid)
		{
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
					hasIdleAnimation.Add(true);
					animationTarget[i].animation["idle"].wrapMode = WrapMode.Loop;

				}
			
				// Check that every animationTarget has a play animation.
				// If it doesn't, check if there is a corresponding start time entry.

				if (!animationTarget[i].animation["play"])
				{
					if (debugMode)
						Debug.Log(animationTarget[i].name + " has no play animation.");

					// animationStartTime.Count must have 1 subtracted, because the first array entry starts at 0
					// If there isn't a start time entry, create one with -1 in it to prevent playing "play"
					// If the is a start time entry. Change it to -1.

					if (i > (animationStartTime.Count - 1))
					{

						animationStartTime.Add(-1f);

					} else {

						animationStartTime[i] = -1f;
					}
					
				} else {

					// If it does, set the wrapmode to once.
					animationTarget[i].animation["play"].wrapMode = WrapMode.Once;

					if (i > (animationStartTime.Count - 1))

					{
						// If there isn't a start time entry, create one with 2 in it so that the "play" animation is seen.
						animationStartTime.Add(2);

						if (debugMode)
							Debug.Log(animationTarget[i].name+ " has a play animation but no start time entry. Creating 2 second entry.");
						
					} 
				
				}
			
				// Make a trigger bool for this animation to record if it's started and finished.
				// Also give an de-activated trigger time.
				playAnimationStarted.Add(false);
				playAnimationFinished.Add(false);
				triggeredAnimationStarted.Add(false);

				// Perform a check on each triggerMesh to find out if there is one there
				if (i > (triggerMesh.Count - 1))
				{
					if (debugMode)
						Debug.Log(this.name + " has a no MeshCollider entry at index "+i+". Creating a null entry");
					triggerMesh.Add(null);
						
				}

				if (triggerMesh[i] == null)
				{
					
					if (debugMode)
						Debug.Log(this.name + " has a null MeshCollider entry in index "+i+".");

				}


			
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (isValid) 
		{

			// Debug triggers
			if (Input.GetKeyDown (KeyCode.Alpha0))
			{
				if (debugMode)
					Debug.Log ("START");

				StartAnimations ();
			}

			if (Input.GetKeyDown (KeyCode.Alpha9))
			{
				if (debugMode)
					Debug.Log ("STOP");
				StopAnimations ();
			}

			if (Input.GetKeyDown (KeyCode.Alpha1))
			{
				if (debugMode)
					Debug.Log ("TRIGGER 1");

				if ((startPageTime != -1) && (triggerMesh[0] != null))
					triggeredAnimationStarted[0] = true;

			}

			if (Input.GetKeyDown (KeyCode.Alpha2))
			{
				if (debugMode)
					Debug.Log ("TRIGGER 2");

				if ((startPageTime != -1) && (triggerMesh[1] != null))
					triggeredAnimationStarted[1] = true;
			}

			if (Input.GetKeyDown (KeyCode.Alpha3))
			{
				if (debugMode)
					Debug.Log ("TRIGGER 3");

				if ((startPageTime != -1) && (triggerMesh[2] != null))
					triggeredAnimationStarted[2] = true;
			}
			
			// Go through each animationTarget
			if (startPageTime > -1)
			{
				for (int i=0; i < animationTarget.Count; i++)
				{

					// If the time passes the target's start time, the play animation has not been triggered and it actually has a start time
					if ((Time.time > animationStartTime[i] + startPageTime) && !playAnimationStarted[i] && animationStartTime[i] != -1)
					{
						animationTarget[i].Play ("play");
						playAnimationStarted[i] = true;
						if (debugMode)
							Debug.Log(animationTarget[i].name + " has begun playing 'play' as a start event");
					}

					if (triggeredAnimationStarted[i] && animationStartTime[i] != -1)
					{
						animationTarget[i].Stop ();
						animationTarget[i].Play ("play");
						if (debugMode)
							Debug.Log(animationTarget[i].name + " has begun playing 'play' as a triggered event");
						triggeredAnimationStarted[i] = false;

					}

					// If the play animation has been triggered, the play animation has finished playing and the finish declaration hasn't happened
					if (playAnimationStarted[i] && !animationTarget[i].IsPlaying("play") && !playAnimationFinished[i])
					{
						if (debugMode)
							Debug.Log(animationTarget[i].name + " has finished playing 'play' as a start event");
						playAnimationFinished[i] = true;

						// Restart the idle animation if the animationTarget has one
						if (hasIdleAnimation[i])
							animationTarget[i].Play ("idle");
							
					}

					if (playAnimationStarted[i] && !animationTarget[i].IsPlaying("play") && !playAnimationFinished[i])
					{
						if (debugMode)
							Debug.Log(animationTarget[i].name + " has finished playing 'play' as a triggered event");
						
						// Restart the idle animation if the animationTarget has one
						if (hasIdleAnimation[i])
							animationTarget[i].Play ("idle");
						
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
				if (hasIdleAnimation[i])
					animationTarget[i].Play ("idle");

			}
		}
	}

	public void StopAnimations () {
		if (isValid)
		{
			startPageTime = -1f;
			for (int i=0; i < animationTarget.Count; i++)
			{
				animationTarget[i].Stop(); 
				playAnimationStarted[i] = false;
				playAnimationFinished[i] = false;
			
			}
		}
	}

	public void TriggerAnimation ( int target) {
		if (isValid)
		{
			if (startPageTime != -1)
				triggeredAnimationStarted[target] = true;
		}
	}
}
