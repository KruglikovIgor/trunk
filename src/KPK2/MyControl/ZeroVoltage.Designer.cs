using UserControlsKPK2;

namespace KPK2.MyControl
{
    partial class ZeroVoltage
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ZeroVoltage));
      this.grpBox_Zero = new System.Windows.Forms.GroupBox();
      this.label2 = new System.Windows.Forms.Label();
      this.siZero3 = new SegmentIndication();
      this.label1 = new System.Windows.Forms.Label();
      this.siZero2 = new SegmentIndication();
      this.lZero1 = new System.Windows.Forms.Label();
      this.siZero1 = new SegmentIndication();
      this.label38 = new System.Windows.Forms.Label();
      this.label37 = new System.Windows.Forms.Label();
      this.label36 = new System.Windows.Forms.Label();
      this.grpBox_Zero.SuspendLayout();
      this.SuspendLayout();
      // 
      // grpBox_Zero
      // 
      this.grpBox_Zero.Controls.Add(this.label2);
      this.grpBox_Zero.Controls.Add(this.siZero3);
      this.grpBox_Zero.Controls.Add(this.label1);
      this.grpBox_Zero.Controls.Add(this.siZero2);
      this.grpBox_Zero.Controls.Add(this.lZero1);
      this.grpBox_Zero.Controls.Add(this.siZero1);
      this.grpBox_Zero.Controls.Add(this.label38);
      this.grpBox_Zero.Controls.Add(this.label37);
      this.grpBox_Zero.Controls.Add(this.label36);
      resources.ApplyResources(this.grpBox_Zero, "grpBox_Zero");
      this.grpBox_Zero.Name = "grpBox_Zero";
      this.grpBox_Zero.TabStop = false;
      // 
      // label2
      // 
      resources.ApplyResources(this.label2, "label2");
      this.label2.Name = "label2";
      // 
      // siZero3
      // 
      this.siZero3.BackColor = System.Drawing.Color.Transparent;
      this.siZero3.BGColor = System.Drawing.Color.Black;
      this.siZero3.DarkColor = System.Drawing.Color.Transparent;
      resources.ApplyResources(this.siZero3, "siZero3");
      this.siZero3.FractPart = 0;
      this.siZero3.Inactive = false;
      this.siZero3.InactiveBGColor = System.Drawing.Color.Gray;
      this.siZero3.InvalidColor = System.Drawing.Color.Red;
      this.siZero3.LightColor = System.Drawing.Color.Lime;
      this.siZero3.MaxValue = 120D;
      this.siZero3.MinValue = 0D;
      this.siZero3.Name = "siZero3";
      this.siZero3.TabStop = false;
      this.siZero3.Value = 0D;
      this.siZero3.ValueString = "0";
      this.siZero3.WholePart = 0;
      // 
      // label1
      // 
      resources.ApplyResources(this.label1, "label1");
      this.label1.Name = "label1";
      // 
      // siZero2
      // 
      this.siZero2.BackColor = System.Drawing.Color.Transparent;
      this.siZero2.BGColor = System.Drawing.Color.Black;
      this.siZero2.DarkColor = System.Drawing.Color.Transparent;
      resources.ApplyResources(this.siZero2, "siZero2");
      this.siZero2.FractPart = 0;
      this.siZero2.Inactive = false;
      this.siZero2.InactiveBGColor = System.Drawing.Color.Gray;
      this.siZero2.InvalidColor = System.Drawing.Color.Red;
      this.siZero2.LightColor = System.Drawing.Color.Lime;
      this.siZero2.MaxValue = 120D;
      this.siZero2.MinValue = 0D;
      this.siZero2.Name = "siZero2";
      this.siZero2.TabStop = false;
      this.siZero2.Value = 0D;
      this.siZero2.ValueString = "0";
      this.siZero2.WholePart = 0;
      // 
      // lZero1
      // 
      resources.ApplyResources(this.lZero1, "lZero1");
      this.lZero1.Name = "lZero1";
      // 
      // siZero1
      // 
      this.siZero1.BackColor = System.Drawing.Color.Transparent;
      this.siZero1.BGColor = System.Drawing.Color.Black;
      this.siZero1.DarkColor = System.Drawing.Color.Transparent;
      resources.ApplyResources(this.siZero1, "siZero1");
      this.siZero1.FractPart = 0;
      this.siZero1.Inactive = false;
      this.siZero1.InactiveBGColor = System.Drawing.Color.Gray;
      this.siZero1.InvalidColor = System.Drawing.Color.Red;
      this.siZero1.LightColor = System.Drawing.Color.Lime;
      this.siZero1.MaxValue = 120D;
      this.siZero1.MinValue = 0D;
      this.siZero1.Name = "siZero1";
      this.siZero1.TabStop = false;
      this.siZero1.Value = 0D;
      this.siZero1.ValueString = "0";
      this.siZero1.WholePart = 0;
      // 
      // label38
      // 
      this.label38.ForeColor = System.Drawing.SystemColors.ControlText;
      resources.ApplyResources(this.label38, "label38");
      this.label38.Name = "label38";
      // 
      // label37
      // 
      this.label37.ForeColor = System.Drawing.SystemColors.ControlText;
      resources.ApplyResources(this.label37, "label37");
      this.label37.Name = "label37";
      // 
      // label36
      // 
      this.label36.ForeColor = System.Drawing.SystemColors.ControlText;
      resources.ApplyResources(this.label36, "label36");
      this.label36.Name = "label36";
      // 
      // ZeroVoltage
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.Controls.Add(this.grpBox_Zero);
      resources.ApplyResources(this, "$this");
      this.Name = "ZeroVoltage";
      this.EnabledChanged += new System.EventHandler(this.ZeroVoltage_EnabledChanged);
      this.grpBox_Zero.ResumeLayout(false);
      this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBox_Zero;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label lZero1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        public SegmentIndication siZero1;
        public SegmentIndication siZero3;
        public SegmentIndication siZero2;
    }
}
