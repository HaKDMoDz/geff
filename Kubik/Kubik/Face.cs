using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kubik
{
    public class Face
    {
        public int[] Values { get; set; }

        public Face(int value1, int value2)
        {
            this.Values = new int[]{value1, value2};
        }
    }
}
