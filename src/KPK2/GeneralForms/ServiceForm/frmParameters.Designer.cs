namespace KPK2.GeneralForms.ServiceForm
{
    partial class frmParameters
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
      this.bSelect = new System.Windows.Forms.Button();
      this.dgvParams = new System.Windows.Forms.DataGridView();
      this.cId = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.cParamName = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.cNote = new System.Windows.Forms.DataGridViewTextBoxColumn();
      ((System.ComponentModel.ISupportInitialize)(this.dgvParams)).BeginInit();
      this.SuspendLayout();
      // 
      // bSelect
      // 
      this.bSelect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.bSelect.Location = new System.Drawing.Point(187, 501);
      this.bSelect.Name = "bSelect";
      this.bSelect.Size = new System.Drawing.Size(162, 45);
      this.bSelect.TabIndex = 0;
      this.bSelect.Text = "Выбрать";
      this.bSelect.UseVisualStyleBackColor = true;
      this.bSelect.Click += new System.EventHandler(this.bSelect_Click);
      // 
      // dgvParams
      // 
      this.dgvParams.AllowUserToAddRows = false;
      this.dgvParams.AllowUserToDeleteRows = false;
      this.dgvParams.AllowUserToResizeColumns = false;
      this.dgvParams.AllowUserToResizeRows = false;
      this.dgvParams.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.dgvParams.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvParams.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cId,
            this.cParamName,
            this.cNote});
      this.dgvParams.Location = new System.Drawing.Point(12, 26);
      this.dgvParams.MultiSelect = false;
      this.dgvParams.Name = "dgvParams";
      this.dgvParams.RowHeadersVisible = false;
      this.dgvParams.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.dgvParams.Size = new System.Drawing.Size(512, 468);
      this.dgvParams.TabIndex = 1;
      this.dgvParams.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvParams_CellDoubleClick);
      // 
      // cId
      // 
      this.cId.HeaderText = "Id";
      this.cId.Name = "cId";
      this.cId.ReadOnly = true;
      // 
      // cParamName
      // 
      this.cParamName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.cParamName.HeaderText = "Наименование параметра";
      this.cParamName.Name = "cParamName";
      // 
      // cNote
      // 
      this.cNote.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.cNote.HeaderText = "Описание";
      this.cNote.Name = "cNote";
      // 
      // frmParameters
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(536, 554);
      this.ControlBox = false;
      this.Controls.Add(this.dgvParams);
      this.Controls.Add(this.bSelect);
      this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
      this.MaximizeBox = false;
      this.Name = "frmParameters";
      this.Text = "Параметры";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmParameters_FormClosing);
      this.Load += new System.EventHandler(this.frmParameters_Load);
      ((System.ComponentModel.ISupportInitialize)(this.dgvParams)).EndInit();
      this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bSelect;
        private System.Windows.Forms.DataGridView dgvParams;
        private System.Windows.Forms.DataGridViewTextBoxColumn cId;
        private System.Windows.Forms.DataGridViewTextBoxColumn cParamName;
        private System.Windows.Forms.DataGridViewTextBoxColumn cNote;
    }
}