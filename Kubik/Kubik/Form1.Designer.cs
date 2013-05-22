namespace Kubik
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.txtSrc = new System.Windows.Forms.TextBox();
            this.btnGo = new System.Windows.Forms.Button();
            this.txtDst = new System.Windows.Forms.TextBox();
            this.txtSens = new System.Windows.Forms.TextBox();
            this.pic = new System.Windows.Forms.PictureBox();
            this.pnl = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pic)).BeginInit();
            this.pnl.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSrc
            // 
            this.txtSrc.Location = new System.Drawing.Point(12, 12);
            this.txtSrc.Multiline = true;
            this.txtSrc.Name = "txtSrc";
            this.txtSrc.Size = new System.Drawing.Size(99, 507);
            this.txtSrc.TabIndex = 0;
            this.txtSrc.Text = resources.GetString("txtSrc.Text");
            this.txtSrc.TextChanged += new System.EventHandler(this.txtSrc_TextChanged);
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(117, 12);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(75, 23);
            this.btnGo.TabIndex = 2;
            this.btnGo.Text = "Go";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // txtDst
            // 
            this.txtDst.Location = new System.Drawing.Point(198, 12);
            this.txtDst.Multiline = true;
            this.txtDst.Name = "txtDst";
            this.txtDst.Size = new System.Drawing.Size(178, 507);
            this.txtDst.TabIndex = 3;
            this.txtDst.Text = "\r\n";
            // 
            // txtSens
            // 
            this.txtSens.Location = new System.Drawing.Point(387, 15);
            this.txtSens.Multiline = true;
            this.txtSens.Name = "txtSens";
            this.txtSens.Size = new System.Drawing.Size(75, 507);
            this.txtSens.TabIndex = 4;
            this.txtSens.Text = "\r\n";
            // 
            // pic
            // 
            this.pic.BackColor = System.Drawing.Color.White;
            this.pic.Location = new System.Drawing.Point(3, 3);
            this.pic.Name = "pic";
            this.pic.Size = new System.Drawing.Size(517, 654);
            this.pic.TabIndex = 5;
            this.pic.TabStop = false;
            // 
            // pnl
            // 
            this.pnl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnl.AutoScroll = true;
            this.pnl.Controls.Add(this.pic);
            this.pnl.Location = new System.Drawing.Point(468, 15);
            this.pnl.Name = "pnl";
            this.pnl.Size = new System.Drawing.Size(564, 504);
            this.pnl.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1044, 531);
            this.Controls.Add(this.pnl);
            this.Controls.Add(this.txtSens);
            this.Controls.Add(this.txtDst);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.txtSrc);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pic)).EndInit();
            this.pnl.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSrc;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.TextBox txtDst;
        private System.Windows.Forms.TextBox txtSens;
        private System.Windows.Forms.PictureBox pic;
        private System.Windows.Forms.Panel pnl;
    }
}

