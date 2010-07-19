using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ThreadAStar.AStar
{
    public class Cell
    {
        public Point Position { get; set; }
        public float Cost { get; set; }
        public List<Cell> ListNeighbour { get; set; }

        public Cell(int x, int y, float cost)
        {
            this.Position = new Point(x, y);
            this.Cost = cost;
            this.ListNeighbour = new List<Cell>();
        }
    }
}
