namespace KPK2.MyControl
{
  partial class ControlDVS_24
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControlDVS_24));
      this.bKZ2 = new System.Windows.Forms.Button();
      this.tbB8_Forward = new System.Windows.Forms.TextBox();
      this.tbB8_ToSide = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.bCheckMapCorner = new System.Windows.Forms.Button();
      this.gbBlock8 = new System.Windows.Forms.GroupBox();
      this.gbBlock10 = new System.Windows.Forms.GroupBox();
      this.label1 = new System.Windows.Forms.Label();
      this.tbB10_Up = new System.Windows.Forms.TextBox();
      this.tbB10_ToSide = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.lTime = new System.Windows.Forms.Label();
      this.pbTime = new System.Windows.Forms.ProgressBar();
      this.gbBlock8.SuspendLayout();
      this.gbBlock10.SuspendLayout();
      this.SuspendLayout();
      // 
      // bKZ2
      // 
      resources.ApplyResources(this.bKZ2, "bKZ2");
      this.bKZ2.Name = "bKZ2";
      this.bKZ2.UseVisualStyleBackColor = true;
      this.bKZ2.Click += new System.EventHandler(this.bKZ2_Click);
      // 
      // tbB8_Forward
      // 
      resources.ApplyResources(this.tbB8_Forward, "tbB8_Forward");
      this.tbB8_Forward.Name = "tbB8_Forward";
      this.tbB8_Forward.Validated += new System.EventHandler(this.TextBox_Validated);
      // 
      // tbB8_ToSide
      // 
      resources.ApplyResources(this.tbB8_ToSide, "tbB8_ToSide");
      this.tbB8_ToSide.Name = "tbB8_ToSide";
      this.tbB8_ToSide.Validated += new System.EventHandler(this.TextBox_Validated);
      // 
      // label3
      // 
      resources.ApplyResources(this.label3, "label3");
      this.label3.Name = "label3";
      // 
      // label4
      // 
      resources.ApplyResources(this.label4, "label4");
      this.label4.Name = "label4";
      // 
      // bCheckMapCorner
      // 
      resources.ApplyResources(this.bCheckMapCorner, "bCheckMapCorner");
      this.bCheckMapCorner.Name = "bCheckMapCorner";
      this.bCheckMapCorner.UseVisualStyleBackColor = true;
      this.bCheckMapCorner.Click += new System.EventHandler(this.bCheckMapCorner_Click);
      // 
      // gbBlock8
      // 
      this.gbBlock8.Controls.Add(this.label3);
      this.gbBlock8.Controls.Add(this.tbB8_Forward);
      this.gbBlock8.Controls.Add(this.tbB8_ToSide);
      this.gbBlock8.Controls.Add(this.label4);
      resources.ApplyResources(this.gbBlock8, "gbBlock8");
      this.gbBlock8.Name = "gbBlock8";
      this.gbBlock8.TabStop = false;
      // 
      // gbBlock10
      // 
      this.gbBlock10.Controls.Add(this.label1);
      this.gbBlock10.Controls.Add(this.tbB10_Up);
      this.gbBlock10.Controls.Add(this.tbB10_ToSide);
      this.gbBlock10.Controls.Add(this.label2);
      resources.ApplyResources(this.gbBlock10, "gbBlock10");
      this.gbBlock10.Name = "gbBlock10";
      this.gbBlock10.TabStop = false;
      // 
      // label1
      // 
      resources.ApplyResources(this.label1, "label1");
      this.label1.Name = "label1";
      // 
      // tbB10_Up
      // 
      resources.ApplyResources(this.tbB10_Up, "tbB10_Up");
      this.tbB10_Up.Name = "tbB10_Up";
      this.tbB10_Up.Validated += new System.EventHandler(this.TextBox_Validated);
      // 
      // tbB10_ToSide
      // 
      resources.ApplyResources(this.tbB10_ToSide, "tbB10_ToSide");
      this.tbB10_ToSide.Name = "tbB10_ToSide";
      this.tbB10_ToSide.Validated += new System.EventHandler(this.TextBox_Validated);
      // 
      // label2
      // 
      resources.ApplyResources(this.label2, "label2");
      this.label2.Name = "label2";
      // 
      // lTime
      // 
      resources.ApplyResources(this.lTime, "lTime");
      this.lTime.Name = "lTime";
      // 
      // pbTime
      // 
      resources.ApplyResources(this.pbTime, "pbTime");
      this.pbTime.Maximum = 900;
      this.pbTime.Name = "pbTime";
      this.pbTime.Step = 1;
      // 
      // ControlDVS_24
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
      this.Controls.Add(this.lTime);
      this.Controls.Add(this.pbTime);
      this.Controls.Add(this.gbBlock10);
      this.Controls.Add(this.gbBlock8);
      this.Controls.Add(this.bCheckMapCorner);
      this.Controls.Add(this.bKZ2);
      this.Name = "ControlDVS_24";
      this.gbBlock8.ResumeLayout(false);
      this.gbBlock8.PerformLayout();
      this.gbBlock10.ResumeLayout(false);
      this.gbBlock10.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button bKZ2;
    private System.Windows.Forms.TextBox tbB8_Forward;
    private System.Windows.Forms.TextBox tbB8_ToSide;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Button bCheckMapCorner;
    private System.Windows.Forms.GroupBox gbBlock8;
    private System.Windows.Forms.GroupBox gbBlock10;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox tbB10_Up;
    private System.Windows.Forms.TextBox tbB10_ToSide;
    private System.Windows.Forms.Label label2;
    internal System.Windows.Forms.Label lTime;
    internal System.Windows.Forms.ProgressBar pbTime;
  }
}
