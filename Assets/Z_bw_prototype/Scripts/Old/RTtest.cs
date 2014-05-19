using UnityEngine;
using System.Collections;
 
public class RTtest : MonoBehaviour 
{	
	public float refresh = 1f;
 
	// Update is called once per frame
	void Start (){
		// this invokes the SimulateRenderTexture method after waiting zero seconds, then every refresh
		InvokeRepeating( "SimulateRenderTexure", 0f, refresh );
	}
 
	void SimulateRenderTexure(){		
		renderer.material.mainTexture = RenderTextureFree.Capture();
	}
}