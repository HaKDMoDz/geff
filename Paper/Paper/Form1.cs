using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

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

        List<Cuboid> listCuboid = new List<Cuboid>();
        Cuboid curCuboid = null;
        Line lineMidScreen = null;
        int depthUnity = 20;

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
            lineMidScreen = new Line();
            lineMidScreen.P1 = new Point(0, pic.Height / 2);
            lineMidScreen.P2 = new Point(pic.Width, pic.Height / 2);

            DrawScene();
        }

        private void pic_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                foreach (Cuboid cuboid in listCuboid)
                {
                    if (cuboid.ModeSelection == ModeSelection.NearMove)
                    {
                        cuboid.ModeSelection = ModeSelection.SelectedMove;
                        curCuboid = cuboid;
                    }

                    if (cuboid.ModeSelection == ModeSelection.NearResize)
                    {
                        cuboid.ModeSelection = ModeSelection.SelectedResize;
                        curCuboid = cuboid;
                    }
                }

                if (curCuboid == null)
                {
                    curCuboid = new Cuboid(e.X, e.Y, 1, 50);
                    curCuboid.ModeSelection = ModeSelection.SelectedMove;
                    listCuboid.Add(curCuboid);
                    SortCuboid();
                }

                DrawScene();
            }
        }

        private void pic_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                listCuboid = new List<Cuboid>();
                DrawScene();
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                curCuboid = null;
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                for (int i = 0; i < listCuboid.Count; i++)
                {
                    if (listCuboid[i].ModeSelection == ModeSelection.NearMove)
                    {
                        this.listCuboid.RemoveAt(i);
                        i--;
                    }
                }

                DrawScene();
            }
        }

        private void pic_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left && curCuboid != null)
            {
                if (curCuboid.ModeSelection == ModeSelection.SelectedMove)
                    curCuboid.Location = new Point(e.X, e.Y);

                if (curCuboid.ModeSelection == ModeSelection.SelectedResize)
                {
                    curCuboid.Width = e.X - curCuboid.Location.X;
                    curCuboid.Depth = (e.Y - lineMidScreen.P1.Y) / depthUnity;

                    if (curCuboid.Depth == 0)
                        curCuboid.Depth = 1;

                    SortCuboid();
                }

                CalcCuboidIntersections();

                DrawScene();
            }

            if (e.Button == System.Windows.Forms.MouseButtons.None)
            {
                int distanceNearestCuboid = int.MaxValue;

                Cuboid nearestCuboid = null;
                ModeSelection nearestModeSelection = ModeSelection.None;

                foreach (Cuboid cuboid in listCuboid)
                {
                    if (cuboid.ModeSelection != ModeSelection.SelectedMove)
                        cuboid.ModeSelection = ModeSelection.None;

                    //--- Distance move
                    int distance = Tools.Distance(new Point(e.X, e.Y), cuboid.Location);

                    if (distance < 5 && distance < distanceNearestCuboid)
                    {
                        nearestCuboid = cuboid;
                        nearestModeSelection = ModeSelection.NearMove;
                    }
                    //---

                    //--- Distance resize
                    distance = Tools.Distance(new Point(e.X, e.Y), new Point(cuboid.Location.X + cuboid.Width, lineMidScreen.P1.Y + depthUnity * cuboid.Depth));

                    if (distance < 5 && distance < distanceNearestCuboid)
                    {
                        nearestCuboid = cuboid;
                        nearestModeSelection = ModeSelection.NearResize;
                    }
                    //---
                }

                if (nearestCuboid != null)
                    nearestCuboid.ModeSelection = nearestModeSelection;

                DrawScene();
            }
        }

        private void SortCuboid()
        {
            listCuboid.Sort((c1, c2) => c1.Depth - c2.Depth);
        }

        private void CalcCuboidIntersections()
        {
            foreach (Cuboid cuboid in listCuboid)
            {
                if (cuboid.Depth > 1)
                {
                    Rectangle recTopCuboid = new Rectangle(cuboid.Location.X, cuboid.Location.Y - cuboid.Depth * depthUnity, cuboid.Width, cuboid.Depth * depthUnity);

                    foreach (Cuboid innerCuboid in listCuboid)
                    {
                        if (innerCuboid.Depth < cuboid.Depth)
                        {
                            Rectangle recFaceInnerCuboid = new Rectangle(innerCuboid.Location.X, innerCuboid.Location.Y, innerCuboid.Width, lineMidScreen.P1.Y - innerCuboid.Location.Y + innerCuboid.Depth * depthUnity);

                            recFaceInnerCuboid.Intersect(recTopCuboid);

                            if (recFaceInnerCuboid.Width * recFaceInnerCuboid.Height > 0)
                            {
                                int a = 0;
                            }
                        }
                    }
                }
            }
        }



        private void DrawScene()
        {
            gBmp.Clear(Color.White);

            SolidBrush brush = new SolidBrush(Color.FromArgb(91, 177, 255));
            gBmp.FillRectangle(brush, lineMidScreen.P1.X, lineMidScreen.P1.Y, lineMidScreen.P2.X, 2 * depthUnity);

            brush = new SolidBrush(Color.FromArgb(128, 194, 255));
            gBmp.FillRectangle(brush, lineMidScreen.P1.X, lineMidScreen.P1.Y + 2 * depthUnity, lineMidScreen.P2.X, 2 * depthUnity);

            brush = new SolidBrush(Color.FromArgb(181, 219, 255));
            gBmp.FillRectangle(brush, lineMidScreen.P1.X, lineMidScreen.P1.Y + 4 * depthUnity, lineMidScreen.P2.X, 2 * depthUnity);

            gBmp.DrawLine(penDotFar, new Point(0, pic.Height / 2), new Point(pic.Width, pic.Height / 2));

            Pen pen = Pens.Black;

            foreach (Cuboid cuboid in listCuboid)
            {
                gBmp.FillRectangle(Brushes.White, cuboid.Location.X, cuboid.Location.Y - depthUnity * cuboid.Depth, cuboid.Width, 2 * depthUnity * cuboid.Depth + lineMidScreen.P1.Y - cuboid.Location.Y);


                brush = new SolidBrush(Color.FromArgb(91, 177, 255));
                int depthFactor = cuboid.Depth > 1 ? 2 : 1;
                gBmp.FillRectangle(brush, cuboid.Location.X, cuboid.Location.Y - cuboid.Depth * depthUnity, cuboid.Width, depthFactor * depthUnity);

                if (cuboid.Depth > 2)
                {
                    brush = new SolidBrush(Color.FromArgb(128, 194, 255));
                    depthFactor = cuboid.Depth > 3 ? 2 : 1;
                    gBmp.FillRectangle(brush, cuboid.Location.X, cuboid.Location.Y + (2 - cuboid.Depth) * depthUnity, cuboid.Width, depthFactor * depthUnity);
                }

                if (cuboid.Depth > 4)
                {
                    brush = new SolidBrush(Color.FromArgb(181, 219, 255));
                    depthFactor = cuboid.Depth > 5 ? 2 : 1;
                    gBmp.FillRectangle(brush, cuboid.Location.X, cuboid.Location.Y + (4 - cuboid.Depth) * depthUnity + 2, cuboid.Width, depthFactor * depthUnity);
                }

                gBmp.DrawLine(pen, cuboid.Location.X, cuboid.Location.Y - depthUnity * cuboid.Depth, cuboid.Location.X, lineMidScreen.P1.Y + depthUnity * cuboid.Depth);
                gBmp.DrawLine(pen, cuboid.Location.X + cuboid.Width, cuboid.Location.Y - depthUnity * cuboid.Depth, cuboid.Location.X + cuboid.Width, lineMidScreen.P1.Y + depthUnity * cuboid.Depth);

                gBmp.DrawLine(penDotNear, cuboid.Location.X, cuboid.Location.Y, cuboid.Location.X + cuboid.Width, cuboid.Location.Y);

                gBmp.DrawLine(penDotFar, cuboid.Location.X, cuboid.Location.Y - depthUnity * cuboid.Depth, cuboid.Location.X + cuboid.Width, cuboid.Location.Y - depthUnity * cuboid.Depth);
                gBmp.DrawLine(penDotFar, cuboid.Location.X, lineMidScreen.P1.Y + depthUnity * cuboid.Depth, cuboid.Location.X + cuboid.Width, lineMidScreen.P1.Y + depthUnity * cuboid.Depth);

                if (cuboid.ModeSelection != ModeSelection.None)
                {
                    gBmp.DrawImage(Resource.Move, cuboid.Location.X - Resource.Move.Width / 2, cuboid.Location.Y - Resource.Move.Height / 2);
                    gBmp.DrawImage(Resource.Circle, cuboid.Location.X - Resource.Circle.Width / 2, cuboid.Location.Y - Resource.Circle.Height / 2);

                    gBmp.DrawImage(Resource.Resize, cuboid.Location.X + cuboid.Width - Resource.Resize.Width / 2, lineMidScreen.P1.Y + depthUnity * cuboid.Depth - Resource.Resize.Height / 2);
                    gBmp.DrawImage(Resource.Circle, cuboid.Location.X + cuboid.Width - Resource.Circle.Width / 2, lineMidScreen.P1.Y + depthUnity * cuboid.Depth - Resource.Circle.Height / 2);
                }
                else
                {
                    gBmp.DrawRectangle(Pens.Blue, cuboid.Location.X - 1, cuboid.Location.Y - 1, 2, 2);
                }
            }

            g.DrawImage(bmp, 0, 0);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && curCuboid != null)
            {
                this.listCuboid.Remove(curCuboid);
                curCuboid = null;
                DrawScene();
            }
        }


    }
}
