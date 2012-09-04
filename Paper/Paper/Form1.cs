using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Paper
{
    public partial class Form1 : Form
    {
        Graphics g;
        Graphics gBmp;
        Bitmap bmp;
        Pen penDotFar;
        Pen penDotNear;

        List<Rec> listRec = new List<Rec>();
        Rec curRec = null;

        public Form1()
        {
            InitializeComponent();

            penDotFar = new Pen(Color.Green, 1);
            penDotFar.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            penDotNear = new Pen(Color.Blue, 1);
            penDotNear.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            Form1_Resize(null, null);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            g = pic.CreateGraphics();
            bmp = new Bitmap(pic.Width, pic.Height);
            gBmp = Graphics.FromImage(bmp);

            DrawScene();
        }

        private void pic_MouseDown(object sender, MouseEventArgs e)
        {
            curRec = new Rec(e.X, e.Y, 0, 0);

            listRec.Add(curRec);


        }

        private void pic_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                listRec = new List<Rec>();
                DrawScene();
            }
        }

        private void pic_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                //curRec = listRec[listRec.Count - 1];
                curRec.Width = e.X - curRec.X;
                curRec.Height = e.Y - curRec.Y;

                //listRec[listRec.Count - 1] = curRec;

                CalcRecFolding(curRec);

                DrawScene();
            }
        }

        private void SortRectangle()
        {
            listRec.Sort((r1, r2) =>  r1.Width * r1.Height - r2.Width * r2.Height);
        }

        private void CalcRecFolding(Rec rectangle)
        {
            SortRectangle();

            rectangle.Error = false;

            foreach (Rec rec in listRec)
            {
                if (rec != rectangle && rec.IntersectWithRectangle(rectangle))
                {
                    rectangle.FoldWithRectangle(rec);
                    return;
                }
            }

            if(rectangle.IntersectWithLine(new Point(0, pic.Height / 2)))
            {
                rectangle.FoldWithLine(new Point(0, pic.Height / 2), new Point(pic.Width, pic.Height / 2));
                return;
            }

            rectangle.ListFold = new List<Fold>();
        }

        private void DrawScene()
        {
            gBmp.Clear(Color.White);

            gBmp.DrawLine(penDotFar, new Point(0, pic.Height / 2), new Point(pic.Width, pic.Height / 2));


            foreach (Rec rec in listRec)
            {
                Pen pen = Pens.Black;
                if (rec.Error)
                    pen = Pens.Red;

                gBmp.DrawRectangle(pen, rec.Rectangle);

                foreach (Fold fold in rec.ListFold)
                {
                    foreach (Line line in fold.ListLine)
                    {
                        gBmp.DrawLine(penDotNear, line.P1, line.P2);
                    }
                }
            }

            g.DrawImage(bmp, 0, 0);
        }


    }
}
