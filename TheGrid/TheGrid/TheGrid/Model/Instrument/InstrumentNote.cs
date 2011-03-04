using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheGrid.Model.Instrument
{
    [Serializable]
    public class InstrumentNote : InstrumentBase
    {
        public float Frequency { get; set; }
        public string NoteName { get; set; }

        public InstrumentNote()
        {
        }

        public InstrumentNote(float frequencey, string noteName)
        {
            Frequency = frequencey;
            NoteName = noteName;
        }
    }
}
