using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreadAStar.ThreadingMethod;
using System.Management;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;
using ThreadAStar.ThreadManager;
using ThreadAStar.Model;

namespace ThreadAStar.ThreadManager
{
    public class ThreadManagerNative : ThreadManagerBase
    {
        private Thread _thread { get; set; }
        private ThreadStart _threadStart { get; set; }
        private int _nextIdToCompute { get; set; }

        public ThreadManagerNative(int countThread, List<IComputable> listComputable)
            : base(countThread, listComputable)
        {
        }

        #region Méthodes publiques
        public override void StartComputation()
        {
            _threadStart = new ThreadStart(LaunchPrimaryThread);
            _thread = new Thread(_threadStart);

            _nextIdToCompute = 0;

            _thread.Start();
        }

        public override void StopComputation()
        {
            _thread.Abort();

            foreach (ThreadingBaseMethod threadingMethod in this.ListThread)
            {
                threadingMethod.Stop();
            }
        }
        #endregion

        #region Méthodes protégées
        protected override void CalculCompleted(IComputable computable)
        {
            CountCalculated++;

            base.CalculCompleted(computable);

            if (!IsThreadAlive())
            {
                return;
            }

            ThreadingBaseMethod threadingMethod = this.ListThread.Find(t => t.computable == computable);

            this.ListThread.Remove(threadingMethod);

            CreateNewThread(1);
        }
        #endregion

        #region Méthodes privées
        private void LaunchPrimaryThread()
        {
            CreateNewThread(this.CountThread);
        }

        private void CreateNewThread(int countThread)
        {
            for (int i = 0; i < countThread; i++)
            {
                if (_nextIdToCompute < this.ListComputable.Count && IsThreadAlive())
                {
                    ThreadingNativeMethod threadingMethod = new ThreadingNativeMethod(this, this.ListComputable[_nextIdToCompute]);

                    threadingMethod.CalculCompletedEvent += new EventHandler(threadingMethod_CalculCompletedEvent);
                    this.ListThread.Add(threadingMethod);

                    _nextIdToCompute++;

                    threadingMethod.Start();
                }
            }

            if (CountCalculated >= this.ListComputable.Count)
            {
                AllCalculCompleted();
            }
        }

        private bool IsThreadAlive()
        {
            return (_thread.ThreadState == System.Threading.ThreadState.Running || _thread.ThreadState == System.Threading.ThreadState.WaitSleepJoin);
        }

        protected override void AllCalculCompleted()
        {
            base.AllCalculCompleted();
        }

        private void threadingMethod_CalculCompletedEvent(object sender, EventArgs e)
        {
            ThreadingBaseMethod threadingMethod = (ThreadingBaseMethod)sender;
            CalculCompleted(threadingMethod.computable);
        }
        #endregion
    }
}
