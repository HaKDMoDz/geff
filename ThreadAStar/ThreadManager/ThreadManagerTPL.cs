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

        private void ParallelComputBody2(IComputable computable)
        {
            computable.Compute();

            CountCalculated++;
        }


        private void ParallelComputeFinally(int index)
        {
            base.CalculCompleted(this.ListComputable[index]);

            if (CountCalculated >= this.ListComputable.Count && !AreAllCalculCompleted)
                base.AllCalculCompleted();
        }

        public override void StartComputation()
        {
            //--- Options de la parallalisation TPL
            parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = this.CountThread+20;

            cancellationToken = new CancellationTokenSource();
            parallelOptions.CancellationToken = cancellationToken.Token;
            //---

            Task.Factory.StartNew(() =>
            {
                Parallel.For<int>(0, this.ListComputable.Count, parallelOptions,
                    () => { return 0; },
                    (i,loopState, j) =>
                    {
                        ParallelComputBody(i);

                        return i;
                    },
                    i =>
                    {
                        ParallelComputeFinally(i);
                    }
                );

            }, parallelOptions.CancellationToken);

        }

        public override void StopComputation()
        {
            cancellationToken.Cancel();
        }

        protected override void CalculCompleted(IComputable computable)
        {
            base.CalculCompleted(computable);
        }

    }
}
