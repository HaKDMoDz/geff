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
        public DateTime CreationDate { get; set; }

        public List<Rectangle> ListCutting { get; set; }

        public Cuboid(int x, int y, int depth, int width)
        {
            this.Location = new Point(x, y);
            this.Depth = depth;
            this.Width = width;
            this.CreationDate = DateTime.Now;
            this.ListCutting = new List<Rectangle>();
        }
    }

    public class CuboidComparerWithSelection : IComparer<Cuboid>
    {
        public int Compare(Cuboid x, Cuboid y)
        {
            int ret = 0;

            if (x.ModeSelection != ModeSelection.None && y.ModeSelection == ModeSelection.None)
                ret = 1;
            else if (x.ModeSelection == ModeSelection.None && y.ModeSelection != ModeSelection.None)
                ret = -1;

            if (ret == 0)
                ret = x.Depth - y.Depth;

            if (ret == 0)
                ret = x.CreationDate.CompareTo(y.CreationDate);

            return ret;
        }
    }

    public class CuboidComparer : IComparer<Cuboid>
    {
        public int Compare(Cuboid x, Cuboid y)
        {
            int ret = x.Depth - y.Depth;

            if (ret == 0)
                ret = x.CreationDate.CompareTo(y.CreationDate);

            return ret;
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
