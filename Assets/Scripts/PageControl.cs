using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PageControl : MonoBehaviour {

	public bool debugMode = true;
	public float startPageTime = -1f;
	
	public List<Animation> animationTarget;
	public List<float> animationStartTime;
	//public List<float> animationLength;
	public List<bool> playAnimationTriggered;
	public List<bool> playAnimationFinished;

	// Use this for initialization
	void Start () {
	
		// Start all animations
		for (int i=0; i < animationTarget.Count; i++)
		{
			if (!animationTarget[i].animation["play"])
			{
				if (debugMode)
					Debug.Log(animationTarget[i].name + " has no play animation.");
				
			} else {
				
				animationTarget[i].animation["play"].wrapMode = WrapMode.Once;

				// Check that the animation clip has a corresponding start time. 
				// animationStartTime.Length must have 1 subtracted, because the first array entry starts at 0
				if (i > (animationStartTime.Count - 1))
				{
					// If there is no start time for this clip, a new entry is added to animationStartTime[].
					// A value of -1 indicates that animation clip "play" will never play when the target is triggered.
					animationStartTime.Add(2);
					//animationLength.Add(-1);

					if (debugMode)
						Debug.Log(animationTarget[i].name+ " has a play animation but no start time entry. Creating 2 second entry.");
					
				} 
			
			}

			// Make a trigger bool for this animation to record when it's triggered
			playAnimationTriggered.Add(false);
			playAnimationFinished.Add(false);

		}

	}
	
	// Update is called once per frame
	void Update () {

		// Debug triggers
		if (Input.GetKeyDown (KeyCode.Alpha0))
		{
			if (debugMode)
				Debug.Log ("START");
			startPageTime = Time.time;
			StartAnimations ();
		}

		if (Input.GetKeyDown (KeyCode.Alpha1))
		{
			Debug.Log ("TRIGGER 1");
		}

		if (Input.GetKeyDown (KeyCode.Alpha2))
		{
			Debug.Log ("TRIGGER 2");
		}

		if (Input.GetKeyDown (KeyCode.Alpha3))
		{
			Debug.Log ("TRIGGER 3");
		}

		if (Input.GetKeyDown (KeyCode.Alpha4))
		{
			Debug.Log (animationTarget[0].animation["idle"]);
			Debug.Log (animationTarget[2].animation["idle"]);
		}


		
		// Go through each animationTarget
		if (startPageTime > -1)
		{
			for (int i=0; i < animationTarget.Count; i++)
			{
				// If the time passes the target's start time, the play animation has not been triggered and it actually has a start time
				if ((Time.time > animationStartTime[i] + startPageTime) && !playAnimationTriggered[i] && animationStartTime[i] != -1)
				{
					animationTarget[i].Play ("play");
					playAnimationTriggered[i] = true;
				}
				// If the play animation has been triggered, the play animation has finished playing and the finish declaration hasn't happened
				if (playAnimationTriggered[i] && !animationTarget[i].IsPlaying("play") && !playAnimationFinished[i])
				{
					if (debugMode)
						Debug.Log(animationTarget[i].name + " has finished playing 'play'");
					playAnimationFinished[i] = true;

					// Restart the idle animation if the animationTarget has one
					if (!animationTarget[i].animation["idle"])
					{
						if (debugMode)
							Debug.Log(animationTarget[i].name + " has no idle animation. Skipping,");
						
					} else {

						animationTarget[i].Play ("idle");
						
					}


				}

			}
		}


	}

	void StartAnimations () {

		for (int i=0; i < animationTarget.Count; i++)
		{
			if (!animationTarget[i].animation["idle"])
			{
				if (debugMode)
					Debug.Log(animationTarget[i].name + " has no idle animation. Skipping,");

			} else {
			
				animationTarget[i].animation["idle"].wrapMode = WrapMode.Loop;
				animationTarget[i].Play ("idle");

			}
		}
	}
}
