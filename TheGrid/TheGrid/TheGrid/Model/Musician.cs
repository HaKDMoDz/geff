using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheGrid.Model.Instrument;

namespace TheGrid.Model
{
    public class Musician
    {
        public Cell CurrentCell { get; set; }
        public int CurrentDirection { get; set; }
        public Cell NextCell { get; set; }
        public List<InstrumentBase> Instruments  { get; set; }
    }
}
