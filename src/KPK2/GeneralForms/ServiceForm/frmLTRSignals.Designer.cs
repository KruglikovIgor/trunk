namespace KPK2.GeneralForms.ServiceForm
{
    partial class frmLTRSignals
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
            this.dgvSignals = new System.Windows.Forms.DataGridView();
            this.cId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cSlot = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.cChannel = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.cRange = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.cAlone = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.cACDC = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.bSave = new System.Windows.Forms.Button();
            this.bOK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSignals)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvSignals
            // 
            this.dgvSignals.AllowUserToDeleteRows = false;
            this.dgvSignals.AllowUserToResizeRows = false;
            this.dgvSignals.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSignals.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvSignals.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSignals.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cId,
            this.cSlot,
            this.cChannel,
            this.cRange,
            this.cAlone,
            this.cACDC});
            this.dgvSignals.Location = new System.Drawing.Point(12, 12);
            this.dgvSignals.MultiSelect = false;
            this.dgvSignals.Name = "dgvSignals";
            this.dgvSignals.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvSignals.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSignals.Size = new System.Drawing.Size(847, 283);
            this.dgvSignals.TabIndex = 0;
            this.dgvSignals.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSignals_CellValueChanged);
            this.dgvSignals.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvSignals_RowsAdded);
            // 
            // cId
            // 
            this.cId.HeaderText = "Номер сигнала";
            this.cId.Name = "cId";
            this.cId.ReadOnly = true;
            this.cId.Width = 101;
            // 
            // cSlot
            // 
            this.cSlot.HeaderText = "Номер слота";
            this.cSlot.Name = "cSlot";
            this.cSlot.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cSlot.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.cSlot.Width = 90;
            // 
            // cChannel
            // 
            this.cChannel.HeaderText = "Номер канала";
            this.cChannel.Name = "cChannel";
            this.cChannel.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cChannel.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.cChannel.Width = 97;
            // 
            // cRange
            // 
            this.cRange.HeaderText = "Диапазон измерений";
            this.cRange.Name = "cRange";
            this.cRange.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cRange.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.cRange.Width = 130;
            // 
            // cAlone
            // 
            this.cAlone.HeaderText = "Канал измеряется отдельно";
            this.cAlone.Name = "cAlone";
            this.cAlone.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cAlone.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.cAlone.Width = 161;
            // 
            // cACDC
            // 
            this.cACDC.HeaderText = "Переменное/постоянное напряжение";
            this.cACDC.Name = "cACDC";
            this.cACDC.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cACDC.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.cACDC.Width = 205;
            // 
            // bSave
            // 
            this.bSave.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.bSave.Enabled = false;
            this.bSave.Location = new System.Drawing.Point(364, 301);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(117, 39);
            this.bSave.TabIndex = 1;
            this.bSave.Text = "Сохранить";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // bOK
            // 
            this.bOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bOK.Location = new System.Drawing.Point(668, 303);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(191, 37);
            this.bOK.TabIndex = 2;
            this.bOK.Text = "Выбрать";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // frmLTRSignals
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(871, 347);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.bSave);
            this.Controls.Add(this.dgvSignals);
            this.Name = "frmLTRSignals";
            this.Text = "LTRSignals";
            this.Load += new System.EventHandler(this.frmLTRSignals_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSignals)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvSignals;
        private System.Windows.Forms.DataGridViewTextBoxColumn cId;
        private System.Windows.Forms.DataGridViewComboBoxColumn cSlot;
        private System.Windows.Forms.DataGridViewComboBoxColumn cChannel;
        private System.Windows.Forms.DataGridViewComboBoxColumn cRange;
        private System.Windows.Forms.DataGridViewComboBoxColumn cAlone;
        private System.Windows.Forms.DataGridViewComboBoxColumn cACDC;
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.Button bOK;
    }
}