using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using NewFlowar.Common;

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
        public int H { get; set; }

        public Map(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            this.R = 5;
            this.H = 3;
        }

        public void CreateGrid()
        {
            Cells = new List<Cell>();

            Random rnd = new Random();

            float d = (float)Math.Sqrt(0.75);

            for (int y = 1; y <= Height/2; y++)
            {
                for (int x = 1; x <= Width; x++)
                {
                    float fx = (float)x;
                    float fy = (float)y;


                    Cell cell2 = new Cell(this, x, y * 2 - 1,
                         (int)((2.5f + fx * 3f) * R),
                         (int)((fy) * (2f * d * R)));


                    Cells.Add(cell2);

                    cell2.Height = (float)(rnd.NextDouble() * R * H);

                    if (x == Height/2 && y == Height/2)
                        cell2.Height = 100;
                    if (x == Height / 2 && y+1 == Height / 2)
                        cell2.Height = 50;

                    //if (x == Width && y == 1)
                    //    cell2.Height = 50;
                }

                for (int x = 1; x <= Width; x++)
                {
                    float fx = (float)x;
                    float fy = (float)y;

                    Cell cell1 = new Cell(this, x, y * 2,
                        (int)((1f + fx * 3f) * R),
                        (int)((0.5f + fy) * (2f * d * R)));


                    Cells.Add(cell1);

                    cell1.Height = (float)(rnd.NextDouble() * R * H);
                }
            }

            CalcNeighborough();
            CalcPoints();
            CalcHeightPoint();
            CalcNormals();

            //DrawMapIntoImageFile();
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
                cell.Coord.Y + offsetY - 1 >= 0 && cell.Coord.Y + offsetY <= this.Height)
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
                                //st += "\r\nI" + cell.IndexPosition.ToString() + " P(" + i.ToString() + ") => N" + indexCell.ToString() + " I" + neighbourh.IndexPosition.ToString() + " P(" + index.ToString() + ")";
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
                                //st += "\r\nI" + cell.IndexPosition.ToString() + " P(" + i.ToString() + ") => N" + i.ToString() + " I" + neighbourh.IndexPosition.ToString() + " P(" + index.ToString() + ")";
                                neighbourh.Points[index] = indexPoint;
                            }
                        }
                    }
                }

                //st += "\r\n";
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

        public void CalcHeightPointNew(List<int> listIndexPoint)
        {
            for (int i = 0; i < listIndexPoint.Count; i++)
            {
                Vector3 vector3 = Points[listIndexPoint[i]];
                List<Cell> listCells = Cells.FindAll(cell => cell.Points.ContainsValue(listIndexPoint[i]));

                vector3.Z = 0;
                foreach (Cell cell in listCells)
                {
                    vector3.Z += (float)cell.Height;
                }

                vector3.Z /= listCells.Count;
                //TODO : l'égalité ci-dessous est peut être inutile
                Points[listIndexPoint[i]] = vector3;
            }
        }

        public void CalcHeightPoint(Cell cell)
        {
            foreach (int key in cell.Points.Keys)
            {
                Vector3 vector3 = Points[cell.Points[key]];
                List<Cell> listCells = Cells.FindAll(cell2 => cell2.Points.ContainsValue(cell.Points[key]));

                vector3.Z = 0;
                foreach (Cell cell2 in listCells)
                {
                    vector3.Z += (float)cell2.Height;
                }

                vector3.Z /= listCells.Count;
                //TODO : l'égalité ci-dessous est peut être inutile
                Points[cell.Points[key]] = vector3;
            }
        }

        private void CalcNormals()
        {
            Dictionary<int, int[,]> dic = new Dictionary<int, int[,]>();

            dic.Add(1, new int[2, 3] { { 3, 2, 1 }, { 6, 3, 1 } });
            dic.Add(2, new int[1, 3] { { 3, 2, 1 } });
            dic.Add(3, new int[3, 3] { { 3, 2, 1 }, { 6, 4, 3 }, { 6, 3, 1 } });
            dic.Add(4, new int[2, 3] { { 6, 4, 3 }, { 6, 5, 4 } });
            dic.Add(5, new int[1, 3] { { 6, 5, 4 } });
            dic.Add(6, new int[3, 3] { { 6, 4, 3 }, { 6, 5, 4 }, { 6, 3, 1 } });

            Normals = new List<Vector3>();

            for (int i = 0; i < Points.Count; i++)
            {
                int nb = 0;
                Vector3 normal = Vector3.Zero;

                List<Cell> listCell = Cells.FindAll(cell => cell.Points.ContainsValue(i));

                foreach (Cell cell in listCell)
                {
                    foreach (int key in cell.Points.Keys)
                    {
                        if (cell.Points[key] == i)
                        {
                            for (int j = 0; j < dic[key].GetUpperBound(0); j++)
                            {
                                Vector3 vec1 = Points[cell.Points[dic[key][j, 0]]] - Points[cell.Points[dic[key][j, 1]]];
                                Vector3 vec2 = Points[cell.Points[dic[key][j, 2]]] - Points[cell.Points[dic[key][j, 1]]];
                                normal += Vector3.Cross(vec1, vec2);
                                nb++;
                            }
                        }
                    }
                }

                normal /= nb;
                normal.Normalize();
                Normals.Add(normal);
            }

            foreach (Cell cell in Cells)
            {
                cell.Normal = Tools.NormalCell(this, cell);
            }
        }

        private void CalcNormals(List<int> listIndexPoint)
        {
            Dictionary<int, int[,]> dic = new Dictionary<int, int[,]>();

            dic.Add(1, new int[2, 3] { { 3, 2, 1 }, { 6, 3, 1 } });
            dic.Add(2, new int[1, 3] { { 3, 2, 1 } });
            dic.Add(3, new int[3, 3] { { 3, 2, 1 }, { 6, 4, 3 }, { 6, 3, 1 } });
            dic.Add(4, new int[2, 3] { { 6, 4, 3 }, { 6, 5, 4 } });
            dic.Add(5, new int[1, 3] { { 6, 5, 4 } });
            dic.Add(6, new int[3, 3] { { 6, 4, 3 }, { 6, 5, 4 }, { 6, 3, 1 } });

            //Normals = new List<Vector3>();

            for (int i = 0; i < listIndexPoint.Count; i++)
            {
                int index = listIndexPoint[i];
                int nb = 0;
                Vector3 normal = Vector3.Zero;

                List<Cell> listCell = Cells.FindAll(cell => cell.Points.ContainsValue(index));

                foreach (Cell cell in listCell)
                {
                    foreach (int key in cell.Points.Keys)
                    {
                        if (cell.Points[key] == index)
                        {
                            for (int j = 0; j < dic[key].GetUpperBound(0); j++)
                            {
                                Vector3 vec1 = Points[cell.Points[dic[key][j, 0]]] - Points[cell.Points[dic[key][j, 1]]];
                                Vector3 vec2 = Points[cell.Points[dic[key][j, 2]]] - Points[cell.Points[dic[key][j, 1]]];
                                normal += Vector3.Cross(vec1, vec2);
                                nb++;
                            }
                        }
                    }
                }

                normal /= nb;
                normal.Normalize();
                Normals[index] = normal;
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

        public void ElevateCell(Cell cell, float radius)
        {
            //System.Diagnostics.Stopwatch f = new System.Diagnostics.Stopwatch();

            //f.Start();

            List<Cell> cellsToCalcul = new List<Cell>();
            cellsToCalcul.Add(cell);

            List<int> listIndexPoint = new List<int>();


            foreach (Cell cell2 in Cells)
            {
                float distance = Tools.Distance(cell.Location, cell2.Location);
                if (distance < radius)
                {
                    double value = Math.Abs(Tools.GetBellCurvePoint(1 - distance / radius, 0.25));
                    cellsToCalcul.Add(cell2);
                    cell2.Height = (float)value * cell.Height;

                    for (int i = 1; i <= 6; i++)
                    {
                        int index = cell2.Points[i];
                        if (!listIndexPoint.Contains(index))
                            listIndexPoint.Add(index);
                    }
                }
            }

            //f.Stop();
            //TimeSpan elapsed1 = f.Elapsed;

            //f.Restart();
            
            /*
            foreach (Cell cell2 in cellsToCalcul)
            {
                //float distance = Tools.Distance(cell.Location, cell2.Location);
                //if (distance < radius)
                {
                    CalcHeightPoint(cell2);
                }
            }
            */

            CalcHeightPointNew(listIndexPoint);

            //f.Stop();

            //TimeSpan elasped2 = f.Elapsed;
            
            //f.Restart();

            CalcNormals(listIndexPoint);

            //f.Stop();

            //TimeSpan elasped3 = f.Elapsed;
        }

        private byte[,] _matrix = null;
        public byte[,] Matrix 
        {
            get
            {
                if (_matrix == null)
                {
                    _matrix = new byte[this.Width, this.Height*2];

                    for (int y = 1; y <= Height; y++)
                    {
                        for (int x = 1; x <= Width; x++)
                        {
                            _matrix[x-1, y-1] = (byte)Cells[(x-1)+(y-1)*Width].Height;
                        }
                    }
                }

                return _matrix;
            }
        }
    }
}
