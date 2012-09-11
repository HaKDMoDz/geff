using UnityEngine;
using System.Collections;
 
[RequireComponent (typeof(CameraChanger))]
public class PerspectiveSwitcher : MonoBehaviour
{
    private Matrix4x4   ortho,
                        perspective;
    public float        fov     = 120f,
                       	near    = .3f,
                        far     = 1000f,
                        orthographicSize = 50f;
    private float       aspect;
    private CameraChanger blender;
    private bool        orthoOn;
	public Camera camera;
	
    void Start()
    {
		//Debug.Log(camera.fov);
        aspect = (float)Screen.width/(float)Screen.height;

        ortho = Matrix4x4.Ortho(-orthographicSize * aspect, orthographicSize * aspect, -orthographicSize, orthographicSize, near, far);

        perspective = Matrix4x4.Perspective(fov, aspect, near, far);
		
        camera.projectionMatrix = ortho;
		//Camera.main.transform.position.Set(0,0,0);
        orthoOn = true;

        blender = (CameraChanger)GetComponent(typeof(CameraChanger));
    }

    void Update()
    {	
        if (Input.GetKeyDown(KeyCode.Space))
        {
           orthoOn = !orthoOn;
			
			
            if (orthoOn)
                blender.BlendToMatrix(camera, ortho, 2f);
            else
                blender.BlendToMatrix(camera, perspective, 2f);
        }
    }
}