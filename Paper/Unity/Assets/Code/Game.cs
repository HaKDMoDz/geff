using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    public static GameState GameState = GameState.None;

    public static Cuboid CurrentCuboid;
    public static Cuboid CurrentCuboidTool;
    public static Vector3 InitialPositionCurrentCuboid { get; set; }
    public static Vector3 InitialSizeCurrentCuboid { get; set; }

    void Start()
    {
    }

    void Update()
    {
    }


    public static void CreateNewCuboid()
    {
        if (Game.CurrentCuboid != null && !Game.CurrentCuboid.Visible)
            DestroyImmediate(Game.CurrentCuboid.gameObject);

        Game.CurrentCuboid = (Cuboid)Instantiate(Game.CurrentCuboidTool);
        Game.CurrentCuboid.Initialize();
        Game.CurrentCuboid.Visible = false;
        Game.CurrentCuboid.IsTool = false;
        Game.CurrentCuboid.isSelected = true;
        Game.CurrentCuboid.gameObject.tag = null;
    }
}

public enum GameState
{
    PickCuboid,
    None,
    LeftAnchor,
    RightAnchor,
    FaceAnchor
}
