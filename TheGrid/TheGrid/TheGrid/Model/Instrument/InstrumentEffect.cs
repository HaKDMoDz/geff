using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheGrid.Model.Instrument
{
    [Serializable]
    public class InstrumentEffect : InstrumentBase
    {
        public EffectType EffectType { get; set; }
    }
}
