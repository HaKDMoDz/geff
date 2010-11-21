using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NewFlowar.Model
{
    public class Cell
    {
        public Map Map { get; set; }
        public float Height { get; set; }
        public Point Coord { get; set; }
        public Vector2 Location { get; set; }
        public Dictionary<int, Cell> Neighbourghs { get; set; }
        public Dictionary<int, int> Points { get; set; }
        public bool ContainsMinion { get; set; }

        public Cell(Map map, int x, int y, float left, float top)
        {
            this.Map = map;
            this.Coord = new Point(x, y);
            this.Location = new Vector2(left, top);

            this.Neighbourghs = new Dictionary<int, Cell>();
            this.Neighbourghs.Add(1,null);
            this.Neighbourghs.Add(2,null);
            this.Neighbourghs.Add(3,null);
            this.Neighbourghs.Add(4,null);
            this.Neighbourghs.Add(5,null);
            this.Neighbourghs.Add(6,null);

            this.Points = new Dictionary<int, int>();
            this.Points.Add(1, -1);
            this.Points.Add(2, -1);
            this.Points.Add(3, -1);
            this.Points.Add(4, -1);
            this.Points.Add(5, -1);
            this.Points.Add(6, -1);
        }

        public int IndexPosition
        {
            get
            {
                return (this.Coord.Y - 1) * this.Map.Width + this.Coord.X-1;
            }
        }

        public override string ToString()
        {
            return String.Format("{0} : {1},{2}", IndexPosition, Coord.X, Coord.Y);
        }
    }
}
