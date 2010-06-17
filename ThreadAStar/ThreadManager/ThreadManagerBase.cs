using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreadAStar.Threading;
using ThreadAStar.Model;

namespace ThreadAStar.ThreadManager
{
    public abstract class ThreadManagerBase
    {
        public List<ThreadingBaseMethod> ListThread { get; set; }
        public Int32 NombreThread { get; set; }
        
        public List<IComputable> ListComputable { get; set; }
        public Int32 CountCalculated { get; set; }
        public Boolean IsAllCalculCompleted = false;

        public ThreadManagerBase(int nombreThread, List<IComputable> listComputable)
        {
            this.ListThread = new List<ThreadingBaseMethod>();
            this.NombreThread = nombreThread;
            this.ListComputable = listComputable;
        }

        public abstract void StartComputation();

        public abstract void StopComputation();
    }
}
