namespace KPK2.GeneralForms.ServiceForm
{
    partial class FrmDocumentLink
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
            this.dgvLinks = new System.Windows.Forms.DataGridView();
            this.bSave = new System.Windows.Forms.Button();
            this.cId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cDevice = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.cTestType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.cText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLinks)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvLinks
            // 
            this.dgvLinks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvLinks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLinks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cId,
            this.cDevice,
            this.cTestType,
            this.cText});
            this.dgvLinks.Location = new System.Drawing.Point(14, 13);
            this.dgvLinks.Name = "dgvLinks";
            this.dgvLinks.Size = new System.Drawing.Size(1014, 611);
            this.dgvLinks.TabIndex = 0;
            this.dgvLinks.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvLinks_CellValueChanged);
            this.dgvLinks.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvLinks_RowsAdded);
            this.dgvLinks.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvLinks_DataError);
            // 
            // bSave
            // 
            this.bSave.Enabled = false;
            this.bSave.Location = new System.Drawing.Point(426, 631);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(178, 46);
            this.bSave.TabIndex = 1;
            this.bSave.Text = "Сохранить";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // cId
            // 
            this.cId.HeaderText = "Id";
            this.cId.Name = "cId";
            this.cId.Visible = false;
            // 
            // cDevice
            // 
            this.cDevice.HeaderText = "Устройство";
            this.cDevice.Name = "cDevice";
            this.cDevice.Width = 200;
            // 
            // cTestType
            // 
            this.cTestType.HeaderText = "Пункт проверки";
            this.cTestType.Name = "cTestType";
            this.cTestType.Width = 385;
            // 
            // cText
            // 
            this.cText.HeaderText = "Ссылка на документ";
            this.cText.Name = "cText";
            this.cText.Width = 385;
            // 
            // FrmDocumentLink
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1040, 689);
            this.Controls.Add(this.bSave);
            this.Controls.Add(this.dgvLinks);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "FrmDocumentLink";
            this.Text = "Редактор ссылок на нормативные документы";
            this.Load += new System.EventHandler(this.FrmDocumentLink_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLinks)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvLinks;
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.DataGridViewTextBoxColumn cId;
        private System.Windows.Forms.DataGridViewComboBoxColumn cDevice;
        private System.Windows.Forms.DataGridViewComboBoxColumn cTestType;
        private System.Windows.Forms.DataGridViewTextBoxColumn cText;
    }
}