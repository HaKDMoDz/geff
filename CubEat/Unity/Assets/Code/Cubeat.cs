using UnityEngine;
using System.Collections;
using Assets.Code;

public class Cubeat : MonoBehaviour
{
    public Map map;
    public float Speed;
    private float lastTime;
    Vector3 theForwardDirection;

    void Start()
    {
        map = new Map("Bass");
        Speed = 150f;

        //theForwardDirection = Camera.main.transform.TransformDirection(Vector3.forward);
        //theForwardDirection.Normalize();

        //Camera.main.transform.Translate(-theForwardDirection * (layer-3)*4);
        //Camera.main.transform.LookAt(Vector3.zero);

        ComputeCameraPosition();
    }

    private void ComputeCameraPosition()
    {
        float halfHeight = map.Size / 2.0f;

        float halfFov = Camera.main.fov / 2.0f;

        float fovTan = Mathf.Tan(halfFov * Mathf.Deg2Rad);

        theForwardDirection = Camera.main.transform.TransformDirection(Vector3.forward);
        theForwardDirection.Normalize();
        Camera.main.transform.Translate(-theForwardDirection * halfHeight / fovTan/1000f);


        Debug.Log(halfHeight / fovTan);
        Debug.Log(theForwardDirection);

        Camera.main.transform.LookAt(Vector3.zero);
    }

    void Update()
    {
        if (Time.time - lastTime > Speed / 1000f)
        {
            lastTime = Time.time;
            map.Update();
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
}
