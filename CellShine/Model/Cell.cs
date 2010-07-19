using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CellShine.Model
{
    public class Cell
    {
        public Point Position { get; set; }
        public List<Cell> ListNeighbour { get; set; }
        public Boolean Selected { get; set; }
        
        public DateTime StartTime { get; set; }
        public float Coeff { get; set; }
        public float Value { get; set; }

        public Cell(int x, int y)
        {
            this.Position = new Point(x, y);
            this.ListNeighbour = new List<Cell>();
            this.StartTime = DateTime.MinValue;
        }
    }
}
