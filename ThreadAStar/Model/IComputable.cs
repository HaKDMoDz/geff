using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ThreadAStar.Model
{
    public interface IComputable
    {
        void Compute();

        void Draw(Graphics g);
    }
}
