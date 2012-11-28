using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Paper.Model
{
    public abstract class ComponentBase
    {
        public Point Location { get; set; }
        public ModeSelection ModeSelection { get; set; }
        public DateTime CreationDate { get; set; }
        public int ColorIndex { get; set; }
        public abstract Rectangle RectangleSelection { get; }

        public ComponentBase(int x, int y)
        {
            this.Location = new Point(x, y);
            this.CreationDate = DateTime.Now;
        }
    }
}
