using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paper.Model
{
    public class ZoneFoldingV : ComponentBase, IResizableWidth
    {
        public int Width
        {
            get;
            set;
        }

        public override System.Drawing.Rectangle RectangleSelection
        {
            get
            {
                return Rectangle;
            }
        }

        public System.Drawing.Rectangle Rectangle
        {
            get
            {
                return new System.Drawing.Rectangle(Location.X, 0, Width, Common.ScreenSize.Height);
            }
        }

        public ZoneFoldingV(int x, int y, int size)
            : base(x, y)
        {
            this.Width = size;
        }

        public List<Line> LineResizableWidth
        {
            get
            {
                List<Line> _lineResizeable = new List<Line>();

                Line line2 = new Line(Location.X + Width, 0, Location.X + Width, Common.ScreenSize.Height);

                _lineResizeable.Add(line2);

                return _lineResizeable;
            }
        }
    }
}
