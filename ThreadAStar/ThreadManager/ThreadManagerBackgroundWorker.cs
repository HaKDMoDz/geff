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
        private BackgroundWorker _backgroundWorker;

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
            _backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);

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
            CreateNewThread(this.CountThread);

            e.Result = true;
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        private void CreateNewThread(int countThread)
        {
            for (int i = 0; i < countThread; i++)
            {
                if (CountCalculated < this.ListComputable.Count && IsThreadAlive())
                {
                    ThreadingBackgroundWorkerMethod threadingMethod = new ThreadingBackgroundWorkerMethod(this, this.ListComputable[CountCalculated]);

                    threadingMethod.CalculCompletedEvent += new EventHandler(threadingMethod_CalculCompletedEvent);
                    this.ListThread.Add(threadingMethod);

                    CountCalculated++;

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
            return !_backgroundWorker.CancellationPending;
        }

        private void AllCalculCompleted()
        {
            AreAllCalculCompleted = true;
        }

        private void threadingMethod_CalculCompletedEvent(object sender, EventArgs e)
        {
            ThreadingBaseMethod threadingMethod = (ThreadingBaseMethod)sender;
            CalculCompleted(threadingMethod.computable);
        }
        #endregion
    }
}
