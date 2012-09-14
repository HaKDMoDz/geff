using UnityEngine;
using System.Collections;

public class PaperUp : MonoBehaviour, ITouchable
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
	
	public void OnMouseDown (RaycastHit hit)
	{
	}

	public void OnMouseUp (RaycastHit hit)
	{
		if (Game.GameState == GameState.PickCuboid) {
		
			
			if (Game.GameState == GameState.PickCuboid) {
				Game.CurrentCuboid.Visible = true;
					
				Vector3 position = hit.point;// - vecInitialSelection;// + new Vector3(0f, 0.3f, 0f);
				
				Game.CurrentCuboid.transform.position = position;
			}
		} 
	}
	
	public void OnMouseMove (RaycastHit hit)
	{
		if (Game.GameState == GameState.PickCuboid) {
		
		}
	}
}
