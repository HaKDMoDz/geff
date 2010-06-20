using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreadAStar.Model
{
    public class ComputableEventArgs : EventArgs
    {
        public IComputable Computable { get; set; }
    }
}
