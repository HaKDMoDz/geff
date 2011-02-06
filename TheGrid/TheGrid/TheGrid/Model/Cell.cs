using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Xml.Serialization;

namespace TheGrid.Model
{
    [Serializable]
    public class Cell
    {
        [XmlIgnore]
        public Map Map { get; set; }
        public Point Coord { get; set; }
        public Vector2 Location { get; set; }
        [XmlIgnore]
        public Cell[] Neighbourghs { get; set; }
        public Clip Clip { get; set; }
        public Channel Channel { get; set; }

        public Cell() { }

        public Cell(Map map, int x, int y, float left, float top)
        {
            this.Map = map;
            this.Coord = new Point(x, y);
            this.Location = new Vector2(left, top);

            this.Neighbourghs = new Cell[6];
        }

        public int IndexPosition
        {
            get
            {
                return (this.Coord.Y - 1) * this.Map.Width + this.Coord.X - 1;
            }
        }

        public override string ToString()
        {
            return String.Format("{0} : {1},{2}", IndexPosition, Coord.X, Coord.Y);
        }

        public void InitClip()
        {
            if (Clip == null)
                Clip = new Clip();
        }

        public Cell GetDirection(int direction, int iteration)
        {
            Cell cell = this;
            for (int i = 0; i < iteration && cell != null; i++)
            {
                cell = cell.Neighbourghs[direction];
            }

            return cell;
        }
    }
}
