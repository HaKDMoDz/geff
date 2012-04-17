using UnityEngine;
using System.Collections;

public class DrawCollider : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.01f);

        Gizmos.matrix = transform.localToWorldMatrix; 
        if(transform.collider != null)
            Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }
}
