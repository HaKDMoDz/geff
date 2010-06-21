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
using ThreadAStar.Model;

namespace ThreadAStar.ThreadManager
{
    public class ThreadManagerBackgroundWorker : ThreadManagerBase
    {
        private BackgroundWorker _backgroundWorker { get; set; }
        private int _nextIdToCompute { get; set; }

        public ThreadManagerBackgroundWorker(int countThread, List<IComputable> listComputable)
            : base(countThread, listComputable)
        {
        }

        #region Méthodes publiques
        public override void StartComputation()
        {
            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.WorkerSupportsCancellation = true;
            _backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);

            _backgroundWorker.RunWorkerAsync();
        }

        public override void StopComputation()
        {
            _backgroundWorker.CancelAsync();

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
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            _nextIdToCompute = 0;

            CreateNewThread(this.CountThread);

            while (IsThreadAlive())
            {
            }

            e.Result = true;
        }

        private void CreateNewThread(int countThread)
        {
            for (int i = 0; i < countThread; i++)
            {
                if (_nextIdToCompute < this.ListComputable.Count && IsThreadAlive())
                {
                    ThreadingBackgroundWorkerMethod threadingMethod = new ThreadingBackgroundWorkerMethod(this, this.ListComputable[_nextIdToCompute]);

                    threadingMethod.CalculCompletedEvent += new EventHandler(threadingMethod_CalculCompletedEvent);
                    this.ListThread.Add(threadingMethod);

                    _nextIdToCompute++;

                    threadingMethod.Start();
                }
            }

            if (CountCalculated >= this.ListComputable.Count && !AreAllCalculCompleted)
            {
                AllCalculCompleted();
            }
        }

        private bool IsThreadAlive()
        {
            return !_backgroundWorker.CancellationPending;
        }

        protected override void AllCalculCompleted()
        {
            base.AllCalculCompleted();

            StopComputation();
        }

        private void threadingMethod_CalculCompletedEvent(object sender, EventArgs e)
        {
            ThreadingBaseMethod threadingMethod = (ThreadingBaseMethod)sender;
            CalculCompleted(threadingMethod.computable);
        }
        #endregion
    }
}
