using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreadAStar.Model;
using System.Threading;
using ThreadAStar.ThreadManager;
//using System.Diagnostics;

namespace ThreadAStar.Threading
{
    public class ThreadingNativeMethod : ThreadingBaseMethod
    {
        private Thread _thread { get; set; }
        private ThreadStart _threadStart { get; set; }

        public ThreadingNativeMethod(ThreadManagerSimple threadManager, IComputable computable)
            : base(threadManager, computable)
        {
            _threadStart = new ThreadStart(computable.Compute);
            _thread = new Thread(_threadStart);
        }

        public override void Start(params object[] parameter)
        {
            _thread.Start();

            while (_thread.ThreadState == ThreadState.Running || _thread.ThreadState == ThreadState.WaitSleepJoin)
            {
            }

            _threadManager.CalculCompleted(this);
        }

        public override void Stop()
        {
            _thread.Abort();
        }
    }
}
