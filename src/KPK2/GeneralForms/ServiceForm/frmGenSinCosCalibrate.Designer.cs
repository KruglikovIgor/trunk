namespace KPK2.GeneralForms.ServiceForm
{
    partial class frmGenSinCosCalibrate
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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      this.cbChannel = new System.Windows.Forms.ComboBox();
      this.label1 = new System.Windows.Forms.Label();
      this.dgvValues = new System.Windows.Forms.DataGridView();
      this.сId = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.сChannel = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.сIdealValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.сValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.cMeasure = new System.Windows.Forms.DataGridViewButtonColumn();
      this.bSave = new System.Windows.Forms.Button();
      this.label2 = new System.Windows.Forms.Label();
      this.cbFrequency = new System.Windows.Forms.ComboBox();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.nudLimit = new System.Windows.Forms.NumericUpDown();
      this.bShiftCalibrate = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.dgvValues)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.nudLimit)).BeginInit();
      this.SuspendLayout();
      // 
      // cbChannel
      // 
      this.cbChannel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cbChannel.FormattingEnabled = true;
      this.cbChannel.Items.AddRange(new object[] {
            "0 (оп.)",
            "1 (sin)",
            "2 (cos)"});
      this.cbChannel.Location = new System.Drawing.Point(62, 17);
      this.cbChannel.Name = "cbChannel";
      this.cbChannel.Size = new System.Drawing.Size(121, 22);
      this.cbChannel.TabIndex = 0;
      this.cbChannel.SelectedIndexChanged += new System.EventHandler(this.cbChannel_SelectedIndexChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 25);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(44, 14);
      this.label1.TabIndex = 1;
      this.label1.Text = "Канал:";
      // 
      // dgvValues
      // 
      this.dgvValues.AllowUserToResizeColumns = false;
      this.dgvValues.AllowUserToResizeRows = false;
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dgvValues.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      this.dgvValues.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvValues.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.сId,
            this.сChannel,
            this.сIdealValue,
            this.сValue,
            this.cMeasure});
      this.dgvValues.Location = new System.Drawing.Point(12, 63);
      this.dgvValues.Name = "dgvValues";
      this.dgvValues.ReadOnly = true;
      this.dgvValues.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
      this.dgvValues.Size = new System.Drawing.Size(420, 211);
      this.dgvValues.TabIndex = 2;
      this.dgvValues.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvValues_CellClick);
      this.dgvValues.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvValues_CellValueChanged);
      this.dgvValues.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvValues_RowsAdded);
      this.dgvValues.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dgvValues_UserDeletingRow);
      // 
      // сId
      // 
      this.сId.HeaderText = "Column1";
      this.сId.Name = "сId";
      this.сId.ReadOnly = true;
      this.сId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      this.сId.Visible = false;
      // 
      // сChannel
      // 
      this.сChannel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.сChannel.HeaderText = "Канал";
      this.сChannel.Name = "сChannel";
      this.сChannel.ReadOnly = true;
      this.сChannel.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      // 
      // сIdealValue
      // 
      this.сIdealValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.сIdealValue.HeaderText = "Эталонное значение";
      this.сIdealValue.Name = "сIdealValue";
      this.сIdealValue.ReadOnly = true;
      this.сIdealValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      // 
      // сValue
      // 
      this.сValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.сValue.HeaderText = "Заданное значение";
      this.сValue.Name = "сValue";
      this.сValue.ReadOnly = true;
      this.сValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      // 
      // cMeasure
      // 
      this.cMeasure.HeaderText = "";
      this.cMeasure.Name = "cMeasure";
      this.cMeasure.ReadOnly = true;
      this.cMeasure.Text = "Измерить";
      this.cMeasure.UseColumnTextForButtonValue = true;
      // 
      // bSave
      // 
      this.bSave.Enabled = false;
      this.bSave.Location = new System.Drawing.Point(164, 438);
      this.bSave.Name = "bSave";
      this.bSave.Size = new System.Drawing.Size(115, 34);
      this.bSave.TabIndex = 3;
      this.bSave.Text = "Сохранить";
      this.bSave.UseVisualStyleBackColor = true;
      this.bSave.Click += new System.EventHandler(this.bSave_Click);
      // 
      // label2
      // 
      this.label2.Location = new System.Drawing.Point(12, 356);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(420, 79);
      this.label2.TabIndex = 4;
      this.label2.Text = "Примечание:\r\nВсе значения задаются в мВ.\r\nВначале вводится \"Заданное значение\" (н" +
    "апр. 1000) после чего в поле \"Эталонное значение\" вводится значение с вольтметра" +
    "";
      // 
      // cbFrequency
      // 
      this.cbFrequency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cbFrequency.FormattingEnabled = true;
      this.cbFrequency.Items.AddRange(new object[] {
            "20",
            "30",
            "40",
            "100",
            "200"});
      this.cbFrequency.Location = new System.Drawing.Point(329, 17);
      this.cbFrequency.Name = "cbFrequency";
      this.cbFrequency.Size = new System.Drawing.Size(77, 22);
      this.cbFrequency.TabIndex = 5;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(267, 25);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(56, 14);
      this.label3.TabIndex = 6;
      this.label3.Text = "Частота:";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(412, 25);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(20, 14);
      this.label4.TabIndex = 7;
      this.label4.Text = "Гц";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(12, 311);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(259, 14);
      this.label5.TabIndex = 8;
      this.label5.Text = "Фиксированная амплитуда для углов в у.е.:";
      // 
      // nudLimit
      // 
      this.nudLimit.Increment = new decimal(new int[] {
            0,
            0,
            0,
            0});
      this.nudLimit.Location = new System.Drawing.Point(312, 309);
      this.nudLimit.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
      this.nudLimit.Name = "nudLimit";
      this.nudLimit.ReadOnly = true;
      this.nudLimit.Size = new System.Drawing.Size(120, 22);
      this.nudLimit.TabIndex = 9;
      this.nudLimit.ValueChanged += new System.EventHandler(this.nudLimit_ValueChanged);
      this.nudLimit.Click += new System.EventHandler(this.nudLimit_Click);
      this.nudLimit.DoubleClick += new System.EventHandler(this.nudLimit_DoubleClick);
      // 
      // bShiftCalibrate
      // 
      this.bShiftCalibrate.Location = new System.Drawing.Point(219, 279);
      this.bShiftCalibrate.Name = "bShiftCalibrate";
      this.bShiftCalibrate.Size = new System.Drawing.Size(213, 24);
      this.bShiftCalibrate.TabIndex = 10;
      this.bShiftCalibrate.Text = "Калибровка сдвига фазы";
      this.bShiftCalibrate.UseVisualStyleBackColor = true;
      this.bShiftCalibrate.Click += new System.EventHandler(this.bShiftCalibrate_Click);
      // 
      // frmGenSinCosCalibrate
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(444, 477);
      this.Controls.Add(this.bShiftCalibrate);
      this.Controls.Add(this.nudLimit);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.cbFrequency);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.bSave);
      this.Controls.Add(this.dgvValues);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.cbChannel);
      this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "frmGenSinCosCalibrate";
      this.Text = "Калибровка генератора сигналов переменного тока";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmGenSinCosCalibrate_FormClosing);
      this.Load += new System.EventHandler(this.frmGenSinCosCalibrate_Load);
      ((System.ComponentModel.ISupportInitialize)(this.dgvValues)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.nudLimit)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbChannel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvValues;
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbFrequency;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridViewTextBoxColumn сId;
        private System.Windows.Forms.DataGridViewTextBoxColumn сChannel;
        private System.Windows.Forms.DataGridViewTextBoxColumn сIdealValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn сValue;
        private System.Windows.Forms.DataGridViewButtonColumn cMeasure;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nudLimit;
        private System.Windows.Forms.Button bShiftCalibrate;
    }
}