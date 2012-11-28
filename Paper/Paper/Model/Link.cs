using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paper.Model
{
    public class Link : ComponentBase
    {
        public Link(int x, int y)
            : base(x, y)
        {
        }

        public List<Line> LineResizable
        {
            get;
            set;
        }

        public override System.Drawing.Rectangle RectangleSelection
        {
            get
            {
                return new System.Drawing.Rectangle(Location.X, Location.Y, 10, 10);
            }
        }
    }
}
