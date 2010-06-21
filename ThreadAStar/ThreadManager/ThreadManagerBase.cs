using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreadAStar.ThreadingMethod;
using ThreadAStar.Model;

namespace ThreadAStar.ThreadManager
{
    public abstract class ThreadManagerBase
    {
        public event EventHandler<ComputableEventArgs> CalculCompletedEvent;
        public event EventHandler AllCalculCompletedEvent;

        public List<ThreadingBaseMethod> ListThread { get; set; }
        public Int32 CountThread { get; set; }
        
        public List<IComputable> ListComputable { get; set; }
        public Int32 CountCalculated { get; set; }
        public volatile Boolean AreAllCalculCompleted = false;

        public ThreadManagerBase(int countThread, List<IComputable> listComputable)
        {
            this.ListThread = new List<ThreadingBaseMethod>();
            this.CountThread = countThread;
            this.ListComputable = listComputable;
        }

        public abstract void StartComputation();

        public abstract void StopComputation();

        protected virtual void CalculCompleted(IComputable computable)
        {
            if (CalculCompletedEvent != null)
            {
                ComputableEventArgs eventArgs = new ComputableEventArgs();
                eventArgs.Computable = computable;

                CalculCompletedEvent(this, eventArgs);
            }
        }

        protected virtual void AllCalculCompleted()
        {
            AreAllCalculCompleted = true;

            if (AllCalculCompletedEvent != null) 
                AllCalculCompletedEvent(this, null);
        }
    }
}
