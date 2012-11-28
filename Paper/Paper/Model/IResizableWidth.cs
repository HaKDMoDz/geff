using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paper.Model
{
    interface IResizableWidth : IResizable
    {
        int Width { get; set; }
    }
}
