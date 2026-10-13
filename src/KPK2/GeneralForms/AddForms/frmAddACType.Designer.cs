namespace KPK2.GeneralForms.AddForms
{
  partial class frmAddACType
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddACType));
      this.lbExistingACTypes = new System.Windows.Forms.ListBox();
      this.tbNewACType = new System.Windows.Forms.TextBox();
      this.bAdd = new System.Windows.Forms.Button();
      this.bCLose = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // lbExistingACTypes
      // 
      this.lbExistingACTypes.FormattingEnabled = true;
      resources.ApplyResources(this.lbExistingACTypes, "lbExistingACTypes");
      this.lbExistingACTypes.Name = "lbExistingACTypes";
      this.lbExistingACTypes.SelectedIndexChanged += new System.EventHandler(this.lbExistingACTypes_SelectedIndexChanged);
      // 
      // tbNewACType
      // 
      resources.ApplyResources(this.tbNewACType, "tbNewACType");
      this.tbNewACType.Name = "tbNewACType";
      // 
      // bAdd
      // 
      resources.ApplyResources(this.bAdd, "bAdd");
      this.bAdd.Name = "bAdd";
      this.bAdd.UseVisualStyleBackColor = true;
      this.bAdd.Click += new System.EventHandler(this.bAdd_Click);
      // 
      // bCLose
      // 
      resources.ApplyResources(this.bCLose, "bCLose");
      this.bCLose.Name = "bCLose";
      this.bCLose.UseVisualStyleBackColor = true;
      this.bCLose.Click += new System.EventHandler(this.bCLose_Click);
      // 
      // label1
      // 
      resources.ApplyResources(this.label1, "label1");
      this.label1.Name = "label1";
      // 
      // label2
      // 
      resources.ApplyResources(this.label2, "label2");
      this.label2.Name = "label2";
      // 
      // frmAddACType
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ControlBox = false;
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.bCLose);
      this.Controls.Add(this.bAdd);
      this.Controls.Add(this.tbNewACType);
      this.Controls.Add(this.lbExistingACTypes);
      this.Name = "frmAddACType";
      this.Load += new System.EventHandler(this.frmAddACType_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ListBox lbExistingACTypes;
    private System.Windows.Forms.TextBox tbNewACType;
    private System.Windows.Forms.Button bAdd;
    private System.Windows.Forms.Button bCLose;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
  }
}