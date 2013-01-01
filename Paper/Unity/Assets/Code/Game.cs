using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Game : MonoBehaviour
{
	public static GameState GameState = GameState.None;
	public List<Folding> ListFolding;

	void Start ()
	{
		InitFoldings ();
	}

	void Update ()
	{
	}

	private void InitFoldings ()
	{
		ListFolding = new List<Folding> ();

		CreateFoldings (this.transform);
	}

	private void CreateFoldings (Transform parentTransform)
	{
		try {
			for (int i = 0; i < parentTransform.GetChildCount(); i++) 
			{
				Transform childTransform = parentTransform.GetChild (i);
				MeshRenderer meshRenderer = childTransform.gameObject.GetComponent<MeshRenderer> ();
				MeshFilter meshFilter = childTransform.gameObject.GetComponent<MeshFilter> ();
				
				if (meshRenderer != null && childTransform.gameObject.activeSelf) 
				{
					Debug.Log(childTransform.gameObject.name);
					
					childTransform.gameObject.AddComponent<SkinnedMeshRenderer> ();
					SkinnedMeshRenderer skinnedMeshRenderer = childTransform.gameObject.GetComponent<SkinnedMeshRenderer> ();
					
					Mesh mesh = new Mesh ();
					mesh.vertices = meshFilter.mesh.vertices;
					mesh.uv = meshFilter.mesh.uv;
					mesh.triangles = meshFilter.mesh.triangles;
					mesh.RecalculateNormals ();
					
					BoneWeight[] boneWeights = new BoneWeight[meshFilter.mesh.vertexCount];
					
					Vector3[] centerBones = new Vector3[2];
					
					centerBones[0] = new Vector3(float.MaxValue, float.MaxValue, 0f);
					centerBones[1] = new Vector3(float.MaxValue, float.MinValue, float.MinValue);
				
					for (int j = 0; j < mesh.triangles.Length/3; j++)
					{
						Vector3 normal = mesh.normals[mesh.triangles[j*3]];
						normal+= mesh.normals[mesh.triangles[j*3+1]];
						normal+= mesh.normals[mesh.triangles[j*3+2]];
						normal/=3f;
						
						
						float dotZ = Vector3.Dot (normal, new Vector3 (0f, 0f, 1f));
						float dotY = Vector3.Dot (normal, new Vector3 (0f, -1f, 0f));
							
						//Vector3[] verts = new Vector3[3];
						
						//verts[0] = mesh.triangles[j*3];
						//verts[1] = mesh.triangles[j*3+1];
						//verts[2] = mesh.triangles[j*3+2];
						
						for (int k = 0; k < 3; k++)
						{
							int indexVertex = mesh.triangles[j*3+k];
							
							Vector3 vertice = mesh.vertices[indexVertex];
							
							if (dotY > 0.7f) 
							{
								if(boneWeights[indexVertex].boneIndex0 == 0)
									boneWeights[indexVertex].boneIndex0 = 0;
								else
									boneWeights[indexVertex].boneIndex1 = 0;
								
								
								if(vertice.y < centerBones[0].y)
								{
									centerBones[0].x = vertice.x;
									centerBones[0].y = vertice.y;
								}
								
								/*
								if(vertice.z < centerBones[0].z)
								{
									centerBones[0].x = vertice.x;
									centerBones[0].z = vertice.z;
								}*/
								
								
								
								if(vertice.y > centerBones[1].y)
								{
									centerBones[1].x = vertice.x;
									centerBones[1].y = vertice.y;
								}
								
								if(vertice.z > centerBones[1].z)
								{
									centerBones[1].x = vertice.x;
									centerBones[1].z = vertice.z;
								}
								
							}
							
							//---> Si la normale est en haut
							if (dotZ > 0.7f) 
							{
								if(boneWeights[indexVertex].boneIndex0 == 0)
									boneWeights[indexVertex].boneIndex0 = 1;
								else
									boneWeights[indexVertex].boneIndex1 = 1;
								

							}
							
							if(boneWeights[indexVertex].weight0 ==0)
							{
								boneWeights[indexVertex].weight0 = 1;
							}
							else
							{
								boneWeights[indexVertex].weight0 = 0.5f;
								boneWeights[indexVertex].weight1 = 0.5f;
							}
						}
					
					}
					
					Debug.Log("destroy");
					
					Destroy (meshRenderer);
					
					mesh.boneWeights = boneWeights;
					
					Transform[] bones = new Transform[2];
					
					bones [0] = new GameObject ("Lower").transform;
					bones [0].parent = childTransform;
					bones [0].localRotation = Quaternion.identity;
					bones [0].localPosition = centerBones[0];
					
					bones [1] = new GameObject ("Upper").transform;
					bones [1].parent = bones[0];
					bones [1].localRotation = Quaternion.identity;
					bones [1].localPosition = centerBones[1] - centerBones[0];
					
					Matrix4x4[] bindPoses = new Matrix4x4[2];
					
					bindPoses[0] = bones[0].worldToLocalMatrix * childTransform.localToWorldMatrix;
					bindPoses[1] = bones[1].worldToLocalMatrix * childTransform.localToWorldMatrix;
					
					mesh.bindposes = bindPoses;
					
					skinnedMeshRenderer.bones = bones;
					
					skinnedMeshRenderer.sharedMesh = mesh;
					skinnedMeshRenderer.rootBone = bones [0];
					
					
					
					
					childTransform.gameObject.AddComponent<Folding> ();
					Folding folding = childTransform.gameObject.GetComponent<Folding> ();
	                
					if (childTransform.gameObject.name.ToUpper ().Contains ("SCENE"))
						folding.IsScene = true;
	
					ListFolding.Add (folding);
				} 
				
				CreateFoldings (childTransform);
			}
		} catch (Exception ex) {
			Debug.Log (ex.Message);
		}
	}
}

public enum GameState
{
	PickCuboid,
	None,
	LeftAnchor,
	RightAnchor,
	FaceAnchor
}
