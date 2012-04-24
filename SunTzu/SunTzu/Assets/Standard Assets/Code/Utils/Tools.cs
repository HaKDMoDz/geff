using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Tools
{
    public static Vector3 TransYVector(Vector3 vector)
    {
        return new Vector3(vector.x, vector.y + 0.01f, vector.z);
    }
}
