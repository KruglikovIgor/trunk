namespace KPK2.Users
{
    partial class frmEditPost
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEditPost));
      this.lbl = new System.Windows.Forms.Label();
      this.txtBox = new System.Windows.Forms.TextBox();
      this.chckBox = new System.Windows.Forms.CheckBox();
      this.label = new System.Windows.Forms.Label();
      this.btnClose = new System.Windows.Forms.Button();
      this.btnAdd = new System.Windows.Forms.Button();
      this.label2 = new System.Windows.Forms.Label();
      this.postTableAdapter = new PostTableAdapter();
      this.KPK2DataSet = new KPK2.KPK2DataSet();
      ((System.ComponentModel.ISupportInitialize)(this.KPK2DataSet)).BeginInit();
      this.SuspendLayout();
      // 
      // lbl
      // 
      resources.ApplyResources(this.lbl, "lbl");
      this.lbl.Name = "lbl";
      // 
      // txtBox
      // 
      resources.ApplyResources(this.txtBox, "txtBox");
      this.txtBox.Name = "txtBox";
      this.txtBox.TextChanged += new System.EventHandler(this.txtBox_TextChanged);
      // 
      // chckBox
      // 
      resources.ApplyResources(this.chckBox, "chckBox");
      this.chckBox.Name = "chckBox";
      this.chckBox.UseVisualStyleBackColor = true;
      this.chckBox.CheckedChanged += new System.EventHandler(this.chckBox_CheckedChanged);
      // 
      // label
      // 
      this.label.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      resources.ApplyResources(this.label, "label");
      this.label.Name = "label";
      // 
      // btnClose
      // 
      resources.ApplyResources(this.btnClose, "btnClose");
      this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnClose.Name = "btnClose";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
      // 
      // btnAdd
      // 
      resources.ApplyResources(this.btnAdd, "btnAdd");
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
      // 
      // label2
      // 
      resources.ApplyResources(this.label2, "label2");
      this.label2.Name = "label2";
      // 
      // postTableAdapter
      // 
      this.postTableAdapter.ClearBeforeFill = true;
      // 
      // KPK2DataSet
      // 
      this.KPK2DataSet.DataSetName = "KPK2DataSet";
      this.KPK2DataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
      // 
      // frmEditPost
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnClose;
      this.Controls.Add(this.label2);
      this.Controls.Add(this.btnAdd);
      this.Controls.Add(this.btnClose);
      this.Controls.Add(this.label);
      this.Controls.Add(this.chckBox);
      this.Controls.Add(this.txtBox);
      this.Controls.Add(this.lbl);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "frmEditPost";
      this.ShowIcon = false;
      ((System.ComponentModel.ISupportInitialize)(this.KPK2DataSet)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl;
        private System.Windows.Forms.TextBox txtBox;
        private System.Windows.Forms.CheckBox chckBox;
        private System.Windows.Forms.Label label;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label label2;
        private PostTableAdapter postTableAdapter;
        private KPK2DataSet KPK2DataSet;
    }
}