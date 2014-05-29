using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UVAnimateAuto : MonoBehaviour {

	public bool debugMode = false;

	public Vector2 tileSize = new Vector2(4,4);			// The number of tiles on the animated material texture

	private SkinnedMeshRenderer[] bonedMesh;			// All SkinnedMeshRenderers inside the prefab
	private bool hasBonedMesh;							// Confirms a SkinnedMeshRenderer is present		

	private Material UVAMaterial;						// To store the material prefixed "UVA" for animating
	private Material UVBMaterial;						// To store the material prefixed "UVB" for animating
														// "UVC", "UVD" can be added easily

	private Transform UVAAnimatedBone;					// The animated bone in the prefab with prefix "UVA" to relay texture offset
	private Transform UVBAnimatedBone;					// The animated bone in the prefab with prefix "UVB" to relay texture offset
	private Vector3 UVAInitialPos;						// The (world) position "UVA" animated bone at rest
	private Vector3 UVBInitialPos;						// The (world) position "UVB" animated bone at rest

	private Transform[] childTransform;					// temporary store of all objects in the prefab's heirachy


	void Start () {

		FindBonedMesh();

		FindUVMaterials();

		FindUVAnimatedBoneForEachUVMaterial();

	}

	
	// Update is called once per frame
	void Update () {

		// If there's a UVA animated bone, then obviously there's a UVA Material to animate, so start animating
		if (UVAAnimatedBone)
		{
			Vector2 pos;
			pos.x = (UVAAnimatedBone.position.x - UVAInitialPos.x) / tileSize.x;
			pos.y = (UVAAnimatedBone.position.y - UVAInitialPos.y) / tileSize.y;
			UVAMaterial.mainTextureOffset = pos;
		}

		if (UVBAnimatedBone)
		{
			Vector2 pos;
			pos.x = (UVBAnimatedBone.position.x - UVBInitialPos.x) / tileSize.x;
			pos.y = (UVBAnimatedBone.position.y - UVBInitialPos.y) / tileSize.y;
			UVBMaterial.mainTextureOffset = pos;
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

	void FindUVMaterials () {
		
		if(hasBonedMesh)
		{
			// Search all SkinnedMeshRenderers (There will always be one)
			for (int mesh = 0; mesh < bonedMesh.Length; mesh++)
			{
				// Search materials in this SkinnedMeshRenderer
				for (int mat = 0; mat < bonedMesh[mesh].materials.Length; mat++)
				{
					// Find out if it is UVA
					if (bonedMesh[mesh].materials[mat].name.IndexOf("UVA") != -1)
					{
						UVAMaterial = bonedMesh[mesh].materials[mat];
						if (debugMode)
							Debug.Log("UVA Materials is "+ UVAMaterial);
					}
					
					// Find out if it is UVB
					if (bonedMesh[mesh].materials[mat].name.IndexOf("UVB") != -1)
					{
						UVBMaterial = bonedMesh[mesh].materials[mat];
						if (debugMode)
							Debug.Log("UVB Materials is "+ UVBMaterial);
					}
					
				}
			}
			
		}
		
	}

	void FindUVAnimatedBoneForEachUVMaterial (){
		// put all transforms into childTransform
		childTransform = this.GetComponentsInChildren<Transform>();
		
		for (int i=0; i < childTransform.Length; i++)
		{
			// if the transform has the UVA prefix, if there is a UVAMaterial and there's no current UVA Animated Bone
			if (childTransform[i].name.IndexOf("UVA") > -1 && UVAMaterial && !UVAAnimatedBone)
			{
				UVAAnimatedBone = childTransform[i];
				UVAInitialPos = childTransform[i].position;
				if (debugMode)
					Debug.Log(UVAAnimatedBone.name);
			}
			// if the transform has the UVB prefix, if there is a UVBMaterial and there's no current UVB Animated Bone
			if (childTransform[i].name.IndexOf("UVB") > -1 && UVBMaterial)
			{
				UVBAnimatedBone = childTransform[i];
				UVBInitialPos = childTransform[i].position;
				if (debugMode)
					Debug.Log(UVBAnimatedBone.name);
			}
		}

	}
}
