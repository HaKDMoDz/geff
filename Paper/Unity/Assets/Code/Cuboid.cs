using UnityEngine;
using System.Collections;

public class Cuboid : MonoBehaviour
{
	private Vector3 vecInitialSelection = Vector3.zero;
	private BoxCollider paperUp;
	public bool isSelected = false;
	private GameObject planeUp;
	private GameObject planeDown;
	private BoxCollider boxCollider;
	public bool IsTool = false;
	
	// Use this for initialization
	void Start ()
	{
		paperUp = GameObject.Find ("PaperUp").GetComponent<BoxCollider> ();
		planeUp = (GameObject)this.transform.Find ("PlaneUp").gameObject;
		planeDown = (GameObject)this.transform.Find ("PlaneDown").gameObject;
		boxCollider = this.GetComponent<BoxCollider> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		Ray ray = Camera.mainCamera.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		
		if (boxCollider.Raycast (ray, out hit, 100f)) {
			planeUp.renderer.material.color = Color.blue;
		} else {
			planeUp.renderer.material.color = Color.white;
		}
			
		if (Game.GameState == GameState.PickCuboid && isSelected && !IsTool && isTouched) {
			Game.CurrentCuboid.planeUp.renderer.enabled = true;
			Game.CurrentCuboid.planeDown.renderer.enabled = true;
		
		
			if (paperUp.Raycast (ray, out hit, 100f)) {
				if (vecInitialSelection == Vector3.zero) {
					vecInitialSelection = hit.point;
					Debug.Log (vecInitialSelection);
				}
				
				
				
				Vector3 position = hit.point;// - vecInitialSelection;// + new Vector3(0f, 0.3f, 0f);
				
				this.transform.position = position;
			}
			
			
			planeUp.renderer.material.color = Color.yellow;
		} 
	}
	
	public void OnMouseDown ()
	{
		Game.GameState = GameState.PickCuboid;
		
		if (IsTool && !isSelected) {
			Color colorDeselectedPlaneUp = planeUp.renderer.material.color;
			Color colorDeselectedPlaneDown = planeDown.renderer.material.color;
			
			GameObject[] cuboidTools = GameObject.FindGameObjectsWithTag ("Tool");
			
			foreach (GameObject cuboidTool in cuboidTools) {
				cuboidTool.GetComponent<Cuboid> ().planeUp.renderer.material.color = colorDeselectedPlaneUp;
				cuboidTool.GetComponent<Cuboid> ().planeDown.renderer.material.color = colorDeselectedPlaneDown;
			}
			
			planeUp.renderer.material.color = Color.yellow;
			planeDown.renderer.material.color = Color.yellow;
			Game.CurrentCuboidTool = this;
			
			if (Game.CurrentCuboid == null || Game.CurrentCuboid.gameObject.active == true) {
				Game.CurrentCuboid = (Cuboid)Instantiate (this);
				Game.CurrentCuboid.Start();
				Game.CurrentCuboid.planeUp.renderer.enabled = false;
				Game.CurrentCuboid.planeDown.renderer.enabled = false;
				Game.CurrentCuboid.IsTool = false;
				Game.CurrentCuboid.isSelected = true;
			}
		}
			
		if (!IsTool)
			isSelected = true;
		
		vecInitialSelection = Vector3.zero;
	}

	public void OnMouseUp ()
	{
		//Game.GameState = GameState.None;
		isSelected = false;
	}
}
