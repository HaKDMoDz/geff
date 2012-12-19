using UnityEngine;
using System.Collections;

public class Folding : MonoBehaviour
{
    public Transform Bone;
    public Transform Bone_001;
    public Transform Bone_002;

    int startTime = 0;

    public float p;
    public float w;
    public bool Actif = true;

    public float eX =0f;
    public float eY =0f;
    public float eZ =0f;

    // Use this for initialization
    void Start()
    {
        Bone = this.transform.GetChild(0);
        Bone_001 = Bone.GetChild(0);
        Bone_002 = Bone_001.GetChild(0);

        eX = Bone_002.localEulerAngles.x;
        eY = Bone_002.localEulerAngles.y;
        eZ = Bone_002.localEulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (Actif && Time.realtimeSinceStartup > 10f)
        {
            p = (Time.realtimeSinceStartup-10f) / 5f;

            if (p > 1f)
                p = 1f;

            Bone.localPosition = new Vector3(0f, Mathf.MoveTowards(0, 1.9f, p), Mathf.MoveTowards(-1.54f, -3.5f, p));

            //Bone_002.localEulerAngles.Set(0f, Mathf.LerpAngle(90, 180, p), 180f);

            if(Bone_002.localEulerAngles.y < 180f)
                Bone_002.RotateAround(new Vector3(1f, 0f, 0f), -0.1f);

                //.localRotation = new Quaternion(0, Mathf.MoveTowardsAngle(90, 180, p), -180, w);
            
        }
    }
}
