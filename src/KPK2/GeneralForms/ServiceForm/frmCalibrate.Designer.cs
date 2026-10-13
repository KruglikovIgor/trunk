namespace KPK2.GeneralForms.ServiceForm
{
    partial class frmCalibrate
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
            this.dgvParams = new System.Windows.Forms.DataGridView();
            this.cId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cNote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cDevice = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.сMin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.сMax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPrecision = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cKoef = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cCommand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AddCommand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bSave = new System.Windows.Forms.Button();
            this.bSaveToXMLFile = new System.Windows.Forms.Button();
            this.cbOftenUsages = new System.Windows.Forms.CheckBox();
            this.bReadFromXMLFile = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvParams)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvParams
            // 
            this.dgvParams.AllowUserToAddRows = false;
            this.dgvParams.AllowUserToDeleteRows = false;
            this.dgvParams.AllowUserToResizeRows = false;
            this.dgvParams.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvParams.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvParams.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvParams.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cId,
            this.cNote,
            this.cName,
            this.cDevice,
            this.сMin,
            this.сMax,
            this.cPrecision,
            this.cKoef,
            this.cCommand,
            this.AddCommand});
            this.dgvParams.Location = new System.Drawing.Point(4, 3);
            this.dgvParams.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dgvParams.Name = "dgvParams";
            this.dgvParams.Size = new System.Drawing.Size(1020, 344);
            this.dgvParams.TabIndex = 0;
            this.dgvParams.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvParams_CellClick);
            this.dgvParams.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvParams_CellValueChanged);
            this.dgvParams.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvParams_RowsAdded);
            // 
            // cId
            // 
            this.cId.HeaderText = "Id";
            this.cId.Name = "cId";
            this.cId.ReadOnly = true;
            this.cId.Width = 45;
            // 
            // cNote
            // 
            this.cNote.HeaderText = "Описание";
            this.cNote.Name = "cNote";
            this.cNote.Width = 96;
            // 
            // cName
            // 
            this.cName.HeaderText = "Название параметра";
            this.cName.Name = "cName";
            this.cName.Width = 154;
            // 
            // cDevice
            // 
            this.cDevice.HeaderText = "Устройство";
            this.cDevice.Name = "cDevice";
            this.cDevice.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cDevice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.cDevice.Width = 105;
            // 
            // сMin
            // 
            this.сMin.HeaderText = "Минимальное значение";
            this.сMin.Name = "сMin";
            this.сMin.Width = 169;
            // 
            // сMax
            // 
            this.сMax.HeaderText = "Максимальное значение";
            this.сMax.Name = "сMax";
            this.сMax.Width = 174;
            // 
            // cPrecision
            // 
            this.cPrecision.HeaderText = "Точность";
            this.cPrecision.Name = "cPrecision";
            this.cPrecision.Width = 91;
            // 
            // cKoef
            // 
            this.cKoef.HeaderText = "Коэфициент";
            this.cKoef.Name = "cKoef";
            this.cKoef.Width = 115;
            // 
            // cCommand
            // 
            this.cCommand.HeaderText = "Команда";
            this.cCommand.Name = "cCommand";
            this.cCommand.ReadOnly = true;
            this.cCommand.Width = 88;
            // 
            // AddCommand
            // 
            this.AddCommand.HeaderText = "Дополнительные параметры команды";
            this.AddCommand.Name = "AddCommand";
            this.AddCommand.Width = 202;
            // 
            // bSave
            // 
            this.bSave.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bSave.Enabled = false;
            this.bSave.Location = new System.Drawing.Point(368, 353);
            this.bSave.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(291, 56);
            this.bSave.TabIndex = 1;
            this.bSave.Text = "Сохранить";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // bSaveToXMLFile
            // 
            this.bSaveToXMLFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bSaveToXMLFile.Location = new System.Drawing.Point(788, 353);
            this.bSaveToXMLFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.bSaveToXMLFile.Name = "bSaveToXMLFile";
            this.bSaveToXMLFile.Size = new System.Drawing.Size(236, 33);
            this.bSaveToXMLFile.TabIndex = 2;
            this.bSaveToXMLFile.Text = "Сохранить данные в XML файл";
            this.bSaveToXMLFile.UseVisualStyleBackColor = true;
            this.bSaveToXMLFile.Click += new System.EventHandler(this.bSaveToXMLFile_Click);
            // 
            // cbOftenUsages
            // 
            this.cbOftenUsages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbOftenUsages.AutoSize = true;
            this.cbOftenUsages.Location = new System.Drawing.Point(12, 381);
            this.cbOftenUsages.Name = "cbOftenUsages";
            this.cbOftenUsages.Size = new System.Drawing.Size(281, 18);
            this.cbOftenUsages.TabIndex = 3;
            this.cbOftenUsages.Text = "Только часто используемые параметры";
            this.cbOftenUsages.UseVisualStyleBackColor = true;
            this.cbOftenUsages.CheckedChanged += new System.EventHandler(this.cbOftenUsages_CheckedChanged);
            // 
            // bReadFromXMLFile
            // 
            this.bReadFromXMLFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bReadFromXMLFile.Location = new System.Drawing.Point(788, 381);
            this.bReadFromXMLFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.bReadFromXMLFile.Name = "bReadFromXMLFile";
            this.bReadFromXMLFile.Size = new System.Drawing.Size(236, 33);
            this.bReadFromXMLFile.TabIndex = 4;
            this.bReadFromXMLFile.Text = "Прочитать данные из XML файла";
            this.bReadFromXMLFile.UseVisualStyleBackColor = true;
            this.bReadFromXMLFile.Click += new System.EventHandler(this.bReadFromXMLFile_Click);
            // 
            // frmCalibrate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 411);
            this.Controls.Add(this.bReadFromXMLFile);
            this.Controls.Add(this.cbOftenUsages);
            this.Controls.Add(this.bSaveToXMLFile);
            this.Controls.Add(this.bSave);
            this.Controls.Add(this.dgvParams);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "frmCalibrate";
            this.Text = "Калибровка";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmCalibrate_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvParams)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvParams;
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.Button bSaveToXMLFile;
        private System.Windows.Forms.CheckBox cbOftenUsages;
        private System.Windows.Forms.DataGridViewTextBoxColumn cId;
        private System.Windows.Forms.DataGridViewTextBoxColumn cNote;
        private System.Windows.Forms.DataGridViewTextBoxColumn cName;
        private System.Windows.Forms.DataGridViewComboBoxColumn cDevice;
        private System.Windows.Forms.DataGridViewTextBoxColumn сMin;
        private System.Windows.Forms.DataGridViewTextBoxColumn сMax;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPrecision;
        private System.Windows.Forms.DataGridViewTextBoxColumn cKoef;
        private System.Windows.Forms.DataGridViewTextBoxColumn cCommand;
        private System.Windows.Forms.DataGridViewTextBoxColumn AddCommand;
        private System.Windows.Forms.Button bReadFromXMLFile;
    }
}