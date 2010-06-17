using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreadAStar.ThreadManager
{
    public interface IThreadManager
    {
        void StartComputation();

        void StopComputation();
    }
}
