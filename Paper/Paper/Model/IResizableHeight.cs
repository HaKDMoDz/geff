using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paper.Model
{
    interface IResizableHeight
    {
        int Height { get; set; }

        List<Line> LineResizableHeight { get; }
    }
}
