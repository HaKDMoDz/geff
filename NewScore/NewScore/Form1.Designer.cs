namespace NewScore
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.btnOuvrir = new System.Windows.Forms.Button();
            this.trackZom = new System.Windows.Forms.TrackBar();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.optNombre = new System.Windows.Forms.RadioButton();
            this.optFrancaise = new System.Windows.Forms.RadioButton();
            this.optAmericaine = new System.Windows.Forms.RadioButton();
            this.optAucune = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackZom)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Location = new System.Drawing.Point(16, 73);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1010, 949);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.vScrollBar1.Location = new System.Drawing.Point(1030, 73);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(48, 949);
            this.vScrollBar1.TabIndex = 1;
            this.vScrollBar1.ValueChanged += new System.EventHandler(this.vScrollBar1_ValueChanged);
            // 
            // btnOuvrir
            // 
            this.btnOuvrir.Location = new System.Drawing.Point(16, 15);
            this.btnOuvrir.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOuvrir.Name = "btnOuvrir";
            this.btnOuvrir.Size = new System.Drawing.Size(100, 50);
            this.btnOuvrir.TabIndex = 2;
            this.btnOuvrir.Text = "&Ouvrir";
            this.btnOuvrir.UseVisualStyleBackColor = true;
            this.btnOuvrir.Click += new System.EventHandler(this.btnOuvrir_Click);
            // 
            // trackZom
            // 
            this.trackZom.Location = new System.Drawing.Point(124, 15);
            this.trackZom.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.trackZom.Maximum = 60;
            this.trackZom.Minimum = 8;
            this.trackZom.Name = "trackZom";
            this.trackZom.Size = new System.Drawing.Size(392, 56);
            this.trackZom.TabIndex = 3;
            this.trackZom.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackZom.Value = 36;
            this.trackZom.Scroll += new System.EventHandler(this.trackZom_Scroll);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.optNombre);
            this.groupBox1.Controls.Add(this.optFrancaise);
            this.groupBox1.Controls.Add(this.optAmericaine);
            this.groupBox1.Controls.Add(this.optAucune);
            this.groupBox1.Location = new System.Drawing.Point(524, 15);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(552, 55);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Notes";
            // 
            // optNombre
            // 
            this.optNombre.AutoSize = true;
            this.optNombre.Location = new System.Drawing.Point(317, 25);
            this.optNombre.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.optNombre.Name = "optNombre";
            this.optNombre.Size = new System.Drawing.Size(79, 21);
            this.optNombre.TabIndex = 3;
            this.optNombre.Text = "Nombre";
            this.optNombre.UseVisualStyleBackColor = true;
            this.optNombre.CheckedChanged += new System.EventHandler(this.optAucune_CheckedChanged);
            // 
            // optFrancaise
            // 
            this.optFrancaise.AutoSize = true;
            this.optFrancaise.Location = new System.Drawing.Point(215, 23);
            this.optFrancaise.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.optFrancaise.Name = "optFrancaise";
            this.optFrancaise.Size = new System.Drawing.Size(91, 21);
            this.optFrancaise.TabIndex = 2;
            this.optFrancaise.Text = "Française";
            this.optFrancaise.UseVisualStyleBackColor = true;
            this.optFrancaise.CheckedChanged += new System.EventHandler(this.optAucune_CheckedChanged);
            // 
            // optAmericaine
            // 
            this.optAmericaine.AutoSize = true;
            this.optAmericaine.Checked = true;
            this.optAmericaine.Location = new System.Drawing.Point(105, 23);
            this.optAmericaine.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.optAmericaine.Name = "optAmericaine";
            this.optAmericaine.Size = new System.Drawing.Size(99, 21);
            this.optAmericaine.TabIndex = 1;
            this.optAmericaine.TabStop = true;
            this.optAmericaine.Text = "Américaine";
            this.optAmericaine.UseVisualStyleBackColor = true;
            this.optAmericaine.CheckedChanged += new System.EventHandler(this.optAucune_CheckedChanged);
            // 
            // optAucune
            // 
            this.optAucune.AutoSize = true;
            this.optAucune.Location = new System.Drawing.Point(9, 25);
            this.optAucune.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.optAucune.Name = "optAucune";
            this.optAucune.Size = new System.Drawing.Size(77, 21);
            this.optAucune.TabIndex = 0;
            this.optAucune.Text = "Aucune";
            this.optAucune.UseVisualStyleBackColor = true;
            this.optAucune.CheckedChanged += new System.EventHandler(this.optAucune_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1088, 687);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.trackZom);
            this.Controls.Add(this.btnOuvrir);
            this.Controls.Add(this.vScrollBar1);
            this.Controls.Add(this.pictureBox1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "New Sheet Music Viewer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackZom)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.Button btnOuvrir;
        private System.Windows.Forms.TrackBar trackZom;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton optFrancaise;
        private System.Windows.Forms.RadioButton optAmericaine;
        private System.Windows.Forms.RadioButton optAucune;
        private System.Windows.Forms.RadioButton optNombre;
    }
}

