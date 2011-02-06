using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheGrid.Model.Instrument
{
    [Serializable]
    public class InstrumentSample : InstrumentBase
    {
        public Sample Sample { get; set; }
    }
}
