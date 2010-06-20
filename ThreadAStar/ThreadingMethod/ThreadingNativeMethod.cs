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
            //--- Initialise le thread de la résolution du calcul
            _threadStart = new ThreadStart(computable.Compute);
            _thread = new Thread(_threadStart);
            //---

            //---> démarre le thread
            _thread.Start();

            //--- Attends tant que le thread est en activité
            while (_thread.ThreadState == ThreadState.Running || _thread.ThreadState == ThreadState.WaitSleepJoin)
            { }
            //--

            //---> Une fois le thread terminé, le calcul est terminé
            //     Appel des évènements CalculCompleted
            base.CalculCompleted();
        }

        public override void Stop()
        {
            _thread.Abort();
        }
    }
}
