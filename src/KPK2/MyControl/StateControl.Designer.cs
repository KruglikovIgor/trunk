namespace KPK2.MyControl
{
  partial class StateControl
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
      this.gbDevice = new System.Windows.Forms.GroupBox();
      this.lState = new System.Windows.Forms.Label();
      this.gbDevice.SuspendLayout();
      this.SuspendLayout();
      // 
      // gbDevice
      // 
      this.gbDevice.Controls.Add(this.lState);
      this.gbDevice.Location = new System.Drawing.Point(3, 3);
      this.gbDevice.Name = "gbDevice";
      this.gbDevice.Size = new System.Drawing.Size(329, 98);
      this.gbDevice.TabIndex = 89;
      this.gbDevice.TabStop = false;
      this.gbDevice.Text = "Состояние";
      // 
      // lState
      // 
      this.lState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.lState.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.lState.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lState.Location = new System.Drawing.Point(6, 32);
      this.lState.Name = "lState";
      this.lState.Size = new System.Drawing.Size(317, 63);
      this.lState.TabIndex = 27;
      this.lState.Text = "...";
      // 
      // StateControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.gbDevice);
      this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.Name = "StateControl";
      this.Size = new System.Drawing.Size(336, 103);
      this.gbDevice.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox gbDevice;
    public System.Windows.Forms.Label lState;
  }
}
