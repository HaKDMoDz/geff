using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreadAStar.Model
{
    public enum TypeThreading : int
    {
        Natif = 1,
        BackgroundWorker = 2,
        TaskParallelLibrary = 3,
        None = 4,
    }
}
