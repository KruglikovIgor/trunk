
namespace KPK2.GeneralForms.ReferenceForm
{
    partial class frmACType
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmACType));
            this.dgvACType = new System.Windows.Forms.DataGridView();
            this.cId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ÒACType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.helicopterTypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.bDeleteMod = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.bAddMod = new System.Windows.Forms.Button();
            this.bEditMod = new System.Windows.Forms.Button();
            this.bClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvACType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.helicopterTypeBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvACType
            // 
            this.dgvACType.AllowUserToAddRows = false;
            this.dgvACType.AllowUserToDeleteRows = false;
            this.dgvACType.AllowUserToResizeRows = false;
            resources.ApplyResources(this.dgvACType, "dgvACType");
            this.dgvACType.AutoGenerateColumns = false;
            this.dgvACType.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvACType.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cId,
            this.ÒACType});
            this.dgvACType.DataSource = this.helicopterTypeBindingSource;
            this.dgvACType.MultiSelect = false;
            this.dgvACType.Name = "dgvACType";
            this.dgvACType.ReadOnly = true;
            this.dgvACType.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvACType.StandardTab = true;
            this.dgvACType.CurrentCellChanged += new System.EventHandler(this.dgvACType_CurrentCellChanged);
            this.dgvACType.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.helicopterDataGridView_RowEnter);
            this.dgvACType.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.helicopterDataGridView_RowsAdded);
            this.dgvACType.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.helicopterDataGridView_RowsRemoved);
            this.dgvACType.KeyUp += new System.Windows.Forms.KeyEventHandler(this.jobsReferenceDataGridView_KeyUp);
            // 
            // cId
            // 
            this.cId.DataPropertyName = "Id";
            resources.ApplyResources(this.cId, "cId");
            this.cId.Name = "cId";
            this.cId.ReadOnly = true;
            // 
            // ÒACType
            // 
            this.ÒACType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ÒACType.DataPropertyName = "ACTypeName";
            resources.ApplyResources(this.ÒACType, "ÒACType");
            this.ÒACType.Name = "ÒACType";
            this.ÒACType.ReadOnly = true;
            // 
            // helicopterTypeBindingSource
            // 
            this.helicopterTypeBindingSource.DataMember = "HelicopterType";
            // 
            // bDeleteMod
            // 
            resources.ApplyResources(this.bDeleteMod, "bDeleteMod");
            this.bDeleteMod.Name = "bDeleteMod";
            this.bDeleteMod.UseVisualStyleBackColor = true;
            this.bDeleteMod.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Name = "label1";
            // 
            // bAddMod
            // 
            resources.ApplyResources(this.bAddMod, "bAddMod");
            this.bAddMod.Name = "bAddMod";
            this.bAddMod.UseVisualStyleBackColor = true;
            this.bAddMod.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // bEditMod
            // 
            resources.ApplyResources(this.bEditMod, "bEditMod");
            this.bEditMod.Name = "bEditMod";
            this.bEditMod.UseVisualStyleBackColor = true;
            this.bEditMod.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // bClose
            // 
            resources.ApplyResources(this.bClose, "bClose");
            this.bClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bClose.Name = "bClose";
            this.bClose.UseVisualStyleBackColor = true;
            this.bClose.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmACType
            // 
            this.AcceptButton = this.bAddMod;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bClose;
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bClose);
            this.Controls.Add(this.bEditMod);
            this.Controls.Add(this.bAddMod);
            this.Controls.Add(this.bDeleteMod);
            this.Controls.Add(this.dgvACType);
            this.MinimizeBox = false;
            this.Name = "frmACType";
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.frmHelicopter_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvACType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.helicopterTypeBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvACType;
        private System.Windows.Forms.Button bDeleteMod;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bAddMod;
        private System.Windows.Forms.Button bEditMod;
        private System.Windows.Forms.Button bClose;
        private System.Windows.Forms.BindingSource helicopterTypeBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn cId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ÒACType;
    }
}