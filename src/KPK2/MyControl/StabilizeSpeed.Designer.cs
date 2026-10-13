using UserControlsKPK2;

namespace KPK2.MyControl
{
    partial class StabilizeSpeed
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StabilizeSpeed));
      this.grpBox_Speed = new System.Windows.Forms.GroupBox();
      this.lWorking3 = new System.Windows.Forms.Label();
      this.lWorking2 = new System.Windows.Forms.Label();
      this.siSpeed = new SegmentIndication();
      this.siTime = new SegmentIndication();
      this.btn_Speed_1 = new System.Windows.Forms.Button();
      this.btn_Speed_5 = new System.Windows.Forms.Button();
      this.btn_Speed_4 = new System.Windows.Forms.Button();
      this.btn_Speed_3 = new System.Windows.Forms.Button();
      this.btn_Speed_2 = new System.Windows.Forms.Button();
      this.tbInfo = new System.Windows.Forms.TextBox();
      this.grpBox_Speed.SuspendLayout();
      this.SuspendLayout();
      // 
      // grpBox_Speed
      // 
      this.grpBox_Speed.Controls.Add(this.lWorking3);
      this.grpBox_Speed.Controls.Add(this.lWorking2);
      this.grpBox_Speed.Controls.Add(this.siSpeed);
      this.grpBox_Speed.Controls.Add(this.siTime);
      this.grpBox_Speed.Controls.Add(this.btn_Speed_1);
      this.grpBox_Speed.Controls.Add(this.btn_Speed_5);
      this.grpBox_Speed.Controls.Add(this.btn_Speed_4);
      this.grpBox_Speed.Controls.Add(this.btn_Speed_3);
      this.grpBox_Speed.Controls.Add(this.btn_Speed_2);
      resources.ApplyResources(this.grpBox_Speed, "grpBox_Speed");
      this.grpBox_Speed.Name = "grpBox_Speed";
      this.grpBox_Speed.TabStop = false;
      // 
      // lWorking3
      // 
      resources.ApplyResources(this.lWorking3, "lWorking3");
      this.lWorking3.Name = "lWorking3";
      // 
      // lWorking2
      // 
      resources.ApplyResources(this.lWorking2, "lWorking2");
      this.lWorking2.Name = "lWorking2";
      // 
      // siSpeed
      // 
      this.siSpeed.BackColor = System.Drawing.Color.Transparent;
      this.siSpeed.BGColor = System.Drawing.Color.Black;
      this.siSpeed.DarkColor = System.Drawing.Color.Transparent;
      resources.ApplyResources(this.siSpeed, "siSpeed");
      this.siSpeed.FractPart = 2;
      this.siSpeed.Inactive = true;
      this.siSpeed.InactiveBGColor = System.Drawing.Color.Gray;
      this.siSpeed.InvalidColor = System.Drawing.Color.Red;
      this.siSpeed.LightColor = System.Drawing.Color.Lime;
      this.siSpeed.MaxValue = 120D;
      this.siSpeed.MinValue = 0D;
      this.siSpeed.Name = "siSpeed";
      this.siSpeed.Value = 0D;
      this.siSpeed.ValueString = "0.0";
      this.siSpeed.WholePart = 0;
      // 
      // siTime
      // 
      this.siTime.BackColor = System.Drawing.Color.Transparent;
      this.siTime.BGColor = System.Drawing.Color.Black;
      this.siTime.DarkColor = System.Drawing.Color.Transparent;
      resources.ApplyResources(this.siTime, "siTime");
      this.siTime.FractPart = 2;
      this.siTime.Inactive = true;
      this.siTime.InactiveBGColor = System.Drawing.Color.Gray;
      this.siTime.InvalidColor = System.Drawing.Color.Red;
      this.siTime.LightColor = System.Drawing.Color.Lime;
      this.siTime.MaxValue = 120D;
      this.siTime.MinValue = 0D;
      this.siTime.Name = "siTime";
      this.siTime.Value = 0D;
      this.siTime.ValueString = "0.0";
      this.siTime.WholePart = 0;
      // 
      // btn_Speed_1
      // 
      this.btn_Speed_1.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
      resources.ApplyResources(this.btn_Speed_1, "btn_Speed_1");
      this.btn_Speed_1.Name = "btn_Speed_1";
      this.btn_Speed_1.UseVisualStyleBackColor = true;
      this.btn_Speed_1.Click += new System.EventHandler(this.btn_Speed_Click);
      // 
      // btn_Speed_5
      // 
      this.btn_Speed_5.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
      resources.ApplyResources(this.btn_Speed_5, "btn_Speed_5");
      this.btn_Speed_5.Name = "btn_Speed_5";
      this.btn_Speed_5.UseVisualStyleBackColor = true;
      this.btn_Speed_5.Click += new System.EventHandler(this.btn_Speed_Click);
      // 
      // btn_Speed_4
      // 
      this.btn_Speed_4.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
      resources.ApplyResources(this.btn_Speed_4, "btn_Speed_4");
      this.btn_Speed_4.Name = "btn_Speed_4";
      this.btn_Speed_4.UseVisualStyleBackColor = true;
      this.btn_Speed_4.Click += new System.EventHandler(this.btn_Speed_Click);
      // 
      // btn_Speed_3
      // 
      this.btn_Speed_3.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
      resources.ApplyResources(this.btn_Speed_3, "btn_Speed_3");
      this.btn_Speed_3.Name = "btn_Speed_3";
      this.btn_Speed_3.UseVisualStyleBackColor = true;
      this.btn_Speed_3.Click += new System.EventHandler(this.btn_Speed_Click);
      // 
      // btn_Speed_2
      // 
      this.btn_Speed_2.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
      resources.ApplyResources(this.btn_Speed_2, "btn_Speed_2");
      this.btn_Speed_2.Name = "btn_Speed_2";
      this.btn_Speed_2.UseVisualStyleBackColor = true;
      this.btn_Speed_2.Click += new System.EventHandler(this.btn_Speed_Click);
      // 
      // tbInfo
      // 
      this.tbInfo.BackColor = System.Drawing.SystemColors.Control;
      resources.ApplyResources(this.tbInfo, "tbInfo");
      this.tbInfo.Name = "tbInfo";
      // 
      // StabilizeSpeed
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.tbInfo);
      this.Controls.Add(this.grpBox_Speed);
      this.Name = "StabilizeSpeed";
      this.Load += new System.EventHandler(this.StabilizeSpeed_Load);
      this.EnabledChanged += new System.EventHandler(this.StabilizeSpeed_EnabledChanged);
      this.grpBox_Speed.ResumeLayout(false);
      this.grpBox_Speed.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBox_Speed;
        private System.Windows.Forms.Button btn_Speed_1;
        private System.Windows.Forms.Button btn_Speed_5;
        private System.Windows.Forms.Button btn_Speed_4;
        private System.Windows.Forms.Button btn_Speed_3;
        private System.Windows.Forms.Button btn_Speed_2;
        private System.Windows.Forms.Label lWorking3;
        private System.Windows.Forms.Label lWorking2;
        private SegmentIndication siSpeed;
        private SegmentIndication siTime;
        private System.Windows.Forms.TextBox tbInfo;
    }
}
