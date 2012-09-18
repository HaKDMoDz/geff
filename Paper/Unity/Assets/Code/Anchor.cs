using UnityEngine;
using System.Collections;

public class Anchor : MonoBehaviour, ITouchable
{
    private Cuboid parentCuboid;

    // Use this for initialization
    void Start()
    {
        parentCuboid = this.gameObject.transform.parent.gameObject.GetComponent<Cuboid>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MouseDown(RaycastHit hit)
    {
        if (parentCuboid.IsTool)
            return;

        if (this.name == "LeftAnchor")
            Game.GameState = GameState.LeftAnchor;
        if (this.name == "RightAnchor")
            Game.GameState = GameState.RightAnchor;
        else if (this.name == "FaceAnchor")
            Game.GameState = GameState.FaceAnchor;

        Game.CurrentCuboid = parentCuboid;
        Game.InitialPositionCurrentCuboid = Game.CurrentCuboid.transform.localPosition;
        parentCuboid.isSelected = true;
    }

    public void MouseUp(RaycastHit hit)
    {
        //Game.GameState = GameState.PickCuboid;
    }

    public void MouseMove(RaycastHit hit)
    {
        parentCuboid.isMouseOver = true;
    }
}
