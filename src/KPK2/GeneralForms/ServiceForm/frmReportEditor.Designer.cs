namespace KPK2.GeneralForms.ServiceForm
{
    partial class frmReportEditor
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
      this.dgvReport = new System.Windows.Forms.DataGridView();
      this.bSave = new System.Windows.Forms.Button();
      this.cbDevices = new System.Windows.Forms.ComboBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.cbLanguage = new System.Windows.Forms.ComboBox();
      this.cId = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.cElementNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.cSubitemNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.cOrderNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.cCheckName = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.cUnits = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.cRequirement = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.cParamName = new System.Windows.Forms.DataGridViewTextBoxColumn();
      ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).BeginInit();
      this.SuspendLayout();
      // 
      // dgvReport
      // 
      this.dgvReport.AllowUserToDeleteRows = false;
      this.dgvReport.AllowUserToResizeRows = false;
      this.dgvReport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.dgvReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvReport.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cId,
            this.cElementNumber,
            this.cSubitemNumber,
            this.cOrderNumber,
            this.cCheckName,
            this.cUnits,
            this.cRequirement,
            this.cParamName});
      this.dgvReport.Location = new System.Drawing.Point(12, 51);
      this.dgvReport.Name = "dgvReport";
      this.dgvReport.RowHeadersVisible = false;
      this.dgvReport.Size = new System.Drawing.Size(992, 401);
      this.dgvReport.TabIndex = 3;
      this.dgvReport.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvReport_CellClick);
      this.dgvReport.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvReport_CellValueChanged);
      this.dgvReport.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvReport_RowsAdded);
      // 
      // bSave
      // 
      this.bSave.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.bSave.Enabled = false;
      this.bSave.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.bSave.Location = new System.Drawing.Point(384, 460);
      this.bSave.Name = "bSave";
      this.bSave.Size = new System.Drawing.Size(227, 50);
      this.bSave.TabIndex = 2;
      this.bSave.Text = "Сохранить";
      this.bSave.UseVisualStyleBackColor = true;
      this.bSave.Click += new System.EventHandler(this.bSave_Click);
      // 
      // cbDevices
      // 
      this.cbDevices.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.cbDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cbDevices.FormattingEnabled = true;
      this.cbDevices.Location = new System.Drawing.Point(395, 12);
      this.cbDevices.Name = "cbDevices";
      this.cbDevices.Size = new System.Drawing.Size(238, 22);
      this.cbDevices.TabIndex = 1;
      this.cbDevices.SelectedIndexChanged += new System.EventHandler(this.cbDevices_SelectedIndexChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.label1.Location = new System.Drawing.Point(306, 15);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(76, 14);
      this.label1.TabIndex = 3;
      this.label1.Text = "Устройство:";
      // 
      // label2
      // 
      this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.label2.AutoSize = true;
      this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.label2.Location = new System.Drawing.Point(786, 15);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(37, 14);
      this.label2.TabIndex = 5;
      this.label2.Text = "Язык:";
      // 
      // cbLanguage
      // 
      this.cbLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.cbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cbLanguage.FormattingEnabled = true;
      this.cbLanguage.Items.AddRange(new object[] {
            "Английский (EN)"});
      this.cbLanguage.Location = new System.Drawing.Point(829, 12);
      this.cbLanguage.Name = "cbLanguage";
      this.cbLanguage.Size = new System.Drawing.Size(175, 22);
      this.cbLanguage.TabIndex = 4;
      this.cbLanguage.SelectedIndexChanged += new System.EventHandler(this.cbLanguage_SelectedIndexChanged);
      // 
      // cId
      // 
      this.cId.HeaderText = "Id";
      this.cId.Name = "cId";
      this.cId.Visible = false;
      // 
      // cElementNumber
      // 
      this.cElementNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
      this.cElementNumber.HeaderText = "Номер элемента";
      this.cElementNumber.Name = "cElementNumber";
      this.cElementNumber.Width = 116;
      // 
      // cSubitemNumber
      // 
      this.cSubitemNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
      this.cSubitemNumber.HeaderText = "Номер подпункта";
      this.cSubitemNumber.Name = "cSubitemNumber";
      this.cSubitemNumber.Width = 121;
      // 
      // cOrderNumber
      // 
      this.cOrderNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
      this.cOrderNumber.HeaderText = "Порядковый номер";
      this.cOrderNumber.Name = "cOrderNumber";
      this.cOrderNumber.Width = 130;
      // 
      // cCheckName
      // 
      this.cCheckName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.cCheckName.HeaderText = "Наименование проверки";
      this.cCheckName.Name = "cCheckName";
      // 
      // cUnits
      // 
      this.cUnits.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.cUnits.HeaderText = "Ед. изм.";
      this.cUnits.Name = "cUnits";
      // 
      // cRequirement
      // 
      this.cRequirement.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.cRequirement.HeaderText = "НТП";
      this.cRequirement.Name = "cRequirement";
      // 
      // cParamName
      // 
      this.cParamName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.cParamName.HeaderText = "Параметр";
      this.cParamName.Name = "cParamName";
      // 
      // frmReportEditor
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1016, 522);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.cbLanguage);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.cbDevices);
      this.Controls.Add(this.bSave);
      this.Controls.Add(this.dgvReport);
      this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "frmReportEditor";
      this.Text = "Редактор отчёта";
      this.Load += new System.EventHandler(this.frmReportEditor_Load);
      ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvReport;
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.ComboBox cbDevices;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbLanguage;
        private System.Windows.Forms.DataGridViewTextBoxColumn cId;
        private System.Windows.Forms.DataGridViewTextBoxColumn cElementNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn cSubitemNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn cOrderNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn cCheckName;
        private System.Windows.Forms.DataGridViewTextBoxColumn cUnits;
        private System.Windows.Forms.DataGridViewTextBoxColumn cRequirement;
        private System.Windows.Forms.DataGridViewTextBoxColumn cParamName;
    }
}