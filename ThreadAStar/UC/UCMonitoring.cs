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

        public void StartMonitoring()
        {
            monitor.StartMonitoring();
        }

        public void StopMonitoring()
        {
            monitor.StartMonitoring();
        }


        public void RefreshGraph()
        {
            //if (this.pictureBox.InvokeRequired)
            //{
            //    Refresh_UCMonitoringCallback call = new Refresh_UCMonitoringCallback(RefreshGraph);
            //    pictureBox.Invoke(call);
            //}
            //else
            {
                gImg.Clear(Color.White);
                
                Pen penBlack = new Pen(Brushes.Black, 3f);
                Pen penRed = new Pen(Brushes.Red, 3f);
                Pen penBlue = new Pen(Brushes.Blue, 3f);

                int numThread = 0;
                foreach (ThreadData threadData in  monitor.ListThreadData.Values)
                {
                    numThread++;

                    gImg.DrawLine(penBlack, numThread * 5, 0, numThread * 5, (int)threadData.CPUMax+1);
                    gImg.DrawLine(penRed, numThread * 5, 0, numThread * 5, (int)threadData.CPUAverage+1);
                    gImg.DrawLine(penBlue, numThread * 5, 0, numThread * 5, (int)threadData.CPUMin+1);
                }
                
                gImg.DrawString(DateTime.Now.TimeOfDay.ToString(), this.Font, Brushes.Black, new PointF(this.Width-150,50));

                Monitor.Enter(g);
                g.DrawImage(img, 0, 0);
                Monitor.Exit(g);
            }
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
