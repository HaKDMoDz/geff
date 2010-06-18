namespace ThreadAStar.UC
{
    partial class UCMonitoring
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

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.chkCPU = new System.Windows.Forms.CheckBox();
            this.chkRAM = new System.Windows.Forms.CheckBox();
            this.chkThreadNew = new System.Windows.Forms.CheckBox();
            this.chkThreadDead = new System.Windows.Forms.CheckBox();
            this.chkThreadTotal = new System.Windows.Forms.CheckBox();
            this.lblTotalThread = new ThreadAStar.UC.ThreadSafeLabel();
            this.lblDeadThread = new ThreadAStar.UC.ThreadSafeLabel();
            this.lblNewThread = new ThreadAStar.UC.ThreadSafeLabel();
            this.lblRAM = new ThreadAStar.UC.ThreadSafeLabel();
            this.lblCPU = new ThreadAStar.UC.ThreadSafeLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox.BackColor = System.Drawing.Color.Gray;
            this.pictureBox.Location = new System.Drawing.Point(89, 0);
            this.pictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(688, 426);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // chkCPU
            // 
            this.chkCPU.AutoSize = true;
            this.chkCPU.Checked = true;
            this.chkCPU.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCPU.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkCPU.ForeColor = System.Drawing.Color.White;
            this.chkCPU.Location = new System.Drawing.Point(6, 3);
            this.chkCPU.Name = "chkCPU";
            this.chkCPU.Size = new System.Drawing.Size(45, 17);
            this.chkCPU.TabIndex = 1;
            this.chkCPU.Text = "CPU";
            this.chkCPU.UseVisualStyleBackColor = true;
            // 
            // chkRAM
            // 
            this.chkRAM.AutoSize = true;
            this.chkRAM.Checked = true;
            this.chkRAM.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRAM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkRAM.ForeColor = System.Drawing.Color.White;
            this.chkRAM.Location = new System.Drawing.Point(6, 61);
            this.chkRAM.Name = "chkRAM";
            this.chkRAM.Size = new System.Drawing.Size(47, 17);
            this.chkRAM.TabIndex = 2;
            this.chkRAM.Text = "RAM";
            this.chkRAM.UseVisualStyleBackColor = true;
            // 
            // chkThreadNew
            // 
            this.chkThreadNew.AutoSize = true;
            this.chkThreadNew.Checked = true;
            this.chkThreadNew.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkThreadNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkThreadNew.ForeColor = System.Drawing.Color.White;
            this.chkThreadNew.Location = new System.Drawing.Point(6, 116);
            this.chkThreadNew.Name = "chkThreadNew";
            this.chkThreadNew.Size = new System.Drawing.Size(66, 17);
            this.chkThreadNew.TabIndex = 3;
            this.chkThreadNew.Text = "Thread +";
            this.chkThreadNew.UseVisualStyleBackColor = true;
            // 
            // chkThreadDead
            // 
            this.chkThreadDead.AutoSize = true;
            this.chkThreadDead.Checked = true;
            this.chkThreadDead.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkThreadDead.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkThreadDead.ForeColor = System.Drawing.Color.White;
            this.chkThreadDead.Location = new System.Drawing.Point(6, 175);
            this.chkThreadDead.Name = "chkThreadDead";
            this.chkThreadDead.Size = new System.Drawing.Size(63, 17);
            this.chkThreadDead.TabIndex = 4;
            this.chkThreadDead.Text = "Thread -";
            this.chkThreadDead.UseVisualStyleBackColor = true;
            // 
            // chkThreadTotal
            // 
            this.chkThreadTotal.AutoSize = true;
            this.chkThreadTotal.Checked = true;
            this.chkThreadTotal.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkThreadTotal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkThreadTotal.ForeColor = System.Drawing.Color.White;
            this.chkThreadTotal.Location = new System.Drawing.Point(6, 233);
            this.chkThreadTotal.Name = "chkThreadTotal";
            this.chkThreadTotal.Size = new System.Drawing.Size(80, 17);
            this.chkThreadTotal.TabIndex = 5;
            this.chkThreadTotal.Text = "Thread total";
            this.chkThreadTotal.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkThreadTotal.UseVisualStyleBackColor = true;
            // 
            // lblTotalThread
            // 
            this.lblTotalThread.AutoSize = true;
            this.lblTotalThread.ForeColor = System.Drawing.Color.Silver;
            this.lblTotalThread.Location = new System.Drawing.Point(3, 253);
            this.lblTotalThread.Name = "lblTotalThread";
            this.lblTotalThread.Size = new System.Drawing.Size(56, 26);
            this.lblTotalThread.TabIndex = 10;
            this.lblTotalThread.Text = "Max : 95%\r\nCur : 90%";
            // 
            // lblDeadThread
            // 
            this.lblDeadThread.AutoSize = true;
            this.lblDeadThread.ForeColor = System.Drawing.Color.Silver;
            this.lblDeadThread.Location = new System.Drawing.Point(3, 195);
            this.lblDeadThread.Name = "lblDeadThread";
            this.lblDeadThread.Size = new System.Drawing.Size(56, 26);
            this.lblDeadThread.TabIndex = 9;
            this.lblDeadThread.Text = "Max : 95%\r\nCur : 90%";
            // 
            // lblNewThread
            // 
            this.lblNewThread.AutoSize = true;
            this.lblNewThread.ForeColor = System.Drawing.Color.Silver;
            this.lblNewThread.Location = new System.Drawing.Point(3, 136);
            this.lblNewThread.Name = "lblNewThread";
            this.lblNewThread.Size = new System.Drawing.Size(56, 26);
            this.lblNewThread.TabIndex = 8;
            this.lblNewThread.Text = "Max : 95%\r\nCur : 90%";
            // 
            // lblRAM
            // 
            this.lblRAM.AutoSize = true;
            this.lblRAM.ForeColor = System.Drawing.Color.Silver;
            this.lblRAM.Location = new System.Drawing.Point(3, 81);
            this.lblRAM.Name = "lblRAM";
            this.lblRAM.Size = new System.Drawing.Size(56, 26);
            this.lblRAM.TabIndex = 7;
            this.lblRAM.Text = "Max : 95%\r\nCur : 90%";
            // 
            // lblCPU
            // 
            this.lblCPU.AutoSize = true;
            this.lblCPU.ForeColor = System.Drawing.Color.Silver;
            this.lblCPU.Location = new System.Drawing.Point(3, 23);
            this.lblCPU.Name = "lblCPU";
            this.lblCPU.Size = new System.Drawing.Size(56, 26);
            this.lblCPU.TabIndex = 6;
            this.lblCPU.Text = "Max : 95%\r\nCur : 90%";
            // 
            // UCMonitoring
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Controls.Add(this.lblTotalThread);
            this.Controls.Add(this.lblDeadThread);
            this.Controls.Add(this.lblNewThread);
            this.Controls.Add(this.lblRAM);
            this.Controls.Add(this.lblCPU);
            this.Controls.Add(this.chkThreadTotal);
            this.Controls.Add(this.chkThreadDead);
            this.Controls.Add(this.chkThreadNew);
            this.Controls.Add(this.chkRAM);
            this.Controls.Add(this.chkCPU);
            this.Controls.Add(this.pictureBox);
            this.Name = "UCMonitoring";
            this.Size = new System.Drawing.Size(777, 426);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.CheckBox chkCPU;
        private System.Windows.Forms.CheckBox chkRAM;
        private System.Windows.Forms.CheckBox chkThreadNew;
        private System.Windows.Forms.CheckBox chkThreadDead;
        private System.Windows.Forms.CheckBox chkThreadTotal;
        private ThreadSafeLabel lblCPU;
        private ThreadSafeLabel lblRAM;
        private ThreadSafeLabel lblNewThread;
        private ThreadSafeLabel lblDeadThread;
        private ThreadSafeLabel lblTotalThread;
    }
}
