using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Paper
{
    public class Rec
    {
        public Rectangle Rectangle;
        public List<Fold> ListFold { get; set; }
        public Boolean Error { get; set; }

        public int X
        {
            get
            {
                return this.Rectangle.X;
            }
            set
            {
                this.Rectangle.X = value;
            }
        }

        public int Y
        {
            get
            {
                return this.Rectangle.Y;
            }
            set
            {
                this.Rectangle.Y = value;
            }
        }

        public int Width
        {
            get
            {
                return this.Rectangle.Width;
            }
            set
            {
                this.Rectangle.Width = value;
            }
        }

        public int Height
        {
            get
            {
                return this.Rectangle.Height;
            }
            set
            {
                this.Rectangle.Height = value;
            }
        }

        public Rec(int x, int y, int width, int height)
        {
            this.Rectangle = new Rectangle(x, y, width, height);
            this.ListFold = new List<Fold>();
            this.Error = false;
        }

        public bool IntersectWithRectangle(Rec rec)
        {
            if (this.ListFold.Count > 0 && this.Rectangle.IntersectsWith(rec.Rectangle))
            {
                if (rec.Rectangle.Y < this.ListFold[0].ListLine[0].P1.Y && rec.Rectangle.Bottom > this.ListFold[0].ListLine[0].P1.Y)
                {
                    return true;
                }
                else
                {
                    rec.Error = true;
                    return false;
                }
            }
            else
                return false;
        }

        public void FoldWithRectangle(Rec rec)
        {
            this.ListFold = new List<Fold>();

            this.FoldWithLine(rec.ListFold[0].ListLine[0].P1, rec.ListFold[0].ListLine[0].P1);

        }

        public bool IntersectWithLine(Point point)
        {
            return this.Rectangle.Y < point.Y && this.Rectangle.Bottom > point.Y;
        }

        public void FoldWithLine(Point point, Point point2)
        {
            this.ListFold = new List<Fold>();

            Fold newFold = new Fold();
            Line newLine = new Line();
            //newFold.ListLine.Add(newLine);

            //newLine.P1 = new Point(this.Rectangle.X, point.Y);
            //newLine.P2 = new Point(this.Rectangle.Right, point.Y);

            //this.ListFold.Add(newFold);


            newFold = new Fold();
            newLine = new Line();
            newFold.ListLine.Add(newLine);

            newLine.P1 = new Point(this.Rectangle.X, this.Rectangle.Y + this.Rectangle.Bottom - point.Y);
            newLine.P2 = new Point(this.Rectangle.Right, this.Rectangle.Y + this.Rectangle.Bottom - point.Y);

            this.ListFold.Add(newFold);
        }
    }
}
