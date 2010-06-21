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
using ThreadAStar.AStar;

namespace ThreadAStar
{
    public partial class FrmParallelAStar : Form
    {
        #region Callback
        delegate void RefreshProgression_Callback(int count, IComputable computable);
        delegate void AppendText_Callback(RichTextBox ctrl, string text);
        delegate void SetText_Callback(Control ctrl, string text);
        delegate void SetEnabled_Callback(Control ctrl, bool enabled);
        delegate void Empty_Callback();
        #endregion

        #region Propriétés
        private const String BUTTON_START = "Démarrer";
        private const String BUTTON_STOP = "Arrêter";
        private const String BUTTON_MAP_ON = "Afficher les maps";
        private const String BUTTON_MAP_OFF = "Cacher les maps";

        private ThreadManagerBase currentThreadManager;
        private DateTime startParallel = DateTime.MinValue;
        private TypeThreading methodToStart = TypeThreading.None;
        private List<IComputable> listMap = new List<IComputable>();
        private Graphics gMap;
        #endregion

        #region Constructeur
        public FrmParallelAStar()
        {
            InitializeComponent();
        }
        #endregion

        #region Méthodes privées
        private void InitResolving()
        {
            //--- Initialise le formulaire
            SetText(btnStartResolving, BUTTON_STOP);
            SetEnabled(pnlMethode, false);
            SetEnabled(pnlParametrage, false);
            //---

            //--- Création des MatrixMultiply
            //listMap = new List<IComputable>();
            //for (int i = 0; i < this.numCountMap.Value; i++)
            //{
            //    listMap.Add(new MatrixMultiply());
            //}
            //---

            AddLog("Calcul la liste des map");

            //--- Création des Map
            listMap = new List<IComputable>();
            for (int i = 0; i < this.numCountMap.Value; i++)
            {
                listMap.Add(new AStarMap(picMap.Width, picMap.Height, chkUtiliserGraine.Checked?  (int)numSeed.Value : int.MinValue, (int)numCountNode.Value, (int)numDistanceMax.Value));
            }
            //---

            AddLog("Démarrage du monitoring");

            //--- Démarre le monitoring de thread
            ucMonitoring.StartMonitoring((short)this.numRereshRate.Value);
            //---

            methodToStart = TypeThreading.Natif;
        }

        /// <summary>
        /// Démarre la résolution des map A* selon les modèles de parallélisation choisies
        /// </summary>
        private void StartResolving()
        {
            //--- Création du threadManager pour le type Natif
            if (chkMethodeNative.Checked && methodToStart == TypeThreading.Natif)
            {
                AddLog("Parallélisation mode natif - Début");
                currentThreadManager = new ThreadManagerNative((int)this.numNmbThread.Value, listMap);
            }
            else if (!chkMethodeNative.Checked && methodToStart == TypeThreading.Natif)
            {
                methodToStart = TypeThreading.BackgroundWorker;
            }
            //---

            //--- Création du threadManager pour le type BackGroundworker
            if (chkMethodeBackgroundWorker.Checked && methodToStart == TypeThreading.BackgroundWorker)
            {
                AddLog("Parallélisation mode BackgroundWorker - Début");
                currentThreadManager = new ThreadManagerBackgroundWorker((int)this.numNmbThread.Value, listMap);
            }
            else if (!chkMethodeBackgroundWorker.Checked && methodToStart == TypeThreading.BackgroundWorker)
            {
                methodToStart = TypeThreading.TaskParallelLibrary;
            }
            //---

            //--- Création du threadManager pour le type TPL
            if (chkMethodeTaskParallelLibrary.Checked && methodToStart == TypeThreading.TaskParallelLibrary)
            {
                AddLog("Parallélisation mode Task Parallel Library - Début");
                currentThreadManager = new ThreadManagerTPL((int)this.numNmbThread.Value, listMap);
            }
            else if (!chkMethodeTaskParallelLibrary.Checked && methodToStart == TypeThreading.TaskParallelLibrary)
            {
                methodToStart = TypeThreading.None;
            }
            //---

            if (methodToStart != TypeThreading.None)
            {
                InitUI();
                currentThreadManager.CalculCompletedEvent += new EventHandler<ComputableEventArgs>(currentThreadManager_CalculCompletedEvent);
                currentThreadManager.AllCalculCompletedEvent += new EventHandler(currentThreadManager_AllCalculCompletedEvent);
                currentThreadManager.StartComputation();
            }
            else
            {
                StopResolving();
            }
        }

