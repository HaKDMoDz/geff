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
        public bool FixedValue { get; set; }
        public List<float> ListValue { get; set; }

        public InstrumentEffect()
        {
            this.ListValue = new List<float>();
        }

        public InstrumentEffect(EffectType effectType) : this()
        {
        }
    }
}
