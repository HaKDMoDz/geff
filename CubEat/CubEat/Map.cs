using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace CubEat
{
    public class Map
    {
        public Cell[,] Cells { get; set; }
        public int Size { get; set; }
        public LibraySample CurrentLibrarySample { get; set; }
        public Dictionary<int, int> PlayedSample { get; set; }

        public Dictionary<int, List<Cell>> LayerCells { get; set; }

        public Map(int size, LibraySample currentLibrarySample)
        {
            this.Size = size;
            this.Cells = new Cell[size, size];
            this.LayerCells = new Dictionary<int,List<Cell>>();
            this.CurrentLibrarySample = currentLibrarySample;
            this.DrainPlayedSample();
        }

        public void CreateMapWithSamples()
        {
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    Cell cell = new Cell();
                    cell.IsEmpty = true;
                    Cells[x, y] = cell;

                    if (!(x == Size / 2 && y == Size / 2) && Repository.rnd.NextDouble() > 0.809016994)
                    {
                        int center = (Size / 2);
                        int distance = (int)Math.Sqrt((x - center) * (x - center) + (y - center) * (y - center));

                        if (CurrentLibrarySample.ListSampleModel.Count - 1 < distance)
                        {
                            distance = CurrentLibrarySample.ListSampleModel.Count - 1;
                        }

                        cell.TypeSymbol = (TypeSymbol)Repository.rnd.Next(1, 4);
                        cell.Sample = new Sample(CurrentLibrarySample.ListSampleModel[distance - 1]);
                        cell.IsEmpty = false;
                    }

                    CalculCellLayerAndNumber(cell, x, y);
                }
            }
        }

        private void CalculCellLayerAndNumber(Cell cell, int x, int y)
        {
            Point center = new Point(Size / 2, Size / 2);
            Point point = new Point(x, y);
            Point delta = new Point(point.X - center.X, point.Y - center.Y);

            for (int layer = 1; layer <= Size / 2; layer++)
            {
                if (delta.Y == -layer && delta.X >= -layer && delta.X <= layer)
                {
                    cell.Layer = layer;
                    cell.NumberOnLayer = delta.X+layer;
                }
                else if (delta.Y == layer && delta.X >= -layer && delta.X <= layer)
                {
                    cell.Layer = layer;
                    cell.NumberOnLayer = layer*5 - delta.X;
                }
                else if (delta.X == layer && delta.Y >= -layer && delta.Y <= layer)
                {
                    cell.Layer = layer;
                    cell.NumberOnLayer = layer * 3 + delta.Y;
                }
                else if (delta.X == -layer && delta.Y >= -layer && delta.Y <= layer)
                {
                    cell.Layer = layer;
                    cell.NumberOnLayer = layer * 7 - delta.Y;
                }
            }

            //--- Ajout de la cellule à la représentation par couche
            if(!this.LayerCells.ContainsKey(cell.Layer))
                this.LayerCells.Add(cell.Layer, new List<Cell>());

            this.LayerCells[cell.Layer].Add(cell);
            //---

            cell.IsOnMeasure = cell.NumberOnLayer % 4 == 0;
        }

        public void DrainPlayedSample()
        {
            PlayedSample = new Dictionary<int, int>();
            for (int i = 0; i < CurrentLibrarySample.ListSampleModel.Count; i++)
            {
                PlayedSample.Add(CurrentLibrarySample.ListSampleModel[i].SampleModelId, 0);
            }
        }
    }
}
