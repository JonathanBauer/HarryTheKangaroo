using UnityEngine;
using System.Collections;
 
public class RTtest02 : MonoBehaviour 
{	
	public float refresh = 1f;
	
	private static Texture2D tex2D;
	private static Texture tex;
	private Rect myRectangle = new Rect( 0f, 0f, Screen.width, Screen.height );
 
	// Update is called once per frame
	void Start (){
		RenderTextureFree();
	}
 
	void RenderTextureFree(){		
		renderer.material.mainTexture = RenderTextureFreeCapture();
		Debug.Log (RenderTextureFreeCapture());
	}
	
	// Return the entire screen in a texture
	public static Texture RenderTextureFreeCapture(){
		return RenderTextureFreeCapture( new Rect( 0f, 0f, Screen.width, Screen.height ), 0, 0 );
	}
	
	// Return part of the screen in a texture.
	public static Texture RenderTextureFreeCapture( Rect captureZone, int destX, int destY )
	{
		Texture2D result;
		//UnityEngine.Texture2D.Texture2D(int,int,UnityEngine.TextureFormat,bool)
		result = new Texture2D( Mathf.RoundToInt( captureZone.width ) + destX,
					Mathf.RoundToInt( captureZone.height ) + destY,
					TextureFormat.RGB24, false);
		result.ReadPixels(captureZone, destX, destY, false);
 
		// That's the heavy part, it takes a lot of time.
		result.Apply();
 
		return result;
	}
	
	
}