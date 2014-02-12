using UnityEngine;
using System.Collections;
using Assets.Code;
using System.Collections.Generic;

public class Cubeat : MonoBehaviour
{
    public Map map;
    public double BPM;
    private double lastTime;
    Vector3 theForwardDirection;
    Quaternion _initialRotation;

    void Start()
    {
        map = new Map("Rapman", this);
        BPM = 240.0;

        //theForwardDirection = Camera.main.transform.TransformDirection(Vector3.forward);
        //theForwardDirection.Normalize();

        //Camera.main.transform.Translate(-theForwardDirection * (layer-3)*4);
        //Camera.main.transform.LookAt(Vector3.zero);
        ComputeCameraPosition();
    }

    public void ComputeCameraPosition()
    {
        float halfHeight = map.Size / 2.0f;

        float halfFov = Camera.main.fieldOfView / 2.0f;

        float fovTan = Mathf.Tan(halfFov * Mathf.Deg2Rad);

        theForwardDirection = Camera.main.transform.TransformDirection(Vector3.forward);
        theForwardDirection.Normalize();
        //Camera.main.transform.Translate(-theForwardDirection * halfHeight / fovTan/10f);

        Camera.main.transform.position = new Vector3(1f,map.Size*0.9f,-2.5f);

        //Debug.Log(halfHeight / fovTan);
        //Debug.Log(theForwardDirection);

        Camera.main.transform.LookAt(Vector3.zero);

        Camera.main.transform.rotation = Quaternion.Euler(new Vector3(Camera.main.transform.rotation.eulerAngles.x, 0f, 0f));

        _initialRotation = Camera.main.transform.rotation;

    }

    public int numBeatsPerSegment = 16;
    public AudioClip[] clips = new AudioClip[2];
    private double nextEventTime;

    void Update()
    {
        double time = AudioSettings.dspTime;
        map.Update(time);
        //if (time - lastTime + 1.0F > Speed / 1000f)
        //{
        //    lastTime = time;
        //    map.Update(time);
        //    Debug.Log(time);
        //}

        //if (Application.platform == RuntimePlatform.Android)
        {
            this.transform.rotation = Quaternion.Euler(_initialRotation.eulerAngles + new Vector3(Input.acceleration.y, Input.acceleration.x, 0f) * 3f);
        }
    }

    void OnGUI()
    {
        //GUI.color = Color.yellow;

        //GUI.BeginGroup(new Rect(0, Screen.height-50 , Screen.width, 50));
        //GUI.Box(new Rect(10, 10, 50, 50), "Menu");


        ////GUI.Toolbar(new Rect(0,Screen.height-50, Screen.width, 50), -1, buttons)
        //GUI.EndGroup();
    }

    internal static void ComputePartition(Cube cubeTouched)
    {
        throw new System.NotImplementedException();
    }
}
