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

        public Form1()
        {
            InitializeComponent();

            Init();
        }

        private void Init()
        {
            timer.Interval = 10;

            map = new Map(pic.Width, pic.Height, 5000, 50, TypeCellDistribution.Random);

            g = pic.CreateGraphics();

            //---
            float distanceMax = 100;

            curveDistance = new Curve();
            curveDistance.Keys.Add(new CurveKey(0 * distanceMax, 1f));
            curveDistance.Keys.Add(new CurveKey(0.5f * distanceMax, 0.25f));
            curveDistance.Keys.Add(new CurveKey(0.75f * distanceMax, 0.7f));
            curveDistance.Keys.Add(new CurveKey(1f * distanceMax, 0f));
            //---

            //---
            float timeMax = 1000f;

            curveTime = new Curve();
            curveTime.Keys.Add(new CurveKey(0 * timeMax, 1f));
            curveTime.Keys.Add(new CurveKey(1f * timeMax, 0f));
            //---
        }

        private void PropagateWave()
        {
            DateTime startTime = DateTime.Now;

            foreach (Cell cell in map.ListCell)
            {
                float distance = (float)Map.Distance(cell.Position, selectedCell.Position);

                float value = curveDistance.Evaluate(distance) * curveTime.Evaluate(0f);

                if (value > cell.Value)
                {
                    cell.Coeff = curveDistance.Evaluate(distance);
                    cell.StartTime = startTime;
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

                    if (cell.Value < 0.01f)
                        cell.StartTime = DateTime.MinValue;
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
