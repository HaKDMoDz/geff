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

        public ThreadMonitor monitor;

        Graphics gImg;
        Graphics g;
        Image img;

        public UCMonitoring()
        {
            InitializeComponent();

            this.monitor = new ThreadMonitor(this);

            InitGraphics();
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

        int maxRamValue = 0;
        int maxCountThreads = 0;
        int maxCountNewThreads = 0;
        int maxCountDeadThreads = 0;

        public void RefreshGraph()
        {
            try
            {
                //if (this.pictureBox.InvokeRequired)
                //{
                //    Refresh_UCMonitoringCallback call = new Refresh_UCMonitoringCallback(RefreshGraph);
                //    pictureBox.Invoke(call);
                //}
                //else
                {
                    gImg.Clear(Color.DimGray);

                    Pen penBlack = new Pen(Brushes.Black, 1f);
                    Pen penRed = new Pen(Brushes.Red, 1f);
                    Pen penBlueNew = new Pen(Brushes.SkyBlue, 1f);
                    Pen penBlueCur = new Pen(Brushes.DodgerBlue, 1f);
                    Pen penBlueDead = new Pen(Brushes.DarkBlue, 1f);


                    TimelineData prevTimeLineData = new TimelineData();

                    //--- Calcul les valeurs hautes
                    maxRamValue = (int)monitor.ListTimeLineData.Max(t => t.RAM);
                    maxCountThreads = (int)monitor.ListTimeLineData.Max(t => t.CountThreads);
                    maxCountNewThreads = (int)monitor.ListTimeLineData.Max(t => t.CountNewThreads);
                    maxCountDeadThreads = (int)monitor.ListTimeLineData.Max(t => t.CountDeadThreads);
                    //---

                    foreach (TimelineData timelineData in monitor.ListTimeLineData)
                    {
                        //--- Courbe de la charge CPU
                        gImg.DrawLine(penBlack,
                            ConvertPointToGraph((int)prevTimeLineData.Time, (int)prevTimeLineData.CPU, 100),
                            ConvertPointToGraph((int)timelineData.Time, (int)timelineData.CPU, 100));
                        //---

                        //--- Courbe de la charge RAM
                        gImg.DrawLine(penRed,
                            ConvertPointToGraph((int)prevTimeLineData.Time, (int)prevTimeLineData.RAM, maxRamValue),
                            ConvertPointToGraph((int)timelineData.Time, (int)timelineData.RAM, maxRamValue));
                        //---

                        //=== Courbe des threads
                        //---> Total
                        gImg.DrawLine(penBlueCur,
                            ConvertPointToGraph((int)prevTimeLineData.Time, (int)prevTimeLineData.CountThreads, maxCountThreads),
                            ConvertPointToGraph((int)timelineData.Time, (int)timelineData.CountThreads, maxCountThreads));

                        //---> Nouveaux
                        gImg.DrawLine(penBlueNew,
                            ConvertPointToGraph((int)prevTimeLineData.Time, (int)prevTimeLineData.CountNewThreads, maxCountNewThreads),
                            ConvertPointToGraph((int)timelineData.Time, (int)timelineData.CountNewThreads, maxCountNewThreads));

                        //---> Morts
                        gImg.DrawLine(penBlueDead,
                            ConvertPointToGraph((int)prevTimeLineData.Time, (int)prevTimeLineData.CountDeadThreads, maxCountDeadThreads),
                            ConvertPointToGraph((int)timelineData.Time, (int)timelineData.CountDeadThreads, maxCountDeadThreads));
                        //===

                        prevTimeLineData = timelineData;
                    }

                    //Monitor.Enter(g);
                    g.DrawImage(img, 0, 0);
                    //Monitor.Exit(g);
                }
            }
            catch
            {
            }
        }

        private PointF ConvertPointToGraph(int x, int y, int maxY)
        {
            PointF point = Point.Empty;

            point.X = this.pictureBox.Width + x - monitor.ElapsedTimePart;

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

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            RefreshGraph();
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            RefreshGraph();
        }
    }
}
