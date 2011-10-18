namespace InitAgent
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
            this.label1 = new System.Windows.Forms.Label();
            this.numRP = new System.Windows.Forms.NumericUpDown();
            this.numRPr = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numMA = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numBA = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.numRH = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.numVT = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.numVC = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.numDI = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.numRPs = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.numRPt = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.numCA = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.numRM = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.numRU = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.numWe = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.btnRépartir = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txt = new System.Windows.Forms.RichTextBox();
            this.calendar = new System.Windows.Forms.MonthCalendar();
            this.txtMessage = new System.Windows.Forms.RichTextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.numRP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRPr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDI)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRPs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRPt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRU)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWe)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(22, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "RP";
            // 
            // numRP
            // 
            this.numRP.Location = new System.Drawing.Point(57, 14);
            this.numRP.Name = "numRP";
            this.numRP.Size = new System.Drawing.Size(74, 20);
            this.numRP.TabIndex = 1;
            this.numRP.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.numRP.ValueChanged += new System.EventHandler(this.numRP_ValueChanged);
            // 
            // numRPr
            // 
            this.numRPr.Location = new System.Drawing.Point(57, 40);
            this.numRPr.Name = "numRPr";
            this.numRPr.Size = new System.Drawing.Size(74, 20);
            this.numRPr.TabIndex = 3;
            this.numRPr.ValueChanged += new System.EventHandler(this.numRP_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "RP réduit";
            // 
            // numMA
            // 
            this.numMA.Location = new System.Drawing.Point(57, 66);
            this.numMA.Name = "numMA";
            this.numMA.Size = new System.Drawing.Size(74, 20);
            this.numMA.TabIndex = 5;
            this.numMA.ValueChanged += new System.EventHandler(this.numRP_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "MA";
            // 
            // numBA
            // 
            this.numBA.Location = new System.Drawing.Point(57, 92);
            this.numBA.Name = "numBA";
            this.numBA.Size = new System.Drawing.Size(74, 20);
            this.numBA.TabIndex = 7;
            this.numBA.ValueChanged += new System.EventHandler(this.numRP_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(21, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "BA";
            // 
            // numRH
            // 
            this.numRH.Location = new System.Drawing.Point(57, 118);
            this.numRH.Name = "numRH";
            this.numRH.Size = new System.Drawing.Size(74, 20);
            this.numRH.TabIndex = 9;
            this.numRH.ValueChanged += new System.EventHandler(this.numRP_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 120);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "RH";
            // 
            // numVT
            // 
            this.numVT.Location = new System.Drawing.Point(57, 144);
            this.numVT.Name = "numVT";
            this.numVT.Size = new System.Drawing.Size(74, 20);
            this.numVT.TabIndex = 11;
            this.numVT.ValueChanged += new System.EventHandler(this.numRP_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 146);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(21, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "VT";
            // 
            // numVC
            // 
            this.numVC.Location = new System.Drawing.Point(57, 170);
            this.numVC.Name = "numVC";
            this.numVC.Size = new System.Drawing.Size(74, 20);
            this.numVC.TabIndex = 13;
            this.numVC.ValueChanged += new System.EventHandler(this.numRP_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 172);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(21, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "VC";
            // 
            // numDI
            // 
            this.numDI.Location = new System.Drawing.Point(57, 196);
            this.numDI.Name = "numDI";
            this.numDI.Size = new System.Drawing.Size(74, 20);
            this.numDI.TabIndex = 15;
            this.numDI.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numDI.ValueChanged += new System.EventHandler(this.numRP_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 198);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(18, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "DI";
            // 
            // numRPs
            // 
            this.numRPs.Location = new System.Drawing.Point(192, 14);
            this.numRPs.Name = "numRPs";
            this.numRPs.Size = new System.Drawing.Size(74, 20);
            this.numRPs.TabIndex = 17;
            this.numRPs.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numRPs.ValueChanged += new System.EventHandler(this.numRP_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(141, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(27, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "RPs";
            // 
            // numRPt
            // 
            this.numRPt.Location = new System.Drawing.Point(192, 40);
            this.numRPt.Name = "numRPt";
            this.numRPt.Size = new System.Drawing.Size(74, 20);
            this.numRPt.TabIndex = 19;
            this.numRPt.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numRPt.ValueChanged += new System.EventHandler(this.numRP_ValueChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(141, 42);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(25, 13);
            this.label10.TabIndex = 18;
            this.label10.Text = "RPt";
            // 
            // numCA
            // 
            this.numCA.Location = new System.Drawing.Point(192, 66);
            this.numCA.Name = "numCA";
            this.numCA.Size = new System.Drawing.Size(74, 20);
            this.numCA.TabIndex = 21;
            this.numCA.ValueChanged += new System.EventHandler(this.numRP_ValueChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(141, 68);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(21, 13);
            this.label11.TabIndex = 20;
            this.label11.Text = "CA";
            // 
            // numRM
            // 
            this.numRM.Location = new System.Drawing.Point(192, 92);
            this.numRM.Name = "numRM";
            this.numRM.Size = new System.Drawing.Size(74, 20);
            this.numRM.TabIndex = 23;
            this.numRM.ValueChanged += new System.EventHandler(this.numRP_ValueChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(141, 94);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(24, 13);
            this.label12.TabIndex = 22;
            this.label12.Text = "RM";
            // 
            // numRU
            // 
            this.numRU.Location = new System.Drawing.Point(192, 120);
            this.numRU.Name = "numRU";
            this.numRU.Size = new System.Drawing.Size(74, 20);
            this.numRU.TabIndex = 25;
            this.numRU.ValueChanged += new System.EventHandler(this.numRP_ValueChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(141, 122);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(23, 13);
            this.label13.TabIndex = 24;
            this.label13.Text = "RU";
            // 
            // numWe
            // 
            this.numWe.Location = new System.Drawing.Point(192, 196);
            this.numWe.Name = "numWe";
            this.numWe.Size = new System.Drawing.Size(74, 20);
            this.numWe.TabIndex = 27;
            this.numWe.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numWe.ValueChanged += new System.EventHandler(this.numRP_ValueChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(141, 198);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(25, 13);
            this.label14.TabIndex = 26;
            this.label14.Text = "WE";
            // 
            // btnRépartir
            // 
            this.btnRépartir.Location = new System.Drawing.Point(302, 174);
            this.btnRépartir.Name = "btnRépartir";
            this.btnRépartir.Size = new System.Drawing.Size(75, 23);
            this.btnRépartir.TabIndex = 28;
            this.btnRépartir.Text = "&Répartir";
            this.btnRépartir.UseVisualStyleBackColor = true;
            this.btnRépartir.Click += new System.EventHandler(this.btnRépartir_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.numRP);
            this.groupBox1.Controls.Add(this.numWe);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.numRPr);
            this.groupBox1.Controls.Add(this.numRU);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.numMA);
            this.groupBox1.Controls.Add(this.numRM);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.numBA);
            this.groupBox1.Controls.Add(this.numCA);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.numRH);
            this.groupBox1.Controls.Add(this.numRPt);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.numVT);
            this.groupBox1.Controls.Add(this.numRPs);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.numVC);
            this.groupBox1.Controls.Add(this.numDI);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(278, 229);
            this.groupBox1.TabIndex = 29;
            this.groupBox1.TabStop = false;
            // 
            // txt
            // 
            this.txt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt.Location = new System.Drawing.Point(0, 0);
            this.txt.Name = "txt";
            this.txt.Size = new System.Drawing.Size(534, 361);
            this.txt.TabIndex = 30;
            this.txt.Text = "";
            // 
            // calendar
            // 
            this.calendar.Location = new System.Drawing.Point(302, 12);
            this.calendar.Name = "calendar";
            this.calendar.TabIndex = 31;
            this.calendar.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.calendar_DateChanged);
            // 
            // txtMessage
            // 
            this.txtMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMessage.Location = new System.Drawing.Point(0, 0);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(534, 108);
            this.txtMessage.TabIndex = 32;
            this.txtMessage.Text = "";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(12, 247);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtMessage);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txt);
            this.splitContainer1.Size = new System.Drawing.Size(534, 473);
            this.splitContainer1.SplitterDistance = 108;
            this.splitContainer1.TabIndex = 33;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 732);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.calendar);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnRépartir);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Répartition des absences";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numRP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRPr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDI)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRPs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRPt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRU)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWe)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numRP;
        private System.Windows.Forms.NumericUpDown numRPr;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numMA;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numBA;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numRH;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numVT;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numVC;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numDI;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numRPs;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown numRPt;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown numCA;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown numRM;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown numRU;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown numWe;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btnRépartir;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox txt;
        private System.Windows.Forms.MonthCalendar calendar;
        private System.Windows.Forms.RichTextBox txtMessage;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}

