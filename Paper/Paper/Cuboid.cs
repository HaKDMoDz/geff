using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Paper
{
    public class Cuboid
    {
        public Point Location { get; set; }
        public int Depth { get; set; }
        public int Width { get; set; }
        public ModeSelection ModeSelection { get; set; }

        public Cuboid(int x, int y, int depth, int width)
        {
            this.Location = new Point(x, y);
            this.Depth = depth;
            this.Width = width;
        }
    }

    public enum ModeSelection
    {
        None,
        NearMove,
        NearResize,
        SelectedMove,
        SelectedResize
    }
}
