using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using ThreadAStar.Model;

namespace ThreadAStar.AStar
{
    public class AStarMap : IComputable
    {
        #region Propriétés
        public List<Cell> ListCell { get; set; }
        public List<Cell> ListCellPath { get; set; }

        private int _width;
        private int _height;
        private int _seed = int.MinValue;
        private int _countCell { get; set; }
        private int _distanceMax { get; set; }

        private Cell _cellStart = null;
        private Cell _cellEnd = null;
        private List<AStarCell> _listCellOpen = new List<AStarCell>();
        private List<AStarCell> _listCellClose = new List<AStarCell>();
        private Random _rnd;
        #endregion

        public AStarMap(int width, int height, int seed, int countCell, int distanceMax)
        {
            _width = width;
            _height = height;
            _seed = seed;
            _countCell = countCell;
            _distanceMax = distanceMax;

            //--- Initialisation du générateur aléatoire
            _rnd = new Random(_seed);
            //---

            CreateCells();

            CalcNeighbourgh();

            //--- Détermine aléatoirement la case de départ et la case d'arrivée
            _cellStart = ListCell[_rnd.Next(ListCell.Count)];
            _cellEnd = ListCell[_rnd.Next(ListCell.Count)];
            //---
        }

        #region Méthodes publiques
        public void Init()
        {
            _listCellOpen = new List<AStarCell>();
            _listCellClose = new List<AStarCell>();
            ListCellPath = new List<Cell>();
        }

        public void Compute()
        {
            //--- Démarre le calcul
            ListCellPath = CalcPath(false, 1f);
            //---
        }

        public void Draw(Graphics g)
        {
            Bitmap img = new Bitmap(_width, _height);
            Graphics gImg = Graphics.FromImage(img);

            gImg.Clear(Color.Black);

            float cellCost = 1f;

            foreach (Cell cell in ListCell)
            {
                //--- Liens entre noeuds voisins
                foreach (Cell cell2 in cell.ListNeighbour)
                {
                    gImg.DrawLine(Pens.LightBlue, cell.Position, cell2.Position);
                }

                //---> Cercle de chaque noeud
                gImg.FillEllipse(Brushes.LightGray, cell.Position.X - cellCost * 7, cell.Position.Y - cellCost * 7, cellCost * 15, cellCost * 15);
            }

            //---> Cercle de la cellule de départ
            gImg.FillEllipse(Brushes.YellowGreen, _cellStart.Position.X - cellCost * 7, _cellStart.Position.Y - cellCost * 7, cellCost * 14, cellCost * 14);

            //---> Cercle de la cellule d'arrivée
            gImg.FillEllipse(Brushes.Orange, _cellEnd.Position.X - cellCost * 7, _cellEnd.Position.Y - cellCost * 7, cellCost * 14, cellCost * 14);

            Pen penPath = new Pen(Brushes.Red, 5f);

            for (int i = 0; i < ListCellPath.Count - 1; i++)
            {
                gImg.DrawLine(penPath, ListCellPath[i].Position, ListCellPath[i + 1].Position);
            }

            g.DrawImage(img, new Point(0, 0));
        }
        #endregion

        #region Méthodes privées
        private void CreateCells()
        {
            ListCell = new List<Cell>();
            _listCellClose = new List<AStarCell>();
            _listCellOpen = new List<AStarCell>();

            for (int i = 0; i < _countCell; i++)
            {
                Cell cell = new Cell(_rnd.Next(_width), _rnd.Next(_height), (float)_rnd.NextDouble());
                ListCell.Add(cell);
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

        private List<Cell> CalcPath(bool useCost, float costMax)
        {
            List<Cell> listCellPath = new List<Cell>();

            //--- Ajout de la case de départ
            AStarCell aStarCell = new AStarCell();
            aStarCell.Cell = _cellStart;
            //---

            //---
            _listCellClose.Add(aStarCell);

            bool pathFound = CalcPathFromCell(aStarCell, useCost, costMax);

            if (!pathFound)
                return listCellPath;
            //---

            //--- Reconstitue le chemin
            bool pathCompleted = false;
            Cell cell = _cellEnd;

            while (!pathCompleted)
            {
                if (_listCellClose.Count == 0)
                    pathCompleted = true;

                foreach (AStarCell aStarCellChild in _listCellClose)
                {
                    if (aStarCellChild.Cell == cell)
                    {
                        listCellPath.Add(aStarCellChild.Cell);

                        cell = aStarCellChild.ParentCell;

                        if (aStarCellChild.Cell == _cellStart)
                        {
                            pathCompleted = true;
                        }
                    }
                }
            }
            //---

            return listCellPath;
        }

        private bool CalcPathFromCell(AStarCell cellParent, bool useCost, float costMax)
        {
            foreach (Cell cell in cellParent.Cell.ListNeighbour)
            {
                //---> La cellule n'est pas dans la liste fermée
                if (!_listCellClose.Exists(c => c.Cell == cell))
                {
                    AStarCell aStarCellPrev = _listCellOpen.Find(c => c.Cell == cell);

                    AStarCell aStarCell = new AStarCell();
                    aStarCell.Cell = cell;
                    aStarCell.ParentCell = cellParent.Cell;
                    aStarCell.G = cellParent.G + Distance(cell.Position, cellParent.Cell.Position);
                    aStarCell.H = Distance(cell.Position, _cellEnd.Position);
                    aStarCell.F = aStarCell.G + aStarCell.H;

                    if (useCost && cell.Cost < costMax)
                    {
                        aStarCell.F = (int)((float)aStarCell.F * cell.Cost);
                    }

                    if (aStarCellPrev != null && aStarCell.G < aStarCellPrev.G)
                    {
                        aStarCellPrev.ParentCell = cellParent.Cell;
                        aStarCellPrev.G = aStarCell.G;
                        aStarCellPrev.H = aStarCell.H;
                        aStarCellPrev.F = aStarCell.F;
                    }
                    else if (aStarCellPrev == null)
                    {
                        _listCellOpen.Add(aStarCell);
                    }
                }
            }

            AStarCell aStarChoosenCell = null;

            foreach (AStarCell aStarCell in _listCellOpen)
            {
                if (aStarChoosenCell == null)
                    aStarChoosenCell = aStarCell;

                if (aStarCell.F < aStarChoosenCell.F)
                    aStarChoosenCell = aStarCell;
            }

            //---
            _listCellOpen.Remove(aStarChoosenCell);
            _listCellClose.Add(aStarChoosenCell);
            //---

            //---
            if (aStarChoosenCell == null)
                return false;
            else if (aStarChoosenCell.Cell == _cellEnd)
                return true;
            else
                return CalcPathFromCell(aStarChoosenCell, useCost, costMax);
            //---
        }

        /// <summary>
        /// Distance entre deux points
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        private int Distance(Point point1, Point point2)
        {
            int distance = 0;

            Point pointResultat = new Point(point2.X - point1.X, point2.Y - point1.Y);

            distance = (int)Math.Sqrt((double)(pointResultat.X * pointResultat.X + pointResultat.Y * pointResultat.Y));

            return distance;
        }

        #endregion
    }
}
