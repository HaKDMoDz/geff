using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreadAStar.ThreadingMethod;
using System.Management;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;
using ThreadAStar.Model;

namespace ThreadAStar.ThreadManager
{
    public class ThreadManagerTPL : ThreadManagerBase
    {
        ParallelOptions parallelOptions;
        CancellationTokenSource cancellationToken;

        //private bool _cancelComputation = false;

        public ThreadManagerTPL(int countThread, List<IComputable> listComputable)
            : base(countThread, listComputable)
        {
        }

        private void ParallelComputBody(int index)
        {
            this.ListComputable[index].Compute();

            CountCalculated++;
        }

        private void ParallelComputBody(IComputable computable)
        {
            computable.Compute();

            CountCalculated++;
        }

        private void ParallelComputeFinally(int index)
        {

        }

        public override void StartComputation()
        {
            parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = this.CountThread;
            
            cancellationToken = new CancellationTokenSource();
            parallelOptions.CancellationToken = cancellationToken.Token;
            
            Task.Factory.StartNew(() =>
            {

                //parallelOptions.CancellationToken.

                //Parallel.For(0, this.ListComputable.Count, parallelOptions, 
                //    i => {ParallelComputBody(i);},
                //    i => { ParallelComputeFinally(i); });

                Parallel.ForEach<IComputable>(this.ListComputable, parallelOptions, computable => { ParallelComputBody(computable); });
            }, parallelOptions.CancellationToken);

        }

        public override void StopComputation()
        {
            //_cancelComputation = true;
            cancellationToken.Cancel();
        }

        protected override void CalculCompleted(IComputable computable)
        {
            base.CalculCompleted(computable);
        }
    }
}
