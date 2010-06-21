namespace ThreadAStar
{
    partial class FrmParallelAStar
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnStartResolving = new System.Windows.Forms.Button();
            this.pnlMethode = new System.Windows.Forms.Panel();
            this.chkMethodeTaskParallelLibrary = new System.Windows.Forms.CheckBox();
            this.chkMethodeBackgroundWorker = new System.Windows.Forms.CheckBox();
            this.chkMethodeNative = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlParametrage = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.numDistanceMax = new System.Windows.Forms.NumericUpDown();
            this.chkUtiliserGraine = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.numSeed = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.numCountNode = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.numCountMap = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.numRereshRate = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.numNmbThread = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnShowMapResolving = new System.Windows.Forms.Button();
            this.chkSynchroMonitoring = new System.Windows.Forms.CheckBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.bntEffacerLog = new System.Windows.Forms.Button();
            this.lblDureeCalcul = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.picMap = new System.Windows.Forms.PictureBox();
            this.ucMonitoring = new ThreadAStar.UC.UCMonitoring();
            this.pnlMethode.SuspendLayout();
            this.pnlParametrage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDistanceMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCountNode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCountMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRereshRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNmbThread)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMap)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStartResolving
            // 
            this.btnStartResolving.BackColor = System.Drawing.Color.DarkGray;
            this.btnStartResolving.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnStartResolving.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartResolving.ForeColor = System.Drawing.Color.Black;
            this.btnStartResolving.Location = new System.Drawing.Point(361, 223);
            this.btnStartResolving.Name = "btnStartResolving";
            this.btnStartResolving.Size = new System.Drawing.Size(75, 23);
            this.btnStartResolving.TabIndex = 0;
            this.btnStartResolving.Text = "Démarrer";
            this.btnStartResolving.UseVisualStyleBackColor = false;
            this.btnStartResolving.Click += new System.EventHandler(this.btnStartResolving_Click);
            // 
            // pnlMethode
            // 
            this.pnlMethode.BackColor = System.Drawing.Color.DimGray;
            this.pnlMethode.Controls.Add(this.chkMethodeTaskParallelLibrary);
            this.pnlMethode.Controls.Add(this.chkMethodeBackgroundWorker);
            this.pnlMethode.Controls.Add(this.chkMethodeNative);
            this.pnlMethode.Controls.Add(this.label1);
            this.pnlMethode.Location = new System.Drawing.Point(12, 11);
            this.pnlMethode.Name = "pnlMethode";
            this.pnlMethode.Size = new System.Drawing.Size(203, 206);
            this.pnlMethode.TabIndex = 2;
            // 
            // chkMethodeTaskParallelLibrary
            // 
            this.chkMethodeTaskParallelLibrary.AutoSize = true;
            this.chkMethodeTaskParallelLibrary.ForeColor = System.Drawing.Color.White;
            this.chkMethodeTaskParallelLibrary.Location = new System.Drawing.Point(14, 94);
            this.chkMethodeTaskParallelLibrary.Name = "chkMethodeTaskParallelLibrary";
            this.chkMethodeTaskParallelLibrary.Size = new System.Drawing.Size(121, 17);
            this.chkMethodeTaskParallelLibrary.TabIndex = 3;
            this.chkMethodeTaskParallelLibrary.Text = "Task Parallel Library";
            this.chkMethodeTaskParallelLibrary.UseVisualStyleBackColor = true;
            // 
            // chkMethodeBackgroundWorker
            // 
            this.chkMethodeBackgroundWorker.AutoSize = true;
            this.chkMethodeBackgroundWorker.ForeColor = System.Drawing.Color.White;
            this.chkMethodeBackgroundWorker.Location = new System.Drawing.Point(14, 70);
            this.chkMethodeBackgroundWorker.Name = "chkMethodeBackgroundWorker";
            this.chkMethodeBackgroundWorker.Size = new System.Drawing.Size(119, 17);
            this.chkMethodeBackgroundWorker.TabIndex = 2;
            this.chkMethodeBackgroundWorker.Text = "BackgroundWorker";
            this.chkMethodeBackgroundWorker.UseVisualStyleBackColor = true;
            // 
            // chkMethodeNative
            // 
            this.chkMethodeNative.AutoSize = true;
            this.chkMethodeNative.Checked = true;
            this.chkMethodeNative.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMethodeNative.ForeColor = System.Drawing.Color.White;
            this.chkMethodeNative.Location = new System.Drawing.Point(14, 46);
            this.chkMethodeNative.Name = "chkMethodeNative";
            this.chkMethodeNative.Size = new System.Drawing.Size(57, 17);
            this.chkMethodeNative.TabIndex = 1;
            this.chkMethodeNative.Text = "Native";
            this.chkMethodeNative.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.CornflowerBlue;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(-1, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(204, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Méthodes de parallélisation";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlParametrage
            // 
            this.pnlParametrage.BackColor = System.Drawing.Color.DimGray;
            this.pnlParametrage.Controls.Add(this.label9);
            this.pnlParametrage.Controls.Add(this.numDistanceMax);
            this.pnlParametrage.Controls.Add(this.chkUtiliserGraine);
            this.pnlParametrage.Controls.Add(this.label8);
            this.pnlParametrage.Controls.Add(this.numSeed);
            this.pnlParametrage.Controls.Add(this.label7);
            this.pnlParametrage.Controls.Add(this.numCountNode);
            this.pnlParametrage.Controls.Add(this.panel1);
            this.pnlParametrage.Controls.Add(this.label6);
            this.pnlParametrage.Controls.Add(this.numCountMap);
            this.pnlParametrage.Controls.Add(this.label5);
            this.pnlParametrage.Controls.Add(this.numRereshRate);
            this.pnlParametrage.Controls.Add(this.label4);
            this.pnlParametrage.Controls.Add(this.numNmbThread);
            this.pnlParametrage.Controls.Add(this.label2);
            this.pnlParametrage.Location = new System.Drawing.Point(221, 11);
            this.pnlParametrage.Name = "pnlParametrage";
            this.pnlParametrage.Size = new System.Drawing.Size(339, 206);
            this.pnlParametrage.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(172, 137);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(148, 13);
            this.label9.TabIndex = 15;
            this.label9.Text = "Distance maximum de liaison :";
            // 
            // numDistanceMax
            // 
            this.numDistanceMax.BackColor = System.Drawing.Color.Silver;
            this.numDistanceMax.Location = new System.Drawing.Point(175, 153);
            this.numDistanceMax.Name = "numDistanceMax";
            this.numDistanceMax.Size = new System.Drawing.Size(150, 20);
            this.numDistanceMax.TabIndex = 14;
            this.numDistanceMax.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // chkUtiliserGraine
            // 
            this.chkUtiliserGraine.AutoSize = true;
            this.chkUtiliserGraine.Location = new System.Drawing.Point(172, 117);
            this.chkUtiliserGraine.Name = "chkUtiliserGraine";
            this.chkUtiliserGraine.Size = new System.Drawing.Size(15, 14);
            this.chkUtiliserGraine.TabIndex = 13;
            this.chkUtiliserGraine.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(169, 95);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(156, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "Graine du générateur aléatoire :";
            // 
            // numSeed
            // 
            this.numSeed.BackColor = System.Drawing.Color.Silver;
            this.numSeed.Enabled = false;
            this.numSeed.Location = new System.Drawing.Point(193, 111);
            this.numSeed.Name = "numSeed";
            this.numSeed.Size = new System.Drawing.Size(129, 20);
            this.numSeed.TabIndex = 11;
            this.numSeed.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(3, 137);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(144, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Nombre de noeuds par map :";
            // 
            // numCountNode
            // 
            this.numCountNode.BackColor = System.Drawing.Color.Silver;
            this.numCountNode.Location = new System.Drawing.Point(6, 153);
            this.numCountNode.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numCountNode.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numCountNode.Name = "numCountNode";
            this.numCountNode.Size = new System.Drawing.Size(150, 20);
            this.numCountNode.TabIndex = 9;
            this.numCountNode.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.panel1.Location = new System.Drawing.Point(3, 83);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(336, 5);
            this.panel1.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(3, 95);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Nombre de map :";
            // 
            // numCountMap
            // 
            this.numCountMap.BackColor = System.Drawing.Color.Silver;
            this.numCountMap.Location = new System.Drawing.Point(6, 111);
            this.numCountMap.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numCountMap.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numCountMap.Name = "numCountMap";
            this.numCountMap.Size = new System.Drawing.Size(150, 20);
            this.numCountMap.TabIndex = 6;
            this.numCountMap.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(169, 40);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(153, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Taux de rafraichissement (ms) :";
            // 
            // numRereshRate
            // 
            this.numRereshRate.BackColor = System.Drawing.Color.Silver;
            this.numRereshRate.Location = new System.Drawing.Point(172, 56);
            this.numRereshRate.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.numRereshRate.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numRereshRate.Name = "numRereshRate";
            this.numRereshRate.Size = new System.Drawing.Size(150, 20);
            this.numRereshRate.TabIndex = 4;
            this.numRereshRate.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(3, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Nombre de threads :";
            // 
            // numNmbThread
            // 
            this.numNmbThread.BackColor = System.Drawing.Color.Silver;
            this.numNmbThread.Location = new System.Drawing.Point(6, 56);
            this.numNmbThread.Name = "numNmbThread";
            this.numNmbThread.Size = new System.Drawing.Size(150, 20);
            this.numNmbThread.TabIndex = 2;
            this.numNmbThread.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.CornflowerBlue;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(339, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Paramétrage";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.CornflowerBlue;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(12, 382);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(549, 23);
            this.label3.TabIndex = 4;
            this.label3.Text = "Surveillance du process";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnShowMapResolving
            // 
            this.btnShowMapResolving.BackColor = System.Drawing.Color.DarkGray;
            this.btnShowMapResolving.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnShowMapResolving.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowMapResolving.ForeColor = System.Drawing.Color.Black;
            this.btnShowMapResolving.Location = new System.Drawing.Point(442, 223);
            this.btnShowMapResolving.Name = "btnShowMapResolving";
            this.btnShowMapResolving.Size = new System.Drawing.Size(118, 23);
            this.btnShowMapResolving.TabIndex = 6;
            this.btnShowMapResolving.Text = "Cacher les maps";
            this.btnShowMapResolving.UseVisualStyleBackColor = false;
            this.btnShowMapResolving.Click += new System.EventHandler(this.btnShowMapResolving_Click);
            // 
            // chkSynchroMonitoring
            // 
            this.chkSynchroMonitoring.AutoSize = true;
            this.chkSynchroMonitoring.ForeColor = System.Drawing.Color.White;
            this.chkSynchroMonitoring.Location = new System.Drawing.Point(177, 230);
            this.chkSynchroMonitoring.Name = "chkSynchroMonitoring";
            this.chkSynchroMonitoring.Size = new System.Drawing.Size(181, 17);
            this.chkSynchroMonitoring.TabIndex = 14;
            this.chkSynchroMonitoring.Text = "Synchroniser l\'arrêt du monitoring";
            this.chkSynchroMonitoring.UseVisualStyleBackColor = true;
            this.chkSynchroMonitoring.CheckedChanged += new System.EventHandler(this.chkSynchroMonitoring_CheckedChanged);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 369);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(548, 10);
            this.progressBar.TabIndex = 15;
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.Color.DarkGray;
            this.txtLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLog.Location = new System.Drawing.Point(12, 253);
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(337, 110);
            this.txtLog.TabIndex = 16;
            this.txtLog.Text = "";
            // 
            // bntEffacerLog
            // 
            this.bntEffacerLog.BackColor = System.Drawing.Color.DarkGray;
            this.bntEffacerLog.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bntEffacerLog.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.bntEffacerLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bntEffacerLog.ForeColor = System.Drawing.Color.Black;
            this.bntEffacerLog.Location = new System.Drawing.Point(12, 223);
            this.bntEffacerLog.Name = "bntEffacerLog";
            this.bntEffacerLog.Size = new System.Drawing.Size(118, 23);
            this.bntEffacerLog.TabIndex = 17;
            this.bntEffacerLog.Text = "Effacer log";
            this.bntEffacerLog.UseVisualStyleBackColor = false;
            this.bntEffacerLog.Click += new System.EventHandler(this.bntEffacerLog_Click);
            // 
            // lblDureeCalcul
            // 
            this.lblDureeCalcul.AutoSize = true;
            this.lblDureeCalcul.ForeColor = System.Drawing.Color.White;
            this.lblDureeCalcul.Location = new System.Drawing.Point(358, 263);
            this.lblDureeCalcul.Name = "lblDureeCalcul";
            this.lblDureeCalcul.Size = new System.Drawing.Size(0, 13);
            this.lblDureeCalcul.TabIndex = 18;
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // picMap
            // 
            this.picMap.BackColor = System.Drawing.Color.Black;
            this.picMap.Location = new System.Drawing.Point(571, 11);
            this.picMap.Name = "picMap";
            this.picMap.Size = new System.Drawing.Size(563, 687);
            this.picMap.TabIndex = 19;
            this.picMap.TabStop = false;
            // 
            // ucMonitoring
            // 
            this.ucMonitoring.BackColor = System.Drawing.Color.DimGray;
            this.ucMonitoring.Location = new System.Drawing.Point(12, 405);
            this.ucMonitoring.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.ucMonitoring.Name = "ucMonitoring";
            this.ucMonitoring.Size = new System.Drawing.Size(549, 293);
            this.ucMonitoring.TabIndex = 5;
            // 
            // FrmParallelAStar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(1146, 709);
            this.Controls.Add(this.picMap);
            this.Controls.Add(this.lblDureeCalcul);
            this.Controls.Add(this.bntEffacerLog);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.chkSynchroMonitoring);
            this.Controls.Add(this.btnShowMapResolving);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnStartResolving);
            this.Controls.Add(this.ucMonitoring);
            this.Controls.Add(this.pnlParametrage);
            this.Controls.Add(this.pnlMethode);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmParallelAStar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Parallélisation et algorithme A*";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmParallelAStar_FormClosing);
            this.Load += new System.EventHandler(this.FrmParallelAStar_Load);
            this.pnlMethode.ResumeLayout(false);
            this.pnlMethode.PerformLayout();
            this.pnlParametrage.ResumeLayout(false);
            this.pnlParametrage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDistanceMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCountNode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCountMap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRereshRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNmbThread)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMap)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStartResolving;
        private System.Windows.Forms.Panel pnlMethode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlParametrage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private ThreadAStar.UC.UCMonitoring ucMonitoring;
        private System.Windows.Forms.CheckBox chkMethodeTaskParallelLibrary;
        private System.Windows.Forms.CheckBox chkMethodeBackgroundWorker;
        private System.Windows.Forms.CheckBox chkMethodeNative;
        private System.Windows.Forms.Button btnShowMapResolving;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numRereshRate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numNmbThread;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown numDistanceMax;
        private System.Windows.Forms.CheckBox chkUtiliserGraine;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numSeed;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numCountNode;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numCountMap;
        private System.Windows.Forms.CheckBox chkSynchroMonitoring;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.RichTextBox txtLog;
        private System.Windows.Forms.Button bntEffacerLog;
        private System.Windows.Forms.Label lblDureeCalcul;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.PictureBox picMap;
    }
}

