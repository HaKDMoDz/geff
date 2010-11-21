using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewFlowar.Model;
using NewFlowar.Common;

namespace NewFlowar.Logic.AI
{
    public static class PathFinding
    {
        public static List<int> CalcPath(Cell cellStart, Cell cellEnd, bool useCost, float costMax)
        {
            List<int> listCellPath = new List<int>();
            List<AStarCell> listCellClose = new List<AStarCell>();
            List<AStarCell> listCellOpen = new List<AStarCell>();

            //--- Ajout de la case de départ
            AStarCell aStarCell = new AStarCell();
            aStarCell.Cell = cellStart;
            //---

            //---
            listCellClose.Add(aStarCell);

            bool pathFound = CalcPathFromCell(aStarCell, cellEnd, listCellClose, listCellOpen, useCost, costMax);

            if (!pathFound)
                return listCellPath;
            //---

            //--- Reconstitue le chemin
            bool pathCompleted = false;
            Cell cell = cellEnd;

            while (!pathCompleted)
            {
                if (listCellClose.Count == 0)
                    pathCompleted = true;

                foreach (AStarCell aStarCellChild in listCellClose)
                {
                    if (aStarCellChild.Cell == cell)
                    {
                        listCellPath.Add(aStarCellChild.Cell.IndexPosition);

                        cell = aStarCellChild.ParentCell;

                        if (aStarCellChild.Cell == cellStart)
                        {
                            pathCompleted = true;
                        }
                    }
                }
            }
            //---

            listCellPath.Reverse();

            return listCellPath;
        }

        private static bool CalcPathFromCell(AStarCell cellParent, Cell cellEnd, List<AStarCell> listCellClose, List<AStarCell> listCellOpen, bool useCost, float costMax)
        {
            foreach (Cell cell in cellParent.Cell.Neighbourghs.Values)
            {
                //---> La cellule n'est pas dans la liste fermée
                if (cell != null && !listCellClose.Exists(c => c.Cell == cell))
                {
                    AStarCell aStarCellPrev = listCellOpen.Find(c => c.Cell == cell);

                    AStarCell aStarCell = new AStarCell();
                    aStarCell.Cell = cell;
                    aStarCell.ParentCell = cellParent.Cell;
                    aStarCell.G = cellParent.G + Tools.Distance(cell.Location, cellParent.Cell.Location);
                    aStarCell.H = Tools.Distance(cell.Location, cellEnd.Location);
                    aStarCell.F = aStarCell.G + aStarCell.H;

                    if (useCost && cell.Height < costMax)
                    {
                        aStarCell.F = aStarCell.F * cell.Height;
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
                        listCellOpen.Add(aStarCell);
                    }
                }
            }

            AStarCell aStarChoosenCell = null;

            foreach (AStarCell aStarCell in listCellOpen)
            {
                if (aStarChoosenCell == null)
                    aStarChoosenCell = aStarCell;

                if (aStarCell.F < aStarChoosenCell.F)
                    aStarChoosenCell = aStarCell;
            }

            //---
            listCellOpen.Remove(aStarChoosenCell);
            listCellClose.Add(aStarChoosenCell);
            //---

            //---
            if (aStarChoosenCell == null)
                return false;
            else if (aStarChoosenCell.Cell == cellEnd)
                return true;
            else
                return CalcPathFromCell(aStarChoosenCell, cellEnd, listCellClose, listCellOpen, useCost, costMax);
            //---
        }
    }
}
