namespace KPK2.GeneralForms.ServiceForm
{
    partial class frmPS115VCalibrate
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
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.nudVoltage = new System.Windows.Forms.NumericUpDown();
      this.nudCurrentLimit = new System.Windows.Forms.NumericUpDown();
      this.nudFrequency = new System.Windows.Forms.NumericUpDown();
      this.bSet = new System.Windows.Forms.Button();
      this.nudRealVoltage = new System.Windows.Forms.NumericUpDown();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.bCalculate = new System.Windows.Forms.Button();
      this.bSaveClose = new System.Windows.Forms.Button();
      this.lLineResistance = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.nudVoltage)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.nudCurrentLimit)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.nudFrequency)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.nudRealVoltage)).BeginInit();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(10, 28);
      this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(81, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Напряжение:";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(10, 84);
      this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(56, 14);
      this.label2.TabIndex = 1;
      this.label2.Text = "Частота:";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(10, 57);
      this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(131, 14);
      this.label3.TabIndex = 2;
      this.label3.Text = "Ограничение по току:";
      // 
      // nudVoltage
      // 
      this.nudVoltage.Location = new System.Drawing.Point(152, 28);
      this.nudVoltage.Margin = new System.Windows.Forms.Padding(2);
      this.nudVoltage.Maximum = new decimal(new int[] {
            140,
            0,
            0,
            0});
      this.nudVoltage.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
      this.nudVoltage.Name = "nudVoltage";
      this.nudVoltage.Size = new System.Drawing.Size(51, 22);
      this.nudVoltage.TabIndex = 3;
      this.nudVoltage.Value = new decimal(new int[] {
            130,
            0,
            0,
            0});
      // 
      // nudCurrentLimit
      // 
      this.nudCurrentLimit.Location = new System.Drawing.Point(152, 57);
      this.nudCurrentLimit.Margin = new System.Windows.Forms.Padding(2);
      this.nudCurrentLimit.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
      this.nudCurrentLimit.Name = "nudCurrentLimit";
      this.nudCurrentLimit.Size = new System.Drawing.Size(51, 22);
      this.nudCurrentLimit.TabIndex = 4;
      this.nudCurrentLimit.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
      // 
      // nudFrequency
      // 
      this.nudFrequency.Location = new System.Drawing.Point(152, 84);
      this.nudFrequency.Margin = new System.Windows.Forms.Padding(2);
      this.nudFrequency.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
      this.nudFrequency.Name = "nudFrequency";
      this.nudFrequency.Size = new System.Drawing.Size(51, 22);
      this.nudFrequency.TabIndex = 5;
      this.nudFrequency.Value = new decimal(new int[] {
            400,
            0,
            0,
            0});
      // 
      // bSet
      // 
      this.bSet.Location = new System.Drawing.Point(227, 29);
      this.bSet.Margin = new System.Windows.Forms.Padding(2);
      this.bSet.Name = "bSet";
      this.bSet.Size = new System.Drawing.Size(115, 76);
      this.bSet.TabIndex = 6;
      this.bSet.Text = "Установить";
      this.bSet.UseVisualStyleBackColor = true;
      this.bSet.Click += new System.EventHandler(this.bSet_Click);
      // 
      // nudRealVoltage
      // 
      this.nudRealVoltage.Location = new System.Drawing.Point(5, 24);
      this.nudRealVoltage.Margin = new System.Windows.Forms.Padding(2);
      this.nudRealVoltage.Maximum = new decimal(new int[] {
            150000,
            0,
            0,
            0});
      this.nudRealVoltage.Name = "nudRealVoltage";
      this.nudRealVoltage.Size = new System.Drawing.Size(75, 22);
      this.nudRealVoltage.TabIndex = 8;
      this.nudRealVoltage.ThousandsSeparator = true;
      this.nudRealVoltage.Value = new decimal(new int[] {
            130000,
            0,
            0,
            0});
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.nudRealVoltage);
      this.groupBox1.Location = new System.Drawing.Point(13, 129);
      this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
      this.groupBox1.Size = new System.Drawing.Size(328, 57);
      this.groupBox1.TabIndex = 9;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Значение напряжения на клеммах (в мВ):";
      // 
      // bCalculate
      // 
      this.bCalculate.Location = new System.Drawing.Point(11, 220);
      this.bCalculate.Margin = new System.Windows.Forms.Padding(2);
      this.bCalculate.Name = "bCalculate";
      this.bCalculate.Size = new System.Drawing.Size(330, 35);
      this.bCalculate.TabIndex = 10;
      this.bCalculate.Text = "Рассчитать";
      this.bCalculate.UseVisualStyleBackColor = true;
      this.bCalculate.Click += new System.EventHandler(this.bCalculate_Click);
      // 
      // bSaveClose
      // 
      this.bSaveClose.Location = new System.Drawing.Point(11, 278);
      this.bSaveClose.Margin = new System.Windows.Forms.Padding(2);
      this.bSaveClose.Name = "bSaveClose";
      this.bSaveClose.Size = new System.Drawing.Size(330, 35);
      this.bSaveClose.TabIndex = 11;
      this.bSaveClose.Text = "Сохранить и закрыть";
      this.bSaveClose.UseVisualStyleBackColor = true;
      this.bSaveClose.Click += new System.EventHandler(this.bSaveClose_Click);
      // 
      // lLineResistance
      // 
      this.lLineResistance.AutoSize = true;
      this.lLineResistance.Location = new System.Drawing.Point(16, 196);
      this.lLineResistance.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.lLineResistance.Name = "lLineResistance";
      this.lLineResistance.Size = new System.Drawing.Size(137, 14);
      this.lLineResistance.TabIndex = 12;
      this.lLineResistance.Text = "Сопротивление линии:";
      // 
      // frmPS115VCalibrate
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(353, 325);
      this.Controls.Add(this.lLineResistance);
      this.Controls.Add(this.bSaveClose);
      this.Controls.Add(this.bCalculate);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.bSet);
      this.Controls.Add(this.nudFrequency);
      this.Controls.Add(this.nudCurrentLimit);
      this.Controls.Add(this.nudVoltage);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.Margin = new System.Windows.Forms.Padding(2);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "frmPS115VCalibrate";
      this.Text = "Калибровка ИП 115В";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPS115VCalibrate_FormClosing);
      this.Load += new System.EventHandler(this.frmPS115VCalibrate_Load);
      ((System.ComponentModel.ISupportInitialize)(this.nudVoltage)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.nudCurrentLimit)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.nudFrequency)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.nudRealVoltage)).EndInit();
      this.groupBox1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudVoltage;
        private System.Windows.Forms.NumericUpDown nudCurrentLimit;
        private System.Windows.Forms.NumericUpDown nudFrequency;
        private System.Windows.Forms.Button bSet;
        private System.Windows.Forms.NumericUpDown nudRealVoltage;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button bCalculate;
        private System.Windows.Forms.Button bSaveClose;
        private System.Windows.Forms.Label lLineResistance;
    }
}