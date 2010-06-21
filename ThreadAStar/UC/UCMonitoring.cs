using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ThreadAStar.Model;
using System.Threading;

namespace ThreadAStar.UC
{
    public partial class UCMonitoring : UserControl
    {
        delegate void Refresh_UCMonitoringCallback();
        private DateTime _lastRefreshLabel = DateTime.MinValue;
        private int maxCpuValue = 0;
        private int maxRamValue = 0;
        private int maxCountThreads = 0;
        private int maxCountNewThreads = 0;
        private int maxCountDeadThreads = 0;

        private Graphics gImg;
        private Graphics g;
        private Image img;

        private Pen penCPU = new Pen(Brushes.LightGreen, 1f);
        private Pen penRAM = new Pen(Brushes.Yellow, 1f);
        private Pen penThreadNew = new Pen(Brushes.SkyBlue, 1f);
        private Pen penThreadDead = new Pen(Brushes.DarkBlue, 1f);
        private Pen penThreadTotal = new Pen(Brushes.DodgerBlue, 1f);

        public ThreadMonitor monitor;

        public UCMonitoring()
        {
            InitializeComponent();

            this.monitor = new ThreadMonitor(this);

            InitGraphics();

            lblCPU.ForeColor = penCPU.Color;
            lblRAM.ForeColor = penRAM.Color;
            lblNewThread.ForeColor = penThreadNew.Color;
            lblDeadThread.ForeColor = penThreadDead.Color;
            lblTotalThread.ForeColor = penThreadTotal.Color;
        }

        private void InitGraphics()
        {
            g = pictureBox.CreateGraphics();
            img = new Bitmap(pictureBox.Width, pictureBox.Height);
            gImg = Graphics.FromImage(img);
        }

        public void StartMonitoring(short refreshRate)
        {
            monitor.StartMonitoring(refreshRate);
        }

        public void StopMonitoring()
        {
            monitor.StopMonitoring();
        }

        public void RefreshGraph()
        {
            try
            {
                DateTime currentRefresh = DateTime.Now;

                gImg.Clear(Color.Gray);

                TimelineData prevTimeLineData = new TimelineData();

                //--- Calcul les valeurs hautes
                maxCpuValue = (int)monitor.ListTimeLineData.Max(t => t.CPU);
                maxRamValue = (int)monitor.ListTimeLineData.Max(t => t.RAM);
                maxCountThreads = (int)monitor.ListTimeLineData.Max(t => t.CountThreads);
                maxCountNewThreads = (int)monitor.ListTimeLineData.Max(t => t.CountNewThreads);
                maxCountDeadThreads = (int)monitor.ListTimeLineData.Max(t => t.CountDeadThreads);
                //---

                foreach (TimelineData timelineData in monitor.ListTimeLineData)
                {
                    //--- Courbe de la charge CPU
                    if (chkCPU.Checked)
                    {
                        gImg.DrawLine(penCPU,
                            ConvertPointToGraph((int)prevTimeLineData.Time, (int)prevTimeLineData.CPU, 100),
                            ConvertPointToGraph((int)timelineData.Time, (int)timelineData.CPU, 100));
                    }
                    //---

                    //--- Courbe de la charge RAM
                    if (chkRAM.Checked)
                    {
                        gImg.DrawLine(penRAM,
                        ConvertPointToGraph((int)prevTimeLineData.Time, (int)prevTimeLineData.RAM, maxRamValue),
                        ConvertPointToGraph((int)timelineData.Time, (int)timelineData.RAM, maxRamValue));
                    }
                    //---

                    //=== Courbe des threads
                    //---> Nouveaux
                    if (chkThreadNew.Checked)
                    {
                        gImg.DrawLine(penThreadNew,
                            ConvertPointToGraph((int)prevTimeLineData.Time, (int)prevTimeLineData.CountNewThreads, maxCountNewThreads),
                            ConvertPointToGraph((int)timelineData.Time, (int)timelineData.CountNewThreads, maxCountNewThreads));
                    }

                    //---> Morts
                    if (chkThreadDead.Checked)
                    {
                        gImg.DrawLine(penThreadDead,
                            ConvertPointToGraph((int)prevTimeLineData.Time, (int)prevTimeLineData.CountDeadThreads, maxCountDeadThreads),
                            ConvertPointToGraph((int)timelineData.Time, (int)timelineData.CountDeadThreads, maxCountDeadThreads));
                    }

                    //---> Total
                    if (chkThreadTotal.Checked)
                    {
                        gImg.DrawLine(penThreadTotal,
                            ConvertPointToGraph((int)prevTimeLineData.Time, (int)prevTimeLineData.CountThreads, maxCountThreads),
                            ConvertPointToGraph((int)timelineData.Time, (int)timelineData.CountThreads, maxCountThreads));
                    }
                    //===

                    prevTimeLineData = timelineData;
                }

                //---> Rafraichissement des labels toutes les 5 secondes
                //     Ces labels sont Thread Safe
                if (currentRefresh.Subtract(_lastRefreshLabel).TotalMilliseconds > 1)
                {
                    _lastRefreshLabel = currentRefresh;
                    lblCPU.SetText(String.Format("Max: {0}%\r\nCur:  {1}%", maxCpuValue, prevTimeLineData.CPU));
                    lblRAM.SetText(String.Format("Max: {0}\r\nCur:  {1}", FormatRAMValue(maxRamValue), FormatRAMValue(prevTimeLineData.RAM)));
                    lblTotalThread.SetText(String.Format("Max: {0}\r\nCur:  {1}", maxCountThreads, prevTimeLineData.CountThreads));
                    lblNewThread.SetText(String.Format("Max: {0}\r\nCur:  {1}", maxCountNewThreads, prevTimeLineData.CountNewThreads));
                    lblDeadThread.SetText(String.Format("Max: {0}\r\nCur:  {1}", maxCountDeadThreads, prevTimeLineData.CountDeadThreads));
                }

                g.DrawImage(img, 0, 0);
            }
            catch
            {
            }
        }

        private string FormatRAMValue(long ram)
        {
            int pas = 0;
            string[] sizes = new string[] { "O", "Ko", "Mo", "Go", "To" };

            long convertedDiskSize = GetMinDiskSize(ram, ref pas);

            return String.Format("{0} {1}", convertedDiskSize, sizes[pas]);
        }

        private long GetMinDiskSize(long diskSize, ref int pas)
        {
            if (diskSize > 1024)
            {
                pas++;
                return GetMinDiskSize(diskSize / 1024, ref pas);
            }
            else
            {
                return diskSize;
            }
        }

        private PointF ConvertPointToGraph(int x, int y, int maxY)
        {
            PointF point = Point.Empty;

            //---> Calcul du point selon la portion de temps écoulée (une portion de temps équivaut à 3 pixels afin de rendre les courbes plus lisibles)
            point.X = this.pictureBox.Width + (x - monitor.ElapsedTimePart) * 3;

            if (maxY > 0)
            {
                point.Y = this.pictureBox.Height - (0.95f * (float)this.pictureBox.Height * (float)y) / (float)maxY;
            }
            else
            {
                point.Y = this.pictureBox.Height - y;
            }

            return point;
        }

        private PointF ConvertPointToGraph(int x, int y)
        {
            return ConvertPointToGraph(x, y, 0);
        }
    }
}
