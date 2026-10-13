namespace KPK2.GeneralForms.ServiceForm
{
  partial class frmGenSinCosAngleCalibrate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGenSinCosAngleCalibrate));
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.bSave = new System.Windows.Forms.Button();
            this.dgvValues = new System.Windows.Forms.DataGridView();
            this.сId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.сChannel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.сIdealValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.сValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.cbChannel = new System.Windows.Forms.ComboBox();
            this.nudFrequency = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.nudVoltageRef = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.nudVoltagePhase = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvValues)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFrequency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVoltageRef)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVoltagePhase)).BeginInit();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(411, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 14);
            this.label4.TabIndex = 17;
            this.label4.Text = "Гц";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(266, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 14);
            this.label3.TabIndex = 16;
            this.label3.Text = "Частота:";
            // 
            // bSave
            // 
            this.bSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.bSave.Enabled = false;
            this.bSave.Location = new System.Drawing.Point(156, 437);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(115, 34);
            this.bSave.TabIndex = 13;
            this.bSave.Text = "Сохранить";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
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
            this.сValue});
            this.dgvValues.Location = new System.Drawing.Point(11, 58);
            this.dgvValues.Name = "dgvValues";
            this.dgvValues.ReadOnly = true;
            this.dgvValues.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvValues.Size = new System.Drawing.Size(420, 211);
            this.dgvValues.TabIndex = 12;
            this.dgvValues.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvValues_CellClick);
            this.dgvValues.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvValues_CellValueChanged);
            this.dgvValues.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvValues_RowsAdded);
            this.dgvValues.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgvValues_UserDeletedRow);
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 14);
            this.label1.TabIndex = 11;
            this.label1.Text = "Канал:";
            // 
            // cbChannel
            // 
            this.cbChannel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbChannel.FormattingEnabled = true;
            this.cbChannel.Items.AddRange(new object[] {
            "1 (sin)",
            "2 (cos)"});
            this.cbChannel.Location = new System.Drawing.Point(61, 12);
            this.cbChannel.Name = "cbChannel";
            this.cbChannel.Size = new System.Drawing.Size(121, 22);
            this.cbChannel.TabIndex = 10;
            this.cbChannel.SelectedIndexChanged += new System.EventHandler(this.cbChannel_SelectedIndexChanged);
            // 
            // nudFrequency
            // 
            this.nudFrequency.Location = new System.Drawing.Point(328, 13);
            this.nudFrequency.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.nudFrequency.Name = "nudFrequency";
            this.nudFrequency.Size = new System.Drawing.Size(77, 22);
            this.nudFrequency.TabIndex = 20;
            this.nudFrequency.Value = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.nudFrequency.ValueChanged += new System.EventHandler(this.nudFrequency_ValueChanged);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label2.Location = new System.Drawing.Point(11, 366);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(420, 64);
            this.label2.TabIndex = 21;
            this.label2.Text = resources.GetString("label2.Text");
            // 
            // nudVoltageRef
            // 
            this.nudVoltageRef.Location = new System.Drawing.Point(207, 275);
            this.nudVoltageRef.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.nudVoltageRef.Name = "nudVoltageRef";
            this.nudVoltageRef.Size = new System.Drawing.Size(77, 22);
            this.nudVoltageRef.TabIndex = 24;
            this.nudVoltageRef.ValueChanged += new System.EventHandler(this.nudVoltageRef_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(290, 282);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(22, 14);
            this.label5.TabIndex = 23;
            this.label5.Text = "мВ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 281);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(190, 14);
            this.label6.TabIndex = 22;
            this.label6.Text = "Напряжение опорного сигнала:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 306);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(189, 14);
            this.label7.TabIndex = 25;
            this.label7.Text = "Напряжение фазового сигнала:";
            // 
            // nudVoltagePhase
            // 
            this.nudVoltagePhase.Location = new System.Drawing.Point(207, 304);
            this.nudVoltagePhase.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.nudVoltagePhase.Name = "nudVoltagePhase";
            this.nudVoltagePhase.Size = new System.Drawing.Size(77, 22);
            this.nudVoltagePhase.TabIndex = 27;
            this.nudVoltagePhase.ValueChanged += new System.EventHandler(this.nudVoltagePhase_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(290, 311);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(22, 14);
            this.label8.TabIndex = 26;
            this.label8.Text = "мВ";
            // 
            // frmGenSinCosAngleCalibrate
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(447, 481);
            this.Controls.Add(this.nudVoltagePhase);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.nudVoltageRef);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nudFrequency);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.bSave);
            this.Controls.Add(this.dgvValues);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbChannel);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmGenSinCosAngleCalibrate";
            this.Text = "Калибровка сдвига фазы генератора сигнала";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmGenSinCosAngleCalibrate_FormClosing);
            this.Load += new System.EventHandler(this.frmGenSinCosAngleCalibrate_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvValues)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFrequency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVoltageRef)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVoltagePhase)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Button bSave;
    private System.Windows.Forms.DataGridView dgvValues;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox cbChannel;
    private System.Windows.Forms.NumericUpDown nudFrequency;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.DataGridViewTextBoxColumn сId;
    private System.Windows.Forms.DataGridViewTextBoxColumn сChannel;
    private System.Windows.Forms.DataGridViewTextBoxColumn сIdealValue;
    private System.Windows.Forms.DataGridViewTextBoxColumn сValue;
    private System.Windows.Forms.NumericUpDown nudVoltageRef;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.NumericUpDown nudVoltagePhase;
    private System.Windows.Forms.Label label8;
  }
}

