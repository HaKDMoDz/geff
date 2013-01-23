using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paper
{
    public class Vertex
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public override string ToString()
        {
            return (X.ToString() + " " + Y.ToString() + " " + Z.ToString()).Replace(",",".");
        }
    }
}
