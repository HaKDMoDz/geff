using System;
using System.Collections.Generic;
using System.Text;

namespace CubEat
{
    public class Cell
    {
        public Sample Sample { get; set; }
        public List<Effect> ListEffect { get; set; }
        public Boolean IsEmitting { get; set; }
        public Boolean IsEmpty { get; set; }
        public TypeSymbol TypeSymbol { get; set; }
        public int Layer { get; set; }
        public int NumberOnLayer { get; set; }
        public Boolean IsOnMeasure { get; set; }
        public Boolean IsInPlayedTime { get; set; }
    }
}
