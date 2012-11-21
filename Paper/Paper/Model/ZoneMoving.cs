using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paper.Model
{
    public class ZoneMoving : ComponentBase, IResizableWidth, IResizableHeight
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

        public ZoneMoving(int x, int y)
            : base(x, y)
        {
        }
    }

    public enum ZoneMovingType
    {
        Horizontal,
        Vertical
    }
}
