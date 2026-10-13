namespace KPK2.MyControl
{
    partial class SVChFreqAndPower
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SVChFreqAndPower));
            this.grpBox_Freq = new System.Windows.Forms.GroupBox();
            this.bMeasurePower = new System.Windows.Forms.Button();
            this.grpBox_Fading = new System.Windows.Forms.GroupBox();
            this.cbSavedKoef = new System.Windows.Forms.CheckBox();
            this.tbCabel = new System.Windows.Forms.TextBox();
            this.tbAttenuator = new System.Windows.Forms.TextBox();
            this.tbKVP = new System.Windows.Forms.TextBox();
            this.tbVP = new System.Windows.Forms.TextBox();
            this.txtBox_Diss = new System.Windows.Forms.TextBox();
            this.label55 = new System.Windows.Forms.Label();
            this.label54 = new System.Windows.Forms.Label();
            this.label53 = new System.Windows.Forms.Label();
            this.label52 = new System.Windows.Forms.Label();
            this.label51 = new System.Windows.Forms.Label();
            this.lbl_Pow_Res = new System.Windows.Forms.Label();
            this.lbl_Power = new System.Windows.Forms.Label();
            this.rbtn_dBm = new System.Windows.Forms.RadioButton();
            this.rbtn_mBt = new System.Windows.Forms.RadioButton();
            this.lbl_Result = new System.Windows.Forms.Label();
            this.lbl_Freq = new System.Windows.Forms.Label();
            this.btn_CalcFreq = new System.Windows.Forms.Button();
            this.txtBox_Temp = new System.Windows.Forms.TextBox();
            this.label42 = new System.Windows.Forms.Label();
            this.label50 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.lPower = new System.Windows.Forms.Label();
            this.label47 = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.tbPower = new System.Windows.Forms.TextBox();
            this.txtBox_Freq = new System.Windows.Forms.TextBox();
            this.grpBox_Freq.SuspendLayout();
            this.grpBox_Fading.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpBox_Freq
            // 
            this.grpBox_Freq.Controls.Add(this.bMeasurePower);
            this.grpBox_Freq.Controls.Add(this.grpBox_Fading);
            this.grpBox_Freq.Controls.Add(this.lbl_Pow_Res);
            this.grpBox_Freq.Controls.Add(this.lbl_Power);
            this.grpBox_Freq.Controls.Add(this.rbtn_dBm);
            this.grpBox_Freq.Controls.Add(this.rbtn_mBt);
            this.grpBox_Freq.Controls.Add(this.lbl_Result);
            this.grpBox_Freq.Controls.Add(this.lbl_Freq);
            this.grpBox_Freq.Controls.Add(this.btn_CalcFreq);
            this.grpBox_Freq.Controls.Add(this.txtBox_Temp);
            this.grpBox_Freq.Controls.Add(this.label42);
            this.grpBox_Freq.Controls.Add(this.label50);
            this.grpBox_Freq.Controls.Add(this.label39);
            this.grpBox_Freq.Controls.Add(this.lPower);
            this.grpBox_Freq.Controls.Add(this.label47);
            this.grpBox_Freq.Controls.Add(this.label40);
            this.grpBox_Freq.Controls.Add(this.tbPower);
            this.grpBox_Freq.Controls.Add(this.txtBox_Freq);
            resources.ApplyResources(this.grpBox_Freq, "grpBox_Freq");
            this.grpBox_Freq.Name = "grpBox_Freq";
            this.grpBox_Freq.TabStop = false;
            this.grpBox_Freq.EnabledChanged += new System.EventHandler(this.grpBox_Freq_EnabledChanged);
            // 
            // bMeasurePower
            // 
            resources.ApplyResources(this.bMeasurePower, "bMeasurePower");
            this.bMeasurePower.Name = "bMeasurePower";
            this.bMeasurePower.UseVisualStyleBackColor = true;
            this.bMeasurePower.Click += new System.EventHandler(this.bMeasurePower_Click);
            // 
            // grpBox_Fading
            // 
            this.grpBox_Fading.Controls.Add(this.cbSavedKoef);
            this.grpBox_Fading.Controls.Add(this.tbCabel);
            this.grpBox_Fading.Controls.Add(this.tbAttenuator);
            this.grpBox_Fading.Controls.Add(this.tbKVP);
            this.grpBox_Fading.Controls.Add(this.tbVP);
            this.grpBox_Fading.Controls.Add(this.txtBox_Diss);
            this.grpBox_Fading.Controls.Add(this.label55);
            this.grpBox_Fading.Controls.Add(this.label54);
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
            // tbCabel
            // 
            resources.ApplyResources(this.tbCabel, "tbCabel");
            this.tbCabel.Name = "tbCabel";
            this.tbCabel.TextChanged += new System.EventHandler(this.txtBox_TextChanged);
            this.tbCabel.Leave += new System.EventHandler(this.txtBox_Leave);
            // 
            // tbAttenuator
            // 
            resources.ApplyResources(this.tbAttenuator, "tbAttenuator");
            this.tbAttenuator.Name = "tbAttenuator";
            this.tbAttenuator.TextChanged += new System.EventHandler(this.txtBox_TextChanged);
            this.tbAttenuator.Leave += new System.EventHandler(this.txtBox_Leave);
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
            // label55
            // 
            this.label55.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.label55, "label55");
            this.label55.Name = "label55";
            // 
            // label54
            // 
            this.label54.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.label54, "label54");
            this.label54.Name = "label54";
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
            // lbl_Pow_Res
            // 
            resources.ApplyResources(this.lbl_Pow_Res, "lbl_Pow_Res");
            this.lbl_Pow_Res.ForeColor = System.Drawing.Color.Green;
            this.lbl_Pow_Res.Name = "lbl_Pow_Res";
            // 
            // lbl_Power
            // 
            resources.ApplyResources(this.lbl_Power, "lbl_Power");
            this.lbl_Power.Name = "lbl_Power";
            // 
            // rbtn_dBm
            // 
            resources.ApplyResources(this.rbtn_dBm, "rbtn_dBm");
            this.rbtn_dBm.Name = "rbtn_dBm";
            this.rbtn_dBm.UseVisualStyleBackColor = true;
            // 
            // rbtn_mBt
            // 
            resources.ApplyResources(this.rbtn_mBt, "rbtn_mBt");
            this.rbtn_mBt.Checked = true;
            this.rbtn_mBt.Name = "rbtn_mBt";
            this.rbtn_mBt.TabStop = true;
            this.rbtn_mBt.UseVisualStyleBackColor = true;
            this.rbtn_mBt.CheckedChanged += new System.EventHandler(this.rbtn_mBt_CheckedChanged);
            // 
            // lbl_Result
            // 
            resources.ApplyResources(this.lbl_Result, "lbl_Result");
            this.lbl_Result.ForeColor = System.Drawing.Color.Green;
            this.lbl_Result.Name = "lbl_Result";
            // 
            // lbl_Freq
            // 
            resources.ApplyResources(this.lbl_Freq, "lbl_Freq");
            this.lbl_Freq.Name = "lbl_Freq";
            // 
            // btn_CalcFreq
            // 
            resources.ApplyResources(this.btn_CalcFreq, "btn_CalcFreq");
            this.btn_CalcFreq.Name = "btn_CalcFreq";
            this.btn_CalcFreq.UseVisualStyleBackColor = true;
            this.btn_CalcFreq.Click += new System.EventHandler(this.btn_CalcFreq_Click);
            // 
            // txtBox_Temp
            // 
            resources.ApplyResources(this.txtBox_Temp, "txtBox_Temp");
            this.txtBox_Temp.Name = "txtBox_Temp";
            this.txtBox_Temp.TextChanged += new System.EventHandler(this.txtBox_TextChanged);
            this.txtBox_Temp.Leave += new System.EventHandler(this.txtBox_Leave);
            // 
            // label42
            // 
            this.label42.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.label42, "label42");
            this.label42.Name = "label42";
            // 
            // label50
            // 
            this.label50.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.label50, "label50");
            this.label50.Name = "label50";
            // 
            // label39
            // 
            this.label39.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.label39, "label39");
            this.label39.Name = "label39";
            // 
            // lPower
            // 
            resources.ApplyResources(this.lPower, "lPower");
            this.lPower.Name = "lPower";
            // 
            // label47
            // 
            resources.ApplyResources(this.label47, "label47");
            this.label47.Name = "label47";
            // 
            // label40
            // 
            resources.ApplyResources(this.label40, "label40");
            this.label40.Name = "label40";
            // 
            // tbPower
            // 
            resources.ApplyResources(this.tbPower, "tbPower");
            this.tbPower.Name = "tbPower";
            this.tbPower.TextChanged += new System.EventHandler(this.txtBox_TextChanged);
            this.tbPower.Leave += new System.EventHandler(this.txtBox_Leave);
            // 
            // txtBox_Freq
            // 
            resources.ApplyResources(this.txtBox_Freq, "txtBox_Freq");
            this.txtBox_Freq.Name = "txtBox_Freq";
            this.txtBox_Freq.TextChanged += new System.EventHandler(this.txtBox_TextChanged);
            this.txtBox_Freq.Leave += new System.EventHandler(this.txtBox_Leave);
            // 
            // SVChFreqAndPower
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpBox_Freq);
            this.Name = "SVChFreqAndPower";
            this.Load += new System.EventHandler(this.SVChFreqAndPower_Load);
            this.grpBox_Freq.ResumeLayout(false);
            this.grpBox_Freq.PerformLayout();
            this.grpBox_Fading.ResumeLayout(false);
            this.grpBox_Fading.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBox_Freq;
        private System.Windows.Forms.GroupBox grpBox_Fading;
        private System.Windows.Forms.TextBox tbCabel;
        private System.Windows.Forms.TextBox tbAttenuator;
        private System.Windows.Forms.TextBox tbKVP;
        private System.Windows.Forms.TextBox tbVP;
        private System.Windows.Forms.TextBox txtBox_Diss;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.Label lbl_Pow_Res;
        private System.Windows.Forms.Label lbl_Power;
        private System.Windows.Forms.RadioButton rbtn_dBm;
        private System.Windows.Forms.RadioButton rbtn_mBt;
        private System.Windows.Forms.Label lbl_Result;
        private System.Windows.Forms.Label lbl_Freq;
        private System.Windows.Forms.Button btn_CalcFreq;
        private System.Windows.Forms.TextBox txtBox_Temp;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Label lPower;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.TextBox tbPower;
        private System.Windows.Forms.TextBox txtBox_Freq;
        private System.Windows.Forms.CheckBox cbSavedKoef;
        private System.Windows.Forms.Button bMeasurePower;
    }
}
