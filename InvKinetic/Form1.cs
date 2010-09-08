using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InvKinetic
{
    public partial class Form1 : Form
    {
        int maxBones = 5;
        Random rnd;
        Graphics g;
        Graphics gBmp;
        float delta = 0.5f;
        Bitmap bmp = null;
        int nbSkeleton = 100;
        int length = 20;

        public List<Skeleton> ListSkeleton { get; set; }

        public Form1()
        {
            InitializeComponent();

            g = pic.CreateGraphics();
            bmp = new Bitmap(pic.Width, pic.Height);

            gBmp = Graphics.FromImage(bmp);

            CreateSkeleton();
        }

        private void pic_Click(object sender, EventArgs e)
        {
            DrawSkeleton();
        }

        private void CreateSkeleton()
        {
            rnd = new Random();
            ListSkeleton = new List<Skeleton>();

            for (int j = 0; j < nbSkeleton; j++)
            {
                Skeleton skeleton = new Skeleton();
                ListSkeleton.Add(skeleton);

                Bone prevBone = null;

                for (int i = 0; i <= maxBones; i++)
                {
                    Bone bone = new Bone();

                    bone.ParentBone = prevBone;
                    bone.PositionEnd = new Point(rnd.Next(-length, length), rnd.Next(-length, length));
                    //bone.PositionEnd = new Point(0, length);
                    bone.Angle = 0f;

                    if (prevBone != null)
                    {
                        prevBone.ChildBone = bone;

                        bone.AbsolutePositionEnd = new Point(bone.ParentBone.AbsolutePositionEnd.X + bone.PositionEnd.X,
                                                             bone.ParentBone.AbsolutePositionEnd.Y + bone.PositionEnd.Y);

                        bone.Length = Distance(bone.AbsolutePositionEnd, bone.ParentBone.AbsolutePositionEnd);

                        if (bone.ParentBone.ParentBone != null)
                        {
                            bone.Angle = GetAngle(GetVector(bone.ParentBone.ParentBone.AbsolutePositionEnd, bone.ParentBone.AbsolutePositionEnd), GetVector(bone.AbsolutePositionEnd, bone.ParentBone.AbsolutePositionEnd));
                        }
                        else
                        {
                            bone.Angle = GetAngle(new Point(1, 0), GetVector(bone.AbsolutePositionEnd, bone.ParentBone.AbsolutePositionEnd));
                        }
                    }
                    else
                    {
                        bone.PositionEnd = new Point((int)((float)pic.Width / 2f + 150f * (float)Math.Cos(6.28f / (float)nbSkeleton * (float)j)), (int)((float)pic.Height / 2f + 150f * (float)Math.Sin(6.28f / (float)nbSkeleton * (float)j)));
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
            gBmp.Clear(Color.White);

            foreach (Skeleton skeleton in ListSkeleton)
            {
                DrawBone(skeleton.LeafBone);
            }

            g.DrawImage(bmp, 0, 0);
        }

        private void DrawBone(Bone bone)
        {
            if (bone.ParentBone != null)
            {
                int size = 4;
                gBmp.DrawLine(Pens.Black, bone.ParentBone.AbsolutePositionEnd, bone.AbsolutePositionEnd);
                //gBmp.DrawRectangle(Pens.Black, bone.AbsolutePositionEnd.X - size / 2, bone.AbsolutePositionEnd.Y - size / 2, size, size);
                gBmp.FillRectangle(Brushes.DarkGray, bone.AbsolutePositionEnd.X - size / 2, bone.AbsolutePositionEnd.Y - size / 2, size, size);
                DrawBone(bone.ParentBone);
            }
        }

        private void pic_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                CreateSkeleton();
            }
            else if (e.Button == MouseButtons.Left)
            {
                Calc(e.Location);
            }

            DrawSkeleton();
        }

        private void pic_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                Calc(e.Location);

            DrawSkeleton();
        }

        private void Calc(Point point)
        {
            foreach (Skeleton skeleton in ListSkeleton)
            {
                bool loop = true;
                int iteration = 0;

                while (loop)
                {
                    CalcBone(skeleton.LeafBone, point);

                    iteration++;

                    OrganizeBone(skeleton.RootBone);

                    if (iteration > 10 || Distance(point, skeleton.LeafBone.AbsolutePositionEnd) < 10)
                    {
                        loop = false;
                    }
                }
            }
        }

 
        private void CalcBone(Bone bone, Point point)
        {
            if (bone.ParentBone != null)
            {
                Point vecBone = GetVector(bone.AbsolutePositionEnd, bone.ParentBone.AbsolutePositionEnd);
                Point vecPoint = GetVector(point, bone.ParentBone.AbsolutePositionEnd);

                float angleBone = GetAngle(new Point(1, 0), vecBone);
                float anglePoint = GetAngle(new Point(1, 0), vecPoint);

                if (angleBone > anglePoint)
                {
                    angleBone = anglePoint + (1f - delta) * (angleBone - anglePoint);
                }
                else
                {
                    angleBone = angleBone + delta * (anglePoint - angleBone);
                }

                bone.PositionEnd = new Point(
                    (int)((float)Math.Cos(angleBone) * (float)bone.Length),
                    (int)((float)Math.Sin(angleBone) * (float)bone.Length));

                bone.AbsolutePositionEnd = new Point(bone.ParentBone.AbsolutePositionEnd.X + bone.PositionEnd.X,
                                                         bone.ParentBone.AbsolutePositionEnd.Y + bone.PositionEnd.Y);
                if (bone.ParentBone.ParentBone != null)
                    bone.Angle = -GetAngle(GetVector(bone.AbsolutePositionEnd, bone.ParentBone.AbsolutePositionEnd), GetVector(bone.ParentBone.ParentBone.AbsolutePositionEnd, bone.ParentBone.AbsolutePositionEnd));
                else
                    bone.Angle = angleBone;

                CalcBone(bone.ParentBone, point);
            }
        }

        private void OrganizeBone(Bone bone)
        {
            if (bone.ParentBone != null)
            {
                float angleParent = GetAngle(new Point(1, 0), bone.ParentBone.PositionEnd);

                float angleBone = bone.Angle;

                if (bone.ParentBone.ParentBone != null)
                    angleBone += angleParent - (float)Math.PI;

                bone.PositionEnd = new Point(
                      (int)((float)Math.Cos(angleBone) * (float)bone.Length),
                      (int)((float)Math.Sin(angleBone) * (float)bone.Length));

                bone.AbsolutePositionEnd = new Point(bone.ParentBone.AbsolutePositionEnd.X + bone.PositionEnd.X,
                                                         bone.ParentBone.AbsolutePositionEnd.Y + bone.PositionEnd.Y);

            }

            if (bone.ChildBone != null)
                OrganizeBone(bone.ChildBone);
        }

        public Point GetVector(Point point1, Point point2)
        {
            return new Point(point1.X - point2.X, point1.Y - point2.Y);
        }

        public float GetAngle(Point vec1, Point vec2)
        {
            float dot = (float)vec1.X * (float)vec2.X + (float)vec1.Y * (float)vec2.Y;
            float pdot = (float)vec1.X * (float)vec2.Y - (float)vec1.Y * (float)vec2.X;
            return (float)Math.Atan2(pdot, dot);
        }

        public int Distance(Point point1, Point point2)
        {
            int distance = (int)Math.Sqrt((double)(point1.X - point2.X) * (double)(point1.X - point2.X) +
                        (double)(point1.Y - point2.Y) * (double)(point1.Y - point2.Y));

            return distance;
        }

        public Point GetPerpVector(Point point)
        {
            Point perpVector = new Point(point.Y, -point.X);

            return perpVector;
        }
    }
}
