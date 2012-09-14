using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	void Update ()
	{
		// Code for OnMouseDown in the iPhone. Unquote to test.
		RaycastHit hit = new RaycastHit ();
		for (int i = 0; i < Input.touchCount; ++i) {
			if (Input.GetTouch (i).phase.Equals (TouchPhase.Began)) {
				// Construct a ray from the current touch coordinates
				Ray ray = Camera.mainCamera.ScreenPointToRay (Input.GetTouch (i).position);
				if (Physics.Raycast (ray, out hit) && hit.transform.gameObject) {
					
					ITouchable touchableObject = (ITouchable)hit.transform.gameObject.GetComponent (typeof(ITouchable));
					
					if (touchableObject != null)
						touchableObject.OnMouseDown (hit);
				}
			} 
			else if (Input.GetTouch (i).phase.Equals (TouchPhase.Ended)) {
				// Construct a ray from the current touch coordinates
				Ray ray = Camera.current.ScreenPointToRay (Input.GetTouch (i).position);
				if (Physics.Raycast (ray, out hit)) {
					hit.transform.gameObject.SendMessage ("OnMouseUp", hit);
				}
			} else if (Input.GetTouch (i).phase.Equals (TouchPhase.Moved)) {
				// Construct a ray from the current touch coordinates
				Ray ray = Camera.current.ScreenPointToRay (Input.GetTouch (i).position);
				if (Physics.Raycast (ray, out hit)) {
					hit.transform.gameObject.SendMessage ("OnMouseMove", hit);
				}
			}
		}
	}
}
