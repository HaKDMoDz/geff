using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreadAStar.Model;
using System.Threading;
using ThreadAStar.ThreadManager;

namespace ThreadAStar.ThreadingMethod
{
    public abstract class ThreadingBaseMethod
    {
        public event EventHandler CalculCompletedEvent;

        public IComputable computable { get; set; }
        protected ThreadManagerBase threadManager { get; set; }

        public ThreadingBaseMethod(ThreadManagerBase threadManager, IComputable computable)
        {
            this.computable = computable;
            this.threadManager = threadManager;
        }

        public abstract void Start(params object[] parameter);

        public abstract void Stop();

        protected virtual void CalculCompleted()
        {
            EventHandler handler = this.CalculCompletedEvent;
            if (handler != null) handler(this, null);
        }
    }
}
