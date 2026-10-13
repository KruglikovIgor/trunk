namespace UserControlsKPK2
{
  partial class IndicatorValue
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IndicatorValue));
            this.tbValue = new System.Windows.Forms.TextBox();
            this.lDownSign = new System.Windows.Forms.Label();
            this.lUpSign = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tbValue
            // 
            resources.ApplyResources(this.tbValue, "tbValue");
            this.tbValue.Name = "tbValue";
            this.tbValue.TextChanged += new System.EventHandler(this.tbValue_TextChanged);
            this.tbValue.Validating += new System.ComponentModel.CancelEventHandler(this.tbValue_Validating);
            // 
            // lDownSign
            // 
            resources.ApplyResources(this.lDownSign, "lDownSign");
            this.lDownSign.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lDownSign.Name = "lDownSign";
            // 
            // lUpSign
            // 
            resources.ApplyResources(this.lUpSign, "lUpSign");
            this.lUpSign.Name = "lUpSign";
            // 
            // IndicatorValue
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tbValue);
            this.Controls.Add(this.lDownSign);
            this.Controls.Add(this.lUpSign);
            resources.ApplyResources(this, "$this");
            this.Name = "IndicatorValue";
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion
    private System.Windows.Forms.Label lDownSign;
    private System.Windows.Forms.Label lUpSign;
        public System.Windows.Forms.TextBox tbValue;
    }
}
