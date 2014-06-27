using UnityEngine;
using System.Collections;

public class SimpleDragDrop : MonoBehaviour {

	public enum connectState
	{
		NotConnected,
		ConnectionMade,
		Connected
		
	}

	public connectState thisConnectState;



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (thisConnectState == connectState.ConnectionMade)
		{
			
			Debug.Log (this + " is connected");

			thisConnectState = SimpleDragDrop.connectState.Connected;

			renderer.material.SetTextureOffset("_MainTex", new Vector2(0.5f,0f));
			
		}


	}
}
