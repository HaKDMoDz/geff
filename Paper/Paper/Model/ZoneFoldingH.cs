using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paper.Model
{
    public class ZoneFoldingH : ComponentBase, IResizableHeight
    {
        public int Height
        {
            get;
            set;
        }

        public System.Drawing.Rectangle Rectangle
        {
            get
            {
                return new System.Drawing.Rectangle(0, Location.Y, Common.ScreenSize.Width, Height);
            }
        }

        public ZoneFoldingH(int x, int y, int size)
            : base(x, y)
        {
            this.Height = size;
        }

        public List<Line> LineResizable
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
                return Rectangle;
            }
        }
    }
}
