using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;

namespace ThreadAStar.Model
{
    public class MatrixMultiply : IComputable
    {
        public void Compute()
        {
            Random rnd = new Random();

            Point[] pts = new Point[3];
            pts[0] = new Point(0, 0);
            pts[1] = new Point(1, 0);
            pts[2] = new Point(0, 1);

            for (int i = 0; i < 500; i++)
            {
                //Matrix mtx1 = new Matrix(new System.Drawing.Rectangle(rnd.Next(0), rnd.Next(0), rnd.Next(10),rnd.Next(10)), pts);
                //Matrix mtx2 = new Matrix(new System.Drawing.Rectangle(rnd.Next(0), rnd.Next(0), rnd.Next(10), rnd.Next(10)), pts);

                Matrix mtx1 = new Matrix((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble());
                Matrix mtx2 = new Matrix((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble());


                mtx1.Multiply(mtx2);

                mtx1.Dispose();
                mtx2.Dispose();

                Thread.Sleep(1);
            }
        }

        public void Draw(Graphics g)
        {
        }
    }
}
