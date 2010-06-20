using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreadAStar.Model;
using System.Threading;
using ThreadAStar.ThreadManager;

namespace ThreadAStar.ThreadingMethod
{
    public class ThreadingNativeMethod : ThreadingBaseMethod
    {
        private Thread _thread { get; set; }
        private ThreadStart _threadStart { get; set; }

        public ThreadingNativeMethod(ThreadManagerBase threadManager, IComputable computable)
            : base(threadManager, computable)
        {
        }

        public override void Start(params object[] parameter)
        {
            _threadStart = new ThreadStart(computable.Compute);
            _thread = new Thread(_threadStart);

            _thread.Start();

            while (_thread.ThreadState == ThreadState.Running || _thread.ThreadState == ThreadState.WaitSleepJoin)
            {
            }

            base.CalculCompleted();
        }

        public override void Stop()
        {
            _thread.Abort();
        }
    }
}
