using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ThreadAStar.Model
{
    public struct ThreadData
    {
        public Int32 ThreadId;
        public Int16 Duration;
        public Byte CPUMin;
        public Byte CPUMax;
        public Byte CPUAverage;
        public Int16 CountRefresh;
        public PerformanceCounter PerformanceCounter;
    }
}
