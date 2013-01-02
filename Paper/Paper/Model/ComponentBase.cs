using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Paper.Model
{
    [Serializable()]
    public abstract class ComponentBase
    {
        public ModeSelection ModeSelection { get; set; }
        public DateTime CreationDate { get; set; }
        public int ColorIndex { get; set; }
        public abstract Rectangle RectangleSelection { get; }

        public ComponentBase()
        {
            this.CreationDate = DateTime.Now;
        }
    }
}
