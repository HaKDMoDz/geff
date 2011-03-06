using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheGrid.Model.Effect;

namespace TheGrid.Model.Instrument
{
    [Serializable]
    public class InstrumentCapture : InstrumentBase
    {
        public Sample Sample { get; set; }

        public InstrumentCapture()
        {
        }

        public InstrumentCapture(Sample sample)
        {
            this.Sample = sample;
        }
    }
}
