using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OnTouchMouse : MonoBehaviour
{
    void Update()
    {

        RaycastHit hit = new RaycastHit();


        //if (Application.platform == RuntimePlatform.Android)
        //{

        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //    if (this.collider.Raycast(ray, out hit, 1000f))
        //    {
        //        hit.transform.gameObject.SendMessage("OnMouseUp");

        //    }

        //}
        //else
        //{
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            if (touch.phase.Equals(TouchPhase.Began))
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out hit))
                {
                    hit.transform.gameObject.SendMessage("OnMouseDown");
                }
            }
            else if (touch.phase.Equals(TouchPhase.Ended))
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out hit))
                {
                    hit.transform.gameObject.SendMessage("OnMouseUp");
                }
            }
        }

        //}
    }
}
