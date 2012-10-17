using UnityEngine;
using System.Collections;
using Assets.Code;

public class Cubeat : MonoBehaviour
{
    public Map map;
    public float Speed;
    private float lastTime;

    // Use this for initialization
    void Start()
    {
        //float width = 4;
        //float height = 4;

        //for (int i = 0; i < width; i++)
        //{
        //    for (int j = 0; j < height; j++)
        //    {
        //        Instantiate(Cube, new Vector3(i * Cube.transform.localScale.x - (width-1) * Cube.transform.localScale.x / 2f, 1f, j * Cube.transform.localScale.z - (height-1) * Cube.transform.localScale.z / 2f), Quaternion.identity);
        //    }
        //}
        
        map = new Map(3);
        Speed = 150f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastTime > Speed/1000f)
        {
            lastTime = Time.time;
            map.Update();
        }
    }
}
