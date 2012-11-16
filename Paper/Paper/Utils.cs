using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Paper
{
    public static class Utils
    {
        public static int Distance(Point point1, Point point2)
        {
            int distance = (int)Math.Sqrt((point1.X - point2.X) * (point1.X - point2.X) + (point1.Y - point2.Y) * (point1.Y - point2.Y));

            return distance;
        }
    }
}
