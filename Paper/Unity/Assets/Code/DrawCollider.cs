using UnityEngine;
using System.Collections;

public class DrawCollider : MonoBehaviour
{
    public float Alpha = 0.5f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 1f, 0f, Alpha);
        Gizmos.DrawSphere(transform.position, 0.01f);

        Gizmos.matrix = transform.localToWorldMatrix;
        if (transform.collider != null)
            Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }
}
