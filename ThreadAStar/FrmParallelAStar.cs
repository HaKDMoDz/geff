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
using ThreadAStar.ThreadManager;

namespace ThreadAStar
{
    public partial class FrmParallelAStar : Form
    {
        #region Propriétés
        private const String BUTTON_START = "Démarrer";
        private const String BUTTON_STOP = "Arrêter";
        private IThreadManager currentThreadManager;

        public static FrmParallelAStar frm; 
        #endregion

        #region Constructeur
        public FrmParallelAStar()
        {
            InitializeComponent();

            FrmParallelAStar.frm = this;
        } 
        #endregion

        #region Méthodes privées
        /// <summary>
        /// Démarre la résolution des map A* selon les modèles de parallélisation choisies
        /// </summary>
        private void StartResolving()
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

            //--- Création du threadManager pour le type Natif
            if (chkMethodeNative.Checked)
            {
                currentThreadManager = new ThreadManagerSimple(10, TypeThreading.Natif, ListMap);
                currentThreadManager.StartComputation();
            }
            //---

            //--- Création du threadManager pour le type BackGroundworker
            //if (chkMethodeBackgroundWorker.Checked)
            //{
            //    currentThreadManager = new ThreadManagerSimple(10, TypeThreading.BackgroundWorker, ListMap);
            //    currentThreadManager.StartComputation();
            //}
            //---

            //--- Création du threadManager pour le type Natif .Net 4
            if (chkMethodeNativeDotNet4.Checked)
            {
                //currentThreadManager = new ThreadManagerDotNet4(10, TypeThreading.DotNet4, ListMap);
                //currentThreadManager.StartComputation();
            }
            //---
        }

        /// <summary>
        /// Arrête la résolution des map A*
        /// </summary>
        private void StopResolving()
        {
            //---> Arrête la surveillance de l'application
            ucMonitoring.StopMonitoring();

            //---> Arrête la résolution des map pour la méthode de parallélisation courante
            currentThreadManager.StopComputation();
        }
        #endregion

        #region Évènements
        private void btnStartResolving_Click(object sender, EventArgs e)
        {
            if (btnStartResolving.Text == BUTTON_START)
            {
                btnStartResolving.Text = BUTTON_STOP;

                pnlMethode.Enabled = false;
                pnlParametrage.Enabled = false;

                StartResolving();
            }
            else
            {
                btnStartResolving.Text = BUTTON_START;

                StopResolving();

                pnlMethode.Enabled = true;
                pnlParametrage.Enabled = true;
            }
        }

        private void FrmParallelAStar_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (btnStartResolving.Text == BUTTON_STOP)
            {
                StopResolving();
            }
        }
        #endregion
    }
}
