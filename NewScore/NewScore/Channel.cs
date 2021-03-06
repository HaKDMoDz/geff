﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewScore
{
    public class Channel
    {
        public List<Measure> ListMeasure { get; set; }

        public Channel()
        {
            ListMeasure = new List<Measure>();
        }

        public Measure FindMeasure(float absolutePosition)
        {
            return ListMeasure.Find(m => m.MeasureStart <= absolutePosition && m.MeasureStart + m.MeasureLength > absolutePosition);
        }

        public void AddMeasure(Measure measure)
        {
            this.ListMeasure.Add(measure);
            measure.NumMeasure = (int)(measure.MeasureStart / measure.MeasureLength + 1);
        }
    }
}
