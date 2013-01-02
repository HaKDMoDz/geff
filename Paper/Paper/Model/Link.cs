using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Drawing;

namespace Paper.Model
{
    [Serializable()]
    public class Link : ComponentBase, IMoveable
    {
        public Link()
        {
        }

        public Link(int x, int y)
            : base()
        {
            this.Location = new Point(x, y);
        }

        [XmlIgnore]
        public List<Line> LineResizable
        {
            get;
            set;
        }

        [XmlIgnore]
        public override System.Drawing.Rectangle RectangleSelection
        {
            get
            {
                return new System.Drawing.Rectangle(Location.X, Location.Y, 10, 10);
            }
        }

        public Point Location
        {
            get;
            set;
        }
    }
}
