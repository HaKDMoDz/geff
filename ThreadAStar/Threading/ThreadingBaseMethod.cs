using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreadAStar.Model;
using System.Threading;

namespace ThreadAStar.Threading
{
    public abstract class ThreadingBaseMethod
    {
        protected IComputable _computable { get; set; }
        protected ThreadManagerSimple _threadManager { get; set; }

        public ThreadingBaseMethod(ThreadManagerSimple threadManager, IComputable computable)
        {
            _computable = computable;
            _threadManager = threadManager;
        }

        public virtual void Start(params object[] parameter)
        {
        }

        public virtual void Stop()
        {
        }
    }
}
