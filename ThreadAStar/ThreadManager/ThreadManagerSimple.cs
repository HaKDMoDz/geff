using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreadAStar.Threading;
using System.Management;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;
using ThreadAStar.ThreadManager;
using ThreadAStar.Model;

namespace ThreadAStar.ThreadManager
{
    public class ThreadManagerSimple : IThreadManager
    {
        public List<ThreadingBaseMethod> ListThread { get; set; }
        public Int32 NombreThread { get; set; }
        public TypeThreading TypeThreading { get; set; }
        public List<IComputable> ListComputable { get; set; }
        public Int32 CountCalculated { get; set; }
        public Boolean IsAllCalculCompleted = false;

        private BackgroundWorker _backgroundWorker;

        public ThreadManagerSimple(int nombreThread, TypeThreading typeThreading, List<IComputable> listComputable)
        {
            this.ListThread = new List<ThreadingBaseMethod>();
            this.NombreThread = nombreThread;
            this.TypeThreading = typeThreading;
            this.ListComputable = listComputable;

            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.WorkerSupportsCancellation = true; 
            _backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            _backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
        }

        public ThreadingBaseMethod CreateThreads(IComputable computable)
        {
            ThreadingBaseMethod threadingMethod = null;

            switch (this.TypeThreading)
            {
                case TypeThreading.Natif:
                    threadingMethod = new ThreadingNativeMethod(this, computable);
                    break;
                case TypeThreading.BackgroundWorker:
                    threadingMethod = new BackGroundWorkerMethod(this, computable);
                    break;
                default:
                    break;
            }

            this.ListThread.Add(threadingMethod);

            return threadingMethod;
        }

        public void StartComputation()
        {
            _backgroundWorker.RunWorkerAsync();
        }

        public void StopComputation()
        {
            _backgroundWorker.CancelAsync();

            foreach (ThreadingBaseMethod threadingMethod in this.ListThread)
            {
                threadingMethod.Stop();
            }
        }

        public void CalculCompleted(ThreadingBaseMethod threadingMethod)
        {
            if (_backgroundWorker.CancellationPending)
            {
                return;
            }

            this.ListThread.Remove(threadingMethod);

            if (CountCalculated < this.ListComputable.Count)
            {
                CountCalculated++;

                ThreadingBaseMethod newThreadingMethod = CreateThreads(this.ListComputable[CountCalculated]);
                newThreadingMethod.Start();
            }
            else
            {
                AllCalculCompleted();
            }
        }

        private void AllCalculCompleted()
        {
            IsAllCalculCompleted = true;
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < NombreThread; i++)
            {
                if (CountCalculated < this.ListComputable.Count && !_backgroundWorker.CancellationPending)
                {
                    ThreadingBaseMethod newThreadingMethod = CreateThreads(this.ListComputable[CountCalculated]);
                    newThreadingMethod.Start();
                    CountCalculated++;
                }
            }

            e.Result = true;
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }
    }
}
