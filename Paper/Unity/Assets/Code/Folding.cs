using UnityEngine;
using System.Collections;

public class Folding : MonoBehaviour
{
    public Transform Bone_001;
    public Transform Bone_002;

    int startTime = 0;

    public float p;
    public bool Actif = true;

    public float startAnimation = 0;
    public float animationDuration =1f;
    public int animationSens = 0;
    public bool IsScene = false;

    void Start()
    {
		
        Bone_001 = this.transform.GetChild(0);
        //Bone_002 = this.transform.GetChild(1);
        Bone_002 = Bone_001.GetChild(0);
    }

    void Update()
    {
        if (Actif && Time.realtimeSinceStartup - startAnimation < animationDuration)
        {
            p = Time.deltaTime / animationDuration;

            if(!IsScene)
            	Bone_002.Rotate(new Vector3(90f * p * animationSens, 0f, 0f));
                
			Bone_001.Rotate(new Vector3(-90f * p * animationSens, 0f , 0f));
        }
    }
}
