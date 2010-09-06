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

        int maxBones = 5;
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

            for (int i = 0; i <= maxBones; i++)
            {
                Bone bone = new Bone();

                bone.ParentBone = prevBone;
                bone.PositionEnd = new Point(rnd.Next(0, pic.Width), rnd.Next(0, pic.Height));
                bone.Angle = 0f;

                prevBone = bone;

                if (i == 0)
                    RootBone = bone;

                if (i == maxBones)
                    LeafBone = bone;
            }
        }

        private void DrawSkeleton()
        {
            DrawBone(LeafBone);
        }

        private void DrawBone(Bone bone)
        {
            if (bone.ParentBone != null)
            {
                g.DrawLine(Pens.Black, bone.ParentBone.PositionEnd, bone.PositionEnd);

                DrawBone(bone.ParentBone);
            }
        }

        private void pic_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                CreateSkeleton();
            else
            {
                Calc(e.Location);

                DrawSkeleton();
            }
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

                if (iteration > 20)
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
            Point perpVector = GetPerpVector(bone.PositionEnd);
            Point vector = new Point(point.X - bone.PositionEnd.X, point.Y - bone.PositionEnd.Y);

            float angle= GetAngle(perpVector, vector);



            if (bone.ParentBone != null)
                InitCalc(bone.ParentBone);
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
