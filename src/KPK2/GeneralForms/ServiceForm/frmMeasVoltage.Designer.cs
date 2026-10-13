namespace KPK2.GeneralForms.ServiceForm
{
  partial class frmMeasVoltage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMeasVoltage));
            this.btnAuto = new System.Windows.Forms.Button();
            this.btnNextStep = new System.Windows.Forms.Button();
            this.btnMeasure = new System.Windows.Forms.Button();
            this.dgvValues = new System.Windows.Forms.DataGridView();
            this.colUset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBench = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMeas = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDelta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.labelNameIK = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rbChannel2 = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.rbChannel1 = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgvValues)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAuto
            // 
            this.btnAuto.Location = new System.Drawing.Point(409, 12);
            this.btnAuto.Name = "btnAuto";
            this.btnAuto.Size = new System.Drawing.Size(195, 23);
            this.btnAuto.TabIndex = 0;
            this.btnAuto.Text = "Авто";
            this.btnAuto.UseVisualStyleBackColor = true;
            this.btnAuto.Click += new System.EventHandler(this.btnAuto_Click);
            // 
            // btnNextStep
            // 
            this.btnNextStep.Location = new System.Drawing.Point(12, 81);
            this.btnNextStep.Name = "btnNextStep";
            this.btnNextStep.Size = new System.Drawing.Size(195, 23);
            this.btnNextStep.TabIndex = 1;
            this.btnNextStep.Text = "Следующий шаг";
            this.btnNextStep.UseVisualStyleBackColor = true;
            this.btnNextStep.Click += new System.EventHandler(this.btnNextStep_Click);
            // 
            // btnMeasure
            // 
            this.btnMeasure.Location = new System.Drawing.Point(409, 81);
            this.btnMeasure.Name = "btnMeasure";
            this.btnMeasure.Size = new System.Drawing.Size(195, 23);
            this.btnMeasure.TabIndex = 2;
            this.btnMeasure.Text = "Измерить и записать";
            this.btnMeasure.UseVisualStyleBackColor = true;
            this.btnMeasure.Click += new System.EventHandler(this.btnMeasure_Click);
            // 
            // dgvValues
            // 
            this.dgvValues.AllowUserToAddRows = false;
            this.dgvValues.AllowUserToDeleteRows = false;
            this.dgvValues.AllowUserToOrderColumns = true;
            this.dgvValues.AllowUserToResizeColumns = false;
            this.dgvValues.AllowUserToResizeRows = false;
            this.dgvValues.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvValues.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colUset,
            this.colBench,
            this.colMeas,
            this.colDelta});
            this.dgvValues.Location = new System.Drawing.Point(12, 144);
            this.dgvValues.Name = "dgvValues";
            this.dgvValues.RowHeadersVisible = false;
            this.dgvValues.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.Format = "N4";
            dataGridViewCellStyle1.NullValue = null;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvValues.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvValues.Size = new System.Drawing.Size(954, 331);
            this.dgvValues.TabIndex = 3;
            this.dgvValues.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvValues_CellClick);
            // 
            // colUset
            // 
            this.colUset.HeaderText = "Uуст, В";
            this.colUset.Name = "colUset";
            // 
            // colBench
            // 
            this.colBench.HeaderText = "Uэт, B";
            this.colBench.Name = "colBench";
            // 
            // colMeas
            // 
            this.colMeas.HeaderText = "Uизм, B";
            this.colMeas.Name = "colMeas";
            // 
            // colDelta
            // 
            this.colDelta.HeaderText = "ΔU, B";
            this.colDelta.Name = "colDelta";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 14);
            this.label1.TabIndex = 4;
            this.label1.Text = "Измерительный канал:";
            // 
            // labelNameIK
            // 
            this.labelNameIK.AutoSize = true;
            this.labelNameIK.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.labelNameIK.Location = new System.Drawing.Point(151, 12);
            this.labelNameIK.Name = "labelNameIK";
            this.labelNameIK.Size = new System.Drawing.Size(0, 17);
            this.labelNameIK.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.labelNameIK);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(290, 42);
            this.panel1.TabIndex = 8;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.rbChannel2);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.rbChannel1);
            this.panel2.Location = new System.Drawing.Point(759, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(207, 42);
            this.panel2.TabIndex = 9;
            // 
            // rbChannel2
            // 
            this.rbChannel2.AutoSize = true;
            this.rbChannel2.Location = new System.Drawing.Point(153, 10);
            this.rbChannel2.Name = "rbChannel2";
            this.rbChannel2.Size = new System.Drawing.Size(32, 18);
            this.rbChannel2.TabIndex = 2;
            this.rbChannel2.Text = "2";
            this.rbChannel2.UseVisualStyleBackColor = true;
            this.rbChannel2.CheckedChanged += new System.EventHandler(this.rbChannel2_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 14);
            this.label2.TabIndex = 1;
            this.label2.Text = "Канал ИП:";
            // 
            // rbChannel1
            // 
            this.rbChannel1.AutoSize = true;
            this.rbChannel1.Checked = true;
            this.rbChannel1.Location = new System.Drawing.Point(95, 10);
            this.rbChannel1.Name = "rbChannel1";
            this.rbChannel1.Size = new System.Drawing.Size(32, 18);
            this.rbChannel1.TabIndex = 0;
            this.rbChannel1.TabStop = true;
            this.rbChannel1.Text = "1";
            this.rbChannel1.UseVisualStyleBackColor = true;
            this.rbChannel1.CheckedChanged += new System.EventHandler(this.rbChannel2_CheckedChanged);
            // 
            // frmMeasVoltage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(978, 501);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dgvValues);
            this.Controls.Add(this.btnMeasure);
            this.Controls.Add(this.btnNextStep);
            this.Controls.Add(this.btnAuto);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMeasVoltage";
            this.Text = "Автоматизированная поверка выходного напряжения";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMeasVoltage_FormClosing);
            this.Load += new System.EventHandler(this.frmMeasVoltage_Load);
            this.Shown += new System.EventHandler(this.frmMeasVoltage_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvValues)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button btnAuto;
    private System.Windows.Forms.Button btnNextStep;
    private System.Windows.Forms.Button btnMeasure;
    private System.Windows.Forms.DataGridView dgvValues;
    private System.Windows.Forms.DataGridViewTextBoxColumn colUset;
    private System.Windows.Forms.DataGridViewTextBoxColumn colBench;
    private System.Windows.Forms.DataGridViewTextBoxColumn colMeas;
    private System.Windows.Forms.DataGridViewTextBoxColumn colDelta;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label labelNameIK;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.RadioButton rbChannel2;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.RadioButton rbChannel1;
  }
}