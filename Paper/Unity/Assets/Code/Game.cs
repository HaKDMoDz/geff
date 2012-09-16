using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
   public static GameState GameState = GameState.None;
	
	public static Cuboid CurrentCuboid;
	public static Cuboid CurrentCuboidTool;
	
    void Start()
    {
    }

    void Update()
    {
    }
}

public enum GameState
{
    PickCuboid,
	None
}
