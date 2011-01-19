using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheGrid.Model.Instrument;

namespace TheGrid.Model
{
    public class Clip
    {
        public InstrumentBase Instrument { get; set; }
        public bool[] Directions { get; set; }
        public int? Repeater { get; set; }
        public int Speed { get; set; }

        public Clip()
        {
            Directions = new bool[6];
        }
    }
}
