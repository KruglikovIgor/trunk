namespace KPK2.GeneralForms.ServiceForm
{
    partial class frmSettings
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cbNSKS = new System.Windows.Forms.CheckBox();
            this.cbIsSilkway = new System.Windows.Forms.CheckBox();
            this.cbShowDiss32 = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbSelfTest = new System.Windows.Forms.CheckBox();
            this.cbFirstRun = new System.Windows.Forms.CheckBox();
            this.cbPoverkaEnabled = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lWorkTime = new System.Windows.Forms.Label();
            this.bClose = new System.Windows.Forms.Button();
            this.bSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cbSpectrumAnalyzer = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.nudAddFlyDirection = new System.Windows.Forms.NumericUpDown();
            this.nudAddAngleKZ1 = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.clbChecks = new System.Windows.Forms.CheckedListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.bACApply = new System.Windows.Forms.Button();
            this.clbACTypes = new System.Windows.Forms.CheckedListBox();
            this.cbJUPEnable = new System.Windows.Forms.CheckBox();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAddFlyDirection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAddAngleKZ1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cbNSKS);
            this.groupBox4.Controls.Add(this.cbIsSilkway);
            this.groupBox4.Controls.Add(this.cbShowDiss32);
            this.groupBox4.Enabled = false;
            this.groupBox4.Location = new System.Drawing.Point(318, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(298, 108);
            this.groupBox4.TabIndex = 47;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Опции для конкретных заказчиков (устар.)";
            this.groupBox4.Visible = false;
            // 
            // cbNSKS
            // 
            this.cbNSKS.AutoSize = true;
            this.cbNSKS.Enabled = false;
            this.cbNSKS.Location = new System.Drawing.Point(6, 49);
            this.cbNSKS.Name = "cbNSKS";
            this.cbNSKS.Size = new System.Drawing.Size(246, 18);
            this.cbNSKS.TabIndex = 11;
            this.cbNSKS.Text = "Версия НСК-С (ДИСС-15Г, ДИСС-32-90)";
            this.cbNSKS.UseVisualStyleBackColor = true;
            this.cbNSKS.CheckedChanged += new System.EventHandler(this.cbParameterChanged_CheckedChanged);
            // 
            // cbIsSilkway
            // 
            this.cbIsSilkway.AutoSize = true;
            this.cbIsSilkway.Enabled = false;
            this.cbIsSilkway.Location = new System.Drawing.Point(6, 25);
            this.cbIsSilkway.Name = "cbIsSilkway";
            this.cbIsSilkway.Size = new System.Drawing.Size(204, 18);
            this.cbIsSilkway.TabIndex = 4;
            this.cbIsSilkway.Text = "Версия для Silkway (ДИСС-013)";
            this.cbIsSilkway.UseVisualStyleBackColor = true;
            this.cbIsSilkway.CheckedChanged += new System.EventHandler(this.cbParameterChanged_CheckedChanged);
            // 
            // cbShowDiss32
            // 
            this.cbShowDiss32.AutoSize = true;
            this.cbShowDiss32.Enabled = false;
            this.cbShowDiss32.Location = new System.Drawing.Point(6, 73);
            this.cbShowDiss32.Name = "cbShowDiss32";
            this.cbShowDiss32.Size = new System.Drawing.Size(142, 18);
            this.cbShowDiss32.TabIndex = 6;
            this.cbShowDiss32.Text = "Показывать ДИСС-32";
            this.cbShowDiss32.UseVisualStyleBackColor = true;
            this.cbShowDiss32.CheckedChanged += new System.EventHandler(this.cbParameterChanged_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbSelfTest);
            this.groupBox2.Controls.Add(this.cbFirstRun);
            this.groupBox2.Controls.Add(this.cbPoverkaEnabled);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(278, 108);
            this.groupBox2.TabIndex = 48;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Опции";
            // 
            // cbSelfTest
            // 
            this.cbSelfTest.AutoSize = true;
            this.cbSelfTest.Enabled = false;
            this.cbSelfTest.Location = new System.Drawing.Point(6, 45);
            this.cbSelfTest.Name = "cbSelfTest";
            this.cbSelfTest.Size = new System.Drawing.Size(249, 18);
            this.cbSelfTest.TabIndex = 0;
            this.cbSelfTest.Text = "Запустить диагностику при запуске ПО";
            this.cbSelfTest.UseVisualStyleBackColor = true;
            this.cbSelfTest.CheckedChanged += new System.EventHandler(this.cbParameterChanged_CheckedChanged);
            // 
            // cbFirstRun
            // 
            this.cbFirstRun.AutoSize = true;
            this.cbFirstRun.Enabled = false;
            this.cbFirstRun.Location = new System.Drawing.Point(6, 69);
            this.cbFirstRun.Name = "cbFirstRun";
            this.cbFirstRun.Size = new System.Drawing.Size(130, 18);
            this.cbFirstRun.TabIndex = 14;
            this.cbFirstRun.Text = "Первый запуск ПО";
            this.cbFirstRun.UseVisualStyleBackColor = true;
            this.cbFirstRun.CheckedChanged += new System.EventHandler(this.cbParameterChanged_CheckedChanged);
            // 
            // cbPoverkaEnabled
            // 
            this.cbPoverkaEnabled.AutoSize = true;
            this.cbPoverkaEnabled.Enabled = false;
            this.cbPoverkaEnabled.Location = new System.Drawing.Point(6, 21);
            this.cbPoverkaEnabled.Name = "cbPoverkaEnabled";
            this.cbPoverkaEnabled.Size = new System.Drawing.Size(207, 18);
            this.cbPoverkaEnabled.TabIndex = 5;
            this.cbPoverkaEnabled.Text = "\"Поверка\" вместо \"Аттестация\"";
            this.cbPoverkaEnabled.UseVisualStyleBackColor = true;
            this.cbPoverkaEnabled.CheckedChanged += new System.EventHandler(this.cbParameterChanged_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lWorkTime);
            this.panel1.Controls.Add(this.bClose);
            this.panel1.Controls.Add(this.bSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 519);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(628, 60);
            this.panel1.TabIndex = 49;
            // 
            // lWorkTime
            // 
            this.lWorkTime.AutoSize = true;
            this.lWorkTime.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lWorkTime.Location = new System.Drawing.Point(318, 22);
            this.lWorkTime.Name = "lWorkTime";
            this.lWorkTime.Size = new System.Drawing.Size(0, 29);
            this.lWorkTime.TabIndex = 48;
            // 
            // bClose
            // 
            this.bClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bClose.Location = new System.Drawing.Point(372, 12);
            this.bClose.Name = "bClose";
            this.bClose.Size = new System.Drawing.Size(244, 40);
            this.bClose.TabIndex = 49;
            this.bClose.Text = "Закрыть";
            this.bClose.UseVisualStyleBackColor = true;
            this.bClose.Click += new System.EventHandler(this.bClose_Click);
            // 
            // bSave
            // 
            this.bSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bSave.Enabled = false;
            this.bSave.Location = new System.Drawing.Point(7, 12);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(244, 40);
            this.bSave.TabIndex = 48;
            this.bSave.Text = "Сохранить";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 492);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(390, 14);
            this.label1.TabIndex = 50;
            this.label1.Text = "Важно: для применения настроек необходимо перезапустить ПО!";
            // 
            // cbSpectrumAnalyzer
            // 
            this.cbSpectrumAnalyzer.AutoSize = true;
            this.cbSpectrumAnalyzer.Enabled = false;
            this.cbSpectrumAnalyzer.Location = new System.Drawing.Point(18, 393);
            this.cbSpectrumAnalyzer.Name = "cbSpectrumAnalyzer";
            this.cbSpectrumAnalyzer.Size = new System.Drawing.Size(247, 18);
            this.cbSpectrumAnalyzer.TabIndex = 51;
            this.cbSpectrumAnalyzer.Text = "Используется USB анализатор спектра";
            this.cbSpectrumAnalyzer.UseVisualStyleBackColor = true;
            this.cbSpectrumAnalyzer.CheckedChanged += new System.EventHandler(this.cbParameterChanged_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 426);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(368, 14);
            this.label2.TabIndex = 52;
            this.label2.Text = "Дополнительный разворот угла по полёту (блок 8) (ДИСС-32):";
            // 
            // nudAddFlyDirection
            // 
            this.nudAddFlyDirection.Location = new System.Drawing.Point(389, 420);
            this.nudAddFlyDirection.Maximum = new decimal(new int[] {
            359,
            0,
            0,
            0});
            this.nudAddFlyDirection.Name = "nudAddFlyDirection";
            this.nudAddFlyDirection.Size = new System.Drawing.Size(120, 22);
            this.nudAddFlyDirection.TabIndex = 53;
            this.nudAddFlyDirection.ValueChanged += new System.EventHandler(this.nudAddFlyDirection_ValueChanged);
            // 
            // nudAddAngleKZ1
            // 
            this.nudAddAngleKZ1.Location = new System.Drawing.Point(389, 457);
            this.nudAddAngleKZ1.Maximum = new decimal(new int[] {
            359,
            0,
            0,
            0});
            this.nudAddAngleKZ1.Name = "nudAddAngleKZ1";
            this.nudAddAngleKZ1.Size = new System.Drawing.Size(120, 22);
            this.nudAddAngleKZ1.TabIndex = 55;
            this.nudAddAngleKZ1.Value = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.nudAddAngleKZ1.ValueChanged += new System.EventHandler(this.nudAddAngleKZ1_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 461);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(368, 14);
            this.label3.TabIndex = 54;
            this.label3.Text = "Дополнительный разворот угла по наклонам (КЗ-1) (ДИСС-32):";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.clbChecks);
            this.groupBox1.Location = new System.Drawing.Point(12, 126);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(278, 216);
            this.groupBox1.TabIndex = 56;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Видимость проверок";
            // 
            // clbChecks
            // 
            this.clbChecks.CheckOnClick = true;
            this.clbChecks.FormattingEnabled = true;
            this.clbChecks.Location = new System.Drawing.Point(6, 21);
            this.clbChecks.Name = "clbChecks";
            this.clbChecks.Size = new System.Drawing.Size(262, 191);
            this.clbChecks.TabIndex = 0;
            this.clbChecks.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbChecks_ItemCheck);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.bACApply);
            this.groupBox3.Controls.Add(this.clbACTypes);
            this.groupBox3.Location = new System.Drawing.Point(338, 126);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(278, 266);
            this.groupBox3.TabIndex = 57;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Видимость проверок по типам ВС";
            // 
            // bACApply
            // 
            this.bACApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bACApply.Enabled = false;
            this.bACApply.Location = new System.Drawing.Point(15, 218);
            this.bACApply.Name = "bACApply";
            this.bACApply.Size = new System.Drawing.Size(244, 40);
            this.bACApply.TabIndex = 50;
            this.bACApply.Text = "Применить";
            this.bACApply.UseVisualStyleBackColor = true;
            this.bACApply.Click += new System.EventHandler(this.bACApply_Click);
            // 
            // clbACTypes
            // 
            this.clbACTypes.CheckOnClick = true;
            this.clbACTypes.FormattingEnabled = true;
            this.clbACTypes.Location = new System.Drawing.Point(6, 21);
            this.clbACTypes.Name = "clbACTypes";
            this.clbACTypes.Size = new System.Drawing.Size(262, 191);
            this.clbACTypes.TabIndex = 0;
            this.clbACTypes.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbACTypes_ItemCheck);
            // 
            // cbJUPEnable
            // 
            this.cbJUPEnable.AutoSize = true;
            this.cbJUPEnable.Location = new System.Drawing.Point(18, 369);
            this.cbJUPEnable.Name = "cbJUPEnable";
            this.cbJUPEnable.Size = new System.Drawing.Size(112, 18);
            this.cbJUPEnable.TabIndex = 58;
            this.cbJUPEnable.Text = "Включить ЖУП";
            this.cbJUPEnable.UseVisualStyleBackColor = true;
            this.cbJUPEnable.CheckedChanged += new System.EventHandler(this.cb_JupEnable_CheckedChanged);
            // 
            // frmSettings
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(628, 579);
            this.Controls.Add(this.cbJUPEnable);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.nudAddAngleKZ1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nudAddFlyDirection);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbSpectrumAnalyzer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox4);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "frmSettings";
            this.Text = "Настройки";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSettings_FormClosing);
            this.Load += new System.EventHandler(this.frmSettings_Load);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAddFlyDirection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAddAngleKZ1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox cbIsSilkway;
        private System.Windows.Forms.CheckBox cbShowDiss32;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cbSelfTest;
        private System.Windows.Forms.CheckBox cbFirstRun;
        private System.Windows.Forms.CheckBox cbPoverkaEnabled;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lWorkTime;
        private System.Windows.Forms.Button bClose;
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbNSKS;
        private System.Windows.Forms.CheckBox cbSpectrumAnalyzer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudAddFlyDirection;
        private System.Windows.Forms.NumericUpDown nudAddAngleKZ1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox clbChecks;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckedListBox clbACTypes;
        private System.Windows.Forms.Button bACApply;
        private System.Windows.Forms.CheckBox cbJUPEnable;
    }
}

