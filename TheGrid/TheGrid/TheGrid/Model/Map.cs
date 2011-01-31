using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TheGrid.Common;
using System.Threading.Tasks;

namespace TheGrid.Model
{
    public class Map
    {
        public List<Cell> Cells { get; set; }
        public List<Vector3> Points { get; set; }
        public List<Vector3> Normals { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
        public float R { get; set; }

        public Map(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            this.R = 0.5f;// 9.9855f;
        }

        public void CreateGrid()
        {
            Cells = new List<Cell>();

            Random rnd = new Random();

            //float d = (float)Math.Sqrt(0.75);

            float width = R;
            float height =R * (float)Math.Sin(MathHelper.Pi / 3);

            for (int y = 0; y <= Height / 2; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    float fx = (float)x;
                    float fy = (float)y;

                    fx = fx * width * 3f;
                    fy = fy * height * 2f;

                    Cell cell2 = new Cell(this, x, y * 2,
                         fx,
                         fy);

                    Cells.Add(cell2);

                    cell2.Height = 0;
                }

                for (int x = 0; x < Width; x++)
                {
                    float fx = (float)x;
                    float fy = (float)y;

                    fx = fx * width * 3f + width*1.5f;
                    fy = fy * height*2f-height;

                    Cell cell1 = new Cell(this, x, y * 2-1,
                         fx,
                         fy);

                    Cells.Add(cell1);

                    cell1.Height = 0;
                }
            }

            CalcNeighborough();

            //DrawMapIntoImageFile();
        }

        public void CalcNeighborough()
        {
            foreach (Cell cell in Cells)
            {
                if (cell.Coord.Y % 2 == 1)
                {
                    //cell.Neighbourghs[1] = GetNeighborough(cell, 1, -1);
                    cell.Neighbourghs[1] = GetNeighborough(cell, 0, -2);
                    cell.Neighbourghs[2] = GetNeighborough(cell, 1, 1);
                    cell.Neighbourghs[3] = GetNeighborough(cell, 0, 2);
                    cell.Neighbourghs[4] = GetNeighborough(cell, 0, 1);
                    cell.Neighbourghs[5] = GetNeighborough(cell, 0, -1);
                    //cell.Neighbourghs[6] = GetNeighborough(cell, 0, -2);
                    cell.Neighbourghs[6] = GetNeighborough(cell, 1, -1);
                }
                else
                {
                    //cell.Neighbourghs[1] = GetNeighborough(cell, 0, -1);
                    cell.Neighbourghs[6] = GetNeighborough(cell, 0, -2);
                    cell.Neighbourghs[2] = GetNeighborough(cell, 0, 1);
                    cell.Neighbourghs[3] = GetNeighborough(cell, 0, 2);
                    cell.Neighbourghs[4] = GetNeighborough(cell, -1, 1);
                    cell.Neighbourghs[5] = GetNeighborough(cell, -1, -1);
                    //cell.Neighbourghs[6] = GetNeighborough(cell, 0, -2);
                    cell.Neighbourghs[1] = GetNeighborough(cell, 0, -1);
                }
            }
        }

        private Cell GetNeighborough(Cell cell, int offsetX, int offsetY)
        {
            if (cell.Coord.X + offsetX - 1 >= 0 && cell.Coord.X + offsetX <= this.Width &&
                cell.Coord.Y + offsetY - 1 >= 0 && cell.Coord.Y + offsetY <= this.Height)
            {
                return Cells[(cell.Coord.Y + offsetY - 1) * this.Width + cell.Coord.X + offsetX - 1];
            }

            return null;
        }

      
        public void DrawMapIntoImageFile()
        {
            System.Drawing.Image img = new System.Drawing.Bitmap(3000, 800);

            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(img);
            //Bitmap b = new Bitmap(600, 600, g);

            g.Clear(System.Drawing.Color.White);

            int index = 0;
            int d = 5;

            foreach (Cell cell in Cells)
            {
                //g.DrawRectangle(System.Drawing.Pens.Black, cell.Location.X * d, cell.Location.Y * d, 7 * d, d*2);
                g.DrawString(cell.Coord.ToString() + " " + index.ToString(), new System.Drawing.Font("Arial", 10f), System.Drawing.Brushes.Black, new System.Drawing.PointF(cell.Location.X * d - 40, cell.Location.Y * d - d));
                index++;

                int prevPoint = -1;
                for (int i = 1; i <= 6; i++)
                {
                    if (prevPoint != -1)
                        g.DrawLine(System.Drawing.Pens.Red, Points[cell.Points[prevPoint]].X * d, Points[cell.Points[prevPoint]].Y * d, Points[cell.Points[i]].X * d, Points[cell.Points[i]].Y * d);

                    prevPoint = i;
                }

                g.DrawLine(System.Drawing.Pens.Red, Points[cell.Points[prevPoint]].X * d, Points[cell.Points[prevPoint]].Y * d, Points[cell.Points[1]].X * d, Points[cell.Points[1]].Y * d);

                //for (int i = 1; i <= 6; i++)
                //{
                //    //g.DrawRectangle(System.Drawing.Pens.Blue, cell.Points[i].X * d, cell.Points[i].Y * d, 5, 5);

                //    g.DrawString(i.ToString(), new System.Drawing.Font("Arial", 10f), System.Drawing.Brushes.Black, Points[cell.Points[1]].X * d, Points[cell.Points[1]].Y * d);
                //}
            }

            img.Save(@"D:\test.png", System.Drawing.Imaging.ImageFormat.Png);
        }
    }
}
