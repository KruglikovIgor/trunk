namespace KPK2.GeneralForms
{
    partial class frmReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmReport));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btn_Close = new System.Windows.Forms.Button();
            this.btn_Print = new System.Windows.Forms.Button();
            this.lbl_Sign = new System.Windows.Forms.Label();
            this.lbl_HelicoptNumb = new System.Windows.Forms.Label();
            this.lbl_Mark = new System.Windows.Forms.Label();
            this.lbl_Name = new System.Windows.Forms.Label();
            this.lbl_dtRepair = new System.Windows.Forms.Label();
            this.lbl_dt = new System.Windows.Forms.Label();
            this.lbl_Number = new System.Windows.Forms.Label();
            this.lbl2 = new System.Windows.Forms.Label();
            this.lbl4 = new System.Windows.Forms.Label();
            this.lbl7 = new System.Windows.Forms.Label();
            this.lbl6 = new System.Windows.Forms.Label();
            this.lbl5 = new System.Windows.Forms.Label();
            this.lbl3 = new System.Windows.Forms.Label();
            this.lbl1 = new System.Windows.Forms.Label();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DocumentLink = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lHint = new System.Windows.Forms.Label();
            this.lCheckVerify = new System.Windows.Forms.Label();
            this.cbCheckVerify = new System.Windows.Forms.ComboBox();
            this.ds = new KPK2.KPK2DataSet();
            this.report_15_TA = new KPK2.Report_15TableAdapter();
            this.viewFullNameTA = new KPK2.ViewFullNameTableAdapter();
            this.measuringTA = new KPK2.MeasuringParamTableAdapter();
            this.descriptionTA = new KPK2.DescriptionParamTableAdapter();
            this.report_32_TA = new KPK2.Report_32TableAdapter();
            this.cbAutoResult = new System.Windows.Forms.CheckBox();
            this.cbEmptyReport = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ds)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_Close
            // 
            resources.ApplyResources(this.btn_Close, "btn_Close");
            this.btn_Close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.UseVisualStyleBackColor = true;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // btn_Print
            // 
            resources.ApplyResources(this.btn_Print, "btn_Print");
            this.btn_Print.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Print.Name = "btn_Print";
            this.btn_Print.UseVisualStyleBackColor = true;
            this.btn_Print.Click += new System.EventHandler(this.btn_Print_Click);
            // 
            // lbl_Sign
            // 
            resources.ApplyResources(this.lbl_Sign, "lbl_Sign");
            this.lbl_Sign.Name = "lbl_Sign";
            // 
            // lbl_HelicoptNumb
            // 
            resources.ApplyResources(this.lbl_HelicoptNumb, "lbl_HelicoptNumb");
            this.lbl_HelicoptNumb.Name = "lbl_HelicoptNumb";
            // 
            // lbl_Mark
            // 
            resources.ApplyResources(this.lbl_Mark, "lbl_Mark");
            this.lbl_Mark.Name = "lbl_Mark";
            // 
            // lbl_Name
            // 
            resources.ApplyResources(this.lbl_Name, "lbl_Name");
            this.lbl_Name.Name = "lbl_Name";
            // 
            // lbl_dtRepair
            // 
            resources.ApplyResources(this.lbl_dtRepair, "lbl_dtRepair");
            this.lbl_dtRepair.Name = "lbl_dtRepair";
            // 
            // lbl_dt
            // 
            resources.ApplyResources(this.lbl_dt, "lbl_dt");
            this.lbl_dt.Name = "lbl_dt";
            // 
            // lbl_Number
            // 
            resources.ApplyResources(this.lbl_Number, "lbl_Number");
            this.lbl_Number.Name = "lbl_Number";
            // 
            // lbl2
            // 
            resources.ApplyResources(this.lbl2, "lbl2");
            this.lbl2.Name = "lbl2";
            // 
            // lbl4
            // 
            resources.ApplyResources(this.lbl4, "lbl4");
            this.lbl4.Name = "lbl4";
            // 
            // lbl7
            // 
            resources.ApplyResources(this.lbl7, "lbl7");
            this.lbl7.Name = "lbl7";
            // 
            // lbl6
            // 
            resources.ApplyResources(this.lbl6, "lbl6");
            this.lbl6.Name = "lbl6";
            // 
            // lbl5
            // 
            resources.ApplyResources(this.lbl5, "lbl5");
            this.lbl5.Name = "lbl5";
            // 
            // lbl3
            // 
            resources.ApplyResources(this.lbl3, "lbl3");
            this.lbl3.Name = "lbl3";
            // 
            // lbl1
            // 
            resources.ApplyResources(this.lbl1, "lbl1");
            this.lbl1.Name = "lbl1";
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.AllowUserToResizeColumns = false;
            this.dgv.AllowUserToResizeRows = false;
            resources.ApplyResources(this.dgv, "dgv");
            this.dgv.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgv.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7,
            this.DocumentLink});
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Tahoma", 9F);
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(223)))), ((int)(((byte)(227)))));
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv.DefaultCellStyle = dataGridViewCellStyle7;
            this.dgv.MultiSelect = false;
            this.dgv.Name = "dgv";
            this.dgv.ReadOnly = true;
            this.dgv.RowHeadersVisible = false;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_CellMouseEnter);
            this.dgv.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_CellMouseLeave);
            // 
            // Column1
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.Column1, "Column1");
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Column2.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.Column2, "Column2");
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column3
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Column3.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this.Column3, "Column3");
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column4
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Column4.DefaultCellStyle = dataGridViewCellStyle5;
            resources.ApplyResources(this.Column4, "Column4");
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column5
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.Format = "N2";
            dataGridViewCellStyle6.NullValue = null;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Column5.DefaultCellStyle = dataGridViewCellStyle6;
            resources.ApplyResources(this.Column5, "Column5");
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column6
            // 
            resources.ApplyResources(this.Column6, "Column6");
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            // 
            // Column7
            // 
            resources.ApplyResources(this.Column7, "Column7");
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            // 
            // DocumentLink
            // 
            resources.ApplyResources(this.DocumentLink, "DocumentLink");
            this.DocumentLink.Name = "DocumentLink";
            this.DocumentLink.ReadOnly = true;
            // 
            // lHint
            // 
            resources.ApplyResources(this.lHint, "lHint");
            this.lHint.AutoEllipsis = true;
            this.lHint.BackColor = System.Drawing.Color.LightYellow;
            this.lHint.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lHint.Name = "lHint";
            // 
            // lCheckVerify
            // 
            resources.ApplyResources(this.lCheckVerify, "lCheckVerify");
            this.lCheckVerify.Name = "lCheckVerify";
            // 
            // cbCheckVerify
            // 
            resources.ApplyResources(this.cbCheckVerify, "cbCheckVerify");
            this.cbCheckVerify.FormattingEnabled = true;
            this.cbCheckVerify.Items.AddRange(new object[] {
            resources.GetString("cbCheckVerify.Items"),
            resources.GetString("cbCheckVerify.Items1")});
            this.cbCheckVerify.Name = "cbCheckVerify";
            this.cbCheckVerify.SelectedIndexChanged += new System.EventHandler(this.cbCheckVerify_SelectedIndexChanged);
            // 
            // ds
            // 
            this.ds.DataSetName = "KPK2DataSet";
            this.ds.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // report_15_TA
            // 
            this.report_15_TA.ClearBeforeFill = true;
            // 
            // viewFullNameTA
            // 
            this.viewFullNameTA.ClearBeforeFill = true;
            // 
            // measuringTA
            // 
            this.measuringTA.ClearBeforeFill = true;
            // 
            // descriptionTA
            // 
            this.descriptionTA.ClearBeforeFill = true;
            // 
            // report_32_TA
            // 
            this.report_32_TA.ClearBeforeFill = true;
            // 
            // cbAutoResult
            // 
            resources.ApplyResources(this.cbAutoResult, "cbAutoResult");
            this.cbAutoResult.Checked = true;
            this.cbAutoResult.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAutoResult.Name = "cbAutoResult";
            this.cbAutoResult.UseVisualStyleBackColor = true;
            this.cbAutoResult.CheckedChanged += new System.EventHandler(this.cbAutoResult_CheckedChanged);
            // 
            // cbEmptyReport
            // 
            resources.ApplyResources(this.cbEmptyReport, "cbEmptyReport");
            this.cbEmptyReport.Name = "cbEmptyReport";
            this.cbEmptyReport.UseVisualStyleBackColor = true;
            // 
            // frmReport
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btn_Close;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.cbEmptyReport);
            this.Controls.Add(this.cbAutoResult);
            this.Controls.Add(this.cbCheckVerify);
            this.Controls.Add(this.lCheckVerify);
            this.Controls.Add(this.lHint);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.lbl_Sign);
            this.Controls.Add(this.lbl_HelicoptNumb);
            this.Controls.Add(this.lbl_Mark);
            this.Controls.Add(this.lbl_Name);
            this.Controls.Add(this.lbl_dtRepair);
            this.Controls.Add(this.lbl_dt);
            this.Controls.Add(this.lbl_Number);
            this.Controls.Add(this.lbl2);
            this.Controls.Add(this.lbl4);
            this.Controls.Add(this.lbl7);
            this.Controls.Add(this.lbl6);
            this.Controls.Add(this.lbl5);
            this.Controls.Add(this.lbl3);
            this.Controls.Add(this.lbl1);
            this.Controls.Add(this.btn_Print);
            this.Controls.Add(this.btn_Close);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmReport";
            this.ShowIcon = false;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmReport_Load);
            this.Shown += new System.EventHandler(this.frmReport_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ds)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Close;
        private System.Windows.Forms.Button btn_Print;
        public System.Windows.Forms.Label lbl_Sign;
        public System.Windows.Forms.Label lbl_HelicoptNumb;
        public System.Windows.Forms.Label lbl_Mark;
        public System.Windows.Forms.Label lbl_Name;
        public System.Windows.Forms.Label lbl_dtRepair;
        public System.Windows.Forms.Label lbl_dt;
        public System.Windows.Forms.Label lbl_Number;
        private System.Windows.Forms.Label lbl2;
        private System.Windows.Forms.Label lbl4;
        private System.Windows.Forms.Label lbl7;
        private System.Windows.Forms.Label lbl6;
        private System.Windows.Forms.Label lbl5;
        private System.Windows.Forms.Label lbl3;
        private System.Windows.Forms.Label lbl1;
        private KPK2DataSet ds;
        private Report_15TableAdapter report_15_TA;
        private ViewFullNameTableAdapter viewFullNameTA;
        private MeasuringParamTableAdapter measuringTA;
        private DescriptionParamTableAdapter descriptionTA;
        private Report_32TableAdapter report_32_TA;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Label lHint;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn DocumentLink;
        private System.Windows.Forms.Label lCheckVerify;
        private System.Windows.Forms.ComboBox cbCheckVerify;
        private System.Windows.Forms.CheckBox cbAutoResult;
        private System.Windows.Forms.CheckBox cbEmptyReport;
    }
}