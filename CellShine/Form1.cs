using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CellShine.Model;
using Microsoft.Xna.Framework;

namespace CellShine
{
    public partial class Form1 : Form
    {
        Map map;
        Graphics g;
        Cell selectedCell;
        Cell prevSelectedCell;
        Curve curveDistance;
        Curve curveTime;
        float timeMax = 0f;

        public Form1()
        {
            InitializeComponent();

            Init();
        }

        private void Init()
        {
            timer.Interval = 10;

            map = new Map(pic.Width, pic.Height, 1000, 20, TypeCellDistribution.Hexagon);

            g = pic.CreateGraphics();

            //---
            float distanceMax = 100;


            curveDistance = new Curve();
            //---> Donut
            //curveDistance.Keys.Add(new CurveKey(0 * distanceMax, 0.25f));
            //curveDistance.Keys.Add(new CurveKey(0.5f * distanceMax, 0.25f));
            //curveDistance.Keys.Add(new CurveKey(0.75f * distanceMax, 0.7f));
            //curveDistance.Keys.Add(new CurveKey(1f * distanceMax, 0f));

            curveDistance.Keys.Add(new CurveKey(0 * distanceMax, 1f));
            curveDistance.Keys.Add(new CurveKey(1f * distanceMax, 0f));
            //---

            //---
            timeMax = 500f;

            curveTime = new Curve();
            curveTime.Keys.Add(new CurveKey(0 * timeMax, 0.05f));
            curveTime.Keys.Add(new CurveKey(0.7f * timeMax, 1f));
            curveTime.Keys.Add(new CurveKey(1f * timeMax, 0f));
            //---
        }

        private void PropagateWave()
        {
            DateTime startTime = DateTime.Now;

            foreach (Cell cell in map.ListCell)
            {
                float distance = (float)Map.Distance(cell.FixedPosition, selectedCell.FixedPosition);

                float valueDistance = curveDistance.Evaluate(distance);
                float valueTime = curveTime.Evaluate(0f);
                float value = valueDistance * valueTime;

                if (cell.StartTime == DateTime.MinValue || value > cell.Value)
                {
                    cell.Coeff = valueDistance;
                    cell.StartTime = startTime;

                    if (cell != selectedCell)
                    {
                        cell.Vector = new PointF(((float)cell.FixedPosition.X - (float)selectedCell.FixedPosition.X) / distance,
                                                    ((float)cell.FixedPosition.Y - (float)selectedCell.FixedPosition.Y) / distance);
                    }
                    else
                    {
                        cell.Vector = new PointF(0, 0);
                    }
                }
            }
        }

        private void UpdateWave()
        {
            DateTime currentTime = DateTime.Now;

            foreach (Cell cell in map.ListCell)
            {
                if (cell.StartTime != DateTime.MinValue)
                {
                    TimeSpan timeSpan = currentTime.Subtract(cell.StartTime);

                    float valueTime = curveTime.Evaluate((float)timeSpan.TotalMilliseconds);

                    cell.Value = cell.Coeff * valueTime;

                    if (timeSpan.TotalMilliseconds >= (double)timeMax)
                        cell.StartTime = DateTime.MinValue;



                    cell.Position= new System.Drawing.Point(cell.FixedPosition.X + (int)(cell.Value * cell.Vector.X*20f),
                                                            cell.FixedPosition.Y + (int)(cell.Value * cell.Vector.Y*20f));
                }
            }
        }

        #region Évènements
        private void pic_MouseMove(object sender, MouseEventArgs e)
        {
            map.ListCell.ForEach(cell => cell.Selected = false);

            prevSelectedCell = selectedCell;

            selectedCell = map.GetNearestCell(e.Location, 10);

            if (selectedCell != null)
                selectedCell.Selected = true;

            if (prevSelectedCell != selectedCell && selectedCell != null)
            {
                PropagateWave();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (map != null)
            {
                map.Draw(g);

                UpdateWave();
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            Init();
        }
        #endregion
    }
}
