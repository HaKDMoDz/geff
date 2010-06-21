using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using ThreadAStar.Model;
using ThreadAStar.ThreadManager;

namespace ThreadAStar.ThreadingMethod
{
    public class ThreadingBackgroundWorkerMethod : ThreadingBaseMethod
    {
        private BackgroundWorker _backgroundWorker { get; set; }

        public ThreadingBackgroundWorkerMethod(ThreadManagerBackgroundWorker threadManager, IComputable computable)
            : base(threadManager, computable)
        {
        }

        void _backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            computable.Compute();
            e.Result = true;
        }

        void _backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _backgroundWorker.Dispose();
            base.CalculCompleted();
        }

        public override void Start(params object[] parameter)
        {
            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.WorkerSupportsCancellation = true;
            _backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_backgroundWorker_RunWorkerCompleted);
            _backgroundWorker.DoWork += new DoWorkEventHandler(_backgroundWorker_DoWork);

            _backgroundWorker.RunWorkerAsync(parameter);
        }

        public override void Stop()
        {
            _backgroundWorker.CancelAsync();
        }
    }
}
