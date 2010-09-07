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
        public Bone RootBone { get; set; }
        public Bone LeafBone { get; set; }

        int maxBones = 10;
        Random rnd;
        Graphics g;
        float delta = 0.01f;

        public Form1()
        {
            InitializeComponent();

            g = pic.CreateGraphics();
            CreateSkeleton();
        }

        private void pic_Click(object sender, EventArgs e)
        {
            DrawSkeleton();
        }

        private void CreateSkeleton()
        {
            rnd = new Random();
            Bone prevBone = null;
            int length = 100;

            for (int i = 0; i <= maxBones; i++)
            {
                Bone bone = new Bone();

                bone.ParentBone = prevBone;
                bone.PositionEnd = new Point(rnd.Next(-length, length), rnd.Next(-length, length));
                bone.Angle = 0f;

                if (prevBone != null)
                {
                    bone.AbsolutePositionEnd = new Point(bone.ParentBone.AbsolutePositionEnd.X + bone.PositionEnd.X,
                                                         bone.ParentBone.AbsolutePositionEnd.Y + bone.PositionEnd.Y);

                    bone.Length = (int)Math.Sqrt((double)(bone.PositionEnd.X) * (double)(bone.PositionEnd.X) +
                        (double)(bone.PositionEnd.Y) * (double)(bone.PositionEnd.Y));
                }
                else
                {
                    //bone.PositionEnd = new Point(rnd.Next(-length, length), rnd.Next(-length, length));
                    //bone.AbsolutePositionEnd = new Point(100 + bone.PositionEnd.X, 100 + bone.PositionEnd.Y);

                    bone.PositionEnd = new Point(0,0);
                    bone.AbsolutePositionEnd = new Point(0,0);
                }

                prevBone = bone;

                if (i == 0)
                    RootBone = bone;

                if (i == maxBones)
                    LeafBone = bone;
            }
        }

        private void DrawSkeleton()
        {
            g.Clear(Color.White);
            DrawBone(LeafBone);
        }

        private void DrawBone(Bone bone)
        {
            if (bone.ParentBone != null)
            {
                g.DrawLine(Pens.Black, bone.ParentBone.AbsolutePositionEnd, bone.AbsolutePositionEnd);

                DrawBone(bone.ParentBone);
            }
        }

        private void pic_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                CreateSkeleton();
            }
            else if(e.Button == MouseButtons.Left)
            {
                Calc(e.Location);
            }

            DrawSkeleton();
        }

        private void pic_MouseMove(object sender, MouseEventArgs e)
        {
            Calc(e.Location);

            DrawSkeleton();
        }

        private void Calc(Point point)
        {
            bool loop = true;
            int iteration = 0;

            InitCalc(LeafBone);

            while (loop)
            {
                CalcBone(LeafBone, point);

                iteration++;

                if (iteration > 50)
                {
                    loop = false;
                }
            }
        }

        private void InitCalc(Bone bone)
        {
            bone.TempPosition = bone.PositionEnd;

            if (bone.ParentBone != null)
                InitCalc(bone.ParentBone);
        }


        private void CalcBone(Bone bone, Point point)
        {
            if (bone.ParentBone != null)
            {
                Point vecBone = GetVector(bone.PositionEnd, bone.ParentBone.PositionEnd);
                Point vecPoint = GetVector(point, bone.ParentBone.PositionEnd);

                float angleBone = GetAngle(new Point(1, 0), vecBone);
                float anglePoint = GetAngle(new Point(1, 0), vecPoint);

                if (angleBone > anglePoint)
                {
                    angleBone = anglePoint + 0.90f * (angleBone - anglePoint);
                }
                else
                {
                    angleBone = angleBone + 0.1f * (anglePoint - angleBone);
                }

                bone.PositionEnd = new Point(
                    (int)((float)Math.Cos(angleBone) * (float)bone.Length),
                    (int)((float)Math.Sin(angleBone) * (float)bone.Length));

                bone.AbsolutePositionEnd = new Point(bone.ParentBone.AbsolutePositionEnd.X + bone.PositionEnd.X,
                                                         bone.ParentBone.AbsolutePositionEnd.Y + bone.PositionEnd.Y);


                CalcBone(bone.ParentBone, point);
            }
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

        public Point GetPerpVector(Point point)
        {
            Point perpVector = new Point(point.Y, -point.X);

            return perpVector;
        }
    }
}
