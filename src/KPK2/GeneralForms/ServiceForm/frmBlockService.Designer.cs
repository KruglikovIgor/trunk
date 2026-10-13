namespace KPK2.GeneralForms.ServiceForm
{
  partial class frmBlockService
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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      this.dgvBlocks = new System.Windows.Forms.DataGridView();
      this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.BlockName = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.SerNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.TBO = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.TBR = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Delete = new System.Windows.Forms.DataGridViewButtonColumn();
      this.dgvTests = new System.Windows.Forms.DataGridView();
      this.bSave = new System.Windows.Forms.Button();
      this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.cDeviceId = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.cType = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.cNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.cDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.cEmployee = new System.Windows.Forms.DataGridViewTextBoxColumn();
      ((System.ComponentModel.ISupportInitialize)(this.dgvBlocks)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.dgvTests)).BeginInit();
      this.SuspendLayout();
      // 
      // dgvBlocks
      // 
      this.dgvBlocks.AllowUserToAddRows = false;
      this.dgvBlocks.AllowUserToDeleteRows = false;
      this.dgvBlocks.AllowUserToResizeColumns = false;
      this.dgvBlocks.AllowUserToResizeRows = false;
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dgvBlocks.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      this.dgvBlocks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvBlocks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.BlockName,
            this.SerNo,
            this.TBO,
            this.TBR,
            this.Delete});
      this.dgvBlocks.Location = new System.Drawing.Point(12, 151);
      this.dgvBlocks.Name = "dgvBlocks";
      this.dgvBlocks.RowHeadersVisible = false;
      this.dgvBlocks.Size = new System.Drawing.Size(810, 118);
      this.dgvBlocks.TabIndex = 12;
      this.dgvBlocks.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBlocks_CellClick);
      this.dgvBlocks.Click += new System.EventHandler(this.dgvBlocks_Click);
      // 
      // Id
      // 
      this.Id.HeaderText = "Id";
      this.Id.Name = "Id";
      this.Id.Visible = false;
      // 
      // BlockName
      // 
      this.BlockName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.BlockName.HeaderText = "Наименование";
      this.BlockName.Name = "BlockName";
      this.BlockName.ReadOnly = true;
      this.BlockName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      // 
      // SerNo
      // 
      this.SerNo.HeaderText = "Заводской №";
      this.SerNo.Name = "SerNo";
      this.SerNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      this.SerNo.Width = 185;
      // 
      // TBO
      // 
      this.TBO.HeaderText = "Наработка СНЭ";
      this.TBO.Name = "TBO";
      this.TBO.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      this.TBO.Width = 185;
      // 
      // TBR
      // 
      this.TBR.HeaderText = "Наработка ППР";
      this.TBR.Name = "TBR";
      this.TBR.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      this.TBR.Width = 185;
      // 
      // Delete
      // 
      this.Delete.HeaderText = "X";
      this.Delete.Name = "Delete";
      this.Delete.ReadOnly = true;
      this.Delete.Width = 35;
      // 
      // dgvTests
      // 
      this.dgvTests.AllowUserToAddRows = false;
      this.dgvTests.AllowUserToDeleteRows = false;
      this.dgvTests.AllowUserToResizeColumns = false;
      this.dgvTests.AllowUserToResizeRows = false;
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
      dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dgvTests.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
      this.dgvTests.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvTests.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.cDeviceId,
            this.cType,
            this.cNumber,
            this.cDate,
            this.cEmployee});
      this.dgvTests.Location = new System.Drawing.Point(12, 12);
      this.dgvTests.Name = "dgvTests";
      this.dgvTests.RowHeadersVisible = false;
      this.dgvTests.Size = new System.Drawing.Size(810, 118);
      this.dgvTests.TabIndex = 13;
      this.dgvTests.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTests_CellClick);
      // 
      // bSave
      // 
      this.bSave.Location = new System.Drawing.Point(351, 336);
      this.bSave.Name = "bSave";
      this.bSave.Size = new System.Drawing.Size(140, 30);
      this.bSave.TabIndex = 14;
      this.bSave.Text = "Сохранить";
      this.bSave.UseVisualStyleBackColor = true;
      this.bSave.Click += new System.EventHandler(this.bSave_Click);
      // 
      // dataGridViewTextBoxColumn1
      // 
      this.dataGridViewTextBoxColumn1.HeaderText = "Id";
      this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
      this.dataGridViewTextBoxColumn1.Visible = false;
      // 
      // cDeviceId
      // 
      this.cDeviceId.HeaderText = "DeviceId";
      this.cDeviceId.Name = "cDeviceId";
      this.cDeviceId.Visible = false;
      // 
      // cType
      // 
      this.cType.HeaderText = "Тип прибора";
      this.cType.Name = "cType";
      // 
      // cNumber
      // 
      this.cNumber.HeaderText = "Номер прибора";
      this.cNumber.Name = "cNumber";
      // 
      // cDate
      // 
      this.cDate.HeaderText = "Дата проверки";
      this.cDate.Name = "cDate";
      // 
      // cEmployee
      // 
      this.cEmployee.HeaderText = "Исполнитель";
      this.cEmployee.Name = "cEmployee";
      // 
      // frmBlockService
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.ClientSize = new System.Drawing.Size(838, 378);
      this.Controls.Add(this.bSave);
      this.Controls.Add(this.dgvTests);
      this.Controls.Add(this.dgvBlocks);
      this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "frmBlockService";
      this.Text = "Управление блоками";
      this.Load += new System.EventHandler(this.frmBlockService_Load);
      ((System.ComponentModel.ISupportInitialize)(this.dgvBlocks)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.dgvTests)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.DataGridView dgvBlocks;
    private System.Windows.Forms.DataGridViewTextBoxColumn Id;
    private System.Windows.Forms.DataGridViewTextBoxColumn BlockName;
    private System.Windows.Forms.DataGridViewTextBoxColumn SerNo;
    private System.Windows.Forms.DataGridViewTextBoxColumn TBO;
    private System.Windows.Forms.DataGridViewTextBoxColumn TBR;
    private System.Windows.Forms.DataGridViewButtonColumn Delete;
    private System.Windows.Forms.DataGridView dgvTests;
    private System.Windows.Forms.Button bSave;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private System.Windows.Forms.DataGridViewTextBoxColumn cDeviceId;
    private System.Windows.Forms.DataGridViewTextBoxColumn cType;
    private System.Windows.Forms.DataGridViewTextBoxColumn cNumber;
    private System.Windows.Forms.DataGridViewTextBoxColumn cDate;
    private System.Windows.Forms.DataGridViewTextBoxColumn cEmployee;
  }
}

