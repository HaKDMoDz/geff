using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace CellShine.Model
{
    public class Map
    {
        #region Propriétés
        public List<Cell> ListCell { get; set; }

        private int _width;
        private int _height;
        private int _countCell { get; set; }
        private int _distanceMax { get; set; }
        private TypeCellDistribution _typeCellDistribution { get; set; }

        private Random _rnd;
        #endregion

        public Map(int width, int height, int countCell, int distanceMax, TypeCellDistribution typeCellDistribution)
        {
            _width = width;
            _height = height;
            _countCell = countCell;
            _distanceMax = distanceMax;
            _typeCellDistribution = typeCellDistribution;

            //--- Initialisation du générateur aléatoire
            _rnd = new Random();
            //---

            CreateCells();

            //CalcNeighbourgh();
        }

        #region Méthodes publiques
        public void Draw(Graphics g)
        {
            Bitmap img = new Bitmap(_width, _height);
            Graphics gImg = Graphics.FromImage(img);
            
            float cellCost = 3f;

            gImg.Clear(Color.Black);

            foreach (Cell cell in ListCell)
            {
                //--- Liens entre noeuds voisins
                //foreach (Cell cell2 in cell.ListNeighbour)
                //{
                //    gImg.DrawLine(Pens.LightBlue, cell.Position, cell2.Position);
                //}

                Brush brush = new SolidBrush(Color.FromArgb((byte)(255f * (cell.Value)), Color.White));

                //---> Cercle de chaque noeud
                gImg.FillEllipse(brush, (float)cell.Position.X - cellCost * 0.5f, (float)cell.Position.Y - cellCost * 0.5f, cellCost, cellCost);
            }

            Pen penPath = new Pen(Brushes.Red, 5f);

            g.DrawImage(img, new Point(0, 0));
        }
        #endregion

        #region Méthodes privées
        private void CreateCells()
        {
            ListCell = new List<Cell>();

            if (_typeCellDistribution == TypeCellDistribution.Random)
            {
                for (int i = 0; i < _countCell; i++)
                {
                    Cell cell = new Cell(_rnd.Next(_width), _rnd.Next(_height));
                    ListCell.Add(cell);
                }
            }
            else if (_typeCellDistribution == TypeCellDistribution.Square)
            {
            }
            else if (_typeCellDistribution == TypeCellDistribution.Hexagon)
            {
            }
        }

        private void CalcNeighbourgh()
        {
            foreach (Cell cell1 in ListCell)
            {
                foreach (Cell cell2 in ListCell)
                {
                    if (cell1 != cell2 && !cell1.ListNeighbour.Contains(cell2))
                    {
                        if (Distance(cell1.Position, cell2.Position) < _distanceMax)
                        {
                            cell1.ListNeighbour.Add(cell2);
                            cell2.ListNeighbour.Add(cell1);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Distance entre deux points
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static int Distance(Point point1, Point point2)
        {
            int distance = 0;

            Point pointResultat = new Point(point2.X - point1.X, point2.Y - point1.Y);

            distance = (int)Math.Sqrt((double)(pointResultat.X * pointResultat.X + pointResultat.Y * pointResultat.Y));

            return distance;
        }

        #endregion

        public Cell GetNearestCell(Point position, int distanceMax)
        {
            Cell nearestCell = null;

            foreach (Cell cell in ListCell)
            {
                if (Distance(cell.Position, position) <= distanceMax)
                {
                    if (nearestCell == null ||
                        Distance(cell.Position, position) <Distance(nearestCell.Position, position))
                    {
                        nearestCell = cell;
                    }
                }
            }

            return nearestCell;
        }
    }
}
