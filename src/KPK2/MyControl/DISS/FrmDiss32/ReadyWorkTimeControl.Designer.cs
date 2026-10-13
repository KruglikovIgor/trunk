namespace KPK2.MyControl.DISS.FrmDiss32
{
    partial class ReadyWorkTimeControl
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.siWorkTime = new UserControlsKPK2.SegmentIndication();
            this.label1 = new System.Windows.Forms.Label();
            this.gbTimerTask = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.siUS = new UserControlsKPK2.SegmentIndicatorControl();
            this.siUp = new UserControlsKPK2.SegmentIndicatorControl();
            this.gbTimerTask.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // siWorkTime
            // 
            this.siWorkTime.BackColor = System.Drawing.Color.Transparent;
            this.siWorkTime.BGColor = System.Drawing.Color.Black;
            this.siWorkTime.CalculationMethod = UserControlsKPK2.Enums.CalcMethod.MinMax;
            this.siWorkTime.DarkColor = System.Drawing.Color.Transparent;
            this.siWorkTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.siWorkTime.FractPart = 0;
            this.siWorkTime.Inactive = false;
            this.siWorkTime.InactiveBGColor = System.Drawing.Color.Gray;
            this.siWorkTime.InvalidColor = System.Drawing.Color.Red;
            this.siWorkTime.LightColor = System.Drawing.Color.Lime;
            this.siWorkTime.Location = new System.Drawing.Point(11, 96);
            this.siWorkTime.Margin = new System.Windows.Forms.Padding(8);
            this.siWorkTime.MaxValue = 180D;
            this.siWorkTime.MinValue = 0D;
            this.siWorkTime.Name = "siWorkTime";
            this.siWorkTime.NeedAlign = true;
            this.siWorkTime.Size = new System.Drawing.Size(58, 37);
            this.siWorkTime.TabIndex = 20;
            this.siWorkTime.Unit = "";
            this.siWorkTime.Value = 0D;
            this.siWorkTime.ValueString = "000";
            this.siWorkTime.WholePart = 0;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F);
            this.label1.Location = new System.Drawing.Point(0, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(331, 66);
            this.label1.TabIndex = 21;
            // 
            // gbTimerTask
            // 
            this.gbTimerTask.Controls.Add(this.label2);
            this.gbTimerTask.Controls.Add(this.siWorkTime);
            this.gbTimerTask.Controls.Add(this.label1);
            this.gbTimerTask.Location = new System.Drawing.Point(211, 3);
            this.gbTimerTask.Name = "gbTimerTask";
            this.gbTimerTask.Size = new System.Drawing.Size(343, 138);
            this.gbTimerTask.TabIndex = 22;
            this.gbTimerTask.TabStop = false;
            this.gbTimerTask.Text = "Таймер отработки задач";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.label2.Location = new System.Drawing.Point(70, 116);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 17);
            this.label2.TabIndex = 22;
            this.label2.Text = "сек.";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.siUS);
            this.groupBox1.Controls.Add(this.siUp);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(202, 135);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            // 
            // siUS
            // 
            this.siUS.CalculationMethod = UserControlsKPK2.Enums.CalcMethod.MinMax;
            this.siUS.CheckValueFunction = null;
            this.siUS.FractPart = 0;
            this.siUS.Inactive = false;
            this.siUS.Location = new System.Drawing.Point(103, 16);
            this.siUS.Margin = new System.Windows.Forms.Padding(0);
            this.siUS.MaxValue = 0D;
            this.siUS.MinValue = 0D;
            this.siUS.Name = "siUS";
            this.siUS.NameLabelDown = "УС 0";
            this.siUS.NameLabelUp = "α, град";
            this.siUS.NameSegment = "IndicatorWxr";
            this.siUS.ParameterName = null;
            this.siUS.Size = new System.Drawing.Size(91, 75);
            this.siUS.TabIndex = 2;
            this.siUS.TestId = 0;
            this.siUS.UsbBaseNew = null;
            this.siUS.Value = 0D;
            this.siUS.ValueString = "0";
            this.siUS.WholePart = 0;
            // 
            // siUp
            // 
            this.siUp.CalculationMethod = UserControlsKPK2.Enums.CalcMethod.MinMax;
            this.siUp.CheckValueFunction = null;
            this.siUp.FractPart = 0;
            this.siUp.Inactive = false;
            this.siUp.Location = new System.Drawing.Point(3, 16);
            this.siUp.Margin = new System.Windows.Forms.Padding(0);
            this.siUp.MaxValue = 0D;
            this.siUp.MinValue = 0D;
            this.siUp.Name = "siUp";
            this.siUp.NameLabelDown = "Вперед";
            this.siUp.NameLabelUp = "V, м/c";
            this.siUp.NameSegment = "IndicatorWxr";
            this.siUp.ParameterName = null;
            this.siUp.Size = new System.Drawing.Size(91, 75);
            this.siUp.TabIndex = 0;
            this.siUp.TestId = 0;
            this.siUp.UsbBaseNew = null;
            this.siUp.Value = 0D;
            this.siUp.ValueString = "0";
            this.siUp.WholePart = 0;
            // 
            // ReadyWorkTimeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbTimerTask);
            this.Name = "ReadyWorkTimeControl";
            this.Size = new System.Drawing.Size(560, 144);
            this.gbTimerTask.ResumeLayout(false);
            this.gbTimerTask.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private UserControlsKPK2.SegmentIndication siWorkTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbTimerTask;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private UserControlsKPK2.SegmentIndicatorControl siUp;
        private UserControlsKPK2.SegmentIndicatorControl siUS;
    }
}
