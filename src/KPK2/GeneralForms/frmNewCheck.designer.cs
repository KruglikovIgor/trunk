namespace KPK2.GeneralForms
{
    partial class frmNewCheck
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmNewCheck));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblProductNumber = new System.Windows.Forms.Label();
            this.bClose = new System.Windows.Forms.Button();
            this.lblHelicopterMark = new System.Windows.Forms.Label();
            this.lblNumberHelicopter = new System.Windows.Forms.Label();
            this.lblSignHelicopter = new System.Windows.Forms.Label();
            this.tbHelicopterNumber = new System.Windows.Forms.TextBox();
            this.tbHelicopterSign = new System.Windows.Forms.TextBox();
            this.lbl_Note = new System.Windows.Forms.Label();
            this.tbNote = new System.Windows.Forms.TextBox();
            this.lblDateMaking = new System.Windows.Forms.Label();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.cbMestreBTK = new System.Windows.Forms.ComboBox();
            this.lblFullNameMestreBTK = new System.Windows.Forms.Label();
            this.cbMestre = new System.Windows.Forms.ComboBox();
            this.lblFullNameMestre = new System.Windows.Forms.Label();
            this.tbProductNumber = new System.Windows.Forms.TextBox();
            this.cbEmployee = new System.Windows.Forms.ComboBox();
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.lblFullName = new System.Windows.Forms.Label();
            this.bSave = new System.Windows.Forms.Button();
            this.lbl = new System.Windows.Forms.Label();
            this.bWork = new System.Windows.Forms.Button();
            this.blockTA = new KPK2.BlockTableAdapter();
            this.ds = new KPK2.KPK2DataSet();
            this.executorsTA = new KPK2.ExecutorsTableAdapter();
            this.label1 = new System.Windows.Forms.Label();
            this.cbFormTO = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.nudTotalRun = new System.Windows.Forms.NumericUpDown();
            this.nudLastRun = new System.Windows.Forms.NumericUpDown();
            this.dgvBlocks = new System.Windows.Forms.DataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BlockName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SerNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TBO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TBR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Delete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.label4 = new System.Windows.Forms.Label();
            this.bShowPassportData = new System.Windows.Forms.Button();
            this.errProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.cbHelicopterType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dtpCheckDate = new System.Windows.Forms.DateTimePicker();
            this.groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTotalRun)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLastRun)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBlocks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // lblProductNumber
            // 
            resources.ApplyResources(this.lblProductNumber, "lblProductNumber");
            this.lblProductNumber.Name = "lblProductNumber";
            // 
            // bClose
            // 
            resources.ApplyResources(this.bClose, "bClose");
            this.bClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bClose.Name = "bClose";
            this.bClose.UseVisualStyleBackColor = true;
            this.bClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblHelicopterMark
            // 
            resources.ApplyResources(this.lblHelicopterMark, "lblHelicopterMark");
            this.lblHelicopterMark.Name = "lblHelicopterMark";
            // 
            // lblNumberHelicopter
            // 
            resources.ApplyResources(this.lblNumberHelicopter, "lblNumberHelicopter");
            this.lblNumberHelicopter.Name = "lblNumberHelicopter";
            // 
            // lblSignHelicopter
            // 
            resources.ApplyResources(this.lblSignHelicopter, "lblSignHelicopter");
            this.lblSignHelicopter.Name = "lblSignHelicopter";
            // 
            // tbHelicopterNumber
            // 
            this.tbHelicopterNumber.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.tbHelicopterNumber.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            resources.ApplyResources(this.tbHelicopterNumber, "tbHelicopterNumber");
            this.tbHelicopterNumber.Name = "tbHelicopterNumber";
            this.tbHelicopterNumber.TextChanged += new System.EventHandler(this.control_ValueChanged);
            this.tbHelicopterNumber.Validated += new System.EventHandler(this.txtBox_Validated);
            // 
            // tbHelicopterSign
            // 
            this.tbHelicopterSign.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.tbHelicopterSign.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            resources.ApplyResources(this.tbHelicopterSign, "tbHelicopterSign");
            this.tbHelicopterSign.Name = "tbHelicopterSign";
            this.tbHelicopterSign.TextChanged += new System.EventHandler(this.control_ValueChanged);
            this.tbHelicopterSign.Validated += new System.EventHandler(this.txtBox_Validated);
            // 
            // lbl_Note
            // 
            resources.ApplyResources(this.lbl_Note, "lbl_Note");
            this.lbl_Note.Name = "lbl_Note";
            // 
            // tbNote
            // 
            resources.ApplyResources(this.tbNote, "tbNote");
            this.tbNote.Name = "tbNote";
            this.tbNote.TextChanged += new System.EventHandler(this.control_ValueChanged);
            this.tbNote.Validated += new System.EventHandler(this.txtBox_Validated);
            // 
            // lblDateMaking
            // 
            resources.ApplyResources(this.lblDateMaking, "lblDateMaking");
            this.lblDateMaking.Name = "lblDateMaking";
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.cbMestreBTK);
            this.groupBox.Controls.Add(this.lblFullNameMestreBTK);
            this.groupBox.Controls.Add(this.cbMestre);
            this.groupBox.Controls.Add(this.lblFullNameMestre);
            this.groupBox.Controls.Add(this.tbProductNumber);
            this.groupBox.Controls.Add(this.cbEmployee);
            this.groupBox.Controls.Add(this.dateTimePicker);
            this.groupBox.Controls.Add(this.lblFullName);
            this.groupBox.Controls.Add(this.lblProductNumber);
            this.groupBox.Controls.Add(this.lblDateMaking);
            resources.ApplyResources(this.groupBox, "groupBox");
            this.groupBox.Name = "groupBox";
            this.groupBox.TabStop = false;
            // 
            // cbMestreBTK
            // 
            this.cbMestreBTK.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMestreBTK.FormattingEnabled = true;
            resources.ApplyResources(this.cbMestreBTK, "cbMestreBTK");
            this.cbMestreBTK.Name = "cbMestreBTK";
            this.cbMestreBTK.SelectedIndexChanged += new System.EventHandler(this.cb_ValueChanged);
            // 
            // lblFullNameMestreBTK
            // 
            resources.ApplyResources(this.lblFullNameMestreBTK, "lblFullNameMestreBTK");
            this.lblFullNameMestreBTK.Name = "lblFullNameMestreBTK";
            // 
            // cbMestre
            // 
            this.cbMestre.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMestre.FormattingEnabled = true;
            resources.ApplyResources(this.cbMestre, "cbMestre");
            this.cbMestre.Name = "cbMestre";
            this.cbMestre.SelectedIndexChanged += new System.EventHandler(this.cb_ValueChanged);
            // 
            // lblFullNameMestre
            // 
            resources.ApplyResources(this.lblFullNameMestre, "lblFullNameMestre");
            this.lblFullNameMestre.Name = "lblFullNameMestre";
            // 
            // tbProductNumber
            // 
            this.tbProductNumber.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.tbProductNumber.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            resources.ApplyResources(this.tbProductNumber, "tbProductNumber");
            this.tbProductNumber.Name = "tbProductNumber";
            this.tbProductNumber.TextChanged += new System.EventHandler(this.control_ValueChanged);
            this.tbProductNumber.Validated += new System.EventHandler(this.txtBox_Validated);
            // 
            // cbEmployee
            // 
            this.cbEmployee.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEmployee.FormattingEnabled = true;
            resources.ApplyResources(this.cbEmployee, "cbEmployee");
            this.cbEmployee.Name = "cbEmployee";
            this.cbEmployee.SelectedIndexChanged += new System.EventHandler(this.anyBox_ValueChanged);
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dateTimePicker, "dateTimePicker");
            this.dateTimePicker.MaxDate = new System.DateTime(2398, 5, 5, 0, 0, 0, 0);
            this.dateTimePicker.MinDate = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.Value = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            this.dateTimePicker.ValueChanged += new System.EventHandler(this.anyBox_ValueChanged);
            // 
            // lblFullName
            // 
            resources.ApplyResources(this.lblFullName, "lblFullName");
            this.lblFullName.Name = "lblFullName";
            // 
            // bSave
            // 
            resources.ApplyResources(this.bSave, "bSave");
            this.bSave.Name = "bSave";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lbl
            // 
            this.lbl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.lbl, "lbl");
            this.lbl.Name = "lbl";
            // 
            // bWork
            // 
            resources.ApplyResources(this.bWork, "bWork");
            this.bWork.Name = "bWork";
            this.bWork.UseVisualStyleBackColor = true;
            this.bWork.Click += new System.EventHandler(this.bWork_Click);
            // 
            // blockTA
            // 
            this.blockTA.ClearBeforeFill = true;
            // 
            // ds
            // 
            this.ds.DataSetName = "KPK2DataSet";
            this.ds.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // executorsTA
            // 
            this.executorsTA.ClearBeforeFill = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cbFormTO
            // 
            this.cbFormTO.FormattingEnabled = true;
            resources.ApplyResources(this.cbFormTO, "cbFormTO");
            this.cbFormTO.Name = "cbFormTO";
            this.cbFormTO.SelectedIndexChanged += new System.EventHandler(this.anyBox_ValueChanged);
            this.cbFormTO.TextChanged += new System.EventHandler(this.anyBox_ValueChanged);
            this.cbFormTO.Leave += new System.EventHandler(this.cbFormTO_Leave);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // nudTotalRun
            // 
            this.nudTotalRun.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            resources.ApplyResources(this.nudTotalRun, "nudTotalRun");
            this.nudTotalRun.Maximum = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            this.nudTotalRun.Name = "nudTotalRun";
            this.nudTotalRun.ValueChanged += new System.EventHandler(this.anyBox_ValueChanged);
            this.nudTotalRun.Leave += new System.EventHandler(this.nudTotalRun_Leave);
            // 
            // nudLastRun
            // 
            this.nudLastRun.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            resources.ApplyResources(this.nudLastRun, "nudLastRun");
            this.nudLastRun.Maximum = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            this.nudLastRun.Name = "nudLastRun";
            this.nudLastRun.ValueChanged += new System.EventHandler(this.anyBox_ValueChanged);
            this.nudLastRun.Leave += new System.EventHandler(this.nudLastRun_Leave);
            // 
            // dgvBlocks
            // 
            this.dgvBlocks.AllowUserToAddRows = false;
            this.dgvBlocks.AllowUserToDeleteRows = false;
            this.dgvBlocks.AllowUserToResizeColumns = false;
            this.dgvBlocks.AllowUserToResizeRows = false;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Verdana", 9F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBlocks.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvBlocks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBlocks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.BlockName,
            this.SerNo,
            this.TBO,
            this.TBR,
            this.Delete});
            resources.ApplyResources(this.dgvBlocks, "dgvBlocks");
            this.dgvBlocks.Name = "dgvBlocks";
            this.dgvBlocks.RowHeadersVisible = false;
            this.dgvBlocks.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBlocks_CellClick);
            this.dgvBlocks.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBlocks_CellValueChanged);
            // 
            // Id
            // 
            resources.ApplyResources(this.Id, "Id");
            this.Id.Name = "Id";
            // 
            // BlockName
            // 
            this.BlockName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.BlockName, "BlockName");
            this.BlockName.Name = "BlockName";
            this.BlockName.ReadOnly = true;
            this.BlockName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // SerNo
            // 
            resources.ApplyResources(this.SerNo, "SerNo");
            this.SerNo.Name = "SerNo";
            this.SerNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // TBO
            // 
            resources.ApplyResources(this.TBO, "TBO");
            this.TBO.Name = "TBO";
            this.TBO.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // TBR
            // 
            resources.ApplyResources(this.TBR, "TBR");
            this.TBR.Name = "TBR";
            this.TBR.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Delete
            // 
            resources.ApplyResources(this.Delete, "Delete");
            this.Delete.Name = "Delete";
            this.Delete.ReadOnly = true;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // bShowPassportData
            // 
            resources.ApplyResources(this.bShowPassportData, "bShowPassportData");
            this.bShowPassportData.Name = "bShowPassportData";
            this.bShowPassportData.UseVisualStyleBackColor = true;
            this.bShowPassportData.Click += new System.EventHandler(this.bShowPassportData_Click);
            // 
            // errProvider
            // 
            this.errProvider.ContainerControl = this;
            // 
            // cbHelicopterType
            // 
            this.cbHelicopterType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbHelicopterType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbHelicopterType.FormattingEnabled = true;
            resources.ApplyResources(this.cbHelicopterType, "cbHelicopterType");
            this.cbHelicopterType.Name = "cbHelicopterType";
            this.cbHelicopterType.SelectedValueChanged += new System.EventHandler(this.control_ValueChanged);
            this.cbHelicopterType.Leave += new System.EventHandler(this.cbHelicopterType_Leave);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // dtpCheckDate
            // 
            resources.ApplyResources(this.dtpCheckDate, "dtpCheckDate");
            this.dtpCheckDate.Name = "dtpCheckDate";
            this.dtpCheckDate.ValueChanged += new System.EventHandler(this.dtpCheckDate_ValueChanged);
            // 
            // frmNewCheck
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bClose;
            this.Controls.Add(this.dtpCheckDate);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cbHelicopterType);
            this.Controls.Add(this.bShowPassportData);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dgvBlocks);
            this.Controls.Add(this.nudLastRun);
            this.Controls.Add(this.nudTotalRun);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbFormTO);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bWork);
            this.Controls.Add(this.lbl);
            this.Controls.Add(this.groupBox);
            this.Controls.Add(this.tbNote);
            this.Controls.Add(this.tbHelicopterSign);
            this.Controls.Add(this.tbHelicopterNumber);
            this.Controls.Add(this.bSave);
            this.Controls.Add(this.bClose);
            this.Controls.Add(this.lblSignHelicopter);
            this.Controls.Add(this.lblNumberHelicopter);
            this.Controls.Add(this.lblHelicopterMark);
            this.Controls.Add(this.lbl_Note);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmNewCheck";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmNewCheck_FormClosing);
            this.Load += new System.EventHandler(this.frmNewCheck_Load);
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTotalRun)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLastRun)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBlocks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblProductNumber;
        /// <summary>
        /// Закрываем форму
        /// </summary>
        private System.Windows.Forms.Button bClose;
        private System.Windows.Forms.Label lblHelicopterMark;
        private System.Windows.Forms.Label lblNumberHelicopter;
        private System.Windows.Forms.Label lblSignHelicopter;
        /// <summary>
        /// Номер вертолета 
        /// </summary>
        private System.Windows.Forms.TextBox tbHelicopterNumber;
        /// <summary>
        /// Опозновательный знак вертолета 
        /// </summary>
        private System.Windows.Forms.TextBox tbHelicopterSign;
        private System.Windows.Forms.Label lbl_Note;
        /// <summary>
        /// Примечание 
        /// </summary>
        private System.Windows.Forms.TextBox tbNote;
        private System.Windows.Forms.Label lblDateMaking;
        private System.Windows.Forms.GroupBox groupBox;
        /// <summary>
        /// Дата изготовления прибора 
        /// </summary>
        private System.Windows.Forms.DateTimePicker dateTimePicker;
        private System.Windows.Forms.Label lblFullName;
        /// <summary>
        /// "Сохранить" введенные данные 
        /// </summary>
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.Label lbl;
        /// <summary>
        /// "Начать проверку"
        /// </summary>
        private System.Windows.Forms.Button bWork;
        /// <summary>
        /// TableAdapter для таблицы Block 
        /// </summary>
        private BlockTableAdapter blockTA;
        /// <summary>
        /// Мой DataSet 
        /// </summary>
        private KPK2DataSet ds;
        /// <summary>
        /// TableAdapter для таблицы Executors 
        /// </summary>
        private ExecutorsTableAdapter executorsTA;
        private System.Windows.Forms.ComboBox cbEmployee;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbFormTO;
        private System.Windows.Forms.TextBox tbProductNumber;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.NumericUpDown nudTotalRun;
        private System.Windows.Forms.NumericUpDown nudLastRun;
        private System.Windows.Forms.DataGridView dgvBlocks;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button bShowPassportData;
        private System.Windows.Forms.ErrorProvider errProvider;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn BlockName;
        private System.Windows.Forms.DataGridViewTextBoxColumn SerNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn TBO;
        private System.Windows.Forms.DataGridViewTextBoxColumn TBR;
        private System.Windows.Forms.DataGridViewButtonColumn Delete;
        private System.Windows.Forms.ComboBox cbHelicopterType;
        private System.Windows.Forms.ComboBox cbMestreBTK;
        private System.Windows.Forms.Label lblFullNameMestreBTK;
        private System.Windows.Forms.ComboBox cbMestre;
        private System.Windows.Forms.Label lblFullNameMestre;
        private System.Windows.Forms.DateTimePicker dtpCheckDate;
        private System.Windows.Forms.Label label5;
    }
}