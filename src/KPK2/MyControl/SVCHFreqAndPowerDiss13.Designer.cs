namespace KPK2.MyControl
{
    partial class SVCHFreqAndPowerDiss13
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SVCHFreqAndPowerDiss13));
            this.grpBox_Freq = new System.Windows.Forms.GroupBox();
            this.rbPower = new System.Windows.Forms.RadioButton();
            this.rbFrequency = new System.Windows.Forms.RadioButton();
            this.gbFrequency = new System.Windows.Forms.GroupBox();
            this.bCalculateFrequency = new System.Windows.Forms.Button();
            this.label40 = new System.Windows.Forms.Label();
            this.tbFreq = new System.Windows.Forms.TextBox();
            this.lFreqResult = new System.Windows.Forms.Label();
            this.label47 = new System.Windows.Forms.Label();
            this.lFreqScribe = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.tbTemperature = new System.Windows.Forms.TextBox();
            this.label50 = new System.Windows.Forms.Label();
            this.gbPower = new System.Windows.Forms.GroupBox();
            this.bMeasurePower = new System.Windows.Forms.Button();
            this.lPower = new System.Windows.Forms.Label();
            this.lPowerResult = new System.Windows.Forms.Label();
            this.tbPower = new System.Windows.Forms.TextBox();
            this.label42 = new System.Windows.Forms.Label();
            this.grpBox_Fading = new System.Windows.Forms.GroupBox();
            this.cbSavedKoef = new System.Windows.Forms.CheckBox();
            this.tbAdapter = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbWire = new System.Windows.Forms.TextBox();
            this.tbKVP = new System.Windows.Forms.TextBox();
            this.tbDiss = new System.Windows.Forms.TextBox();
            this.label55 = new System.Windows.Forms.Label();
            this.label53 = new System.Windows.Forms.Label();
            this.label51 = new System.Windows.Forms.Label();
            this.bCalculatePower = new System.Windows.Forms.Button();
            this.lPowerScribe = new System.Windows.Forms.Label();
            this.rbMvt = new System.Windows.Forms.RadioButton();
            this.rbDBM = new System.Windows.Forms.RadioButton();
            this.grpBox_Freq.SuspendLayout();
            this.gbFrequency.SuspendLayout();
            this.gbPower.SuspendLayout();
            this.grpBox_Fading.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpBox_Freq
            // 
            resources.ApplyResources(this.grpBox_Freq, "grpBox_Freq");
            this.grpBox_Freq.Controls.Add(this.rbPower);
            this.grpBox_Freq.Controls.Add(this.rbFrequency);
            this.grpBox_Freq.Controls.Add(this.gbFrequency);
            this.grpBox_Freq.Controls.Add(this.gbPower);
            this.grpBox_Freq.Name = "grpBox_Freq";
            this.grpBox_Freq.TabStop = false;
            // 
            // rbPower
            // 
            resources.ApplyResources(this.rbPower, "rbPower");
            this.rbPower.Name = "rbPower";
            this.rbPower.UseVisualStyleBackColor = true;
            this.rbPower.CheckedChanged += new System.EventHandler(this.rbPower_CheckedChanged);
            // 
            // rbFrequency
            // 
            resources.ApplyResources(this.rbFrequency, "rbFrequency");
            this.rbFrequency.Checked = true;
            this.rbFrequency.Name = "rbFrequency";
            this.rbFrequency.TabStop = true;
            this.rbFrequency.UseVisualStyleBackColor = true;
            this.rbFrequency.CheckedChanged += new System.EventHandler(this.rbFrequency_CheckedChanged);
            // 
            // gbFrequency
            // 
            this.gbFrequency.Controls.Add(this.bCalculateFrequency);
            this.gbFrequency.Controls.Add(this.label40);
            this.gbFrequency.Controls.Add(this.tbFreq);
            this.gbFrequency.Controls.Add(this.lFreqResult);
            this.gbFrequency.Controls.Add(this.label47);
            this.gbFrequency.Controls.Add(this.lFreqScribe);
            this.gbFrequency.Controls.Add(this.label39);
            this.gbFrequency.Controls.Add(this.tbTemperature);
            this.gbFrequency.Controls.Add(this.label50);
            resources.ApplyResources(this.gbFrequency, "gbFrequency");
            this.gbFrequency.Name = "gbFrequency";
            this.gbFrequency.TabStop = false;
            // 
            // bCalculateFrequency
            // 
            resources.ApplyResources(this.bCalculateFrequency, "bCalculateFrequency");
            this.bCalculateFrequency.Name = "bCalculateFrequency";
            this.bCalculateFrequency.UseVisualStyleBackColor = true;
            this.bCalculateFrequency.Click += new System.EventHandler(this.bCalculateFrequency_Click);
            // 
            // label40
            // 
            resources.ApplyResources(this.label40, "label40");
            this.label40.Name = "label40";
            // 
            // tbFreq
            // 
            resources.ApplyResources(this.tbFreq, "tbFreq");
            this.tbFreq.Name = "tbFreq";
            this.tbFreq.Leave += new System.EventHandler(this.tb_Leave);
            // 
            // lFreqResult
            // 
            resources.ApplyResources(this.lFreqResult, "lFreqResult");
            this.lFreqResult.Name = "lFreqResult";
            // 
            // label47
            // 
            resources.ApplyResources(this.label47, "label47");
            this.label47.Name = "label47";
            // 
            // lFreqScribe
            // 
            resources.ApplyResources(this.lFreqScribe, "lFreqScribe");
            this.lFreqScribe.Name = "lFreqScribe";
            // 
            // label39
            // 
            this.label39.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.label39, "label39");
            this.label39.Name = "label39";
            // 
            // tbTemperature
            // 
            resources.ApplyResources(this.tbTemperature, "tbTemperature");
            this.tbTemperature.Name = "tbTemperature";
            this.tbTemperature.TextChanged += new System.EventHandler(this.tb_TextChanged);
            // 
            // label50
            // 
            this.label50.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.label50, "label50");
            this.label50.Name = "label50";
            // 
            // gbPower
            // 
            resources.ApplyResources(this.gbPower, "gbPower");
            this.gbPower.Controls.Add(this.bMeasurePower);
            this.gbPower.Controls.Add(this.lPower);
            this.gbPower.Controls.Add(this.lPowerResult);
            this.gbPower.Controls.Add(this.tbPower);
            this.gbPower.Controls.Add(this.label42);
            this.gbPower.Controls.Add(this.grpBox_Fading);
            this.gbPower.Controls.Add(this.bCalculatePower);
            this.gbPower.Controls.Add(this.lPowerScribe);
            this.gbPower.Controls.Add(this.rbMvt);
            this.gbPower.Controls.Add(this.rbDBM);
            this.gbPower.Name = "gbPower";
            this.gbPower.TabStop = false;
            // 
            // bMeasurePower
            // 
            resources.ApplyResources(this.bMeasurePower, "bMeasurePower");
            this.bMeasurePower.Name = "bMeasurePower";
            this.bMeasurePower.UseVisualStyleBackColor = true;
            this.bMeasurePower.Click += new System.EventHandler(this.bMeasurePower_Click);
            // 
            // lPower
            // 
            resources.ApplyResources(this.lPower, "lPower");
            this.lPower.Name = "lPower";
            // 
            // lPowerResult
            // 
            resources.ApplyResources(this.lPowerResult, "lPowerResult");
            this.lPowerResult.Name = "lPowerResult";
            // 
            // tbPower
            // 
            resources.ApplyResources(this.tbPower, "tbPower");
            this.tbPower.Name = "tbPower";
            this.tbPower.TextChanged += new System.EventHandler(this.tb_TextChanged);
            // 
            // label42
            // 
            this.label42.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.label42, "label42");
            this.label42.Name = "label42";
            // 
            // grpBox_Fading
            // 
            this.grpBox_Fading.Controls.Add(this.cbSavedKoef);
            this.grpBox_Fading.Controls.Add(this.tbAdapter);
            this.grpBox_Fading.Controls.Add(this.label1);
            this.grpBox_Fading.Controls.Add(this.tbWire);
            this.grpBox_Fading.Controls.Add(this.tbKVP);
            this.grpBox_Fading.Controls.Add(this.tbDiss);
            this.grpBox_Fading.Controls.Add(this.label55);
            this.grpBox_Fading.Controls.Add(this.label53);
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
            // tbAdapter
            // 
            resources.ApplyResources(this.tbAdapter, "tbAdapter");
            this.tbAdapter.Name = "tbAdapter";
            this.tbAdapter.TextChanged += new System.EventHandler(this.tb_TextChanged);
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tbWire
            // 
            resources.ApplyResources(this.tbWire, "tbWire");
            this.tbWire.Name = "tbWire";
            this.tbWire.TextChanged += new System.EventHandler(this.tb_TextChanged);
            // 
            // tbKVP
            // 
            resources.ApplyResources(this.tbKVP, "tbKVP");
            this.tbKVP.Name = "tbKVP";
            this.tbKVP.TextChanged += new System.EventHandler(this.tb_TextChanged);
            // 
            // tbDiss
            // 
            resources.ApplyResources(this.tbDiss, "tbDiss");
            this.tbDiss.Name = "tbDiss";
            this.tbDiss.TextChanged += new System.EventHandler(this.tb_TextChanged);
            // 
            // label55
            // 
            this.label55.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.label55, "label55");
            this.label55.Name = "label55";
            // 
            // label53
            // 
            this.label53.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.label53, "label53");
            this.label53.Name = "label53";
            // 
            // label51
            // 
            this.label51.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.label51, "label51");
            this.label51.Name = "label51";
            // 
            // bCalculatePower
            // 
            resources.ApplyResources(this.bCalculatePower, "bCalculatePower");
            this.bCalculatePower.Name = "bCalculatePower";
            this.bCalculatePower.UseVisualStyleBackColor = true;
            this.bCalculatePower.Click += new System.EventHandler(this.bCalculate_Click);
            // 
            // lPowerScribe
            // 
            resources.ApplyResources(this.lPowerScribe, "lPowerScribe");
            this.lPowerScribe.Name = "lPowerScribe";
            // 
            // rbMvt
            // 
            resources.ApplyResources(this.rbMvt, "rbMvt");
            this.rbMvt.Checked = true;
            this.rbMvt.Name = "rbMvt";
            this.rbMvt.TabStop = true;
            this.rbMvt.UseVisualStyleBackColor = true;
            this.rbMvt.CheckedChanged += new System.EventHandler(this.rbMvt_CheckedChanged);
            // 
            // rbDBM
            // 
            resources.ApplyResources(this.rbDBM, "rbDBM");
            this.rbDBM.Name = "rbDBM";
            this.rbDBM.UseVisualStyleBackColor = true;
            this.rbDBM.CheckedChanged += new System.EventHandler(this.rbDBM_CheckedChanged);
            // 
            // SVCHFreqAndPowerDiss13
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpBox_Freq);
            this.Name = "SVCHFreqAndPowerDiss13";
            this.Load += new System.EventHandler(this.SVCHFreqAndPowerDiss13_Load);
            this.grpBox_Freq.ResumeLayout(false);
            this.grpBox_Freq.PerformLayout();
            this.gbFrequency.ResumeLayout(false);
            this.gbFrequency.PerformLayout();
            this.gbPower.ResumeLayout(false);
            this.gbPower.PerformLayout();
            this.grpBox_Fading.ResumeLayout(false);
            this.grpBox_Fading.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBox_Freq;
        private System.Windows.Forms.GroupBox grpBox_Fading;
        private System.Windows.Forms.TextBox tbWire;
        private System.Windows.Forms.TextBox tbKVP;
        private System.Windows.Forms.TextBox tbDiss;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.Label lPowerScribe;
        private System.Windows.Forms.RadioButton rbDBM;
        private System.Windows.Forms.RadioButton rbMvt;
        private System.Windows.Forms.Label lFreqScribe;
        private System.Windows.Forms.TextBox tbTemperature;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Label lPower;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.TextBox tbPower;
        private System.Windows.Forms.TextBox tbFreq;
        private System.Windows.Forms.Label lFreqResult;
        private System.Windows.Forms.Label lPowerResult;
        private System.Windows.Forms.TextBox tbAdapter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbPower;
        private System.Windows.Forms.RadioButton rbPower;
        private System.Windows.Forms.RadioButton rbFrequency;
        private System.Windows.Forms.GroupBox gbFrequency;
        private System.Windows.Forms.Button bCalculateFrequency;
        public System.Windows.Forms.Button bCalculatePower;
        private System.Windows.Forms.CheckBox cbSavedKoef;
        private System.Windows.Forms.Button bMeasurePower;
    }
}
