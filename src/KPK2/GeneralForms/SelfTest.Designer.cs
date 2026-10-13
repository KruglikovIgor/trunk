namespace Diss32TO
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelfTest));
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
            this.dgvSelfTest.AccessibleDescription = null;
            this.dgvSelfTest.AccessibleName = null;
            this.dgvSelfTest.AllowUserToAddRows = false;
            this.dgvSelfTest.AllowUserToDeleteRows = false;
            this.dgvSelfTest.AllowUserToResizeColumns = false;
            this.dgvSelfTest.AllowUserToResizeRows = false;
            resources.ApplyResources(this.dgvSelfTest, "dgvSelfTest");
            this.dgvSelfTest.BackgroundImage = null;
            this.dgvSelfTest.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvSelfTest.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CheckItem,
            this.Result});
            this.dgvSelfTest.Font = null;
            this.dgvSelfTest.MultiSelect = false;
            this.dgvSelfTest.Name = "dgvSelfTest";
            this.dgvSelfTest.ReadOnly = true;
            this.dgvSelfTest.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSelfTest_CellValueChanged);
            this.dgvSelfTest.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // CheckItem
            // 
            this.CheckItem.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.CheckItem.FillWeight = 250F;
            resources.ApplyResources(this.CheckItem, "CheckItem");
            this.CheckItem.Name = "CheckItem";
            this.CheckItem.ReadOnly = true;
            this.CheckItem.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Result
            // 
            this.Result.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Result.FillWeight = 150F;
            resources.ApplyResources(this.Result, "Result");
            this.Result.Name = "Result";
            this.Result.ReadOnly = true;
            this.Result.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // ssStatus
            // 
            this.ssStatus.AccessibleDescription = null;
            this.ssStatus.AccessibleName = null;
            resources.ApplyResources(this.ssStatus, "ssStatus");
            this.ssStatus.BackgroundImage = null;
            this.ssStatus.Font = null;
            this.ssStatus.Name = "ssStatus";
            // 
            // panel1
            // 
            this.panel1.AccessibleDescription = null;
            this.panel1.AccessibleName = null;
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackgroundImage = null;
            this.panel1.Controls.Add(this.bClose);
            this.panel1.Controls.Add(this.bCheck);
            this.panel1.Font = null;
            this.panel1.Name = "panel1";
            // 
            // bClose
            // 
            this.bClose.AccessibleDescription = null;
            this.bClose.AccessibleName = null;
            resources.ApplyResources(this.bClose, "bClose");
            this.bClose.BackgroundImage = null;
            this.bClose.Name = "bClose";
            this.bClose.UseVisualStyleBackColor = true;
            this.bClose.Click += new System.EventHandler(this.bClose_Click);
            // 
            // bCheck
            // 
            this.bCheck.AccessibleDescription = null;
            this.bCheck.AccessibleName = null;
            resources.ApplyResources(this.bCheck, "bCheck");
            this.bCheck.BackgroundImage = null;
            this.bCheck.Name = "bCheck";
            this.bCheck.UseVisualStyleBackColor = true;
            this.bCheck.Click += new System.EventHandler(this.bCheck_Click);
            // 
            // panel2
            // 
            this.panel2.AccessibleDescription = null;
            this.panel2.AccessibleName = null;
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackgroundImage = null;
            this.panel2.Controls.Add(this.lCaption);
            this.panel2.Font = null;
            this.panel2.Name = "panel2";
            // 
            // lCaption
            // 
            this.lCaption.AccessibleDescription = null;
            this.lCaption.AccessibleName = null;
            resources.ApplyResources(this.lCaption, "lCaption");
            this.lCaption.Name = "lCaption";
            // 
            // SelfTest
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.ControlBox = false;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ssStatus);
            this.Controls.Add(this.dgvSelfTest);
            this.Font = null;
            this.Icon = null;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelfTest";
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