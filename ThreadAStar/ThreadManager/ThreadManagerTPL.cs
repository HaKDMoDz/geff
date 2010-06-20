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

        private void ParallelComputBody2(IComputable computable)
        {
            computable.Compute();

            CountCalculated++;
        }


        private void ParallelComputeFinally(int index)
        {
            base.CalculCompleted(this.ListComputable[index]);

            if (CountCalculated >= this.ListComputable.Count)
                base.AllCalculCompleted();
        }

        public override void StartComputation()
        {
            parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = this.CountThread+20;

            cancellationToken = new CancellationTokenSource();
            parallelOptions.CancellationToken = cancellationToken.Token;


            //---
            //         Parallel.For(0, 100, 1,
            //     () =>
            //     {
            //         return new ThreadTimeTracker()
            //         {
            //             ThreadGuid = Guid.NewGuid()
            //         };
            //     },
            //     (i, loopState) =>
            //     {
            //         doWork(i, loopState.ThreadLocalState);
            //     },
            //    (threadLocalState) =>
            //    {
            //        Console.WriteLine(String.Format(
            //            "Thread {0} processed for {1} ms.",
            //            threadLocalState.ThreadGuid.ToString(),
            //            threadLocalState.CumulativeThreadTime
            //            ));
            //    }
            //);
            //---

            Task.Factory.StartNew(() =>
            {

                //parallelOptions.CancellationToken.

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



                //i => {ParallelComputBody(i);},
                //j => { ParallelComputeFinally(j); });

                //Parallel.ForEach<IComputable>(
                //    this.ListComputable,
                //    parallelOptions,
                //    computable => { ParallelComputBody(computable); }

                //    );

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
