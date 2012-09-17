using UnityEngine;
using System.Collections;

public class Cuboid : MonoBehaviour, ITouchable
{
    public bool isSelected = false;
    public bool isMouseOver = false;
    public BoxCollider AnchorLeftCollider;
    public BoxCollider AnchorRightCollider;
    public BoxCollider AnchorFaceCollider;
    public bool IsTool = false;

    private Vector3 vecInitialSelection = Vector3.zero;
    private BoxCollider paperUp; 
    private GameObject planeUp;
    private GameObject planeDown;
    private BoxCollider boxCollider;
    
    public bool Visible
    {
        get
        {
            return this.planeUp.renderer.enabled;
        }

        set
        {
            this.planeUp.renderer.enabled = value;
            this.planeDown.renderer.enabled = value;
        }
    }

    public void Initialize()
    {
        Start();
        this.gameObject.layer = LayerMask.NameToLayer("Default");
        this.planeUp.layer = LayerMask.NameToLayer("Default");
        this.planeDown.layer = LayerMask.NameToLayer("Default");
    }

    void Start()
    {
        paperUp = GameObject.Find("PaperUp").GetComponent<BoxCollider>();
        planeUp = (GameObject)this.transform.Find("PlaneUp").gameObject;
        planeDown = (GameObject)this.transform.Find("PlaneDown").gameObject;

        AnchorLeftCollider = this.transform.Find("LeftAnchor").GetComponent<BoxCollider>();
        AnchorRightCollider = this.transform.Find("RightAnchor").GetComponent<BoxCollider>();
        AnchorFaceCollider = this.transform.Find("FaceAnchor").GetComponent<BoxCollider>();

        boxCollider = this.GetComponent<BoxCollider>();
    }

    void Update()
    {
        if (!isSelected)
        {
            if (isMouseOver && (Game.CurrentCuboid == null || !Game.CurrentCuboid.Visible))
            {
                planeUp.renderer.material.color = Color.blue;
            }
            else
            {
                planeUp.renderer.material.color = Color.white;
            }
        }

        isMouseOver = false;
    }

    public void MouseDown(RaycastHit hit)
    {
        Game.GameState = GameState.PickCuboid;

        if (IsTool && !isSelected)
        {
            Color colorDeselectedPlaneUp = planeUp.renderer.material.color;
            Color colorDeselectedPlaneDown = planeDown.renderer.material.color;

            GameObject[] cuboidTools = GameObject.FindGameObjectsWithTag("Tool");

            foreach (GameObject cuboidTool in cuboidTools)
            {
                cuboidTool.GetComponent<Cuboid>().isSelected = false;
                cuboidTool.GetComponent<Cuboid>().planeUp.renderer.material.color = colorDeselectedPlaneUp;
                cuboidTool.GetComponent<Cuboid>().planeDown.renderer.material.color = colorDeselectedPlaneDown;
            }

            planeUp.renderer.material.color = Color.yellow;
            planeDown.renderer.material.color = Color.yellow;
            Game.CurrentCuboidTool = this;
            isSelected = true;

            Game.CreateNewCuboid();
        }
    }

    public void MouseUp(RaycastHit hit)
    {
        if (!IsTool)
            isSelected = false;
    }

    public void MouseMove(RaycastHit hit)
    {
        isMouseOver = true;
    }
}
