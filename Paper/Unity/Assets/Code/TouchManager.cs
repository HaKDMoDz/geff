using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour
{
    private bool leftMouseButtonWasPressed;
    // Use this for initialization
    void Start()
    {

    }

    private ITouchable GetTouchedGameObject(Vector2 position, out RaycastHit hit)
    {
        Ray ray = Camera.mainCamera.ScreenPointToRay(position);
        ITouchable touchableObject = null;

        if (Physics.Raycast(ray, out hit))
        {
            touchableObject = (ITouchable)hit.transform.gameObject.GetComponent(typeof(ITouchable));
        }

        return touchableObject;
    }

    void Update()
    {
        ITouchable touchableObject = null;
        RaycastHit hit = new RaycastHit();

        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            if (Input.GetMouseButtonDown(0) && !leftMouseButtonWasPressed)
            {
                leftMouseButtonWasPressed = true;
                touchableObject = GetTouchedGameObject(new Vector2(Input.mousePosition.x, Input.mousePosition.y), out hit);
                if (touchableObject != null)
                    touchableObject.MouseDown(hit);
            }
            else if (Input.GetMouseButtonUp(0) && leftMouseButtonWasPressed)
            {
                leftMouseButtonWasPressed = false;
                touchableObject = GetTouchedGameObject(new Vector2(Input.mousePosition.x, Input.mousePosition.y), out hit);
                if (touchableObject != null)
                    touchableObject.MouseUp(hit);
            }
            
            //else if (Input.GetMouseButtonDown(0) && leftMouseButtonWasPressed)
            {
                touchableObject = GetTouchedGameObject(new Vector2(Input.mousePosition.x, Input.mousePosition.y), out hit);
                if (touchableObject != null)
                    touchableObject.MouseMove(hit);
            }
        }
        else if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            for (int i = 0; i < Input.touchCount; ++i)
            {
                if (Input.GetTouch(i).phase.Equals(TouchPhase.Began))
                {
                    touchableObject = GetTouchedGameObject(Input.GetTouch(i).position, out hit);
                    if (touchableObject != null)
                        touchableObject.MouseDown(hit);
                }
                else if (Input.GetTouch(i).phase.Equals(TouchPhase.Ended))
                {
                    touchableObject = GetTouchedGameObject(Input.GetTouch(i).position, out hit);
                    if (touchableObject != null)
                        touchableObject.MouseUp(hit);
                }
                else if (Input.GetTouch(i).phase.Equals(TouchPhase.Moved))
                {
                    touchableObject = GetTouchedGameObject(Input.GetTouch(i).position, out hit);
                    if (touchableObject != null)
                        touchableObject.MouseMove(hit);
                }
            }
        }
    }
}
