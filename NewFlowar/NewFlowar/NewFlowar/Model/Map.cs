using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NewFlowar.Model
{
    public class Map
    {
        public List<Cell> Cells { get; set; }
        public List<Vector3> Points { get; set; }
        public List<Vector3> Normals { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
        public int R { get; set; }

        public Map(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            this.R = 5;
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

                    cell2.Height = (float)(rnd.NextDouble() * R * 4);

                    if (x == 1 && y == 1)
                        cell2.Height = 100;

                    if (x == Width && y == 1)
                        cell2.Height = 50;
                }

                for (int x = 1; x <= Width; x++)
                {
                    float fx = (float)x;
                    float fy = (float)y;

                    Cell cell1 = new Cell(this, x, y * 2,
                        (int)((1f + fx * 3f) * R),
                        (int)((0.5f + fy) * (2f * d * R)));


                    Cells.Add(cell1);

                    cell1.Height = (float)(rnd.NextDouble() * R * 4);
                }
            }

            CalcNeighborough();
            CalcPoints();
            CalcHeightPoint();
            CalcNormals();

            DrawMapIntoImageFile();
        }

        public void CalcNeighborough()
        {
            foreach (Cell cell in Cells)
            {
                if (cell.Coord.Y % 2 == 1)
                {
                    cell.Neighbourghs[1] = GetNeighborough(cell, 1, -1);
                    cell.Neighbourghs[2] = GetNeighborough(cell, 1, 1);
                    cell.Neighbourghs[3] = GetNeighborough(cell, 0, 2);
                    cell.Neighbourghs[4] = GetNeighborough(cell, 0, 1);
                    cell.Neighbourghs[5] = GetNeighborough(cell, 0, -1);
                    cell.Neighbourghs[6] = GetNeighborough(cell, 0, -2);
                }
                else
                {
                    cell.Neighbourghs[1] = GetNeighborough(cell, 0, -1);
                    cell.Neighbourghs[2] = GetNeighborough(cell, 0, 1);
                    cell.Neighbourghs[3] = GetNeighborough(cell, 0, 2);
                    cell.Neighbourghs[4] = GetNeighborough(cell, -1, 1);
                    cell.Neighbourghs[5] = GetNeighborough(cell, -1, -1);
                    cell.Neighbourghs[6] = GetNeighborough(cell, 0, -2);
                }
            }
        }

        private Cell GetNeighborough(Cell cell, int offsetX, int offsetY)
        {
            if (cell.Coord.X + offsetX - 1 >= 0 && cell.Coord.X + offsetX <= this.Width &&
                cell.Coord.Y + offsetY - 1 >= 0 && cell.Coord.Y + offsetY <= this.Height * 2)
            {
                return Cells[(cell.Coord.Y + offsetY - 1) * this.Width + cell.Coord.X + offsetX - 1];
            }

            return null;
        }

        public void CalcPoints()
        {
            string st = "";
            Cell neighbourh = null;
            Points = new List<Vector3>();

            foreach (Cell cell in Cells)
            {
                for (int i = 1; i <= 6; i++)
                {
                    if (cell.Points[i] == -1)
                    {
                        Vector3 point = new Vector3(
                            (float)cell.Location.X + (float)(Math.Cos(Math.PI / (double)3 * (double)(i - 2)) * (double)R),
                            (float)cell.Location.Y + (int)(Math.Sin(Math.PI / (double)3 * (double)(i - 2)) * (double)R), 0);

                        Points.Add(point);
                        int indexPoint = Points.Count - 1;

                        cell.Points[i] = indexPoint;

                        int indexCell = (i - 1) % 6;
                        if (indexCell == 0)
                            indexCell = 6;

                        neighbourh = cell.Neighbourghs[indexCell];
                        if (neighbourh != null)
                        {
                            int index = (i + 2) % 6;
                            if (index == 0)
                                index = 6;

                            if (neighbourh.Points[index] == -1)
                            {
                                st += "\r\nI" + cell.IndexPosition.ToString() + " P(" + i.ToString() + ") => N" + indexCell.ToString() + " I" + neighbourh.IndexPosition.ToString() + " P(" + index.ToString() + ")";
                                neighbourh.Points[index] = indexPoint;
                            }
                        }

                        neighbourh = cell.Neighbourghs[i];
                        if (neighbourh != null)
                        {
                            int index = (i + 4) % 6;
                            if (index == 0)
                                index = 6;

                            if (neighbourh.Points[index] == -1)
                            {
                                st += "\r\nI" + cell.IndexPosition.ToString() + " P(" + i.ToString() + ") => N" + i.ToString() + " I" + neighbourh.IndexPosition.ToString() + " P(" + index.ToString() + ")";
                                neighbourh.Points[index] = indexPoint;
                            }
                        }
                    }
                }

                st += "\r\n";
            }
        }

        public void CalcHeightPoint()
        {
            for (int i = 0; i < Points.Count; i++)
            {
                Vector3 vector3 = Points[i];
                List<Cell> listCells = Cells.FindAll(cell => cell.Points.ContainsValue(i));

                vector3.Z = 0;
                foreach (Cell cell in listCells)
                {
                    vector3.Z += (float)cell.Height;
                }

                vector3.Z /= listCells.Count;
                //TODO : l'égalité ci-dessous est peut être inutile
                Points[i] = vector3;
            }
        }

        public void CalcNormals()
        {
            Normals = new List<Vector3>();

            for (int i = 0; i < Points.Count; i++)
            {
                if(i%3==0)
                    Normals.Add(Vector3.Up);
                else if(i%3==1)
                    Normals.Add(Vector3.Left);
                else if(i%3 ==2)
                    Normals.Add(Vector3.Right);
            }
        }

        public void DrawMapIntoImageFile()
        {
            System.Drawing.Image img = new System.Drawing.Bitmap(600, 600);

            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(img);
            //Bitmap b = new Bitmap(600, 600, g);

            g.Clear(System.Drawing.Color.White);

            int index = 0;
            int d = 10;

            foreach (Cell cell in Cells)
            {
                //g.DrawRectangle(System.Drawing.Pens.Black, cell.Location.X * d, cell.Location.Y * d, 7 * d, d*2);
                //g.DrawString(cell.Coord.ToString() + " " + index.ToString(), new System.Drawing.Font("Arial", 10f), System.Drawing.Brushes.Black, new System.Drawing.PointF(cell.Location.X *d, cell.Location.Y * d));
                index++;

                //Vector3 prevPoint = Vector3.Zero;
                //for (int i = 1; i <= 6; i++)
                //{
                //    if (prevPoint != Vector3.Zero)
                //        g.DrawLine(System.Drawing.Pens.Red, prevPoint.X * d, prevPoint.Y * d, cell.Points[i].X * d, cell.Points[i].Y * d);

                //    prevPoint = cell.Points[i];
                //}

                //g.DrawLine(System.Drawing.Pens.Red, prevPoint.X * d, prevPoint.Y * d, cell.Points[1].X * d, cell.Points[1].Y * d);

                //for (int i = 1; i <= 6; i++)
                //{
                //    //g.DrawRectangle(System.Drawing.Pens.Blue, cell.Points[i].X * d, cell.Points[i].Y * d, 5, 5);

                //    g.DrawString(i.ToString(), new System.Drawing.Font("Arial", 10f), System.Drawing.Brushes.Black, cell.Points[i].X * d, cell.Points[i].Y * d);

                //}
            }

            img.Save(@"D:\test.png", System.Drawing.Imaging.ImageFormat.Png);
        }
    }
}