        private void InitUI()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Empty_Callback(InitUI));
            }
            else
            {
                this.progressBar.Maximum = (int)this.numCountMap.Value;
                this.progressBar.Value = 0;
                startParallel = DateTime.Now;
                timer.Enabled = true;
                timer.Start();
            }
        }

        private void currentThreadManager_CalculCompletedEvent(object sender, ComputableEventArgs e)
        {
            if (this.InvokeRequired)
            {
                RefreshProgression_Callback call = new RefreshProgression_Callback(RefreshProgression);
                this.Invoke(call, ((ThreadManagerBase)sender).CountCalculated, e.Computable);
            }
            else
            {
                RefreshProgression(((ThreadManagerBase)sender).CountCalculated, e.Computable);
            }
        }

        private void currentThreadManager_AllCalculCompletedEvent(object sender, EventArgs e)
        {
            switch (methodToStart)
            {
                case TypeThreading.None:
                    break;
                case TypeThreading.Natif:
                    AddLog("Parallélisation mode natif - Fin");
                    break;
                case TypeThreading.BackgroundWorker:
                    AddLog("Parallélisation mode BackgroundWorker - Fin");
                    break;
                case TypeThreading.TaskParallelLibrary:
                    AddLog("Parallélisation mode Task Parallel Library - Fin");
                    break;
                default:
                    break;
            }

            //---> Afffiche la durée de résolution
            AddLog(String.Format("Durée : {0}", DateTime.Now.Subtract(startParallel).ToString()));

            //---> Passe à la méthode de parallélisation suivante
            methodToStart++;

            //--- Pause de 500 ms du thread Application
            //    les threads précédents ont ainsi le temps de se terminer
            Thread.Sleep(500);
            //---

            //---> Démarre la méthode de parallélisation
            StartResolving();
        }

        private void RefreshProgression(int count, IComputable computable)
        {
            progressBar.Value = count;

            if (btnShowMapResolving.Text == BUTTON_MAP_OFF)
                computable.Draw(gMap);
        }

        /// <summary>
        /// Arrête la résolution des map A*
        /// </summary>
        private void StopResolving()
        {
            AddLog("Arrêt de la résolution");

            //--- Initialise le formulaire
            timer.Stop();
            SetText(lblDureeCalcul, String.Empty);
            SetText(btnStartResolving, BUTTON_START);
            SetEnabled(pnlMethode, true);
            SetEnabled(pnlParametrage, true);
            //---

            //---> Arrête la surveillance de l'application
            if (chkSynchroMonitoring.Checked)
                ucMonitoring.StopMonitoring();

            //---> Arrête la résolution des map pour la méthode de parallélisation courante
            if (currentThreadManager != null)
                currentThreadManager.StopComputation();
        }

        private void AddLog(string text)
        {
            AppendText(txtLog, String.Format("{0} : {1}\r\n", DateTime.Now.ToLongTimeString(), text));
        }
        #endregion

        #region Méthodes callbacks
        private void AppendText(RichTextBox ctrl, string text)
        {
            if (ctrl.InvokeRequired)
            {
                AppendText_Callback call = new AppendText_Callback(AppendText);
                ctrl.Invoke(call, ctrl, text);
            }
            else
            {
                ctrl.AppendText(text);
            }
        }

        private void SetText(Control ctrl, string text)
        {
            if (ctrl.InvokeRequired)
            {
                SetText_Callback call = new SetText_Callback(SetText);
                ctrl.Invoke(call, ctrl, text);
            }
            else
            {
                ctrl.Text = text;
            }
        }

        private void SetEnabled(Control ctrl, bool enabled)
        {
            if (ctrl.InvokeRequired)
            {
                SetEnabled_Callback call = new SetEnabled_Callback(SetEnabled);
                ctrl.Invoke(call, ctrl, enabled);
            }
            else
            {
                ctrl.Enabled = enabled;
            }
        }
        #endregion

        #region Évènements
        private void FrmParallelAStar_Load(object sender, EventArgs e)
        {
            gMap = picMap.CreateGraphics();
        }

        private void FrmParallelAStar_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (btnStartResolving.Text == BUTTON_STOP)
            {
                StopResolving();
            }
        }

        private void btnStartResolving_Click(object sender, EventArgs e)
        {
            if (btnStartResolving.Text == BUTTON_START)
            {
                InitResolving();
                StartResolving();
            }
            else
            {
                StopResolving();
            }
        }

        private void btnShowMapResolving_Click(object sender, EventArgs e)
        {
            if (btnShowMapResolving.Text == BUTTON_MAP_ON)
            {
                btnShowMapResolving.Text = BUTTON_MAP_OFF;

                this.Width = picMap.Right + 10;
            }
            else
            {
                btnShowMapResolving.Text = BUTTON_MAP_ON;

                this.Width = picMap.Left + 6;
            }

            this.CenterToScreen();
        }

        private void chkSynchroMonitoring_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSynchroMonitoring.Checked && btnStartResolving.Text == BUTTON_START)
            {
                StopResolving();
            }
        }

        private void bntEffacerLog_Click(object sender, EventArgs e)
        {
            txtLog.Clear();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            lblDureeCalcul.Text = String.Format("Durée du calcul : {0}", DateTime.Now.Subtract(startParallel).ToString());
        }
        #endregion
    }
}
