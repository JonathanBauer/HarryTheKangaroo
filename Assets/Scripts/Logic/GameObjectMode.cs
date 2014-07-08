using UnityEngine;
using System.Collections;

public class GameObjectMode : MonoBehaviour {

	public enum ModeList
	{
		BackGroundAnimation,
		StoryAnimation,
		PageTurnPrevious,
		PageTurnNext,
		DragObjectOrigin,
		DragObjectTarget

	}

	public ModeList gameObjectMode = ModeList.StoryAnimation;

	public enum ConnectState
	{
		DragObjOriginPresent,
		DragObjOriginReturned,
		DragObjOriginLifted,
		DragObjOriginNotPresent,
		DragObjTargetNotConnected,
		DragObjTargetConnectionMade,
		DragObjTargetConnected
		
	}
	
	public ConnectState thisConnectState = ConnectState.DragObjTargetNotConnected;

	public int gameObjectID = 0;
	

	void Update () {

		// DRAG OBJECT ORIGIN STATES
		if (thisConnectState == ConnectState.DragObjOriginReturned)
		{
			
			Debug.Log (this + " is returned");
			
			thisConnectState = ConnectState.DragObjOriginPresent;
			
			this.renderer.enabled = true;
			
		}


		if (thisConnectState == ConnectState.DragObjOriginLifted)
		{
			
			Debug.Log (this + " is lifted");
			
			thisConnectState = ConnectState.DragObjOriginNotPresent;
			
			this.renderer.enabled = false;
			
		}


		// DRAG OBJECT TARGET STATES
		
		if (thisConnectState == ConnectState.DragObjTargetConnectionMade)
		{
			
			Debug.Log (this + " is connected");
			
			thisConnectState = ConnectState.DragObjTargetConnected;
			
			renderer.material.SetTextureOffset("_MainTex", new Vector2(0.5f,0f));
			
		}


		
		
	}


}
