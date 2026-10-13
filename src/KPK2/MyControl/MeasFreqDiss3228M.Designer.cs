namespace KPK2.MyControl
{
    partial class MeasFreqDiss3228M
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.bMeasFreq1_3 = new System.Windows.Forms.Button();
            this.bMeasFreq2 = new System.Windows.Forms.Button();
            this.lTimerText = new System.Windows.Forms.Label();
            this.siTimer = new UserControlsKPK2.SegmentIndication();
            this.tbFreq1_3 = new System.Windows.Forms.TextBox();
            this.tbFreq2 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "Частота канала 1,3:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(288, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 14);
            this.label2.TabIndex = 2;
            this.label2.Text = "МГц";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(288, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 14);
            this.label3.TabIndex = 5;
            this.label3.Text = "МГц";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 63);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(127, 14);
            this.label4.TabIndex = 3;
            this.label4.Text = "Частота канала 2:";
            // 
            // bMeasFreq1_3
            // 
            this.bMeasFreq1_3.Location = new System.Drawing.Point(343, 12);
            this.bMeasFreq1_3.Name = "bMeasFreq1_3";
            this.bMeasFreq1_3.Size = new System.Drawing.Size(219, 23);
            this.bMeasFreq1_3.TabIndex = 6;
            this.bMeasFreq1_3.Text = "Начать измерение (Канал 1,3)";
            this.bMeasFreq1_3.UseVisualStyleBackColor = true;
            this.bMeasFreq1_3.Click += new System.EventHandler(this.bMeasFreq1_3_Click);
            // 
            // bMeasFreq2
            // 
            this.bMeasFreq2.Location = new System.Drawing.Point(343, 57);
            this.bMeasFreq2.Name = "bMeasFreq2";
            this.bMeasFreq2.Size = new System.Drawing.Size(219, 23);
            this.bMeasFreq2.TabIndex = 7;
            this.bMeasFreq2.Text = "Начать измерение (Канал 2)";
            this.bMeasFreq2.UseVisualStyleBackColor = true;
            this.bMeasFreq2.Click += new System.EventHandler(this.bMeasFreq2_Click);
            // 
            // lTimerText
            // 
            this.lTimerText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lTimerText.AutoSize = true;
            this.lTimerText.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lTimerText.Location = new System.Drawing.Point(71, 111);
            this.lTimerText.Name = "lTimerText";
            this.lTimerText.Size = new System.Drawing.Size(198, 14);
            this.lTimerText.TabIndex = 49;
            this.lTimerText.Text = "Время до установки частоты:";
            this.lTimerText.Visible = false;
            // 
            // siTimer
            // 
            this.siTimer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.siTimer.BackColor = System.Drawing.Color.Transparent;
            this.siTimer.BGColor = System.Drawing.Color.Black;
            this.siTimer.DarkColor = System.Drawing.Color.Transparent;
            this.siTimer.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.siTimer.FractPart = 0;
            this.siTimer.Inactive = false;
            this.siTimer.InactiveBGColor = System.Drawing.Color.Gray;
            this.siTimer.InvalidColor = System.Drawing.Color.Red;
            this.siTimer.LightColor = System.Drawing.Color.Lime;
            this.siTimer.Location = new System.Drawing.Point(279, 100);
            this.siTimer.Margin = new System.Windows.Forms.Padding(8);
            this.siTimer.MaxValue = 120D;
            this.siTimer.MinValue = 0D;
            this.siTimer.Name = "siTimer";
            this.siTimer.NeedAlign = true;
            this.siTimer.Size = new System.Drawing.Size(110, 37);
            this.siTimer.TabIndex = 48;
            this.siTimer.Unit = "";
            this.siTimer.Value = 0D;
            this.siTimer.ValueString = "00:00:00";
            this.siTimer.Visible = false;
            this.siTimer.WholePart = 0;
            // 
            // tbFreq1_3
            // 
            this.tbFreq1_3.Location = new System.Drawing.Point(158, 16);
            this.tbFreq1_3.Name = "tbFreq1_3";
            this.tbFreq1_3.ReadOnly = true;
            this.tbFreq1_3.Size = new System.Drawing.Size(124, 22);
            this.tbFreq1_3.TabIndex = 50;
            // 
            // tbFreq2
            // 
            this.tbFreq2.Location = new System.Drawing.Point(158, 58);
            this.tbFreq2.Name = "tbFreq2";
            this.tbFreq2.ReadOnly = true;
            this.tbFreq2.Size = new System.Drawing.Size(124, 22);
            this.tbFreq2.TabIndex = 51;
            // 
            // MeasFreqDiss3228M
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbFreq2);
            this.Controls.Add(this.tbFreq1_3);
            this.Controls.Add(this.lTimerText);
            this.Controls.Add(this.siTimer);
            this.Controls.Add(this.bMeasFreq2);
            this.Controls.Add(this.bMeasFreq1_3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "MeasFreqDiss3228M";
            this.Size = new System.Drawing.Size(604, 159);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button bMeasFreq1_3;
        private System.Windows.Forms.Button bMeasFreq2;
        private System.Windows.Forms.Label lTimerText;
        private UserControlsKPK2.SegmentIndication siTimer;
        private System.Windows.Forms.TextBox tbFreq1_3;
        private System.Windows.Forms.TextBox tbFreq2;
    }
}
