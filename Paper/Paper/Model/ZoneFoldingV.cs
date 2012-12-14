using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Paper.Model
{
    [Serializable()]
    public class ZoneFoldingV : ComponentBase, IResizableWidth
    {
        public int Width
        {
            get;
            set;
        }

        [XmlIgnore]
        public override System.Drawing.Rectangle RectangleSelection
        {
            get
            {
                return Rectangle;
            }
        }

        [XmlIgnore]
        public System.Drawing.Rectangle Rectangle
        {
            get
            {
                return new System.Drawing.Rectangle(Location.X + Common.Delta.X, 0, Width, Common.ScreenSize.Height);
            }
        }

        public ZoneFoldingV()
        {
        }

        public ZoneFoldingV(int x, int y, int size)
            : base(x, y)
        {
            this.Width = size;
        }

        [XmlIgnore]
        public List<Line> LineResizableWidth
        {
            get
            {
                List<Line> _lineResizeable = new List<Line>();

                Line line2 = new Line(Location.X + Width + Common.Delta.X, 0, Location.X + Width + Common.Delta.X, Common.ScreenSize.Height);

                _lineResizeable.Add(line2);

                return _lineResizeable;
            }
        }
    }
}
