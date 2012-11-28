﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paper.Model
{
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

        public ZoneMovingH(int x, int y)
            : base(x, y)
        {
            this.ZoneMovingType = Model.ZoneMovingType.Horizontal;
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
                return new System.Drawing.Rectangle(Location.X, Location.Y, Width, Height);
            }
        }
    }
}
