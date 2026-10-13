namespace KPK2.MyControl.DISS.FrmDiss32
{
    partial class CaptureModeControl
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
            this.si = new UserControlsKPK2.SegmentIndicatorControl();
            this.gbTimerTask = new System.Windows.Forms.GroupBox();
            this.btnContinueTask = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.siWorkTime = new UserControlsKPK2.SegmentIndication();
            this.label1 = new System.Windows.Forms.Label();
            this.gbTimerTask.SuspendLayout();
            this.SuspendLayout();
            // 
            // si
            // 
            this.si.CalculationMethod = UserControlsKPK2.Enums.CalcMethod.MinMax;
            this.si.CheckValueFunction = null;
            this.si.FractPart = 0;
            this.si.Inactive = false;
            this.si.Location = new System.Drawing.Point(15, 17);
            this.si.Margin = new System.Windows.Forms.Padding(0);
            this.si.MaxValue = 0D;
            this.si.MinValue = 0D;
            this.si.Name = "si";
            this.si.NameLabelDown = "ПС 1";
            this.si.NameLabelUp = "W, км/ч";
            this.si.NameSegment = "SegmentIndication1";
            this.si.ParameterName = null;
            this.si.Size = new System.Drawing.Size(91, 75);
            this.si.TabIndex = 0;
            this.si.TestId = 0;
            this.si.UsbBaseNew = null;
            this.si.Value = 0D;
            this.si.ValueString = "0";
            this.si.WholePart = 0;
            // 
            // gbTimerTask
            // 
            this.gbTimerTask.Controls.Add(this.btnContinueTask);
            this.gbTimerTask.Controls.Add(this.label2);
            this.gbTimerTask.Controls.Add(this.siWorkTime);
            this.gbTimerTask.Controls.Add(this.label1);
            this.gbTimerTask.Location = new System.Drawing.Point(149, 3);
            this.gbTimerTask.Name = "gbTimerTask";
            this.gbTimerTask.Size = new System.Drawing.Size(578, 138);
            this.gbTimerTask.TabIndex = 23;
            this.gbTimerTask.TabStop = false;
            this.gbTimerTask.Text = "Таймер отработки задач";
            // 
            // btnContinueTask
            // 
            this.btnContinueTask.Location = new System.Drawing.Point(129, 90);
            this.btnContinueTask.Name = "btnContinueTask";
            this.btnContinueTask.Size = new System.Drawing.Size(122, 37);
            this.btnContinueTask.TabIndex = 24;
            this.btnContinueTask.Text = "Пропустить";
            this.btnContinueTask.UseVisualStyleBackColor = true;
            this.btnContinueTask.Visible = false;
            this.btnContinueTask.Click += new System.EventHandler(this.btnContinueTask_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.label2.Location = new System.Drawing.Point(66, 110);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 17);
            this.label2.TabIndex = 22;
            this.label2.Text = "сек.";
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
            this.siWorkTime.Location = new System.Drawing.Point(6, 90);
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
            this.label1.Location = new System.Drawing.Point(6, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(566, 66);
            this.label1.TabIndex = 21;
            // 
            // CaptureModeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbTimerTask);
            this.Controls.Add(this.si);
            this.Name = "CaptureModeControl";
            this.Size = new System.Drawing.Size(730, 162);
            this.gbTimerTask.ResumeLayout(false);
            this.gbTimerTask.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private UserControlsKPK2.SegmentIndicatorControl si;
        private System.Windows.Forms.GroupBox gbTimerTask;
        private System.Windows.Forms.Label label2;
        private UserControlsKPK2.SegmentIndication siWorkTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnContinueTask;
    }
}
