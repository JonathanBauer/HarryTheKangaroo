using UnityEngine;
using System.Collections;



public class QT_CubeQuatRotate : MonoBehaviour {
	
	
	public enum FunctionOption {
		Invert,
		RotateHalfwayToward,
		JustAngleAxis
		
	
		
		
	}
	
	
	public FunctionOption function;
	

	// Define a delegate type. Note the "Quaternion"'s defined just like you would with a ordinary method
	private delegate Quaternion FunctionDelegate (Quaternion a);
	
	// Define a static variable (all the values are already in it, thus static) of the type FunctionDelegate
	// called functionDelegates. The enumeration is the same order as FunctionOption.
	private static FunctionDelegate[] functionDelegates = {
		Invert,
		RotateHalfwayToward,
		JustAngleAxis
	
	
		
		
	};
	
	
	
	
	public float mouseRate = 30.0f;
	private Vector3 rotateAmount = new Vector3(0,0,0);
	private Vector3 rotationEuler = new Vector3(0,0,0);
	
	public Transform fabulousAssistant;
	public Transform evilHenchman;
	
	void OnGUI()
	{
		GUILayout.Label("Mouse X Axis: " + Input.GetAxis("Mouse X"));
		GUILayout.Label("Mouse y Axis: " + Input.GetAxis("Mouse Y"));
		GUILayout.Label("Rotate Amount: " + rotateAmount);
		GUILayout.Label("transform.eulerangles: " + transform.rotation.eulerAngles);
		GUILayout.Label("Quaternion Angle: " + Quaternion.Angle (transform.rotation, fabulousAssistant.transform.rotation));
		GUILayout.Label("RotateTowards Delta: " + Time.time*10f);
	}
	

	
	
	// Update is called once per frame
	void Update () {
	
	
		// This simply has to use the public enum "function" 
		// I have no idea how to make it work privately
		FunctionDelegate f = functionDelegates[(int)function];
		
		rotateAmount.y = Input.GetAxis("Mouse X") * mouseRate;
		rotateAmount.z = - (Input.GetAxis("Mouse Y") * mouseRate);
		rotateAmount.x = 0;
		rotationEuler = rotationEuler + rotateAmount;
		transform.rotation = Quaternion.Euler (rotationEuler);
		
		
		//fabulousAssistant.transform.rotation = f(transform.rotation );
		
		fabulousAssistant.transform.rotation.SetFromToRotation((transform.rotation * Vector3.up), Vector3.forward );
		
		fabulousAssistant.transform.Rotate(0,0,1);
		
		
		
	
	}
	
	private static Quaternion Invert (Quaternion a) {
		return Quaternion.Inverse(a);
	}
	
	private static Quaternion RotateHalfwayToward (Quaternion a) {
		
		
		return Quaternion.RotateTowards(a, Quaternion.identity,(Time.time*10f));
	}
	
	private static Quaternion JustAngleAxis (Quaternion a) {
		
		// Notice that tempAngle values of 90 and 270 move on the "same axis" but in opposite directions.
		float tempAngle = 90f;
		
		Vector3 tempVector = new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * tempAngle), Mathf.Cos(Mathf.Deg2Rad * tempAngle));
		
		return Quaternion.AngleAxis(a.eulerAngles.y, tempVector);
	}
	


	
		
}
