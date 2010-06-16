namespace ThreadAStar
{
    partial class Form1
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
            this.btnDemarrer = new System.Windows.Forms.Button();
            this.pnlMethode = new System.Windows.Forms.Panel();
            this.pnlParametrage = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.chkMethodeNative = new System.Windows.Forms.CheckBox();
            this.chkMethodeBackgroundWorker = new System.Windows.Forms.CheckBox();
            this.chkMethodeNativeDotNet4 = new System.Windows.Forms.CheckBox();
            this.btnShowMapResolving = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.numericUpDown4 = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.numericUpDown5 = new System.Windows.Forms.NumericUpDown();
            this.chkUtiliserGraine = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.numericUpDown6 = new System.Windows.Forms.NumericUpDown();
            this.ucMonitoring = new ThreadAStar.UC.UCMonitoring();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.pnlMethode.SuspendLayout();
            this.pnlParametrage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown6)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDemarrer
            // 
            this.btnDemarrer.BackColor = System.Drawing.Color.Gray;
            this.btnDemarrer.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnDemarrer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDemarrer.Location = new System.Drawing.Point(362, 242);
            this.btnDemarrer.Name = "btnDemarrer";
            this.btnDemarrer.Size = new System.Drawing.Size(75, 23);
            this.btnDemarrer.TabIndex = 0;
            this.btnDemarrer.Text = "Démarrer";
            this.btnDemarrer.UseVisualStyleBackColor = false;
            this.btnDemarrer.Click += new System.EventHandler(this.button1_Click);
            // 
            // pnlMethode
            // 
            this.pnlMethode.BackColor = System.Drawing.Color.DimGray;
            this.pnlMethode.Controls.Add(this.chkMethodeNativeDotNet4);
            this.pnlMethode.Controls.Add(this.chkMethodeBackgroundWorker);
            this.pnlMethode.Controls.Add(this.chkMethodeNative);
            this.pnlMethode.Controls.Add(this.label1);
            this.pnlMethode.Location = new System.Drawing.Point(13, 30);
            this.pnlMethode.Name = "pnlMethode";
            this.pnlMethode.Size = new System.Drawing.Size(200, 206);
            this.pnlMethode.TabIndex = 2;
            // 
            // pnlParametrage
            // 
            this.pnlParametrage.BackColor = System.Drawing.Color.DimGray;
            this.pnlParametrage.Controls.Add(this.label9);
            this.pnlParametrage.Controls.Add(this.numericUpDown6);
            this.pnlParametrage.Controls.Add(this.chkUtiliserGraine);
            this.pnlParametrage.Controls.Add(this.label8);
            this.pnlParametrage.Controls.Add(this.numericUpDown5);
            this.pnlParametrage.Controls.Add(this.label7);
            this.pnlParametrage.Controls.Add(this.numericUpDown4);
            this.pnlParametrage.Controls.Add(this.panel1);
            this.pnlParametrage.Controls.Add(this.label6);
            this.pnlParametrage.Controls.Add(this.numericUpDown3);
            this.pnlParametrage.Controls.Add(this.label5);
            this.pnlParametrage.Controls.Add(this.numericUpDown2);
            this.pnlParametrage.Controls.Add(this.label4);
            this.pnlParametrage.Controls.Add(this.numericUpDown1);
            this.pnlParametrage.Controls.Add(this.label2);
            this.pnlParametrage.Location = new System.Drawing.Point(219, 30);
            this.pnlParametrage.Name = "pnlParametrage";
            this.pnlParametrage.Size = new System.Drawing.Size(342, 206);
            this.pnlParametrage.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.CornflowerBlue;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(-1, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(201, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Méthodes de parallélisation";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.CornflowerBlue;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(342, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Paramétrage";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.CornflowerBlue;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(12, 271);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(549, 23);
            this.label3.TabIndex = 4;
            this.label3.Text = "Surveillance du process";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkMethodeNative
            // 
            this.chkMethodeNative.AutoSize = true;
            this.chkMethodeNative.ForeColor = System.Drawing.Color.White;
            this.chkMethodeNative.Location = new System.Drawing.Point(14, 46);
            this.chkMethodeNative.Name = "chkMethodeNative";
            this.chkMethodeNative.Size = new System.Drawing.Size(57, 17);
            this.chkMethodeNative.TabIndex = 1;
            this.chkMethodeNative.Text = "Native";
            this.chkMethodeNative.UseVisualStyleBackColor = true;
            this.chkMethodeNative.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
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
            this.chkMethodeBackgroundWorker.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // chkMethodeNativeDotNet4
            // 
            this.chkMethodeNativeDotNet4.AutoSize = true;
            this.chkMethodeNativeDotNet4.ForeColor = System.Drawing.Color.White;
            this.chkMethodeNativeDotNet4.Location = new System.Drawing.Point(14, 94);
            this.chkMethodeNativeDotNet4.Name = "chkMethodeNativeDotNet4";
            this.chkMethodeNativeDotNet4.Size = new System.Drawing.Size(89, 17);
            this.chkMethodeNativeDotNet4.TabIndex = 3;
            this.chkMethodeNativeDotNet4.Text = "Native .Net 4";
            this.chkMethodeNativeDotNet4.UseVisualStyleBackColor = true;
            this.chkMethodeNativeDotNet4.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // btnShowMapResolving
            // 
            this.btnShowMapResolving.BackColor = System.Drawing.Color.Gray;
            this.btnShowMapResolving.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnShowMapResolving.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowMapResolving.Location = new System.Drawing.Point(443, 242);
            this.btnShowMapResolving.Name = "btnShowMapResolving";
            this.btnShowMapResolving.Size = new System.Drawing.Size(118, 23);
            this.btnShowMapResolving.TabIndex = 6;
            this.btnShowMapResolving.Text = "Afficher les maps";
            this.btnShowMapResolving.UseVisualStyleBackColor = false;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.BackColor = System.Drawing.Color.DarkGray;
            this.numericUpDown1.Location = new System.Drawing.Point(6, 56);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(150, 20);
            this.numericUpDown1.TabIndex = 2;
            this.numericUpDown1.Value = new decimal(new int[] {
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
            // numericUpDown2
            // 
            this.numericUpDown2.BackColor = System.Drawing.Color.DarkGray;
            this.numericUpDown2.Location = new System.Drawing.Point(172, 56);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.numericUpDown2.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(150, 20);
            this.numericUpDown2.TabIndex = 4;
            this.numericUpDown2.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
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
            // numericUpDown3
            // 
            this.numericUpDown3.BackColor = System.Drawing.Color.DarkGray;
            this.numericUpDown3.Location = new System.Drawing.Point(6, 111);
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(150, 20);
            this.numericUpDown3.TabIndex = 6;
            this.numericUpDown3.Value = new decimal(new int[] {
            10,
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
            // numericUpDown4
            // 
            this.numericUpDown4.BackColor = System.Drawing.Color.DarkGray;
            this.numericUpDown4.Location = new System.Drawing.Point(6, 153);
            this.numericUpDown4.Name = "numericUpDown4";
            this.numericUpDown4.Size = new System.Drawing.Size(150, 20);
            this.numericUpDown4.TabIndex = 9;
            this.numericUpDown4.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
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
            // numericUpDown5
            // 
            this.numericUpDown5.BackColor = System.Drawing.Color.DarkGray;
            this.numericUpDown5.Enabled = false;
            this.numericUpDown5.Location = new System.Drawing.Point(193, 111);
            this.numericUpDown5.Name = "numericUpDown5";
            this.numericUpDown5.Size = new System.Drawing.Size(129, 20);
            this.numericUpDown5.TabIndex = 11;
            this.numericUpDown5.Value = new decimal(new int[] {
            10,
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
            // numericUpDown6
            // 
            this.numericUpDown6.BackColor = System.Drawing.Color.DarkGray;
            this.numericUpDown6.Location = new System.Drawing.Point(175, 153);
            this.numericUpDown6.Name = "numericUpDown6";
            this.numericUpDown6.Size = new System.Drawing.Size(150, 20);
            this.numericUpDown6.TabIndex = 14;
            this.numericUpDown6.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // ucMonitoring
            // 
            this.ucMonitoring.BackColor = System.Drawing.Color.DimGray;
            this.ucMonitoring.Location = new System.Drawing.Point(12, 294);
            this.ucMonitoring.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.ucMonitoring.Name = "ucMonitoring";
            this.ucMonitoring.Size = new System.Drawing.Size(549, 236);
            this.ucMonitoring.TabIndex = 5;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(573, 25);
            this.toolStrip1.TabIndex = 7;
            this.toolStrip1.Text = "toolStrip";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 538);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(573, 22);
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(573, 560);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.btnShowMapResolving);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnDemarrer);
            this.Controls.Add(this.ucMonitoring);
            this.Controls.Add(this.pnlParametrage);
            this.Controls.Add(this.pnlMethode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Parallélisation et Algorithme A*";
            this.pnlMethode.ResumeLayout(false);
            this.pnlMethode.PerformLayout();
            this.pnlParametrage.ResumeLayout(false);
            this.pnlParametrage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown6)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDemarrer;
        private System.Windows.Forms.Panel pnlMethode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlParametrage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private ThreadAStar.UC.UCMonitoring ucMonitoring;
        private System.Windows.Forms.CheckBox chkMethodeNativeDotNet4;
        private System.Windows.Forms.CheckBox chkMethodeBackgroundWorker;
        private System.Windows.Forms.CheckBox chkMethodeNative;
        private System.Windows.Forms.Button btnShowMapResolving;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown numericUpDown6;
        private System.Windows.Forms.CheckBox chkUtiliserGraine;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numericUpDown5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numericUpDown4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
    }
}

