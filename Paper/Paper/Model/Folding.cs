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
                int prevHeight = _height;

                _height = (int)Math.Ceiling((double)value / (double)Common.depthUnity);

                if (_height > Common.MaxDepth * 2)
                    _height = Common.MaxDepth * 2;
                if (_height < 1)
                    _height = 1;

                if (prevHeight != _height)
                {
                    int a =0;
                }

                //this.Location.Offset(0,(prevHeight-_height) * Common.depthUnity);
                _location = new Point(_location.X, _location.Y + (-prevHeight + _height) * Common.depthUnity);
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


        public Point Location
        {
            get
            {
                return _location;
            }
            set
            {
                int y = _location.Y;

                //if (value.Y <= 0)
                //{
                //    y = value.Y;
                //}

                _location = new Point(value.X, value.Y);
            }
        }

        [XmlIgnore]
        public Cutting CuttingFace { get; set; }

        [XmlIgnore]
        public Cutting CuttingTop { get; set; }

        public Folding()
        {
            CuttingFace = new Cutting(this);
            CuttingTop = new Cutting(this);
        }

        public Folding(int x, int y, int width, int height)
            : base()
        {
            this.Location = new Point(x, y);
            this.Width = width;
            _height = height;
            CuttingFace = new Cutting(this);
            CuttingTop = new Cutting(this);
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
        public Rectangle RecTopWithoutDelta
        {
            get
            {
                return new Rectangle(Location.X, Location.Y - _height * Common.depthUnity, Width, _height * Common.depthUnity);
            }
        }

        [XmlIgnore]
        public Rectangle RecFaceWithoutDelta
        {
            get
            {
                return new Rectangle(Location.X, Location.Y, Width, -Location.Y + _height * Common.depthUnity);
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
            IResizableHeight yh = y as IResizableHeight;

            if (ret == 0 && xh != null && yh != null)
                ret = xh.Height - yh.Height;

            if (ret == 0)
                ret = x.CreationDate.CompareTo(y.CreationDate);

            return ret;
        }
    }

    public class CuboidComparerTop : IComparer<ComponentBase>
    {
        public int Compare(ComponentBase x, ComponentBase y)
        {
            Folding xm = x as Folding;
            Folding ym = y as Folding;

            int ret = 0;

            if (xm != null && ym != null)
                ret = xm.RecFace.Height - ym.RecFace.Height;

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
            IResizableHeight yh = y as IResizableHeight;

            int ret = 0;

            if (xh != null && yh != null)
                ret = -xh.Height + yh.Height;

            if (ret == 0)
                ret = x.CreationDate.CompareTo(y.CreationDate);

            return ret;
        }
    }

    public class Cutting
    {
        public Folding ParentFolding { get; set; }

        public List<Cutting> Cuttings { get; set; }

        public Rectangle Rectangle { get; set; }

        public bool IsEmpty { get; set; }

        public bool[] Borders { get; set; }

        public Cutting(Folding parentFolding)
        {
            ParentFolding = parentFolding;
            Cuttings = new List<Cutting>();
            Borders = new bool[4];

            Borders[0] = true;
            Borders[1] = true;
            Borders[2] = true;
            Borders[3] = true;
        }
    }

    public enum ModeSelection
    {
        None,
        Selected,
        NearMove,
        NearResizeWidth,
        NearResizeHeight,
        SelectedMove,
        SelectedResizeWidth,
        SelectedResizeHeight
    }
}
