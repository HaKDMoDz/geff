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

        #region Méthodes publiques
        public override void StartComputation()
        {
            //--- Options de la parallalisation TPL
            parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = this.CountThread;

            cancellationToken = new CancellationTokenSource();
            parallelOptions.CancellationToken = cancellationToken.Token;
            //---

            //---
            Task.Factory.StartNew(() =>
            {
                Parallel.ForEach<IComputable>(this.ListComputable, parallelOptions,
                    computable =>
                    {
                        ParallelCompute(computable);
                    }
                );

            }, parallelOptions.CancellationToken);
        }

        public override void StopComputation()
        {
            cancellationToken.Cancel();
        }
        #endregion

        #region Méthodes privées
        private void ParallelCompute(IComputable computable)
        {
            computable.Compute();

            CountCalculated++;

            base.CalculCompleted(computable);

            if (CountCalculated >= this.ListComputable.Count && !AreAllCalculCompleted)
                base.AllCalculCompleted();
        }
        #endregion
    }
}
