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
        public Point Location { get; set; }

        public Cell(int x, int y, int left, int top)
        {
            this.Coord = new Point(x, y);
            this.Location = new Point(left, top);
        }
    }
}
