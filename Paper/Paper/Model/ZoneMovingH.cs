using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Paper.Model
{
    [Serializable()]
    public class ZoneMovingH : ComponentBase, IResizableWidth, IResizableHeight
    {
        public int Width
        {
            get;
            set;
        }

        public int Height
        {
            get;
            set;
        }

        public ZoneMovingType ZoneMovingType { get; set; }

        public ZoneMovingH()
        {
        }

        public ZoneMovingH(int x, int y, int width, int height)
            : base(x, y)
        {
            this.ZoneMovingType = Model.ZoneMovingType.Horizontal;
            this.Width = width;
            this.Height = height;
        }

        [XmlIgnore]
        public List<Line> LineResizableWidth
        {
            get
            {
                List<Line> _lineResizeable = new List<Line>();
                Line line = new Line(Location.X + this.Width + Common.Delta.X, Location.Y + Common.Delta.Y, Location.X + this.Width + Common.Delta.X, Location.Y + Height + Common.Delta.Y);
                _lineResizeable.Add(line);

                return _lineResizeable;
            }
        }

        [XmlIgnore]
        public List<Line> LineResizableHeight
        {
            get
            {
                List<Line> _lineResizeable = new List<Line>();
                Line line = new Line(Location.X + Common.Delta.X, Location.Y + Height + Common.Delta.Y, Location.X + this.Width + Common.Delta.X, Location.Y + Height + Common.Delta.Y);
                _lineResizeable.Add(line);

                return _lineResizeable;
            }
        }

        [XmlIgnore]
        public override System.Drawing.Rectangle RectangleSelection
        {
            get
            {
                return new System.Drawing.Rectangle(Location.X + Common.Delta.X, Location.Y + Common.Delta.Y, Width, Height);
            }
        }
    }
}
