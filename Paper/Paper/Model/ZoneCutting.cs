using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paper.Model
{
    public class ZoneCutting : ComponentBase, IResizableWidth
    {
        public int Width
        {
            get;
            set;
        }

        public ZoneCuttingType ZoneCuttingType { get; set; }

        public ZoneCutting(int x, int y, ZoneCuttingType zoneCuttingType)
            : base(x, y)
        {
            this.ZoneCuttingType = zoneCuttingType;
        }
    }

    public enum ZoneCuttingType
    {
        Horizontal,
        Vertical
    }
}
