using UnityEngine;
using System.Collections;

public class PaperUp : MonoBehaviour, ITouchable
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MouseDown(RaycastHit hit)
    {
        if (Game.GameState == GameState.PickCuboid)
        {
            Game.CurrentCuboid.Visible = true;

            Vector3 position = new Vector3(hit.point.x, 0f, 5f);// - vecInitialSelection;// + new Vector3(0f, 0.3f, 0f);

            Game.CurrentCuboid.transform.position = position;
            Game.InitialPositionCurrentCuboid = position;
        }
    }

    public void MouseUp(RaycastHit hit)
    {
        if (Game.GameState == GameState.PickCuboid && Game.CurrentCuboid.Visible)
        {
            Game.CurrentCuboid.isSelected = false;
            Game.CreateNewCuboid();
        }
    }

    public void MouseMove(RaycastHit hit)
    {
        if (Game.GameState == GameState.PickCuboid && Game.CurrentCuboid != null && Game.CurrentCuboid.Visible)
        {
            float scaleX = Game.InitialPositionCurrentCuboid.x - hit.point.x;
            float scaleY = hit.point.y + 0f + Game.CurrentCuboid.transform.localScale.z;

            if (scaleY < 0)
                scaleY = 0.01f;

            Game.CurrentCuboid.transform.localScale = new Vector3(Mathf.Abs(scaleX), scaleY, Game.CurrentCuboid.transform.localScale.z);
            Game.CurrentCuboid.transform.position = Game.InitialPositionCurrentCuboid + new Vector3(-scaleX / 2f, 0f, 0f);

            Game.CurrentCuboid.AnchorLeftCollider.center = new Vector3(-0.5f + 0.05f/Game.CurrentCuboid.transform.localScale.x, 1f, -0.5f);
            Game.CurrentCuboid.AnchorLeftCollider.size = new Vector3(0.1f / Game.CurrentCuboid.transform.localScale.x, 0.1f / Game.CurrentCuboid.transform.localScale.y, 1f);

            Game.CurrentCuboid.AnchorRightCollider.center = new Vector3(0.45f, 1f, -0.5f);
            Game.CurrentCuboid.AnchorRightCollider.size = new Vector3(0.1f, 0.1f, 1f);

            Game.CurrentCuboid.AnchorFaceCollider.center = new Vector3(0f, 1f, -0.95f);
            Game.CurrentCuboid.AnchorFaceCollider.size = new Vector3(0.8f, 0.1f, 0.1f);
        }
    }
}
