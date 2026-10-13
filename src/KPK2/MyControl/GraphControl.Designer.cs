namespace KPK2.MyControl
{
  partial class GraphControl
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
      this.components = new System.ComponentModel.Container();
      this.zgcMainGraph = new ZedGraph.ZedGraphControl();
      this.SuspendLayout();
      // 
      // zgcMainGraph
      // 
      this.zgcMainGraph.Location = new System.Drawing.Point(3, 3);
      this.zgcMainGraph.Name = "zgcMainGraph";
      this.zgcMainGraph.ScrollGrace = 0D;
      this.zgcMainGraph.ScrollMaxX = 0D;
      this.zgcMainGraph.ScrollMaxY = 0D;
      this.zgcMainGraph.ScrollMaxY2 = 0D;
      this.zgcMainGraph.ScrollMinX = 0D;
      this.zgcMainGraph.ScrollMinY = 0D;
      this.zgcMainGraph.ScrollMinY2 = 0D;
      this.zgcMainGraph.Size = new System.Drawing.Size(597, 354);
      this.zgcMainGraph.TabIndex = 0;
      this.zgcMainGraph.UseExtendedPrintDialog = true;
      // 
      // GraphControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.zgcMainGraph);
      this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.Name = "GraphControl";
      this.Size = new System.Drawing.Size(600, 360);
      this.ResumeLayout(false);

    }

    #endregion

    private ZedGraph.ZedGraphControl zgcMainGraph;
  }
}
