using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Paper.Model
{
    [Serializable()]
    public class Platform : ComponentBase, IResizableWidth, IResizableHeight
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
            : base(x, y)
        {
            this.Width = width;
            this.Height = height;
        }

        [XmlIgnore]
        public List<Line> LineResizableWidth
        {
            get
            {
                List<Line> _lineResizeable = new List<Line>();
                Line line = new Line(Location.X + this.Width, Location.Y, Location.X + this.Width, Location.Y + Height);
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
                Line line = new Line(Location.X, Location.Y + Height, Location.X + this.Width, Location.Y + Height);
                _lineResizeable.Add(line);

                return _lineResizeable;
            }
        }

        [XmlIgnore]
        public override System.Drawing.Rectangle RectangleSelection
        {
            get
            {
                return new System.Drawing.Rectangle(Location.X, Location.Y, Width, Height);
            }
        }
    }
}
