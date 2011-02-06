using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheGrid.Model.Instrument
{
    [Serializable]
    public class InstrumentBase
    {
        public TimeSpan StartTime { get; set; }
    }
}
