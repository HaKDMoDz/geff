namespace CubEat
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
            this.components = new System.ComponentModel.Container();
            this.pictureBoxNextSample = new System.Windows.Forms.PictureBox();
            this.pictureBoxMap = new System.Windows.Forms.PictureBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.comboBoxLibrary = new System.Windows.Forms.ComboBox();
            this.trackBar = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxNextSample)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxNextSample
            // 
            this.pictureBoxNextSample.Location = new System.Drawing.Point(12, 12);
            this.pictureBoxNextSample.Name = "pictureBoxNextSample";
            this.pictureBoxNextSample.Size = new System.Drawing.Size(211, 52);
            this.pictureBoxNextSample.TabIndex = 0;
            this.pictureBoxNextSample.TabStop = false;
            // 
            // pictureBoxMap
            // 
            this.pictureBoxMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxMap.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxMap.Name = "pictureBoxMap";
            this.pictureBoxMap.Size = new System.Drawing.Size(545, 239);
            this.pictureBoxMap.TabIndex = 1;
            this.pictureBoxMap.TabStop = false;
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 50;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(368, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "New rythm";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(449, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Stop Sound";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // comboBoxLibrary
            // 
            this.comboBoxLibrary.FormattingEnabled = true;
            this.comboBoxLibrary.Location = new System.Drawing.Point(188, 12);
            this.comboBoxLibrary.Name = "comboBoxLibrary";
            this.comboBoxLibrary.Size = new System.Drawing.Size(174, 21);
            this.comboBoxLibrary.TabIndex = 4;
            // 
            // trackBar
            // 
            this.trackBar.LargeChange = 100;
            this.trackBar.Location = new System.Drawing.Point(8, 12);
            this.trackBar.Maximum = 1000;
            this.trackBar.Minimum = 50;
            this.trackBar.Name = "trackBar";
            this.trackBar.Size = new System.Drawing.Size(185, 45);
            this.trackBar.TabIndex = 5;
            this.trackBar.TickFrequency = 100;
            this.trackBar.Value = 50;
            this.trackBar.Scroll += new System.EventHandler(this.trackBar_Scroll);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(545, 239);
            this.Controls.Add(this.trackBar);
            this.Controls.Add(this.comboBoxLibrary);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBoxMap);
            this.Controls.Add(this.pictureBoxNextSample);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxNextSample)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxNextSample;
        private System.Windows.Forms.PictureBox pictureBoxMap;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox comboBoxLibrary;
        private System.Windows.Forms.TrackBar trackBar;
    }
}

