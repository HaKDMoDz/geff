using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ThreadAStar.Model;
using System.Threading;

namespace ThreadAStar
{
    public partial class Form1 : Form
    {
        public static Form1 frm;

        public Form1()
        {
            InitializeComponent();

            Form1.frm = this;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //--- Création des Map
            List<IComputable> ListMap = new List<IComputable>();
            for (int i = 0; i < 20000; i++)
            {
                ListMap.Add(new MatrixMultiply());
            }
            //---

            //--- Démarre le monitoring de thread
            ucMonitoring.StartMonitoring();
            //---

            //--- Création du threadManager pour le type BackGroundworker
            ThreadManagerSimple threadManagerBW = new ThreadManagerSimple(10, TypeThreading.Natif, ListMap);
            threadManagerBW.StartComputation();
            //---

            //monitor.StopMonitoring();

            //while (!threadManagerBW.allCalculCompleted)
            //{
            //    this.Text = threadManagerBW.t.ToString();
            //    Thread.Sleep(10);
            //}
            //---
            //ThreadManager threadManagerNA = new ThreadManager(10, TypeThreading.Natif, ListMap);
            //threadManagerNA.Start();
            //---

            //---
            //---

            //---
            //---

            //---
            //---

            //---
            //---

            //---
            //---
        }
    }
}
