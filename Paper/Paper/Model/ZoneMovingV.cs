using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paper.Model
{
    public class ZoneMovingV : ComponentBase, IResizableWidth, IResizableHeight
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

        public ZoneMovingV(int x, int y, int width, int height)
            : base(x, y)
        {
            this.ZoneMovingType = Model.ZoneMovingType.Vertical;
            this.Width = width;
            this.Height = height;
        }

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

        public override System.Drawing.Rectangle RectangleSelection
        {
            get
            {
                return new System.Drawing.Rectangle(Location.X, Location.Y, Width, Height);
            }
        }
    }

    public enum ZoneMovingType
    {
        Horizontal,
        Vertical
    }
}
