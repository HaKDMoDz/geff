using UnityEngine;
using System.Collections;

public class Cubeat : MonoBehaviour
{
    public Transform Cube;

    // Use this for initialization
    void Start()
    {
        float width = 4;
        float height = 4;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Instantiate(Cube, new Vector3(i * Cube.transform.localScale.x - (width-1) * Cube.transform.localScale.x / 2f, 1f, j * Cube.transform.localScale.z - (height-1) * Cube.transform.localScale.z / 2f), Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
