using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TheGrid.Common;
using System.Threading.Tasks;

namespace TheGrid.Model
{
    [Serializable]
    public class Map
    {
        public List<Cell> Cells { get; set; }
        public List<Channel> Channels { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public Map() { }

        public Map(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public void CreateGrid()
        {
            Cells = new List<Cell>();

            Random rnd = new Random();

            float width = 0.5f;
            float height = 0.5f * (float)Math.Sin(MathHelper.Pi / 3);

            for (int y = 1; y <= Height; y++)
            {
                for (int x = 1; x <= Math.Round((double)Width / 2, MidpointRounding.AwayFromZero); x++)
                {
                    float fx = (float)x;
                    float fy = (float)y;

                    fx = fx * width * 3f;
                    fy = fy * height * 2f;

                    Cell cell2 = new Cell(this, x, y * 2,
                         fx,
                         fy);

                    Cells.Add(cell2);
                }

                for (int x = 1; x <= Width / 2; x++)
                {
                    float fx = (float)x;
                    float fy = (float)y;

                    fx = fx * width * 3f + width * 1.5f;
                    fy = fy * height * 2f - height;

                    Cell cell1 = new Cell(this, x, (y * 2) - 1,
                         fx,
                         fy);

                    Cells.Add(cell1);
                }
            }

            CalcNeighborough();
        }

        public void CalcNeighborough()
        {
            foreach (Cell cell in Cells)
            {
                cell.Neighbourghs = new Cell[6];

                if (cell.Coord.Y % 2 == 1)
                {
                    cell.Neighbourghs[0] = GetNeighborough(cell, 0, -2);
                    cell.Neighbourghs[1] = GetNeighborough(cell, 1, -1);
                    cell.Neighbourghs[2] = GetNeighborough(cell, 1, 1);
                    cell.Neighbourghs[3] = GetNeighborough(cell, 0, 2);
                    cell.Neighbourghs[4] = GetNeighborough(cell, 0, 1);
                    cell.Neighbourghs[5] = GetNeighborough(cell, 0, -1);
                }
                else
                {
                    cell.Neighbourghs[0] = GetNeighborough(cell, 0, -2);
                    cell.Neighbourghs[1] = GetNeighborough(cell, 0, -1);
                    cell.Neighbourghs[2] = GetNeighborough(cell, 0, 1);
                    cell.Neighbourghs[3] = GetNeighborough(cell, 0, 2);
                    cell.Neighbourghs[4] = GetNeighborough(cell, -1, 1);
                    cell.Neighbourghs[5] = GetNeighborough(cell, -1, -1);
                }
            }
        }

        private Cell GetNeighborough(Cell cell, int offsetX, int offsetY)
        {
            return Cells.Find(c => c.Coord.X == cell.Coord.X + offsetX && c.Coord.Y == cell.Coord.Y + offsetY);
        }
    }
}
