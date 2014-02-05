using Assets.Code.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public class InputController : MonoBehaviour
    {
        bool mouseLeftButtonPressed = false;
        RaycastHit[] hits = new RaycastHit[3];

        void Start()
        {
        }

        /// <summary>
        /// Unification des entrées tablette et PC
        /// </summary>
        /// <returns></returns>
        private List<TouchMouse> ConvertTouchtoTouchMouse()
        {
            List<TouchMouse> listToucheMouse = new List<TouchMouse>();

            //---> Conversion des Touch en TouchMouse pour la gestion unifiée
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch currentTouch = Input.GetTouch(i);

                TouchMouse touch = new TouchMouse();
                touch.TouchPhase = currentTouch.phase;
                touch.FingerId = currentTouch.fingerId;
                touch.Position = currentTouch.position;

                listToucheMouse.Add(touch);
            }

            //---> Gestion de la souris
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                if (Input.GetMouseButton(0))
                {
                    TouchMouse touchMouse = new TouchMouse();
                    touchMouse.IsMouse = true;
                    touchMouse.Position = Input.mousePosition;

                    if (mouseLeftButtonPressed)
                    {
                        touchMouse.TouchPhase = TouchPhase.Moved;

                    }
                    else
                    {
                        touchMouse.TouchPhase = TouchPhase.Began;
                        mouseLeftButtonPressed = true;
                    }

                    listToucheMouse.Add(touchMouse);
                }
                else
                {
                    if (mouseLeftButtonPressed)
                    {
                        TouchMouse touchMouse = new TouchMouse();
                        touchMouse.IsMouse = true;
                        touchMouse.Position = Input.mousePosition;
                        touchMouse.TouchPhase = TouchPhase.Ended;

                        listToucheMouse.Add(touchMouse);

                        mouseLeftButtonPressed = false;
                    }
                }
            }

            return listToucheMouse;
        }

        void Update()
        {
            List<TouchMouse> listTouch = ConvertTouchtoTouchMouse();

            for (int i = 0; i < listTouch.Count; i++)
            {
                //PlayerInput playerInput = listPlayerInput.Find(ii => ii.InputId == currentTouch.FingerId);
                TouchMouse currentTouch = listTouch[i];
                Cube cubeTouched = null;
                //Vector3 vecScreenToWorldPoint = Camera.main.ScreenToWorldPoint(currentTouch.Position);

                Ray ray = Camera.main.ScreenPointToRay(currentTouch.Position);

                hits = Physics.RaycastAll(ray);

                Debug.Log(hits.Length);

                //--- Détection du collider le plus proche du pointeur
                int indexNearestCollider = -1;
                float distanceNearestCollider = float.MaxValue;

                for (int j = 0; j < hits.Length; j++)
                {
                    Collider collider = hits[j].collider;

                    if (collider != null)
                    {
                        float distanceCollider = Vector2.Distance(hits[j].point, collider.transform.position);

                        if (distanceCollider < distanceNearestCollider)
                        {
                            distanceNearestCollider = distanceCollider;
                            indexNearestCollider = j;
                        }

                    }
                }

                if (indexNearestCollider > -1)
                    cubeTouched = hits[indexNearestCollider].transform.parent.GetComponent<Cube>();
                //---


                //Debug.Log("Index : " + indexNearestCollider);

                if (cubeTouched != null && currentTouch.TouchPhase == TouchPhase.Ended)
                {
                    cubeTouched.OnMouseUp();
                }
            }
        }
    }
}
