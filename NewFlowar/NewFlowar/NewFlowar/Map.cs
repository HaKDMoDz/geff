using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NewFlowar
{
    public class Map
    {
        public List<Cell> Cells { get; set; }
        public List<Point> Points { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Map(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public void CreateGrid()
        {
            Cells = new List<Cell>();

            Random rnd = new Random();

            float d = (float)Math.Sqrt(0.75);
            float r = 2;

            for (int y = 1; y <= Height; y++)
            {
                for (int x = 1; x <= Width; x++)
                {
                    float fx = (float)x;
                    float fy = (float)y;


                    Cell cell2 = new Cell(this, x, y * 2 - 1,
                         (int)((2.5f + fx * 3f) * r),
                         (int)((fy) * (2f * d * r)));


                    Cells.Add(cell2);

                    cell2.Height = (float)(rnd.NextDouble() * 20);
                }

                for (int x = 1; x <= Width; x++)
                {
                    float fx = (float)x;
                    float fy = (float)y;

                    Cell cell1 = new Cell(this, x, y * 2,
                        (int)((1f + fx * 3f) * r),
                        (int)((0.5f + fy) * (2f * d * r)));


                    Cells.Add(cell1);

                    cell1.Height = (float)(rnd.NextDouble() * 20);
                }
            }

            CalcNeighborough();
        }

        public void CalcNeighborough()
        {
            foreach (Cell cell in Cells)
            {
                cell.Neighbourghs[1] = GetNeighborough(cell, 0, -1);
                cell.Neighbourghs[2] = GetNeighborough(cell, 1, 0);
                cell.Neighbourghs[3] = GetNeighborough(cell, 1, 1);
                cell.Neighbourghs[4] = GetNeighborough(cell, -1, 1);
                cell.Neighbourghs[5] = GetNeighborough(cell, -1, 0);
                cell.Neighbourghs[6] = GetNeighborough(cell, -1, -1);
            }
        }

        private Cell GetNeighborough(Cell cell, int offsetX, int offsetY)
        {
            if (cell.Coord.X + offsetX > 0 && cell.Coord.X + offsetX < this.Width &&
                cell.Coord.Y + offsetY > 0 && cell.Coord.Y + offsetY < this.Height)
            {
                return Cells[(cell.Coord.Y + offsetY - 1) * this.Width + cell.Coord.X + offsetX];
            }

            return null;
        }

        public void DrawMapIntoImageFile()
        {
            System.Drawing.Image img = new System.Drawing.Bitmap(600, 600);

            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(img);
            //Bitmap b = new Bitmap(600, 600, g);

            g.Clear(System.Drawing.Color.White);

            int index = 0;
            foreach (Cell cell in Cells)
            {
                g.DrawRectangle(System.Drawing.Pens.Black, cell.Location.X * 20, cell.Location.Y * 20, 70, 20);
                g.DrawString(cell.Coord.ToString() + " " + index.ToString(), new System.Drawing.Font("Arial", 10f), System.Drawing.Brushes.Black, new System.Drawing.PointF(cell.Location.X * 20, cell.Location.Y * 20));
                index++;
            }

            img.Save(@"D:\test.png", System.Drawing.Imaging.ImageFormat.Png);
        }
    }
}
