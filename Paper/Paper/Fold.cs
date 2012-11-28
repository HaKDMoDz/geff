using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Paper
{
    public class Fold
    {
        public List<Line> ListLine { get; set; }

        public Fold()
        {
            this.ListLine = new List<Line>();
        }
    }

    public class Line
    {
        public Point P1 { get; set; }
        public Point P2 { get; set; }

        public Line() { }

        public Line(int x1, int y1, int x2, int y2)
        {
            this.P1 = new Point(x1, y1);
            this.P2 = new Point(x2, y2);
        }
    }
}
