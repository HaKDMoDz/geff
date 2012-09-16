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

            Vector3 position = hit.point;// - vecInitialSelection;// + new Vector3(0f, 0.3f, 0f);

            Game.CurrentCuboid.transform.position = position;
        }
    }

    public void MouseUp(RaycastHit hit)
    {
        //if (Game.GameState == GameState.PickCuboid)
        //{
        //    Game.CurrentCuboid.Visible = true;

        //    Vector3 position = hit.point;// - vecInitialSelection;// + new Vector3(0f, 0.3f, 0f);

        //    Game.CurrentCuboid.transform.position = position;
        //}
    }

    public void MouseMove(RaycastHit hit)
    {
        if (Game.GameState == GameState.PickCuboid && Game.CurrentCuboid.Visible)
        {
            float scaleX = Mathf.Abs(Game.CurrentCuboid.transform.position.x - hit.point.x);
            Game.CurrentCuboid.transform.localScale = new Vector3(scaleX,Game.CurrentCuboid.transform.localScale.y, Game.CurrentCuboid.transform.localScale.z);
        }
    }
}
