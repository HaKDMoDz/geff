using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace InvKinetic
{
    public partial class Form1 : Form
    {
        int maxBones = 150;
        Random rnd;
        Graphics g;
        Graphics gBmp;
        float delta = 0.001f;
        Bitmap bmp = null;
        int nbSkeleton = 3;
        int length = 8;
        PointF mousePoint = PointF.Empty;

        public List<Skeleton> ListSkeleton { get; set; }

        public Form1()
        {
            InitializeComponent();

            //this.Opacity = 0.4;
            this.MouseWheel += new MouseEventHandler(pic_MouseWheel);
            this.TopMost = true;

            this.Top = 0;
            this.Left = 0;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;

            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);

            this.TransparencyKey = Color.Red;

            g = this.CreateGraphics();
            bmp = new Bitmap(this.Width, this.Height);
            gBmp = Graphics.FromImage(bmp);

            CreateSkeleton();
        }

        private void CreateSkeleton()
        {
            rnd = new Random();
            ListSkeleton = new List<Skeleton>();

            for (int j = 0; j < nbSkeleton; j++)
            {
                Skeleton skeleton = new Skeleton();
                skeleton.Index = j;
                ListSkeleton.Add(skeleton);

                Bone prevBone = null;

                for (int i = 0; i <= maxBones; i++)
                {
                    Bone bone = new Bone();

                    bone.ParentBone = prevBone;
                    //bone.PositionEnd = new PointF(rnd.Next(-length, length), rnd.Next(-length, length));
                    bone.PositionEnd = new PointF(0, length);
                    bone.Level = i;
                    bone.Skeleton = skeleton;

                    if (prevBone != null)
                    {
                        prevBone.ChildBone = bone;

                        bone.AbsolutePositionEnd = new PointF(bone.ParentBone.AbsolutePositionEnd.X + bone.PositionEnd.X,
                                                             bone.ParentBone.AbsolutePositionEnd.Y + bone.PositionEnd.Y);

                        bone.Length = Distance(bone.AbsolutePositionEnd, bone.ParentBone.AbsolutePositionEnd);


                        if (bone.ParentBone.ParentBone != null)
                        {
                            bone.Angle = GetAngle(GetVector(bone.ParentBone.ParentBone.AbsolutePositionEnd, bone.ParentBone.AbsolutePositionEnd), GetVector(bone.AbsolutePositionEnd, bone.ParentBone.AbsolutePositionEnd));
                        }
                        else
                        {
                            bone.Angle = GetAngle(new PointF(1, 0), GetVector(bone.AbsolutePositionEnd, bone.ParentBone.AbsolutePositionEnd));
                        }

                        float constraintAngle = (float)Math.PI / 6f;

                        bone.AngleConstraintMin = Math.Abs(bone.Angle - constraintAngle * ((float)i / (float)maxBones));
                        bone.AngleConstraintMax = Math.Abs(bone.Angle + constraintAngle * ((float)i / (float)maxBones));
                    }
                    else
                    {
                        //bone.PositionEnd = new PointF((int)((float)pic.Width / 2f + 200f * (float)Math.Cos(6.28f / (float)nbSkeleton * (float)j)), (int)((float)pic.Height / 2f + 200f * (float)Math.Sin(6.28f / (float)nbSkeleton * (float)j)));
                        bone.PositionEnd = new PointF(0, this.Height / nbSkeleton * j);
                        bone.AbsolutePositionEnd = bone.PositionEnd;
                    }

                    prevBone = bone;

                    if (i == 0)
                        skeleton.RootBone = bone;

                    if (i == maxBones)
                        skeleton.LeafBone = bone;
                }
            }
        }


        private void DrawSkeleton()
        {
            Color f = Color.Red;//.FromArgb(rnd.Next(255), rnd.Next(255),rnd.Next(255));

            gBmp.Clear(f);

            foreach (Skeleton skeleton in ListSkeleton)
            {
                DrawBone(skeleton.RootBone);
            }


            g.DrawImage(bmp, 0, 0);
        }

        private void DrawBone(Bone bone)
        {
            if (bone.ChildBone != null)
            {
                int sizeBone = maxBones + 15 - bone.Level;

                //int n = (int)(255f-150f * ((float)bone.Level / (float)maxBones));
                int n = (int)(255f - 150f * ((float)bone.Level / (float)maxBones));

                //Brush brush = new SolidBrush(Color.FromArgb(255, n, n, 50 + 180 * bone.Skeleton.Index / nbSkeleton));
                Brush brush = new SolidBrush(Color.FromArgb(255, n, n, n));

                gBmp.FillEllipse(brush, bone.AbsolutePositionEnd.X, bone.AbsolutePositionEnd.Y, sizeBone, sizeBone);

                DrawBone(bone.ChildBone);
            }
        }

        private void Calc(PointF PointF)
        {
            foreach (Skeleton skeleton in ListSkeleton)
            {
                int iteration = 0;

                while (iteration < 10)//&& Distance(PointF, skeleton.LeafBone.AbsolutePositionEnd) > 10f)
                {
                    CalcBone(skeleton.LeafBone, PointF);

                    iteration++;

                    OrganizeBone(skeleton.RootBone);
                }
            }
        }


        private void CalcBone(Bone bone, PointF PointF)
        {
            if (bone.ParentBone != null)
            {
                PointF vecBone = GetVector(bone.AbsolutePositionEnd, bone.ParentBone.AbsolutePositionEnd);
                PointF vecPointF = GetVector(PointF, bone.ParentBone.AbsolutePositionEnd);

                float angleBone = GetAngle(new PointF(1, 0), vecBone);
                float anglePointF = GetAngle(new PointF(1, 0), vecPointF);
                float prevAngle = bone.Angle;

                if (angleBone > anglePointF)
                {
                    angleBone = anglePointF + (1f - delta) * (angleBone - anglePointF);
                }
                else
                {
                    angleBone = angleBone + delta * (anglePointF - angleBone);
                }

                bone.PositionEnd = new PointF(
                    (float)Math.Cos(angleBone) * (float)bone.Length,
                    (float)Math.Sin(angleBone) * (float)bone.Length);

                bone.AbsolutePositionEnd = new PointF(bone.ParentBone.AbsolutePositionEnd.X + bone.PositionEnd.X,
                                                         bone.ParentBone.AbsolutePositionEnd.Y + bone.PositionEnd.Y);
                if (bone.ParentBone.ParentBone != null)
                    bone.Angle = -GetAngle(GetVector(bone.AbsolutePositionEnd, bone.ParentBone.AbsolutePositionEnd), GetVector(bone.ParentBone.ParentBone.AbsolutePositionEnd, bone.ParentBone.AbsolutePositionEnd));
                else
                    bone.Angle = angleBone;

                //--- Contrainte par les angles de torsion
                if (Math.Abs(bone.Angle) < bone.AngleConstraintMin || Math.Abs(bone.Angle) > bone.AngleConstraintMax)
                    bone.Angle = prevAngle;
                //---

                CalcBone(bone.ParentBone, PointF);
            }
        }

        private void OrganizeBone(Bone bone)
        {
            if (bone.ParentBone != null)
            {
                float angleParent = GetAngle(new PointF(1, 0), bone.ParentBone.PositionEnd);

                float angleBone = bone.Angle;

                if (bone.ParentBone.ParentBone != null)
                    angleBone += angleParent - (float)Math.PI;

                bone.PositionEnd = new PointF(
                      (float)Math.Cos(angleBone) * (float)bone.Length,
                      (float)Math.Sin(angleBone) * (float)bone.Length);

                bone.AbsolutePositionEnd = new PointF(bone.ParentBone.AbsolutePositionEnd.X + bone.PositionEnd.X, bone.ParentBone.AbsolutePositionEnd.Y + bone.PositionEnd.Y);
            }

            if (bone.ChildBone != null)
                OrganizeBone(bone.ChildBone);
        }

        public PointF GetVector(PointF PointF1, PointF PointF2)
        {
            return new PointF(PointF1.X - PointF2.X, PointF1.Y - PointF2.Y);
        }

        public float GetAngle(PointF vec1, PointF vec2)
        {
            float dot = vec1.X * vec2.X + vec1.Y * vec2.Y;
            float pdot = vec1.X * vec2.Y - vec1.Y * vec2.X;
            return (float)Math.Atan2(pdot, dot);
        }

        public float Distance(PointF PointF1, PointF PointF2)
        {
            return (float)Math.Sqrt((PointF1.X - PointF2.X) * (PointF1.X - PointF2.X) +
                        (PointF1.Y - PointF2.Y) * (PointF1.Y - PointF2.Y));
        }

        private void pic_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                delta *= 2;
            }
            else
            {
                delta /= 2;
            }
        }

        //private void pic_MouseUp(object sender, MouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Right)
        //    {
        //        CreateSkeleton();
        //    }
        //    else if (e.Button == MouseButtons.Left)
        //    {
        //        Calc(e.Location);
        //    }
        //    Application.Exit();

        //    DrawSkeleton();
        //}

        private void timer1_Tick(object sender, EventArgs e)
        {
            Calc(mousePoint);
            DrawSkeleton();
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            //Application.Exit();

        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            mousePoint = new PointF(e.X, e.Y);

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                g = this.CreateGraphics();
                bmp = new Bitmap(this.Width, this.Height);
                gBmp = Graphics.FromImage(bmp);

                CreateSkeleton();
            }
        }
    }
}
