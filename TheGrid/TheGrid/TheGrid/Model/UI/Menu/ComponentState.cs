using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheGrid.Model.UI.Menu
{
    public enum ComponentState
    {
        Opened,
        Closed,
        Opening,
        Closing,
        WaitDependency
    }
}
