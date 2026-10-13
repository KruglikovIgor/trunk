namespace KPK2.GeneralForms
{
    partial class frmPassportData
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPassportData));
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      this.dgvValues = new System.Windows.Forms.DataGridView();
      this.cParamId = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.cName = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.cValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.bSave = new System.Windows.Forms.Button();
      this.bClose = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.dgvValues)).BeginInit();
      this.SuspendLayout();
      // 
      // dgvValues
      // 
      this.dgvValues.AllowUserToAddRows = false;
      this.dgvValues.AllowUserToDeleteRows = false;
      this.dgvValues.AllowUserToResizeRows = false;
      resources.ApplyResources(this.dgvValues, "dgvValues");
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dgvValues.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      this.dgvValues.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvValues.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cParamId,
            this.cName,
            this.cValue});
      this.dgvValues.Name = "dgvValues";
      this.dgvValues.RowHeadersVisible = false;
      this.dgvValues.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvValues_CellValueChanged);
      // 
      // cParamId
      // 
      resources.ApplyResources(this.cParamId, "cParamId");
      this.cParamId.Name = "cParamId";
      // 
      // cName
      // 
      this.cName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      resources.ApplyResources(this.cName, "cName");
      this.cName.Name = "cName";
      this.cName.ReadOnly = true;
      // 
      // cValue
      // 
      resources.ApplyResources(this.cValue, "cValue");
      this.cValue.Name = "cValue";
      // 
      // bSave
      // 
      resources.ApplyResources(this.bSave, "bSave");
      this.bSave.Name = "bSave";
      this.bSave.UseVisualStyleBackColor = true;
      this.bSave.Click += new System.EventHandler(this.bSave_Click);
      // 
      // bClose
      // 
      resources.ApplyResources(this.bClose, "bClose");
      this.bClose.Name = "bClose";
      this.bClose.UseVisualStyleBackColor = true;
      this.bClose.Click += new System.EventHandler(this.bClose_Click);
      // 
      // frmPassportData
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ControlBox = false;
      this.Controls.Add(this.bClose);
      this.Controls.Add(this.bSave);
      this.Controls.Add(this.dgvValues);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Name = "frmPassportData";
      ((System.ComponentModel.ISupportInitialize)(this.dgvValues)).EndInit();
      this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvValues;
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.Button bClose;
        private System.Windows.Forms.DataGridViewTextBoxColumn cParamId;
        private System.Windows.Forms.DataGridViewTextBoxColumn cName;
        private System.Windows.Forms.DataGridViewTextBoxColumn cValue;
    }
}