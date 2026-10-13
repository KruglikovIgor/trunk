namespace KPK2.Users
{
    partial class frmPost
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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPost));
      this.dgvPost = new System.Windows.Forms.DataGridView();
      this.cId = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.cPost = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.cIsActive = new System.Windows.Forms.DataGridViewCheckBoxColumn();
      this.postBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.KPK2DataSet = new KPK2.KPK2DataSet();
      this.lbl1 = new System.Windows.Forms.Label();
      this.btnAdd = new System.Windows.Forms.Button();
      this.btnEdit = new System.Windows.Forms.Button();
      this.btnClose = new System.Windows.Forms.Button();
      this.btnAllPost = new System.Windows.Forms.Button();
      this.postTableAdapter = new PostTableAdapter();
      ((System.ComponentModel.ISupportInitialize)(this.dgvPost)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.postBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.KPK2DataSet)).BeginInit();
      this.SuspendLayout();
      // 
      // dgvPost
      // 
      this.dgvPost.AllowUserToAddRows = false;
      this.dgvPost.AllowUserToDeleteRows = false;
      this.dgvPost.AllowUserToResizeColumns = false;
      this.dgvPost.AllowUserToResizeRows = false;
      this.dgvPost.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
      this.dgvPost.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dgvPost.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      this.dgvPost.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cId,
            this.cPost,
            this.cIsActive});
      resources.ApplyResources(this.dgvPost, "dgvPost");
      this.dgvPost.MultiSelect = false;
      this.dgvPost.Name = "dgvPost";
      this.dgvPost.ReadOnly = true;
      this.dgvPost.RowHeadersVisible = false;
      this.dgvPost.TabStop = false;
      this.dgvPost.SelectionChanged += new System.EventHandler(this.dgv_SelectionChanged);
      // 
      // cId
      // 
      this.cId.FillWeight = 50F;
      resources.ApplyResources(this.cId, "cId");
      this.cId.Name = "cId";
      this.cId.ReadOnly = true;
      // 
      // cPost
      // 
      this.cPost.FillWeight = 83.24873F;
      resources.ApplyResources(this.cPost, "cPost");
      this.cPost.Name = "cPost";
      this.cPost.ReadOnly = true;
      // 
      // cIsActive
      // 
      this.cIsActive.FillWeight = 90F;
      resources.ApplyResources(this.cIsActive, "cIsActive");
      this.cIsActive.Name = "cIsActive";
      this.cIsActive.ReadOnly = true;
      // 
      // postBindingSource
      // 
      this.postBindingSource.DataMember = "Post";
      this.postBindingSource.DataSource = this.KPK2DataSet;
      this.postBindingSource.Sort = "PostName ASC";
      // 
      // KPK2DataSet
      // 
      this.KPK2DataSet.DataSetName = "KPK2DataSet";
      this.KPK2DataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
      // 
      // lbl1
      // 
      this.lbl1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      resources.ApplyResources(this.lbl1, "lbl1");
      this.lbl1.Name = "lbl1";
      // 
      // btnAdd
      // 
      resources.ApplyResources(this.btnAdd, "btnAdd");
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
      // 
      // btnEdit
      // 
      resources.ApplyResources(this.btnEdit, "btnEdit");
      this.btnEdit.Name = "btnEdit";
      this.btnEdit.UseVisualStyleBackColor = true;
      this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
      // 
      // btnClose
      // 
      this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      resources.ApplyResources(this.btnClose, "btnClose");
      this.btnClose.Name = "btnClose";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
      // 
      // btnAllPost
      // 
      resources.ApplyResources(this.btnAllPost, "btnAllPost");
      this.btnAllPost.Name = "btnAllPost";
      this.btnAllPost.UseVisualStyleBackColor = true;
      this.btnAllPost.Click += new System.EventHandler(this.btnAllPost_Click);
      // 
      // postTableAdapter
      // 
      this.postTableAdapter.ClearBeforeFill = true;
      // 
      // frmPost
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.SystemColors.ButtonFace;
      this.CancelButton = this.btnClose;
      this.Controls.Add(this.btnAllPost);
      this.Controls.Add(this.btnClose);
      this.Controls.Add(this.btnEdit);
      this.Controls.Add(this.btnAdd);
      this.Controls.Add(this.lbl1);
      this.Controls.Add(this.dgvPost);
      this.DoubleBuffered = true;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "frmPost";
      this.ShowIcon = false;
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPost_FormClosing);
      ((System.ComponentModel.ISupportInitialize)(this.dgvPost)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.postBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.KPK2DataSet)).EndInit();
      this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvPost;
        private System.Windows.Forms.Label lbl1;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnAllPost;
        private KPK2DataSet KPK2DataSet;
        private PostTableAdapter postTableAdapter;
        private System.Windows.Forms.BindingSource postBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn cId;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPost;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cIsActive;
    }
}