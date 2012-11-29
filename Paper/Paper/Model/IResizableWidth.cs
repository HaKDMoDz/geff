using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paper.Model
{
    interface IResizableWidth
    {
        int Width { get; set; }

        List<Line> LineResizableWidth { get; }
    }
}
