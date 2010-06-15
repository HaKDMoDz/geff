using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreadAStar.Model
{
    public struct TimelineData
    {
        public double Time;
        public Byte CPU;
        public long RAM;
        public byte CountNewThreads;
        public byte CountDeadThreads;
        public byte CountThreads;
    }
}
