using UnityEngine;
using System.Collections;

public class CodeObject : MonoBehaviour {

	private float currentTime;
	private float changedValue;

	void Update () {
		
		currentTime = Time.time;
		changedValue = Mathf.Abs( Mathf.Sin (currentTime));
		
		// The accessable properties are _Diffuse, _RimColor, _RimWidth, _RimPower, _DissolveColor, _DissolveWidth, _EmissiveStrength and _AlphaCutOut
		
		renderer.material.SetFloat("_AlphaCutOut", changedValue);
		Debug.Log (changedValue);
	
	}
}
