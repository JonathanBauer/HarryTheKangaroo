using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UVAnimate : MonoBehaviour {


	private int maxBoneCount;												// stores the usable number of uvAnimation Bones

	public List<GameObject> uvAnimationBones = new List<GameObject>();		// The UV animation bones provided in the rig

	public List<Material> materialsToBeUvAnimated = new List<Material>();	// The materials that require UV animation. One for each bone.

	public List<Vector2> materialTiling = new List<Vector2>();				// How many tiles on width and height of the texture in the UV animated material

	private List<Vector2> initialOffset = new List<Vector2>();				// The position of each uv Animation Bone at rest
	


	void Start () {

		// If there are more animated bones that materials, reduce the animated bone number to the material count.
		// Otherwise, use all animated bones.
		if (uvAnimationBones.Count > materialsToBeUvAnimated.Count)
		{
			Debug.Log(this+ "There are more animated bones than materials to be animated. There are now only " + materialsToBeUvAnimated.Count + " bones uses in UV animation.");
			maxBoneCount = materialsToBeUvAnimated.Count;
		}
		else
		{
			maxBoneCount = uvAnimationBones.Count;
			Debug.Log(maxBoneCount);
		}


		int i;
		for ( i = 0; i < maxBoneCount; i++ )
		{
			Vector2 pos = new Vector2(0,0);
			pos.x = uvAnimationBones[i].transform.position.x;
			pos.y = uvAnimationBones[i].transform.position.y;
			initialOffset.Add(pos);
		}

	}

	
	// Update is called once per frame
	void Update () {

		int i;
		for ( i = 0; i < maxBoneCount; i++ )
		{
			Vector2 pos = new Vector2(0,0);
			pos.x = (uvAnimationBones[i].transform.position.x - initialOffset[i].x) / materialTiling[0].x;
			pos.y = (uvAnimationBones[i].transform.position.y - initialOffset[i].y) / materialTiling[0].y;
			materialsToBeUvAnimated[i].mainTextureOffset = pos;
		}
	
	}
}
