using UserControlsKPK2;

namespace KPK2.MyControl
{
    partial class Task2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Task2));
            this.grpBox_T2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.siT2_2 = new UserControlsKPK2.SegmentIndication();
            this.lbl_Task_1_Up = new System.Windows.Forms.Label();
            this.siT2_1 = new UserControlsKPK2.SegmentIndication();
            this.btn_T2_Z = new System.Windows.Forms.Button();
            this.btn_T2_Zero = new System.Windows.Forms.Button();
            this.btn_T2_Task = new System.Windows.Forms.Button();
            this.label46 = new System.Windows.Forms.Label();
            this.label45 = new System.Windows.Forms.Label();
            this.lbl_T2_Time = new System.Windows.Forms.Label();
            this.grpBox_T2.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpBox_T2
            // 
            this.grpBox_T2.Controls.Add(this.label1);
            this.grpBox_T2.Controls.Add(this.siT2_2);
            this.grpBox_T2.Controls.Add(this.lbl_Task_1_Up);
            this.grpBox_T2.Controls.Add(this.siT2_1);
            this.grpBox_T2.Controls.Add(this.btn_T2_Z);
            this.grpBox_T2.Controls.Add(this.btn_T2_Zero);
            this.grpBox_T2.Controls.Add(this.btn_T2_Task);
            this.grpBox_T2.Controls.Add(this.label46);
            this.grpBox_T2.Controls.Add(this.label45);
            this.grpBox_T2.Controls.Add(this.lbl_T2_Time);
            resources.ApplyResources(this.grpBox_T2, "grpBox_T2");
            this.grpBox_T2.Name = "grpBox_T2";
            this.grpBox_T2.TabStop = false;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // siT2_2
            // 
            this.siT2_2.BackColor = System.Drawing.Color.Transparent;
            this.siT2_2.BGColor = System.Drawing.Color.Black;
            this.siT2_2.CalculationMethod = UserControlsKPK2.Enums.CalcMethod.MinMax;
            this.siT2_2.DarkColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.siT2_2, "siT2_2");
            this.siT2_2.FractPart = 0;
            this.siT2_2.Inactive = false;
            this.siT2_2.InactiveBGColor = System.Drawing.Color.Gray;
            this.siT2_2.InvalidColor = System.Drawing.Color.Red;
            this.siT2_2.LightColor = System.Drawing.Color.Lime;
            this.siT2_2.MaxValue = 120D;
            this.siT2_2.MinValue = 0D;
            this.siT2_2.Name = "siT2_2";
            this.siT2_2.NeedAlign = true;
            this.siT2_2.TabStop = false;
            this.siT2_2.Value = 0D;
            this.siT2_2.ValueString = "0";
            this.siT2_2.WholePart = 0;
            // 
            // lbl_Task_1_Up
            // 
            resources.ApplyResources(this.lbl_Task_1_Up, "lbl_Task_1_Up");
            this.lbl_Task_1_Up.Name = "lbl_Task_1_Up";
            // 
            // siT2_1
            // 
            this.siT2_1.BackColor = System.Drawing.Color.Transparent;
            this.siT2_1.BGColor = System.Drawing.Color.Black;
            this.siT2_1.CalculationMethod = UserControlsKPK2.Enums.CalcMethod.MinMax;
            this.siT2_1.DarkColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.siT2_1, "siT2_1");
            this.siT2_1.FractPart = 0;
            this.siT2_1.Inactive = false;
            this.siT2_1.InactiveBGColor = System.Drawing.Color.Gray;
            this.siT2_1.InvalidColor = System.Drawing.Color.Red;
            this.siT2_1.LightColor = System.Drawing.Color.Lime;
            this.siT2_1.MaxValue = 120D;
            this.siT2_1.MinValue = 0D;
            this.siT2_1.Name = "siT2_1";
            this.siT2_1.NeedAlign = true;
            this.siT2_1.TabStop = false;
            this.siT2_1.Value = 0D;
            this.siT2_1.ValueString = "0";
            this.siT2_1.WholePart = 0;
            // 
            // btn_T2_Z
            // 
            this.btn_T2_Z.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            resources.ApplyResources(this.btn_T2_Z, "btn_T2_Z");
            this.btn_T2_Z.Name = "btn_T2_Z";
            this.btn_T2_Z.UseVisualStyleBackColor = true;
            this.btn_T2_Z.Click += new System.EventHandler(this.btn_T2_Z_Click);
            // 
            // btn_T2_Zero
            // 
            this.btn_T2_Zero.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            resources.ApplyResources(this.btn_T2_Zero, "btn_T2_Zero");
            this.btn_T2_Zero.Name = "btn_T2_Zero";
            this.btn_T2_Zero.UseVisualStyleBackColor = true;
            this.btn_T2_Zero.Click += new System.EventHandler(this.btn_T2_Zero_Click);
            // 
            // btn_T2_Task
            // 
            this.btn_T2_Task.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            resources.ApplyResources(this.btn_T2_Task, "btn_T2_Task");
            this.btn_T2_Task.Name = "btn_T2_Task";
            this.btn_T2_Task.UseVisualStyleBackColor = true;
            this.btn_T2_Task.Click += new System.EventHandler(this.btn_T2_Task_Click);
            // 
            // label46
            // 
            resources.ApplyResources(this.label46, "label46");
            this.label46.Name = "label46";
            // 
            // label45
            // 
            resources.ApplyResources(this.label45, "label45");
            this.label45.Name = "label45";
            // 
            // lbl_T2_Time
            // 
            resources.ApplyResources(this.lbl_T2_Time, "lbl_T2_Time");
            this.lbl_T2_Time.Name = "lbl_T2_Time";
            // 
            // Task2
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpBox_T2);
            this.Name = "Task2";
            this.Load += new System.EventHandler(this.Task2_Load);
            this.EnabledChanged += new System.EventHandler(this.Task2_EnabledChanged);
            this.grpBox_T2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBox_T2;
        private System.Windows.Forms.Button btn_T2_Z;
        private System.Windows.Forms.Button btn_T2_Zero;
        private System.Windows.Forms.Button btn_T2_Task;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.Label lbl_T2_Time;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_Task_1_Up;
        public SegmentIndication siT2_2;
        public SegmentIndication siT2_1;
    }
}
