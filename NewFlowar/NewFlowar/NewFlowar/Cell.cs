using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NewFlowar
{
    public class Cell
    {
        public List<Cell> Cells { get; set; }
        public float Height { get; set; }
        public Point Coord { get; set; }

        public Cell(int x, int y)
        {
            this.Coord = new Point(x, y);
        }
    }
}
