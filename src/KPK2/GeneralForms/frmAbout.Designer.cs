namespace KPK2.GeneralForms
{
    partial class frmAbout
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAbout));
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      this.lProductName = new System.Windows.Forms.Label();
      this.tbInfo = new System.Windows.Forms.TextBox();
      this.lVersion = new System.Windows.Forms.Label();
      this.lCopyright = new System.Windows.Forms.Label();
      this.dgvIdentity = new System.Windows.Forms.DataGridView();
      this.cName = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.cVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.cHash = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.lIdentityHeader = new System.Windows.Forms.Label();
      this.bOK = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.dgvIdentity)).BeginInit();
      this.SuspendLayout();
      // 
      // lProductName
      // 
      resources.ApplyResources(this.lProductName, "lProductName");
      this.lProductName.ForeColor = System.Drawing.SystemColors.ActiveCaption;
      this.lProductName.Name = "lProductName";
      this.lProductName.Click += new System.EventHandler(this.lProductName_Click);
      this.lProductName.DoubleClick += new System.EventHandler(this.lProductName_DoubleClick);
      // 
      // tbInfo
      // 
      this.tbInfo.BackColor = System.Drawing.SystemColors.Control;
      resources.ApplyResources(this.tbInfo, "tbInfo");
      this.tbInfo.Name = "tbInfo";
      this.tbInfo.ReadOnly = true;
      // 
      // lVersion
      // 
      resources.ApplyResources(this.lVersion, "lVersion");
      this.lVersion.Name = "lVersion";
      // 
      // lCopyright
      // 
      resources.ApplyResources(this.lCopyright, "lCopyright");
      this.lCopyright.Name = "lCopyright";
      // 
      // dgvIdentity
      // 
      this.dgvIdentity.AllowUserToAddRows = false;
      this.dgvIdentity.AllowUserToDeleteRows = false;
      this.dgvIdentity.AllowUserToResizeColumns = false;
      this.dgvIdentity.AllowUserToResizeRows = false;
      this.dgvIdentity.BackgroundColor = System.Drawing.SystemColors.ActiveBorder;
      this.dgvIdentity.BorderStyle = System.Windows.Forms.BorderStyle.None;
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 9F);
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dgvIdentity.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      this.dgvIdentity.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvIdentity.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cName,
            this.cVersion,
            this.cHash});
      dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
      dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle3.Font = new System.Drawing.Font("Tahoma", 9F);
      dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.dgvIdentity.DefaultCellStyle = dataGridViewCellStyle3;
      resources.ApplyResources(this.dgvIdentity, "dgvIdentity");
      this.dgvIdentity.Name = "dgvIdentity";
      this.dgvIdentity.ReadOnly = true;
      dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
      dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle4.Font = new System.Drawing.Font("Tahoma", 9F);
      dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dgvIdentity.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
      this.dgvIdentity.RowHeadersVisible = false;
      this.dgvIdentity.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
      // 
      // cName
      // 
      resources.ApplyResources(this.cName, "cName");
      this.cName.Name = "cName";
      this.cName.ReadOnly = true;
      this.cName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      // 
      // cVersion
      // 
      resources.ApplyResources(this.cVersion, "cVersion");
      this.cVersion.Name = "cVersion";
      this.cVersion.ReadOnly = true;
      this.cVersion.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      // 
      // cHash
      // 
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
      this.cHash.DefaultCellStyle = dataGridViewCellStyle2;
      resources.ApplyResources(this.cHash, "cHash");
      this.cHash.Name = "cHash";
      this.cHash.ReadOnly = true;
      this.cHash.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      // 
      // lIdentityHeader
      // 
      resources.ApplyResources(this.lIdentityHeader, "lIdentityHeader");
      this.lIdentityHeader.Name = "lIdentityHeader";
      // 
      // bOK
      // 
      resources.ApplyResources(this.bOK, "bOK");
      this.bOK.Name = "bOK";
      this.bOK.UseVisualStyleBackColor = true;
      this.bOK.Click += new System.EventHandler(this.bOK_Click);
      // 
      // frmAbout
      // 
      this.AcceptButton = this.bOK;
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.bOK);
      this.Controls.Add(this.dgvIdentity);
      this.Controls.Add(this.lIdentityHeader);
      this.Controls.Add(this.lCopyright);
      this.Controls.Add(this.lVersion);
      this.Controls.Add(this.tbInfo);
      this.Controls.Add(this.lProductName);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "frmAbout";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAbout_FormClosing);
      this.Load += new System.EventHandler(this.frmAbout_Load);
      this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmAbout_KeyDown);
      this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmAbout_KeyUp);
      ((System.ComponentModel.ISupportInitialize)(this.dgvIdentity)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lProductName;
        private System.Windows.Forms.TextBox tbInfo;
        private System.Windows.Forms.Label lVersion;
        private System.Windows.Forms.Label lCopyright;
        private System.Windows.Forms.DataGridView dgvIdentity;
        private System.Windows.Forms.Label lIdentityHeader;
        private System.Windows.Forms.Button bOK;
        private System.Windows.Forms.DataGridViewTextBoxColumn cName;
        private System.Windows.Forms.DataGridViewTextBoxColumn cVersion;
        private System.Windows.Forms.DataGridViewTextBoxColumn cHash;
    }
}