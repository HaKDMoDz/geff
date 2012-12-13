using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Paper.Model
{
    [Serializable()]
    public class Link : ComponentBase
    {
        public Link()
        {
        }

        public Link(int x, int y)
            : base(x, y)
        {
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
    }
}
