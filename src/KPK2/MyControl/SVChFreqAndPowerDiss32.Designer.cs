namespace KPK2.MyControl
{
    partial class SVChFreqAndPowerDiss32
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SVChFreqAndPowerDiss32));
            this.txtBox_Freq = new System.Windows.Forms.TextBox();
            this.tbPower = new System.Windows.Forms.TextBox();
            this.label40 = new System.Windows.Forms.Label();
            this.grpBox_Fading = new System.Windows.Forms.GroupBox();
            this.cbSavedKoef = new System.Windows.Forms.CheckBox();
            this.tbKVP = new System.Windows.Forms.TextBox();
            this.tbVP = new System.Windows.Forms.TextBox();
            this.txtBox_Diss = new System.Windows.Forms.TextBox();
            this.label53 = new System.Windows.Forms.Label();
            this.label52 = new System.Windows.Forms.Label();
            this.label51 = new System.Windows.Forms.Label();
            this.bMeasurePower = new System.Windows.Forms.Button();
            this.grpBox_Freq = new System.Windows.Forms.GroupBox();
            this.grpBox_Fading.SuspendLayout();
            this.grpBox_Freq.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtBox_Freq
            // 
            resources.ApplyResources(this.txtBox_Freq, "txtBox_Freq");
            this.txtBox_Freq.Name = "txtBox_Freq";
            this.txtBox_Freq.TextChanged += new System.EventHandler(this.txtBox_TextChanged);
            this.txtBox_Freq.Leave += new System.EventHandler(this.txtBox_Leave);
            // 
            // tbPower
            // 
            resources.ApplyResources(this.tbPower, "tbPower");
            this.tbPower.ForeColor = System.Drawing.SystemColors.WindowText;
            this.tbPower.Name = "tbPower";
            this.tbPower.TextChanged += new System.EventHandler(this.txtBox_TextChanged);
            this.tbPower.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbPower_KeyPress);
            this.tbPower.Leave += new System.EventHandler(this.txtBox_Leave);
            // 
            // label40
            // 
            resources.ApplyResources(this.label40, "label40");
            this.label40.Name = "label40";
            // 
            // grpBox_Fading
            // 
            this.grpBox_Fading.Controls.Add(this.cbSavedKoef);
            this.grpBox_Fading.Controls.Add(this.tbKVP);
            this.grpBox_Fading.Controls.Add(this.tbVP);
            this.grpBox_Fading.Controls.Add(this.txtBox_Diss);
            this.grpBox_Fading.Controls.Add(this.label53);
            this.grpBox_Fading.Controls.Add(this.label52);
            this.grpBox_Fading.Controls.Add(this.label51);
            resources.ApplyResources(this.grpBox_Fading, "grpBox_Fading");
            this.grpBox_Fading.Name = "grpBox_Fading";
            this.grpBox_Fading.TabStop = false;
            // 
            // cbSavedKoef
            // 
            resources.ApplyResources(this.cbSavedKoef, "cbSavedKoef");
            this.cbSavedKoef.Name = "cbSavedKoef";
            this.cbSavedKoef.UseVisualStyleBackColor = true;
            this.cbSavedKoef.CheckedChanged += new System.EventHandler(this.cbSavedKoef_CheckedChanged);
            // 
            // tbKVP
            // 
            resources.ApplyResources(this.tbKVP, "tbKVP");
            this.tbKVP.Name = "tbKVP";
            this.tbKVP.TextChanged += new System.EventHandler(this.txtBox_TextChanged);
            this.tbKVP.Leave += new System.EventHandler(this.txtBox_Leave);
            // 
            // tbVP
            // 
            resources.ApplyResources(this.tbVP, "tbVP");
            this.tbVP.Name = "tbVP";
            this.tbVP.TextChanged += new System.EventHandler(this.txtBox_TextChanged);
            this.tbVP.Leave += new System.EventHandler(this.txtBox_Leave);
            // 
            // txtBox_Diss
            // 
            resources.ApplyResources(this.txtBox_Diss, "txtBox_Diss");
            this.txtBox_Diss.Name = "txtBox_Diss";
            this.txtBox_Diss.TextChanged += new System.EventHandler(this.txtBox_TextChanged);
            this.txtBox_Diss.Leave += new System.EventHandler(this.txtBox_Leave);
            // 
            // label53
            // 
            this.label53.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.label53, "label53");
            this.label53.Name = "label53";
            // 
            // label52
            // 
            this.label52.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.label52, "label52");
            this.label52.Name = "label52";
            // 
            // label51
            // 
            this.label51.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.label51, "label51");
            this.label51.Name = "label51";
            // 
            // bMeasurePower
            // 
            resources.ApplyResources(this.bMeasurePower, "bMeasurePower");
            this.bMeasurePower.Name = "bMeasurePower";
            this.bMeasurePower.UseVisualStyleBackColor = true;
            this.bMeasurePower.Click += new System.EventHandler(this.bMeasurePower_Click);
            // 
            // grpBox_Freq
            // 
            this.grpBox_Freq.Controls.Add(this.bMeasurePower);
            this.grpBox_Freq.Controls.Add(this.grpBox_Fading);
            this.grpBox_Freq.Controls.Add(this.label40);
            this.grpBox_Freq.Controls.Add(this.tbPower);
            this.grpBox_Freq.Controls.Add(this.txtBox_Freq);
            resources.ApplyResources(this.grpBox_Freq, "grpBox_Freq");
            this.grpBox_Freq.Name = "grpBox_Freq";
            this.grpBox_Freq.TabStop = false;
            this.grpBox_Freq.EnabledChanged += new System.EventHandler(this.grpBox_Freq_EnabledChanged);
            // 
            // SVChFreqAndPowerDiss32
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpBox_Freq);
            this.Name = "SVChFreqAndPowerDiss32";
            this.Load += new System.EventHandler(this.SVChFreqAndPower_Load);
            this.grpBox_Fading.ResumeLayout(false);
            this.grpBox_Fading.PerformLayout();
            this.grpBox_Freq.ResumeLayout(false);
            this.grpBox_Freq.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtBox_Freq;
        private System.Windows.Forms.TextBox tbPower;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.GroupBox grpBox_Fading;
        private System.Windows.Forms.CheckBox cbSavedKoef;
        private System.Windows.Forms.TextBox tbKVP;
        private System.Windows.Forms.TextBox tbVP;
        private System.Windows.Forms.TextBox txtBox_Diss;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.Button bMeasurePower;
        private System.Windows.Forms.GroupBox grpBox_Freq;
    }
}
