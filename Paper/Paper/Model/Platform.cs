using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Drawing;

namespace Paper.Model
{
    [Serializable()]
    public class Platform : ComponentBase, IResizableWidth, IResizableHeight, IMoveable
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
        
        public Platform()
        {
        }

        public Platform(int x, int y, int width, int height)
            : base()
        {
            this.Location = new Point(x, y);
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

        public Point Location
        {
            get;
            set;
        }
    }
}
