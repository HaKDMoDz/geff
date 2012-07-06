using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewScore
{
    public class Measure
    {
        public float MeasureStart { get; set; }
        public float MeasureLength { get; set; }
        public List<Note> ListNode { get; set; }

        public Measure(float measureStart, float measureLength)
        {
            this.MeasureStart = measureStart;
            this.MeasureLength = measureLength;
            this.ListNode = new List<Note>();
        }
    }
}
