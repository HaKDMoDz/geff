using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml.Serialization;

namespace Paper.Model
{
    [Serializable()]
    public class Folding : ComponentBase, IResizableWidth, IResizableHeight, IMoveable
    {
        private int _height = 0;
        private Point _location = Point.Empty;

        [XmlIgnore]
        public int Height
        {
            get
            {
                return _height * Common.depthUnity;
            }
            set
            {
                _height = (int)Math.Ceiling((double)value / (double)Common.depthUnity);

                if (_height > Common.MaxDepth*2)
                    _height = Common.MaxDepth*2;
                if (_height < 1)
                    _height = 1;
            }
        }

        public int HeightSerializable
        {
            get
            {
                return _height;
            }
            set
            {
               _height = value;
            }
        }

        public int Width { get; set; }


        public Point Location {
            get
            {
                return _location;
            }
            set
            {
                int y = _location.Y;

                if (value.Y <= 0)
                {
                    y = value.Y;
                }

                _location = new Point(value.X, y);
            }
        }

        [XmlIgnore]
        public List<Rectangle> ListCutting { get; set; }

        public Folding()
        {
            this.ListCutting = new List<Rectangle>();
        }

        public Folding(int x, int y, int width, int height)
            : base()
        {
            this.Location = new Point(x, y);
            this.Width = width;
            _height = height;
            this.ListCutting = new List<Rectangle>();
        }

        [XmlIgnore]
        public Rectangle RecTop
        {
            get
            {
                return new Rectangle(Location.X + Common.Delta.X, Location.Y - _height * Common.depthUnity + Common.Delta.Y, Width, _height * Common.depthUnity);
            }
        }

        [XmlIgnore]
        public Rectangle RecFace
        {
            get
            {
                return new Rectangle(Location.X + Common.Delta.X, Location.Y + Common.Delta.Y, Width, -Location.Y + _height * Common.depthUnity);
            }
        }

        [XmlIgnore]
        public List<Line> LineResizableWidth
        {
            get
            {
                List<Line> _lineResizeable = new List<Line>();

                Line line = new Line(Location.X + Width + Common.Delta.X, Location.Y - _height * Common.depthUnity + Common.Delta.Y, Location.X + Width + Common.Delta.X, Common.Bottom + (_height * Common.depthUnity) + Common.Delta.Y);

                _lineResizeable.Add(line);

                return _lineResizeable;
            }
        }

        [XmlIgnore]
        public List<Line> LineResizableHeight
        {
            get
            {
                List<Line> _lineResizeable = new List<Line>();

                //Line line1 = new Line(Location.X + Common.Delta.X, Location.Y - Height * Common.depthUnity + Common.Delta.Y, Location.X + Width + Common.Delta.X, Location.Y - Height * Common.depthUnity + Common.Delta.Y);
                Line line1 = new Line(Location.X + Common.Delta.X, Location.Y + Common.Delta.Y, Location.X + Width + Common.Delta.X, Location.Y + Common.Delta.Y);

                Line line2 = new Line(Location.X + Common.Delta.X, _height * Common.depthUnity + Common.Delta.Y, Location.X + Width + Common.Delta.X, _height * Common.depthUnity + Common.Delta.Y);

                _lineResizeable.Add(line1);
                _lineResizeable.Add(line2);

                return _lineResizeable;
            }
        }

        [XmlIgnore]
        public override System.Drawing.Rectangle RectangleSelection
        {
            get
            {
                return new System.Drawing.Rectangle(Location.X + Common.Delta.X, Location.Y - _height * Common.depthUnity + Common.Delta.Y, Width, -Location.Y + 2 * (_height * Common.depthUnity));
            }
        }
    }

    public class CuboidComparerWithSelection : IComparer<ComponentBase>
    {
        public int Compare(ComponentBase x, ComponentBase y)
        {
            int ret = 0;

            if (x.ModeSelection != ModeSelection.None && y.ModeSelection == ModeSelection.None)
                ret = 1;
            else if (x.ModeSelection == ModeSelection.None && y.ModeSelection != ModeSelection.None)
                ret = -1;

            IResizableHeight xh = x as IResizableHeight;
            IResizableHeight yh = x as IResizableHeight;

            if (ret == 0 && xh != null && yh != null)
                ret = xh.Height - yh.Height;

            if (ret == 0)
                ret = x.CreationDate.CompareTo(y.CreationDate);

            return ret;
        }
    }

    public class CuboidComparer : IComparer<ComponentBase>
    {
        public int Compare(ComponentBase x, ComponentBase y)
        {
            IResizableHeight xh = x as IResizableHeight;
            IResizableHeight yh = x as IResizableHeight;

            int ret = 0;

            if (xh != null && yh != null)
                ret = xh.Height - yh.Height;

            if (ret == 0)
                ret = x.CreationDate.CompareTo(y.CreationDate);

            return ret;
        }
    }


    public enum ModeSelection
    {
        None,
        NearMove,
        NearResizeWidth,
        NearResizeHeight,
        SelectedMove,
        SelectedResizeWidth,
        SelectedResizeHeight
    }
}
