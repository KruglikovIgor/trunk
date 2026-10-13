namespace KPK2.GeneralForms
{
    partial class frmViewTest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmViewTest));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.bDelete = new System.Windows.Forms.Button();
            this.bAdd = new System.Windows.Forms.Button();
            this.bClose = new System.Windows.Forms.Button();
            this.dgvChecks = new System.Windows.Forms.DataGridView();
            this.viewFullNameBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.KPK2DataSet = new KPK2.KPK2DataSet();
            this.bRepair = new System.Windows.Forms.Button();
            this.grpBox_Filter = new System.Windows.Forms.GroupBox();
            this.lDissType = new System.Windows.Forms.Label();
            this.cbDissType = new System.Windows.Forms.ComboBox();
            this.cbPerformer = new System.Windows.Forms.ComboBox();
            this.cbNumber = new System.Windows.Forms.ComboBox();
            this.bCancel = new System.Windows.Forms.Button();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lbl4 = new System.Windows.Forms.Label();
            this.lbl3 = new System.Windows.Forms.Label();
            this.lbl2 = new System.Windows.Forms.Label();
            this.lbl1 = new System.Windows.Forms.Label();
            this.blockBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.viewFullNameTA = new KPK2.ViewFullNameTableAdapter();
            this.blockTA = new KPK2.BlockTableAdapter();
            this.dtView_Numb = new System.Data.DataView();
            this.dtView_FullName = new System.Data.DataView();
            this.measuringParamTA = new KPK2.MeasuringParamTableAdapter();
            this.bContinue = new System.Windows.Forms.Button();
            this.ttMain = new System.Windows.Forms.ToolTip(this.components);
            this.btnJupReport = new System.Windows.Forms.Button();
            this.ttipJUP = new System.Windows.Forms.ToolTip(this.components);
            this.NumberOfVerification = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productionNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dateMaking = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dateRepair = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fullName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.helicopterMark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.signHelicopter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.numberHelicopter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.note = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.employeeIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TypeId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.typeBlockDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChecks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewFullNameBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.KPK2DataSet)).BeginInit();
            this.grpBox_Filter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.blockBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtView_Numb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtView_FullName)).BeginInit();
            this.SuspendLayout();
            // 
            // bDelete
            // 
            resources.ApplyResources(this.bDelete, "bDelete");
            this.bDelete.Name = "bDelete";
            this.bDelete.UseVisualStyleBackColor = true;
            this.bDelete.Click += new System.EventHandler(this.btn_Delete_Click);
            // 
            // bAdd
            // 
            resources.ApplyResources(this.bAdd, "bAdd");
            this.bAdd.Name = "bAdd";
            this.bAdd.UseVisualStyleBackColor = true;
            this.bAdd.Click += new System.EventHandler(this.btn_Add_Click);
            // 
            // bClose
            // 
            resources.ApplyResources(this.bClose, "bClose");
            this.bClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bClose.Name = "bClose";
            this.bClose.UseVisualStyleBackColor = true;
            this.bClose.Click += new System.EventHandler(this.btnCurStatusClose_Click);
            // 
            // dgvChecks
            // 
            this.dgvChecks.AllowUserToAddRows = false;
            this.dgvChecks.AllowUserToDeleteRows = false;
            this.dgvChecks.AllowUserToResizeColumns = false;
            this.dgvChecks.AllowUserToResizeRows = false;
            resources.ApplyResources(this.dgvChecks, "dgvChecks");
            this.dgvChecks.AutoGenerateColumns = false;
            this.dgvChecks.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvChecks.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Verdana", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvChecks.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvChecks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvChecks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NumberOfVerification,
            this.productionNumber,
            this.dateMaking,
            this.dateRepair,
            this.fullName,
            this.helicopterMark,
            this.signHelicopter,
            this.numberHelicopter,
            this.note,
            this.employeeIDDataGridViewTextBoxColumn,
            this.TypeId,
            this.typeBlockDataGridViewTextBoxColumn});
            this.dgvChecks.DataSource = this.viewFullNameBindingSource;
            this.dgvChecks.MultiSelect = false;
            this.dgvChecks.Name = "dgvChecks";
            this.dgvChecks.ReadOnly = true;
            this.dgvChecks.RowHeadersVisible = false;
            this.dgvChecks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvChecks.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvChecks_CellDoubleClick);
            this.dgvChecks.SelectionChanged += new System.EventHandler(this.dgv_SelectionChanged);
            // 
            // viewFullNameBindingSource
            // 
            this.viewFullNameBindingSource.DataMember = "ViewFullName";
            this.viewFullNameBindingSource.DataSource = this.KPK2DataSet;
            // 
            // KPK2DataSet
            // 
            this.KPK2DataSet.DataSetName = "KPK2DataSet";
            this.KPK2DataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // bRepair
            // 
            resources.ApplyResources(this.bRepair, "bRepair");
            this.bRepair.Name = "bRepair";
            this.bRepair.UseVisualStyleBackColor = true;
            this.bRepair.Click += new System.EventHandler(this.btn_Repair_Click);
            // 
            // grpBox_Filter
            // 
            resources.ApplyResources(this.grpBox_Filter, "grpBox_Filter");
            this.grpBox_Filter.Controls.Add(this.lDissType);
            this.grpBox_Filter.Controls.Add(this.cbDissType);
            this.grpBox_Filter.Controls.Add(this.cbPerformer);
            this.grpBox_Filter.Controls.Add(this.cbNumber);
            this.grpBox_Filter.Controls.Add(this.bCancel);
            this.grpBox_Filter.Controls.Add(this.dtpTo);
            this.grpBox_Filter.Controls.Add(this.dtpFrom);
            this.grpBox_Filter.Controls.Add(this.lbl4);
            this.grpBox_Filter.Controls.Add(this.lbl3);
            this.grpBox_Filter.Controls.Add(this.lbl2);
            this.grpBox_Filter.Controls.Add(this.lbl1);
            this.grpBox_Filter.Name = "grpBox_Filter";
            this.grpBox_Filter.TabStop = false;
            // 
            // lDissType
            // 
            resources.ApplyResources(this.lDissType, "lDissType");
            this.lDissType.Name = "lDissType";
            // 
            // cbDissType
            // 
            resources.ApplyResources(this.cbDissType, "cbDissType");
            this.cbDissType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDissType.FormattingEnabled = true;
            this.cbDissType.Name = "cbDissType";
            this.cbDissType.SelectedIndexChanged += new System.EventHandler(this.cbDissType_SelectedIndexChanged);
            // 
            // cbPerformer
            // 
            this.cbPerformer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPerformer.FormattingEnabled = true;
            resources.ApplyResources(this.cbPerformer, "cbPerformer");
            this.cbPerformer.Name = "cbPerformer";
            this.cbPerformer.SelectedValueChanged += new System.EventHandler(this.cmbBox_Number_SelectedValueChanged);
            // 
            // cbNumber
            // 
            this.cbNumber.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbNumber.FormattingEnabled = true;
            resources.ApplyResources(this.cbNumber, "cbNumber");
            this.cbNumber.Name = "cbNumber";
            this.cbNumber.SelectedValueChanged += new System.EventHandler(this.cmbBox_Number_SelectedValueChanged);
            // 
            // bCancel
            // 
            resources.ApplyResources(this.bCancel, "bCancel");
            this.bCancel.Name = "bCancel";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // dtpTo
            // 
            this.dtpTo.Checked = false;
            resources.ApplyResources(this.dtpTo, "dtpTo");
            this.dtpTo.MinDate = new System.DateTime(1970, 1, 2, 0, 0, 0, 0);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.ShowCheckBox = true;
            this.dtpTo.ValueChanged += new System.EventHandler(this.cmbBox_Number_SelectedValueChanged);
            // 
            // dtpFrom
            // 
            this.dtpFrom.Checked = false;
            resources.ApplyResources(this.dtpFrom, "dtpFrom");
            this.dtpFrom.MinDate = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.ShowCheckBox = true;
            this.dtpFrom.ValueChanged += new System.EventHandler(this.cmbBox_Number_SelectedValueChanged);
            // 
            // lbl4
            // 
            resources.ApplyResources(this.lbl4, "lbl4");
            this.lbl4.Name = "lbl4";
            // 
            // lbl3
            // 
            resources.ApplyResources(this.lbl3, "lbl3");
            this.lbl3.Name = "lbl3";
            // 
            // lbl2
            // 
            resources.ApplyResources(this.lbl2, "lbl2");
            this.lbl2.Name = "lbl2";
            // 
            // lbl1
            // 
            resources.ApplyResources(this.lbl1, "lbl1");
            this.lbl1.Name = "lbl1";
            // 
            // blockBindingSource
            // 
            this.blockBindingSource.DataMember = "Block";
            this.blockBindingSource.DataSource = this.KPK2DataSet;
            // 
            // viewFullNameTA
            // 
            this.viewFullNameTA.ClearBeforeFill = true;
            // 
            // blockTA
            // 
            this.blockTA.ClearBeforeFill = true;
            // 
            // dtView_Numb
            // 
            this.dtView_Numb.Table = this.KPK2DataSet.ViewFullName;
            // 
            // dtView_FullName
            // 
            this.dtView_FullName.Table = this.KPK2DataSet.ViewFullName;
            // 
            // measuringParamTA
            // 
            this.measuringParamTA.ClearBeforeFill = true;
            // 
            // bContinue
            // 
            resources.ApplyResources(this.bContinue, "bContinue");
            this.bContinue.Name = "bContinue";
            this.bContinue.UseVisualStyleBackColor = true;
            this.bContinue.Click += new System.EventHandler(this.bContinue_Click);
            // 
            // btnJupReport
            // 
            resources.ApplyResources(this.btnJupReport, "btnJupReport");
            this.btnJupReport.Name = "btnJupReport";
            this.ttMain.SetToolTip(this.btnJupReport, resources.GetString("btnJupReport.ToolTip"));
            this.ttipJUP.SetToolTip(this.btnJupReport, resources.GetString("btnJupReport.ToolTip1"));
            this.btnJupReport.UseVisualStyleBackColor = true;
            this.btnJupReport.Click += new System.EventHandler(this.btnJupReport_Click);
            // 
            // NumberOfVerification
            // 
            this.NumberOfVerification.DataPropertyName = "numberOfVerification";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.NumberOfVerification.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.NumberOfVerification, "NumberOfVerification");
            this.NumberOfVerification.Name = "NumberOfVerification";
            this.NumberOfVerification.ReadOnly = true;
            this.NumberOfVerification.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.NumberOfVerification.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // productionNumber
            // 
            this.productionNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.productionNumber.DataPropertyName = "ProductionNumber";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.productionNumber.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.productionNumber, "productionNumber");
            this.productionNumber.Name = "productionNumber";
            this.productionNumber.ReadOnly = true;
            this.productionNumber.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // dateMaking
            // 
            this.dateMaking.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dateMaking.DataPropertyName = "DateMaking";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle4.Format = "d";
            dataGridViewCellStyle4.NullValue = null;
            this.dateMaking.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this.dateMaking, "dateMaking");
            this.dateMaking.Name = "dateMaking";
            this.dateMaking.ReadOnly = true;
            this.dateMaking.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // dateRepair
            // 
            this.dateRepair.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dateRepair.DataPropertyName = "DateRepair";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle5.Format = "d";
            dataGridViewCellStyle5.NullValue = null;
            this.dateRepair.DefaultCellStyle = dataGridViewCellStyle5;
            resources.ApplyResources(this.dateRepair, "dateRepair");
            this.dateRepair.Name = "dateRepair";
            this.dateRepair.ReadOnly = true;
            this.dateRepair.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // fullName
            // 
            this.fullName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.fullName.DataPropertyName = "FullName";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.fullName.DefaultCellStyle = dataGridViewCellStyle6;
            resources.ApplyResources(this.fullName, "fullName");
            this.fullName.Name = "fullName";
            this.fullName.ReadOnly = true;
            this.fullName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // helicopterMark
            // 
            this.helicopterMark.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.helicopterMark.DataPropertyName = "HelicopterMark";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.helicopterMark.DefaultCellStyle = dataGridViewCellStyle7;
            resources.ApplyResources(this.helicopterMark, "helicopterMark");
            this.helicopterMark.Name = "helicopterMark";
            this.helicopterMark.ReadOnly = true;
            // 
            // signHelicopter
            // 
            this.signHelicopter.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.signHelicopter.DataPropertyName = "NumberHelicopter";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.signHelicopter.DefaultCellStyle = dataGridViewCellStyle8;
            resources.ApplyResources(this.signHelicopter, "signHelicopter");
            this.signHelicopter.Name = "signHelicopter";
            this.signHelicopter.ReadOnly = true;
            this.signHelicopter.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // numberHelicopter
            // 
            this.numberHelicopter.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.numberHelicopter.DataPropertyName = "SignHelicopter";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.numberHelicopter.DefaultCellStyle = dataGridViewCellStyle9;
            resources.ApplyResources(this.numberHelicopter, "numberHelicopter");
            this.numberHelicopter.Name = "numberHelicopter";
            this.numberHelicopter.ReadOnly = true;
            // 
            // note
            // 
            this.note.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.note.DataPropertyName = "Note";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.note.DefaultCellStyle = dataGridViewCellStyle10;
            resources.ApplyResources(this.note, "note");
            this.note.Name = "note";
            this.note.ReadOnly = true;
            // 
            // employeeIDDataGridViewTextBoxColumn
            // 
            this.employeeIDDataGridViewTextBoxColumn.DataPropertyName = "EmployeeID";
            resources.ApplyResources(this.employeeIDDataGridViewTextBoxColumn, "employeeIDDataGridViewTextBoxColumn");
            this.employeeIDDataGridViewTextBoxColumn.Name = "employeeIDDataGridViewTextBoxColumn";
            this.employeeIDDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // TypeId
            // 
            this.TypeId.DataPropertyName = "BlockID";
            resources.ApplyResources(this.TypeId, "TypeId");
            this.TypeId.Name = "TypeId";
            this.TypeId.ReadOnly = true;
            // 
            // typeBlockDataGridViewTextBoxColumn
            // 
            this.typeBlockDataGridViewTextBoxColumn.DataPropertyName = "TypeBlock";
            resources.ApplyResources(this.typeBlockDataGridViewTextBoxColumn, "typeBlockDataGridViewTextBoxColumn");
            this.typeBlockDataGridViewTextBoxColumn.Name = "typeBlockDataGridViewTextBoxColumn";
            this.typeBlockDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // frmViewTest
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.CancelButton = this.bClose;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.btnJupReport);
            this.Controls.Add(this.bContinue);
            this.Controls.Add(this.grpBox_Filter);
            this.Controls.Add(this.bRepair);
            this.Controls.Add(this.dgvChecks);
            this.Controls.Add(this.bClose);
            this.Controls.Add(this.bAdd);
            this.Controls.Add(this.bDelete);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmViewTest";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCurStatus_FormClosing);
            this.Load += new System.EventHandler(this.frmViewTest_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvChecks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewFullNameBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.KPK2DataSet)).EndInit();
            this.grpBox_Filter.ResumeLayout(false);
            this.grpBox_Filter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.blockBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtView_Numb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtView_FullName)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        /// <summary>
        /// Удаление выделенной проверки  
        /// </summary>
        private System.Windows.Forms.Button bDelete;
        /// <summary>
        /// Добавить новую проверку 
        /// </summary>
        private System.Windows.Forms.Button bAdd;
        /// <summary>
        /// Закрываем форму 
        /// </summary>
        private System.Windows.Forms.Button bClose;
        /// <summary>
        /// Вывод данных из БД 
        /// </summary>
        private System.Windows.Forms.DataGridView dgvChecks;
        private KPK2DataSet KPK2DataSet;
        private System.Windows.Forms.BindingSource viewFullNameBindingSource;
        private ViewFullNameTableAdapter viewFullNameTA;
        private BlockTableAdapter blockTA;
        private System.Windows.Forms.BindingSource blockBindingSource;
        /// <summary>
        /// Вывод отчета по выделенной проверки 
        /// </summary>
        private System.Windows.Forms.Button bRepair;
        /// <summary>
        /// GroupBox для фильтрации выводимых проверок 
        /// </summary>
        private System.Windows.Forms.GroupBox grpBox_Filter;
        /// <summary>
        /// Выбор исполнителя для фильтрации 
        /// </summary>
        private System.Windows.Forms.ComboBox cbPerformer;
        /// <summary>
        /// Выбор номера ДИССа для фильтрации 
        /// </summary>
        private System.Windows.Forms.ComboBox cbNumber;
        /// <summary>
        /// Отмена фильтрации 
        /// </summary>
        private System.Windows.Forms.Button bCancel;
        /// <summary>
        /// Дата проверки ДО для фильтрации 
        /// </summary>
        private System.Windows.Forms.DateTimePicker dtpTo;
        /// <summary>
        /// Дата проверки ОТ для фильтрации 
        /// </summary>
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lbl4;
        private System.Windows.Forms.Label lbl3;
        private System.Windows.Forms.Label lbl2;
        private System.Windows.Forms.Label lbl1;
        private System.Data.DataView dtView_Numb;
        private System.Data.DataView dtView_FullName;
        private MeasuringParamTableAdapter measuringParamTA;
        private System.Windows.Forms.Label lDissType;
        private System.Windows.Forms.ComboBox cbDissType;
        private System.Windows.Forms.Button bContinue;
        private System.Windows.Forms.ToolTip ttMain;
        private System.Windows.Forms.Button btnJupReport;
        private System.Windows.Forms.ToolTip ttipJUP;
        private System.Windows.Forms.DataGridViewTextBoxColumn NumberOfVerification;
        private System.Windows.Forms.DataGridViewTextBoxColumn productionNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateMaking;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateRepair;
        private System.Windows.Forms.DataGridViewTextBoxColumn fullName;
        private System.Windows.Forms.DataGridViewTextBoxColumn helicopterMark;
        private System.Windows.Forms.DataGridViewTextBoxColumn signHelicopter;
        private System.Windows.Forms.DataGridViewTextBoxColumn numberHelicopter;
        private System.Windows.Forms.DataGridViewTextBoxColumn note;
        private System.Windows.Forms.DataGridViewTextBoxColumn employeeIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TypeId;
        private System.Windows.Forms.DataGridViewTextBoxColumn typeBlockDataGridViewTextBoxColumn;
    }
}