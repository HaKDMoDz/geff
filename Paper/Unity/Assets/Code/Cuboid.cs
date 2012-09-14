using UnityEngine;
using System.Collections;

public class Cuboid : MonoBehaviour//, ITouchable
{
	private Vector3 vecInitialSelection = Vector3.zero;
	private BoxCollider paperUp;
	public bool isSelected = false;
	public bool isMouseOver = false;
	private GameObject planeUp;
	private GameObject planeDown;
	private BoxCollider boxCollider;
	public bool IsTool = false;
	
	public bool Visible {
		get {
			return this.planeUp.renderer.enabled;
		}
	
		set {
			this.planeUp.renderer.enabled = value;
			this.planeDown.renderer.enabled = value;
		}
	}
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
		if (isMouseOver) {
			planeUp.renderer.material.color = Color.blue;
		} else {
			planeUp.renderer.material.color = Color.white;
		}
		
		isMouseOver = false;
	}
	
	public void OnMouseDown ()
	{
		/*
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
				Game.CurrentCuboid.Start ();
				Game.CurrentCuboid.Visible = false;
				Game.CurrentCuboid.IsTool = false;
				Game.CurrentCuboid.isSelected = true;
			}
		}
			
		if (!IsTool)
			isSelected = true;
		
		vecInitialSelection = Vector3.zero;*/
	}

	public void OnMouseUp ()
	{
		//Game.GameState = GameState.None;
		isSelected = false;
	}
	
	public void OnMouseMove ()
	{
		isMouseOver = true;
	}
}
