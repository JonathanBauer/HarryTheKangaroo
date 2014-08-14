using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AlphaFadeAnimate : MonoBehaviour {

	public bool debugMode = true;

	private SkinnedMeshRenderer[] bonedMesh;			// All SkinnedMeshRenderers inside the prefab
	private bool hasBonedMesh;							// Confirms a SkinnedMeshRenderer is present	

	private Material FADMaterial;						// To store the material prefixed "FAD" for alpha-fading

	private Transform FADAnimatedBone;					// The animated bone in the prefab with prefix "FAD" to relay alpha strength

	private float FADInitialYPos;						// The (world) position "FAD" animated bone at rest (y axis)

	private Transform[] childTransform;					// temporary store of all objects in the prefab's heirachy

	public float ypos;

	public float yPosAmplify = 1f;

	// Use this for initialization
	void Start () {

		if (debugMode)
			Debug.Log(this+" has debug mode turned on.");

		FindBonedMesh();

		FindFADMaterial();

		FindFADAnimatedBone();
	
	}
	
	// Update is called once per frame
	void Update () {
		
		// If there's a UVA animated bone, then obviously there's a UVA Material to animate, so start animating
		if (FADAnimatedBone)
		{
			ypos = FADAnimatedBone.position.y - FADInitialYPos;

			ypos *= yPosAmplify;

			FADMaterial.SetFloat("_AlphaStrength", ypos);
		}
		
	}

	void FindBonedMesh () {
		
		bonedMesh = this.GetComponentsInChildren<SkinnedMeshRenderer>();
		
		if (bonedMesh.Length > 0)
		{
			hasBonedMesh = true;
			//Debug.Log ("Has SkinnedMeshRenderer");
			
		} else {
			if (debugMode)
				Debug.Log(this+" has no SkinnededMeshRenderer. Ignoring");
			hasBonedMesh = false;
		}
		
	}

	void FindFADMaterial () {
		
		if(hasBonedMesh)
		{
			// Search all SkinnedMeshRenderers (There will always be one)
			for (int mesh = 0; mesh < bonedMesh.Length; mesh++)
			{
				// Search materials in this SkinnedMeshRenderer
				for (int mat = 0; mat < bonedMesh[mesh].materials.Length; mat++)
				{
					// Find out if it is FAD
					if (bonedMesh[mesh].materials[mat].name.IndexOf("FAD") != -1)
					{
						FADMaterial = bonedMesh[mesh].materials[mat];
						if (debugMode)
							Debug.Log("FAD Materials is "+ FADMaterial);
					}			
				}
			}		
		}	
	}

	void FindFADAnimatedBone (){
		// put all transforms into childTransform
		childTransform = this.GetComponentsInChildren<Transform>();
		
		for (int i=0; i < childTransform.Length; i++)
		{
			// if the transform has the FAD prefix, if there is a UVAMaterial and there's no current UVA Animated Bone
			if (childTransform[i].name.IndexOf("FAD") > -1 && FADMaterial && !FADAnimatedBone)
			{
				FADAnimatedBone = childTransform[i];
				FADInitialYPos = childTransform[i].position.y;
				if (debugMode)
					Debug.Log(FADAnimatedBone.name);
			}

		}
		
	}
}
