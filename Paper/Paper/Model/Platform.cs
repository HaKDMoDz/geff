using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paper.Model
{
    public class Platform : ComponentBase, IResizableWidth, IResizableHeight
    {
        public Platform(int x, int y)
            : base(x, y)
        {
        }

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
    }
}
