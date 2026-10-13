namespace KPK2.GeneralForms
{
    partial class SelfTest
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelfTest));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvSelfTest = new System.Windows.Forms.DataGridView();
            this.CheckItem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Result = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ssStatus = new System.Windows.Forms.StatusStrip();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bClose = new System.Windows.Forms.Button();
            this.bCheck = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lCaption = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelfTest)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvSelfTest
            // 
            this.dgvSelfTest.AllowUserToAddRows = false;
            this.dgvSelfTest.AllowUserToDeleteRows = false;
            this.dgvSelfTest.AllowUserToResizeColumns = false;
            this.dgvSelfTest.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSelfTest.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSelfTest.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSelfTest.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.dgvSelfTest, "dgvSelfTest");
            this.dgvSelfTest.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvSelfTest.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CheckItem,
            this.Result});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Tahoma", 9F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSelfTest.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvSelfTest.MultiSelect = false;
            this.dgvSelfTest.Name = "dgvSelfTest";
            this.dgvSelfTest.ReadOnly = true;
            this.dgvSelfTest.RowHeadersVisible = false;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSelfTest.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvSelfTest.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvSelfTest_CellFormatting);
            //this.dgvSelfTest.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this.dgvSelfTest_CellToolTipTextNeeded);
            // 
            // CheckItem
            // 
            this.CheckItem.FillWeight = 272.8934F;
            resources.ApplyResources(this.CheckItem, "CheckItem");
            this.CheckItem.Name = "CheckItem";
            this.CheckItem.ReadOnly = true;
            this.CheckItem.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.CheckItem.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Result
            // 
            this.Result.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Result.FillWeight = 7.106598F;
            resources.ApplyResources(this.Result, "Result");
            this.Result.Name = "Result";
            this.Result.ReadOnly = true;
            this.Result.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Result.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ssStatus
            // 
            resources.ApplyResources(this.ssStatus, "ssStatus");
            this.ssStatus.Name = "ssStatus";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.bClose);
            this.panel1.Controls.Add(this.bCheck);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // bClose
            // 
            this.bClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.bClose, "bClose");
            this.bClose.Name = "bClose";
            this.bClose.UseVisualStyleBackColor = true;
            this.bClose.Click += new System.EventHandler(this.bClose_Click);
            // 
            // bCheck
            // 
            resources.ApplyResources(this.bCheck, "bCheck");
            this.bCheck.Name = "bCheck";
            this.bCheck.UseVisualStyleBackColor = true;
            this.bCheck.Click += new System.EventHandler(this.bCheck_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lCaption);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // lCaption
            // 
            resources.ApplyResources(this.lCaption, "lCaption");
            this.lCaption.Name = "lCaption";
            // 
            // SelfTest
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.bClose;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ssStatus);
            this.Controls.Add(this.dgvSelfTest);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelfTest";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SelfTest_FormClosing);
            this.Load += new System.EventHandler(this.SelfTest_Load);
            this.Shown += new System.EventHandler(this.SelfTest_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelfTest)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvSelfTest;
        private System.Windows.Forms.StatusStrip ssStatus;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button bCheck;
        private System.Windows.Forms.Button bClose;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lCaption;
        private System.Windows.Forms.DataGridViewTextBoxColumn CheckItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn Result;
    }
}