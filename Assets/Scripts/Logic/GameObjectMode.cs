using UnityEngine;
using System.Collections;

public class GameObjectMode : MonoBehaviour {

	public enum ModeList
	{
		BackGroundAnimation,
		StoryAnimation,
		PageTurnPrevious,
		PageTurnNext,
		DragObject,

	}

	public ModeList gameObjectMode = ModeList.StoryAnimation;


}
