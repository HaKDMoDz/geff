using UnityEngine;
using System.Collections;

public class PaperUp : MonoBehaviour, ITouchable
{
    Vector3 initialPointerPosition = Vector3.zero;

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
            Game.InitialPositionCurrentCuboid = Game.CurrentCuboid.transform.position;
            Game.InitialSizeCurrentCuboid = Game.CurrentCuboid.transform.localScale;
        }
        else if (Game.GameState != GameState.None)
        {
            Game.InitialPositionCurrentCuboid = Game.CurrentCuboid.transform.position;
            Game.InitialSizeCurrentCuboid = Game.CurrentCuboid.transform.localScale;
        }

        initialPointerPosition = hit.point;
    }

    public void MouseUp(RaycastHit hit)
    {
        if (Game.GameState == GameState.PickCuboid && Game.CurrentCuboid.Visible)
        {
            Game.CurrentCuboid.isSelected = false;
            Game.CreateNewCuboid();
        }
        else if (Game.GameState != GameState.None)
        {
            Game.GameState = GameState.PickCuboid;
            Game.CurrentCuboid.isSelected = false;
            Game.CreateNewCuboid();
        }
    }

    public void MouseMove(RaycastHit hit)
    {

        //---> Création du cuboid
        if (Game.GameState == GameState.PickCuboid && Game.CurrentCuboid != null && Game.CurrentCuboid.Visible)
        {
            float scaleX = Game.InitialPositionCurrentCuboid.x - hit.point.x;
            float scaleY = hit.point.y + 0f + Game.CurrentCuboid.transform.localScale.z;

            if (scaleX == 0)
                scaleX = 0.01f;

            if (scaleY <= 0)
                scaleY = 0.01f;

            Game.CurrentCuboid.transform.localScale = new Vector3(Mathf.Abs(scaleX), scaleY, Game.CurrentCuboid.transform.localScale.z);
            Game.CurrentCuboid.transform.position = Game.InitialPositionCurrentCuboid + new Vector3(-scaleX / 2f, 0f, 0f);
        }
        //---> Redimensionnement du cuboid
        else if (Game.GameState == GameState.LeftAnchor && Game.CurrentCuboid != null && Game.CurrentCuboid.Visible)
        {
            float scaleX = initialPointerPosition.x - hit.point.x;

            Game.CurrentCuboid.transform.position = Game.InitialPositionCurrentCuboid + new Vector3(-scaleX / 2f, 0f, 0f);
            Game.CurrentCuboid.transform.localScale = Game.InitialSizeCurrentCuboid + new Vector3(scaleX / 1f, 0f, 0f);
        }
        else if (Game.GameState == GameState.RightAnchor && Game.CurrentCuboid != null && Game.CurrentCuboid.Visible)
        {
            float scaleX = initialPointerPosition.x - hit.point.x;

            Game.CurrentCuboid.transform.position = Game.InitialPositionCurrentCuboid + new Vector3(-scaleX / 2f, 0f, 0f);
            Game.CurrentCuboid.transform.localScale = Game.InitialSizeCurrentCuboid + new Vector3(-scaleX / 1f, 0f, 0f);
        }
        else if (Game.GameState == GameState.FaceAnchor && Game.CurrentCuboid != null && Game.CurrentCuboid.Visible)
        {
            float scaleY = initialPointerPosition.y - hit.point.y;

            if (Game.InitialSizeCurrentCuboid.z + scaleY <= 0)
                scaleY = 0.01f;

            Game.CurrentCuboid.transform.localScale = Game.InitialSizeCurrentCuboid + new Vector3(0f, 0f, scaleY);
        }

        if (Game.GameState != GameState.None && Game.CurrentCuboid != null && Game.CurrentCuboid.Visible)
        {
            Game.CurrentCuboid.AnchorLeft.gameObject.transform.localPosition = new Vector3(-0.5f + 0.1f / Game.CurrentCuboid.transform.localScale.x, 1f, -0.5f);
            Game.CurrentCuboid.AnchorLeft.gameObject.transform.localScale = new Vector3(0.2f / Game.CurrentCuboid.transform.localScale.x, 0.2f / Game.CurrentCuboid.transform.localScale.y, 1f);

            Game.CurrentCuboid.AnchorRight.gameObject.transform.localPosition = new Vector3(0.5f - 0.1f / Game.CurrentCuboid.transform.localScale.x, 1f, -0.5f);
            Game.CurrentCuboid.AnchorRight.gameObject.transform.localScale = new Vector3(0.2f / Game.CurrentCuboid.transform.localScale.x, 0.2f / Game.CurrentCuboid.transform.localScale.y, 1f);

            Game.CurrentCuboid.AnchorFace.gameObject.transform.localPosition = new Vector3(0f, 1f, -1f + 0.02f / Game.CurrentCuboid.transform.localScale.z);
            Game.CurrentCuboid.AnchorFace.gameObject.transform.localScale = new Vector3(1f, 0.15f / Game.CurrentCuboid.transform.localScale.y, 0.15f / Game.CurrentCuboid.transform.localScale.z);
        }
    }
}
