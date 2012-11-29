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

        public ZoneMovingV(int x, int y)
            : base(x, y)
        {
            this.ZoneMovingType = Model.ZoneMovingType.Vertical;
        }

        public List<Line> LineResizableWidth
        {
            get
            {
                List<Line> _lineResizeable = new List<Line>();
                Line line = new Line(0, Location.Y + Height, Common.ScreenSize.Width, Location.Y + Height);
                _lineResizeable.Add(line);

                return _lineResizeable;
            }
        }

        public List<Line> LineResizableHeight
        {
            get
            {
                List<Line> _lineResizeable = new List<Line>();
                Line line = new Line(0, Location.Y + Height, Common.ScreenSize.Width, Location.Y + Height);
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
