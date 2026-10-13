namespace KPK2.MyControl
{
  partial class CorrectionCode
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CorrectionCode));
      this.gbCorrectionCodes = new System.Windows.Forms.GroupBox();
      this.nudCode1 = new System.Windows.Forms.NumericUpDown();
      this.label7 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.mtbCode1 = new System.Windows.Forms.MaskedTextBox();
      this.mtbCode2 = new System.Windows.Forms.MaskedTextBox();
      this.nudCode2 = new System.Windows.Forms.NumericUpDown();
      this.mtbCode3 = new System.Windows.Forms.MaskedTextBox();
      this.nudCode3 = new System.Windows.Forms.NumericUpDown();
      this.gbCorrectionCodes.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.nudCode1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.nudCode2)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.nudCode3)).BeginInit();
      this.SuspendLayout();
      // 
      // gbCorrectionCodes
      // 
      this.gbCorrectionCodes.Controls.Add(this.mtbCode3);
      this.gbCorrectionCodes.Controls.Add(this.nudCode3);
      this.gbCorrectionCodes.Controls.Add(this.mtbCode2);
      this.gbCorrectionCodes.Controls.Add(this.nudCode2);
      this.gbCorrectionCodes.Controls.Add(this.mtbCode1);
      this.gbCorrectionCodes.Controls.Add(this.nudCode1);
      this.gbCorrectionCodes.Controls.Add(this.label7);
      this.gbCorrectionCodes.Controls.Add(this.label6);
      this.gbCorrectionCodes.Controls.Add(this.label5);
      resources.ApplyResources(this.gbCorrectionCodes, "gbCorrectionCodes");
      this.gbCorrectionCodes.Name = "gbCorrectionCodes";
      this.gbCorrectionCodes.TabStop = false;
      // 
      // nudCode1
      // 
      resources.ApplyResources(this.nudCode1, "nudCode1");
      this.nudCode1.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
      this.nudCode1.Name = "nudCode1";
      this.nudCode1.ValueChanged += new System.EventHandler(this.nudCode1_ValueChanged);
      // 
      // label7
      // 
      resources.ApplyResources(this.label7, "label7");
      this.label7.Name = "label7";
      // 
      // label6
      // 
      resources.ApplyResources(this.label6, "label6");
      this.label6.Name = "label6";
      // 
      // label5
      // 
      resources.ApplyResources(this.label5, "label5");
      this.label5.Name = "label5";
      // 
      // mtbCode1
      // 
      resources.ApplyResources(this.mtbCode1, "mtbCode1");
      this.mtbCode1.Name = "mtbCode1";
      this.mtbCode1.Validating += new System.ComponentModel.CancelEventHandler(this.mtbCode1_Validating);
      // 
      // mtbCode2
      // 
      resources.ApplyResources(this.mtbCode2, "mtbCode2");
      this.mtbCode2.Name = "mtbCode2";
      this.mtbCode2.Validating += new System.ComponentModel.CancelEventHandler(this.mtbCode2_Validating);
      // 
      // nudCode2
      // 
      resources.ApplyResources(this.nudCode2, "nudCode2");
      this.nudCode2.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
      this.nudCode2.Name = "nudCode2";
      this.nudCode2.ValueChanged += new System.EventHandler(this.nudCode2_ValueChanged);
      // 
      // mtbCode3
      // 
      resources.ApplyResources(this.mtbCode3, "mtbCode3");
      this.mtbCode3.Name = "mtbCode3";
      this.mtbCode3.Validating += new System.ComponentModel.CancelEventHandler(this.mtbCode3_Validating);
      // 
      // nudCode3
      // 
      resources.ApplyResources(this.nudCode3, "nudCode3");
      this.nudCode3.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
      this.nudCode3.Name = "nudCode3";
      this.nudCode3.ValueChanged += new System.EventHandler(this.nudCode3_ValueChanged);
      // 
      // CorrectionCode
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
      this.Controls.Add(this.gbCorrectionCodes);
      this.Name = "CorrectionCode";
      this.gbCorrectionCodes.ResumeLayout(false);
      this.gbCorrectionCodes.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.nudCode1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.nudCode2)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.nudCode3)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox gbCorrectionCodes;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label label5;
    public System.Windows.Forms.NumericUpDown nudCode1;
    private System.Windows.Forms.MaskedTextBox mtbCode3;
    public System.Windows.Forms.NumericUpDown nudCode3;
    private System.Windows.Forms.MaskedTextBox mtbCode2;
    public System.Windows.Forms.NumericUpDown nudCode2;
    private System.Windows.Forms.MaskedTextBox mtbCode1;
  }
}
