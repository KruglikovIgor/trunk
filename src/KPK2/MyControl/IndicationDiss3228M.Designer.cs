namespace KPK2.MyControl
{
    partial class IndicationDiss3228M
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
            this.cbIndicationWithCtrlVCh = new System.Windows.Forms.CheckBox();
            this.cbIndicationWithoutCtrlVCh = new System.Windows.Forms.CheckBox();
            this.lTimerText = new System.Windows.Forms.Label();
            this.siTimer = new UserControlsKPK2.SegmentIndication();
            this.SuspendLayout();
            // 
            // cbIndicationWithCtrlVCh
            // 
            this.cbIndicationWithCtrlVCh.AutoCheck = false;
            this.cbIndicationWithCtrlVCh.AutoSize = true;
            this.cbIndicationWithCtrlVCh.Location = new System.Drawing.Point(4, 21);
            this.cbIndicationWithCtrlVCh.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbIndicationWithCtrlVCh.Name = "cbIndicationWithCtrlVCh";
            this.cbIndicationWithCtrlVCh.Size = new System.Drawing.Size(281, 18);
            this.cbIndicationWithCtrlVCh.TabIndex = 0;
            this.cbIndicationWithCtrlVCh.Text = "Индикация при включении \"КОНТР ВЧ\"";
            this.cbIndicationWithCtrlVCh.UseVisualStyleBackColor = true;
            // 
            // cbIndicationWithoutCtrlVCh
            // 
            this.cbIndicationWithoutCtrlVCh.AutoCheck = false;
            this.cbIndicationWithoutCtrlVCh.AutoSize = true;
            this.cbIndicationWithoutCtrlVCh.Location = new System.Drawing.Point(4, 59);
            this.cbIndicationWithoutCtrlVCh.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbIndicationWithoutCtrlVCh.Name = "cbIndicationWithoutCtrlVCh";
            this.cbIndicationWithoutCtrlVCh.Size = new System.Drawing.Size(280, 18);
            this.cbIndicationWithoutCtrlVCh.TabIndex = 1;
            this.cbIndicationWithoutCtrlVCh.Text = "Индикация при отсутствии \"КОНТР ВЧ\"";
            this.cbIndicationWithoutCtrlVCh.UseVisualStyleBackColor = true;
            // 
            // lTimerText
            // 
            this.lTimerText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lTimerText.AutoSize = true;
            this.lTimerText.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lTimerText.Location = new System.Drawing.Point(288, 32);
            this.lTimerText.Name = "lTimerText";
            this.lTimerText.Size = new System.Drawing.Size(217, 14);
            this.lTimerText.TabIndex = 47;
            this.lTimerText.Text = "Время до установки индикации:";
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
            this.siTimer.Location = new System.Drawing.Point(510, 21);
            this.siTimer.Margin = new System.Windows.Forms.Padding(8);
            this.siTimer.MaxValue = 120D;
            this.siTimer.MinValue = 0D;
            this.siTimer.Name = "siTimer";
            this.siTimer.NeedAlign = true;
            this.siTimer.Size = new System.Drawing.Size(110, 37);
            this.siTimer.TabIndex = 46;
            this.siTimer.Unit = "";
            this.siTimer.Value = 0D;
            this.siTimer.ValueString = "00:00:00";
            this.siTimer.Visible = false;
            this.siTimer.WholePart = 0;
            // 
            // IndicationDiss3228M
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lTimerText);
            this.Controls.Add(this.siTimer);
            this.Controls.Add(this.cbIndicationWithoutCtrlVCh);
            this.Controls.Add(this.cbIndicationWithCtrlVCh);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "IndicationDiss3228M";
            this.Size = new System.Drawing.Size(637, 111);
            this.Load += new System.EventHandler(this.IndicationDiss3228M_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbIndicationWithCtrlVCh;
        private System.Windows.Forms.CheckBox cbIndicationWithoutCtrlVCh;
        private System.Windows.Forms.Label lTimerText;
        private UserControlsKPK2.SegmentIndication siTimer;
    }
}
