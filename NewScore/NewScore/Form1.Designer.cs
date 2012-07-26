namespace NewScore
{
    partial class FrmNewSheetMusic
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
            this.picPart = new System.Windows.Forms.PictureBox();
            this.vScrollBar = new System.Windows.Forms.VScrollBar();
            this.btnOuvrir = new System.Windows.Forms.Button();
            this.trackZom = new System.Windows.Forms.TrackBar();
            this.groupBoxNote = new System.Windows.Forms.GroupBox();
            this.optNombre = new System.Windows.Forms.RadioButton();
            this.optFrancaise = new System.Windows.Forms.RadioButton();
            this.optAmericaine = new System.Windows.Forms.RadioButton();
            this.optAucune = new System.Windows.Forms.RadioButton();
            this.btnPrevMeasure = new System.Windows.Forms.Button();
            this.btnNextMeasure = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picPart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackZom)).BeginInit();
            this.groupBoxNote.SuspendLayout();
            this.SuspendLayout();
            // 
            // picPart
            // 
            this.picPart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.picPart.BackColor = System.Drawing.Color.White;
            this.picPart.Location = new System.Drawing.Point(16, 73);
            this.picPart.Margin = new System.Windows.Forms.Padding(4);
            this.picPart.Name = "picPart";
            this.picPart.Size = new System.Drawing.Size(1110, 605);
            this.picPart.TabIndex = 0;
            this.picPart.TabStop = false;
            this.picPart.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picPart_MouseDown);
            this.picPart.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picPart_MouseMove);
            this.picPart.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picPart_MouseUp);
            // 
            // vScrollBar
            // 
            this.vScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.vScrollBar.Location = new System.Drawing.Point(1131, 73);
            this.vScrollBar.Name = "vScrollBar";
            this.vScrollBar.Size = new System.Drawing.Size(48, 605);
            this.vScrollBar.TabIndex = 1;
            this.vScrollBar.ValueChanged += new System.EventHandler(this.vScrollBar1_ValueChanged);
            // 
            // btnOuvrir
            // 
            this.btnOuvrir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOuvrir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnOuvrir.Location = new System.Drawing.Point(16, 15);
            this.btnOuvrir.Margin = new System.Windows.Forms.Padding(4);
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
            this.trackZom.Margin = new System.Windows.Forms.Padding(4);
            this.trackZom.Maximum = 60;
            this.trackZom.Minimum = 8;
            this.trackZom.Name = "trackZom";
            this.trackZom.Size = new System.Drawing.Size(392, 56);
            this.trackZom.TabIndex = 3;
            this.trackZom.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackZom.Value = 36;
            this.trackZom.Scroll += new System.EventHandler(this.trackZom_Scroll);
            // 
            // groupBoxNote
            // 
            this.groupBoxNote.Controls.Add(this.optNombre);
            this.groupBoxNote.Controls.Add(this.optFrancaise);
            this.groupBoxNote.Controls.Add(this.optAmericaine);
            this.groupBoxNote.Controls.Add(this.optAucune);
            this.groupBoxNote.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBoxNote.ForeColor = System.Drawing.Color.Silver;
            this.groupBoxNote.Location = new System.Drawing.Point(524, 10);
            this.groupBoxNote.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxNote.Name = "groupBoxNote";
            this.groupBoxNote.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxNote.Size = new System.Drawing.Size(402, 55);
            this.groupBoxNote.TabIndex = 4;
            this.groupBoxNote.TabStop = false;
            this.groupBoxNote.Text = "Notes";
            // 
            // optNombre
            // 
            this.optNombre.AutoSize = true;
            this.optNombre.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optNombre.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.optNombre.Location = new System.Drawing.Point(317, 25);
            this.optNombre.Margin = new System.Windows.Forms.Padding(4);
            this.optNombre.Name = "optNombre";
            this.optNombre.Size = new System.Drawing.Size(78, 21);
            this.optNombre.TabIndex = 3;
            this.optNombre.Text = "Nombre";
            this.optNombre.UseVisualStyleBackColor = true;
            this.optNombre.CheckedChanged += new System.EventHandler(this.optAucune_CheckedChanged);
            // 
            // optFrancaise
            // 
            this.optFrancaise.AutoSize = true;
            this.optFrancaise.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optFrancaise.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.optFrancaise.Location = new System.Drawing.Point(215, 23);
            this.optFrancaise.Margin = new System.Windows.Forms.Padding(4);
            this.optFrancaise.Name = "optFrancaise";
            this.optFrancaise.Size = new System.Drawing.Size(90, 21);
            this.optFrancaise.TabIndex = 2;
            this.optFrancaise.Text = "Française";
            this.optFrancaise.UseVisualStyleBackColor = true;
            this.optFrancaise.CheckedChanged += new System.EventHandler(this.optAucune_CheckedChanged);
            // 
            // optAmericaine
            // 
            this.optAmericaine.AutoSize = true;
            this.optAmericaine.Checked = true;
            this.optAmericaine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optAmericaine.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.optAmericaine.Location = new System.Drawing.Point(105, 23);
            this.optAmericaine.Margin = new System.Windows.Forms.Padding(4);
            this.optAmericaine.Name = "optAmericaine";
            this.optAmericaine.Size = new System.Drawing.Size(98, 21);
            this.optAmericaine.TabIndex = 1;
            this.optAmericaine.TabStop = true;
            this.optAmericaine.Text = "Américaine";
            this.optAmericaine.UseVisualStyleBackColor = true;
            this.optAmericaine.CheckedChanged += new System.EventHandler(this.optAucune_CheckedChanged);
            // 
            // optAucune
            // 
            this.optAucune.AutoSize = true;
            this.optAucune.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optAucune.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.optAucune.Location = new System.Drawing.Point(9, 25);
            this.optAucune.Margin = new System.Windows.Forms.Padding(4);
            this.optAucune.Name = "optAucune";
            this.optAucune.Size = new System.Drawing.Size(76, 21);
            this.optAucune.TabIndex = 0;
            this.optAucune.Text = "Aucune";
            this.optAucune.UseVisualStyleBackColor = true;
            this.optAucune.CheckedChanged += new System.EventHandler(this.optAucune_CheckedChanged);
            // 
            // btnPrevMeasure
            // 
            this.btnPrevMeasure.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrevMeasure.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrevMeasure.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnPrevMeasure.Location = new System.Drawing.Point(944, 15);
            this.btnPrevMeasure.Margin = new System.Windows.Forms.Padding(4);
            this.btnPrevMeasure.Name = "btnPrevMeasure";
            this.btnPrevMeasure.Size = new System.Drawing.Size(87, 50);
            this.btnPrevMeasure.TabIndex = 5;
            this.btnPrevMeasure.Text = "<<";
            this.btnPrevMeasure.UseVisualStyleBackColor = true;
            this.btnPrevMeasure.Click += new System.EventHandler(this.btnPrevMeasure_Click);
            // 
            // btnNextMeasure
            // 
            this.btnNextMeasure.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNextMeasure.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNextMeasure.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnNextMeasure.Location = new System.Drawing.Point(1039, 15);
            this.btnNextMeasure.Margin = new System.Windows.Forms.Padding(4);
            this.btnNextMeasure.Name = "btnNextMeasure";
            this.btnNextMeasure.Size = new System.Drawing.Size(87, 50);
            this.btnNextMeasure.TabIndex = 6;
            this.btnNextMeasure.Text = ">>";
            this.btnNextMeasure.UseVisualStyleBackColor = true;
            this.btnNextMeasure.Click += new System.EventHandler(this.btnNextMeasure_Click);
            // 
            // FrmNewSheetMusic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.ClientSize = new System.Drawing.Size(1188, 687);
            this.Controls.Add(this.btnNextMeasure);
            this.Controls.Add(this.btnPrevMeasure);
            this.Controls.Add(this.groupBoxNote);
            this.Controls.Add(this.trackZom);
            this.Controls.Add(this.btnOuvrir);
            this.Controls.Add(this.vScrollBar);
            this.Controls.Add(this.picPart);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmNewSheetMusic";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "New Sheet Music Viewer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.picPart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackZom)).EndInit();
            this.groupBoxNote.ResumeLayout(false);
            this.groupBoxNote.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picPart;
        private System.Windows.Forms.VScrollBar vScrollBar;
        private System.Windows.Forms.Button btnOuvrir;
        private System.Windows.Forms.TrackBar trackZom;
        private System.Windows.Forms.GroupBox groupBoxNote;
        private System.Windows.Forms.RadioButton optFrancaise;
        private System.Windows.Forms.RadioButton optAmericaine;
        private System.Windows.Forms.RadioButton optAucune;
        private System.Windows.Forms.RadioButton optNombre;
        private System.Windows.Forms.Button btnPrevMeasure;
        private System.Windows.Forms.Button btnNextMeasure;
    }
}

