using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Resource
{
    private static GameObject _cube;
    public static GameObject Cube 
    { 
        get
        {
            if (_cube == null)
                _cube = (GameObject)Resources.Load("Prefab/Cube");

            return _cube;
        }
    }

}
