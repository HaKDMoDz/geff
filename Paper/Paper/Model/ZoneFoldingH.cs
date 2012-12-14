using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Paper.Model
{
    [Serializable()]
    public class ZoneFoldingH : ComponentBase, IResizableHeight
    {
        public int Height
        {
            get;
            set;
        }

        [XmlIgnore]
        public System.Drawing.Rectangle Rectangle
        {
            get
            {
                return new System.Drawing.Rectangle(0, Location.Y + Common.Delta.Y, Common.ScreenSize.Width, Height);
            }
        }

        public ZoneFoldingH()
        {
        }

        public ZoneFoldingH(int x, int y, int size)
            : base(x, y)
        {
            this.Height = size;
        }

        [XmlIgnore]
        public List<Line> LineResizableHeight
        {
            get
            {
                List<Line> _lineResizeable = new List<Line>();
                Line line = new Line(0, Location.Y + Height + Common.Delta.Y, Common.ScreenSize.Width, Location.Y + Height + Common.Delta.Y);
                _lineResizeable.Add(line);

                return _lineResizeable;
            }
        }

        [XmlIgnore]
        public override System.Drawing.Rectangle RectangleSelection
        {
            get
            {
                return Rectangle;
            }
        }
    }
}
