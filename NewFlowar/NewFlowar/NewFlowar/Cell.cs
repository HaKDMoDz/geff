using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NewFlowar
{
    public class Cell
    {
        public Map Map { get; set; }
        public float Height { get; set; }
        public Point Coord { get; set; }
        public Point Location { get; set; }
        public Dictionary<int, Cell> Neighbourghs { get; set; }
        public Dictionary<int,Point> Points { get; set; }

        public Cell(Map map, int x, int y, int left, int top)
        {
            this.Map = map;
            this.Coord = new Point(x, y);
            this.Location = new Point(left, top);

            this.Neighbourghs = new Dictionary<int, Cell>();
            this.Neighbourghs.Add(1,null);
            this.Neighbourghs.Add(2,null);
            this.Neighbourghs.Add(3,null);
            this.Neighbourghs.Add(4,null);
            this.Neighbourghs.Add(5,null);
            this.Neighbourghs.Add(6,null);

            this.Points = new Dictionary<int, Point>();
            this.Points.Add(1, Point.Zero);
            this.Points.Add(2, Point.Zero);
            this.Points.Add(3, Point.Zero);
            this.Points.Add(4, Point.Zero);
            this.Points.Add(5, Point.Zero);
            this.Points.Add(6, Point.Zero);
        }

        public int IndexPosition
        {
            get
            {
                return (this.Coord.Y - 1) * this.Map.Width + this.Coord.X;
            }
        }
    }
}
