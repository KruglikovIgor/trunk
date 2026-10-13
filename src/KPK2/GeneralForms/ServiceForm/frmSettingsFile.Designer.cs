namespace KPK2.GeneralForms.ServiceForm
{
  partial class frmSettingsFile
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
      this.dgvSettings = new System.Windows.Forms.DataGridView();
      this.bSave = new System.Windows.Forms.Button();
      this.bClose = new System.Windows.Forms.Button();
      this.cSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
      this.cPName = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.cValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
      ((System.ComponentModel.ISupportInitialize)(this.dgvSettings)).BeginInit();
      this.SuspendLayout();
      // 
      // dgvSettings
      // 
      this.dgvSettings.AllowUserToAddRows = false;
      this.dgvSettings.AllowUserToDeleteRows = false;
      this.dgvSettings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvSettings.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cSelect,
            this.cPName,
            this.cValue});
      this.dgvSettings.Location = new System.Drawing.Point(12, 12);
      this.dgvSettings.Name = "dgvSettings";
      this.dgvSettings.Size = new System.Drawing.Size(651, 364);
      this.dgvSettings.TabIndex = 0;
      this.dgvSettings.EditModeChanged += new System.EventHandler(this.dgvSettings_EditModeChanged);
      this.dgvSettings.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSettings_CellLeave);
      this.dgvSettings.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvSettings_CellValidating);
      this.dgvSettings.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSettings_CellValueChanged);
      this.dgvSettings.CurrentCellChanged += new System.EventHandler(this.dgvSettings_CurrentCellChanged);
      this.dgvSettings.SelectionChanged += new System.EventHandler(this.dgvSettings_SelectionChanged);
      // 
      // bSave
      // 
      this.bSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.bSave.Location = new System.Drawing.Point(12, 386);
      this.bSave.Name = "bSave";
      this.bSave.Size = new System.Drawing.Size(145, 43);
      this.bSave.TabIndex = 1;
      this.bSave.Text = "Сохранить";
      this.bSave.UseVisualStyleBackColor = true;
      this.bSave.Click += new System.EventHandler(this.bSave_Click);
      // 
      // bClose
      // 
      this.bClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.bClose.Location = new System.Drawing.Point(518, 386);
      this.bClose.Name = "bClose";
      this.bClose.Size = new System.Drawing.Size(145, 43);
      this.bClose.TabIndex = 2;
      this.bClose.Text = "Закрыть";
      this.bClose.UseVisualStyleBackColor = true;
      this.bClose.Click += new System.EventHandler(this.bClose_Click);
      // 
      // cSelect
      // 
      this.cSelect.HeaderText = "Выбор параметра";
      this.cSelect.Name = "cSelect";
      // 
      // cPName
      // 
      this.cPName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
      this.cPName.HeaderText = "Название параметра";
      this.cPName.Name = "cPName";
      this.cPName.ReadOnly = true;
      this.cPName.Width = 135;
      // 
      // cValue
      // 
      this.cValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.cValue.HeaderText = "Значение параметра";
      this.cValue.Name = "cValue";
      // 
      // frmSettingsFile
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(675, 441);
      this.Controls.Add(this.bClose);
      this.Controls.Add(this.bSave);
      this.Controls.Add(this.dgvSettings);
      this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.Name = "frmSettingsFile";
      this.Text = "Создание настроечного файла";
      this.Load += new System.EventHandler(this.frmSettingsFile_Load);
      ((System.ComponentModel.ISupportInitialize)(this.dgvSettings)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.DataGridView dgvSettings;
    private System.Windows.Forms.Button bSave;
    private System.Windows.Forms.Button bClose;
    private System.Windows.Forms.DataGridViewCheckBoxColumn cSelect;
    private System.Windows.Forms.DataGridViewTextBoxColumn cPName;
    private System.Windows.Forms.DataGridViewTextBoxColumn cValue;

  }
}