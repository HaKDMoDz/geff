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
        public int R { get; set; }

        public Map(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            this.R = 2;
        }

        public void CreateGrid()
        {
            Cells = new List<Cell>();

            Random rnd = new Random();

            float d = (float)Math.Sqrt(0.75);

            for (int y = 1; y <= Height; y++)
            {
                for (int x = 1; x <= Width; x++)
                {
                    float fx = (float)x;
                    float fy = (float)y;


                    Cell cell2 = new Cell(this, x, y * 2 - 1,
                         (int)((2.5f + fx * 3f) * R),
                         (int)((fy) * (2f * d * R)));


                    Cells.Add(cell2);

                    cell2.Height = (float)(rnd.NextDouble() * 20);
                }

                for (int x = 1; x <= Width; x++)
                {
                    float fx = (float)x;
                    float fy = (float)y;

                    Cell cell1 = new Cell(this, x, y * 2,
                        (int)((1f + fx * 3f) * R),
                        (int)((0.5f + fy) * (2f * d * R)));


                    Cells.Add(cell1);

                    cell1.Height = (float)(rnd.NextDouble() * 20);
                }
            }

            CalcNeighborough();
            CalcPoints();
        }

        public void CalcNeighborough()
        {
            foreach (Cell cell in Cells)
            {
                cell.Neighbourghs[1] = GetNeighborough(cell, 0, -1);
                cell.Neighbourghs[2] = GetNeighborough(cell, 1, 0);
                cell.Neighbourghs[3] = GetNeighborough(cell, 0, 1);
                cell.Neighbourghs[4] = GetNeighborough(cell, -1, 1);
                cell.Neighbourghs[5] = GetNeighborough(cell, -1, 0);
                cell.Neighbourghs[6] = GetNeighborough(cell, -1, -1);
            }
        }

        private Cell GetNeighborough(Cell cell, int offsetX, int offsetY)
        {
            if (cell.Coord.X + offsetX > 0 && cell.Coord.X + offsetX <= this.Width &&
                cell.Coord.Y + offsetY > 0 && cell.Coord.Y + offsetY <= this.Height)
            {
                return Cells[(cell.Coord.Y + offsetY - 1) * this.Width + cell.Coord.X + offsetX-1];
            }

            return null;
        }

        public void CalcPoints()
        {
            string st = "";
            Cell neighbourh = null;

            foreach (Cell cell in Cells)
            {
                for (int i = 1; i <= 6; i++)
                {
                    if (cell.Points[i] == Point.Zero)
                    {
                        Point point = new Point(
                            cell.Location.X + (int)(Math.Cos(Math.PI / 3 * (double)i) * (double)R),
                            cell.Location.Y + (int)(Math.Sin(Math.PI / 3 * (double)i) * (double)R));

                        cell.Points[i] = point;
                        
                        int indexCell = (i - 1) % 6;
                        if (indexCell == 0)
                            indexCell = 6;

                        neighbourh = cell.Neighbourghs[indexCell];
                        if (neighbourh != null)
                        {
                            int index = (i + 2) % 6;
                            if (index == 0)
                                index = 6;

                            st += "\r\nN" + cell.IndexPosition.ToString() + " P(" + i.ToString() + ") => N" + indexCell.ToString() + " P(" + index.ToString() + ")";
                            neighbourh.Points[index] = point;
                        }

                        neighbourh = cell.Neighbourghs[i];
                        if (neighbourh != null)
                        {
                            int index = (i + 4) % 6;
                            if (index == 0)
                                index = 6;

                            st += "\r\nN" + cell.IndexPosition.ToString() + " P(" + i.ToString() + ") => N" + i.ToString() + " P(" + index.ToString() + ")";
                            neighbourh.Points[index] = point;
                        }
                    }
                }

                st += "\r\n";
            }
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
