using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace CubEat
{
    public partial class Form1 : Form
    {
        public Repository r { get; set; }

        int sampleSize = 20;
        int numberOfLayer = 5;
        int speed = 150;
        Font font;

        Graphics gNextSample;
        Graphics gMap;
        Image imgMap;
        Graphics gImgMap;


        public Form1()
        {
            InitializeComponent();
        }

        #region Méthodes privées
        private void Init()
        {
            this.Top = Screen.PrimaryScreen.WorkingArea.Height - this.Height - 20;
            this.Left = Screen.PrimaryScreen.WorkingArea.Width - this.Width - 20;

            gMap = pictureBoxMap.CreateGraphics();
            gNextSample = pictureBoxNextSample.CreateGraphics();

            imgMap = new Bitmap(pictureBoxMap.Width, pictureBoxMap.Height);
            gImgMap = Graphics.FromImage(imgMap);

            r = new Repository();
            r.StopSound();

            r.CreateNewMap(numberOfLayer * 2 - 1, r.ListLibraySample[0]);

            timer.Interval = speed;
            trackBar.Value = speed;
            trackBar_Scroll(null, null);

            font = new System.Drawing.Font("Symbol", 18f, FontStyle.Regular);

            comboBoxLibrary.Items.Clear();

            foreach (LibraySample library in r.ListLibraySample)
            {
                comboBoxLibrary.Items.Add(library.Name);
            }

            comboBoxLibrary.SelectedIndex = 0;
        }

        private Point ConvertPoint(Map map, Point point)
        {
            Point pointRet = new Point();
            return pointRet;
        }

        public void Draw()
        {
            gImgMap.Clear(Color.White);

            Point map1Location = new Point(50, 70);
            Point map2Location = new Point(50 + r.Map1.Size * sampleSize + 20, 70);

            DrawNextSample();
            DrawMap(r.Map1, map1Location);
            //DrawMap(r.Map2, map2Location);
            //DrawPlayedSample(r.Map1, map1Location);
            //DrawPlayedSample(r.Map2, map2Location);

            DrawCurrentMeasure(r.Map1, map1Location);

            gMap.DrawImage(imgMap, 0, 0);
        }

        public void DrawNextSample()
        {
        }

        public void DrawCurrentMeasure(Map map, Point location)
        {
            HatchBrush brushMeasure = new HatchBrush(HatchStyle.LightDownwardDiagonal, Color.Silver, Color.Transparent);
            HatchBrush brushPlayedTime = new HatchBrush(HatchStyle.Weave, Color.Purple, Color.Transparent);

            location.Offset(new Point(0, (map.Size+2)*sampleSize));
            for (int layer = 0; layer < map.LayerCells.Keys.Count; layer++)
            {
                //--- Nombre de cases sur la couche
                int numberOfCellOnLayer = layer * 8;

                if (numberOfCellOnLayer == 0)
                    numberOfCellOnLayer = 1;
                //---

                //--- Beat  de la couche
                int layerBeat = r.Beat % numberOfCellOnLayer;
                //---

                //--- 
                int minPlayedCell = (layerBeat / 4) * 4;
                //--- 

                for (int i = 0; i < map.LayerCells[layer].Count; i++)
                {
                    Cell cell = map.LayerCells[layer][i];

                    if (cell.IsInPlayedTime)
                    {
                        if (!cell.IsEmpty)
                        {
                            gImgMap.FillRectangle(new SolidBrush(cell.Sample.SampleModel.Color),
                                location.X + (cell.NumberOnLayer - minPlayedCell) * sampleSize, 
                                location.Y + layer * sampleSize, 
                                sampleSize, sampleSize);
                        }

                        if (cell.IsOnMeasure)
                            gImgMap.FillRectangle(brushMeasure, location.X + (cell.NumberOnLayer - minPlayedCell) * sampleSize, location.Y + layer * sampleSize, sampleSize, sampleSize);

                        gImgMap.FillRectangle(brushPlayedTime, location.X + (cell.NumberOnLayer - minPlayedCell) * sampleSize, location.Y + layer * sampleSize, sampleSize, sampleSize);

                        if (cell.IsEmitting)
                            gImgMap.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.Red)), location.X + (cell.NumberOnLayer - minPlayedCell) * sampleSize + sampleSize / 4, location.Y + layer * sampleSize + sampleSize / 4, sampleSize / 2, sampleSize / 2);
                    }
                }
            }
        }

        public void DrawMap(Map map, Point location)
        {
            HatchBrush brushMeasure = new HatchBrush(HatchStyle.LightDownwardDiagonal, Color.Silver, Color.Transparent);
            HatchBrush brushPlayedTime = new HatchBrush(HatchStyle.Weave, Color.Purple, Color.Transparent);

            for (int x = 0; x < map.Size; x++)
            {
                for (int y = 0; y < map.Size; y++)
                {
                    if (!map.Cells[x, y].IsEmpty)
                    {
                        gImgMap.FillRectangle(new SolidBrush(map.Cells[x, y].Sample.SampleModel.Color), location.X + x * sampleSize, location.Y + y * sampleSize, sampleSize, sampleSize);

                        //gImgMap.DrawString(map.Cells[x, y].NumberOnLayer.ToString(), font, Brushes.Black, location.X + ((float)x + 0.3f) * (float)sampleSize, location.Y + ((float)y + 0.2f) * (float)sampleSize);
                    }

                    if (map.Cells[x, y].IsOnMeasure)
                        gImgMap.FillRectangle(brushMeasure, location.X + x * sampleSize, location.Y + y * sampleSize, sampleSize, sampleSize);

                    //if (!map.Cells[x, y].IsEmpty)
                    //    gImgMap.DrawString(map.Cells[x, y].NumberOnLayer.ToString(), font, Brushes.Black, location.X + ((float)x + 0.3f) * (float)sampleSize, location.Y + ((float)y + 0.2f) * (float)sampleSize);


                    if (map.Cells[x, y].IsInPlayedTime)
                        gImgMap.FillRectangle(brushPlayedTime, location.X + x * sampleSize, location.Y + y * sampleSize, sampleSize, sampleSize);


                    if (map.Cells[x, y].IsEmitting)
                        gImgMap.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.Red)), location.X + x * sampleSize + sampleSize / 4, location.Y + y * sampleSize + sampleSize / 4, sampleSize / 2, sampleSize / 2);

                    gImgMap.DrawRectangle(Pens.DarkGray, location.X + location.X * sampleSize, location.Y + location.Y * sampleSize, sampleSize, sampleSize);
                }
            }

            for (int layer = 0; layer < map.Size / 2; layer++)
            {
                Pen pen = new Pen(Color.Black, 3f);

                gImgMap.DrawRectangle(pen, location.X + layer * sampleSize, location.Y + layer * sampleSize, (map.Size - layer * 2) * sampleSize, (map.Size - layer * 2) * sampleSize);
            }

        }

        private void DrawPlayedSample(Map map, Point location)
        {
            int sampleModelNumber = 0;
            foreach (int sampleId in map.PlayedSample.Keys)
            {
                Rectangle rec = new Rectangle(location.X, location.Y + map.Size * sampleSize + 20 + sampleModelNumber * sampleSize / 3, sampleSize / 3 * map.PlayedSample[sampleId], sampleSize / 3);

                gImgMap.FillRectangle(new SolidBrush(map.CurrentLibrarySample.ListSampleModel[sampleId].Color), rec);

                sampleModelNumber++;
            }
        }

        private string GetSymbolValue(TypeSymbol typeSymbol)
        {
            switch (typeSymbol)
            {
                case TypeSymbol.Alpha:
                    return "a";
                case TypeSymbol.Beta:
                    return "b";
                case TypeSymbol.Gamma:
                    return "g";
                default:
                    return "";
            }
        }

        private void Update()
        {
            r.Update();
        }

        #endregion

        #region Évènements
        private void timer_Tick(object sender, EventArgs e)
        {
            Update();

            Draw();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            r.CreateNewMap(numberOfLayer * 2 - 1, r.ListLibraySample[comboBoxLibrary.SelectedIndex]);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            r.StopSound();
        }

        private void trackBar_Scroll(object sender, EventArgs e)
        {
            speed = trackBar.Value;
            timer.Interval = speed;

            this.Text = String.Format("{0} bpm", 60000 / speed);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Init();
        }
        #endregion
    }
}
