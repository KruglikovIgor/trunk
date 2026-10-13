namespace InstallKPK2Db
{
  partial class fMain
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
      this.pbProgress = new System.Windows.Forms.ProgressBar();
      this.lState = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // pbProgress
      // 
      this.pbProgress.Location = new System.Drawing.Point(12, 36);
      this.pbProgress.Maximum = 6;
      this.pbProgress.Name = "pbProgress";
      this.pbProgress.Size = new System.Drawing.Size(433, 23);
      this.pbProgress.Step = 1;
      this.pbProgress.TabIndex = 0;
      // 
      // lState
      // 
      this.lState.AutoSize = true;
      this.lState.Location = new System.Drawing.Point(15, 70);
      this.lState.Name = "lState";
      this.lState.Size = new System.Drawing.Size(0, 13);
      this.lState.TabIndex = 1;
      // 
      // fMain
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(457, 110);
      this.ControlBox = false;
      this.Controls.Add(this.lState);
      this.Controls.Add(this.pbProgress);
      this.Name = "fMain";
      this.Text = "Установка базы данных КПК-2";
      this.Load += new System.EventHandler(this.fMain_Load);
      this.Shown += new System.EventHandler(this.fMain_Shown);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ProgressBar pbProgress;
    private System.Windows.Forms.Label lState;
  }
}

