using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using ThreadAStar.Model;

namespace ThreadAStar.Threading
{
    public class BackGroundWorkerMethod : ThreadingBaseMethod
    {
        private BackgroundWorker _backgroundWorker { get; set; }

        public BackGroundWorkerMethod(ThreadManagerSimple threadManager, IComputable computable)
            : base(threadManager, computable)
        {
            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_backgroundWorker_RunWorkerCompleted);
            _backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(_backgroundWorker_ProgressChanged);
            _backgroundWorker.DoWork += new DoWorkEventHandler(_backgroundWorker_DoWork);
        }

        void _backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            _computable.Compute();
            e.Result = true;
        }

        void _backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

        void _backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this._threadManager.CalculCompleted(this);
        }

        public override void Start(params object[] parameter)
        {
            _backgroundWorker.RunWorkerAsync(parameter);
        }

        public override void Stop()
        {
        }
    }
}
