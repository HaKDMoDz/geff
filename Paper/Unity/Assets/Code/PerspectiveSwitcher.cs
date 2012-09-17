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
	public new Camera camera;
    private float changeDuration = 2f;
    private float startTime = float.MinValue;

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
			startTime = Time.time;

            if (orthoOn)
			{
                blender.BlendToMatrix(camera, ortho, changeDuration);
			}
            else
			{
                camera.cullingMask = -257;//2^LayerMask.NameToLayer("Everything") - 2^LayerMask.NameToLayer("Tools");;
                blender.BlendToMatrix(camera, perspective, changeDuration);
			}
        }

        if (startTime != float.MinValue && orthoOn && Time.time - startTime >= changeDuration)
        {
            camera.cullingMask = -1;//2^LayerMask.NameToLayer("Everything");
            startTime = float.MinValue;
        }
    }
}