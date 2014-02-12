using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Layer
{
    public int LayerID;
    public int NumberOnLayer;

    public Color Color { get; set; }
    public List<Cube> Cubes { get; set; }

    public Layer()
    {
        Cubes = new List<Cube>();
    }
}
