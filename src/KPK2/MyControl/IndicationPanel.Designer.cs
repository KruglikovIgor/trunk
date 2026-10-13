using UserControlsKPK2;

namespace KPK2.MyControl
{
  partial class IndicationPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IndicationPanel));
            this.grpBox_Task = new System.Windows.Forms.GroupBox();
            this.lbl_Set_Task = new System.Windows.Forms.Label();
            this.grpBox_Speed = new System.Windows.Forms.GroupBox();
            this.indValue3 = new UserControlsKPK2.IndicatorValue();
            this.indValue2 = new UserControlsKPK2.IndicatorValue();
            this.indValue1 = new UserControlsKPK2.IndicatorValue();
            this.grpBox_Value = new System.Windows.Forms.GroupBox();
            this.IndicatorLeft = new UserControlsKPK2.SegmentIndicatorControl();
            this.IndicatorDown = new UserControlsKPK2.SegmentIndicatorControl();
            this.IndicatorUp = new UserControlsKPK2.SegmentIndicatorControl();
            this.grpBox_Task.SuspendLayout();
            this.grpBox_Speed.SuspendLayout();
            this.grpBox_Value.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpBox_Task
            // 
            this.grpBox_Task.Controls.Add(this.lbl_Set_Task);
            this.grpBox_Task.Controls.Add(this.grpBox_Speed);
            this.grpBox_Task.Controls.Add(this.grpBox_Value);
            resources.ApplyResources(this.grpBox_Task, "grpBox_Task");
            this.grpBox_Task.Name = "grpBox_Task";
            this.grpBox_Task.TabStop = false;
            // 
            // lbl_Set_Task
            // 
            resources.ApplyResources(this.lbl_Set_Task, "lbl_Set_Task");
            this.lbl_Set_Task.ForeColor = System.Drawing.Color.Red;
            this.lbl_Set_Task.Name = "lbl_Set_Task";
            // 
            // grpBox_Speed
            // 
            this.grpBox_Speed.Controls.Add(this.indValue3);
            this.grpBox_Speed.Controls.Add(this.indValue2);
            this.grpBox_Speed.Controls.Add(this.indValue1);
            resources.ApplyResources(this.grpBox_Speed, "grpBox_Speed");
            this.grpBox_Speed.Name = "grpBox_Speed";
            this.grpBox_Speed.TabStop = false;
            // 
            // indValue3
            // 
            this.indValue3.Description = null;
            resources.ApplyResources(this.indValue3, "indValue3");
            this.indValue3.FailColor = System.Drawing.Color.Red;
            this.indValue3.MaxGreenValue = 0D;
            this.indValue3.MaxValue = 0D;
            this.indValue3.MinGreenValue = 0D;
            this.indValue3.MinValue = 0D;
            this.indValue3.Name = "indValue3";
            this.indValue3.SuccessColor = System.Drawing.Color.LimeGreen;
            this.indValue3.UsualColor = System.Drawing.Color.Black;
            this.indValue3.Value = double.NaN;
            // 
            // indValue2
            // 
            this.indValue2.Description = null;
            resources.ApplyResources(this.indValue2, "indValue2");
            this.indValue2.FailColor = System.Drawing.Color.Red;
            this.indValue2.MaxGreenValue = 0D;
            this.indValue2.MaxValue = 0D;
            this.indValue2.MinGreenValue = 0D;
            this.indValue2.MinValue = 0D;
            this.indValue2.Name = "indValue2";
            this.indValue2.SuccessColor = System.Drawing.Color.LimeGreen;
            this.indValue2.UsualColor = System.Drawing.Color.Black;
            this.indValue2.Value = double.NaN;
            // 
            // indValue1
            // 
            this.indValue1.Description = null;
            resources.ApplyResources(this.indValue1, "indValue1");
            this.indValue1.FailColor = System.Drawing.Color.Red;
            this.indValue1.MaxGreenValue = 0D;
            this.indValue1.MaxValue = 0D;
            this.indValue1.MinGreenValue = 0D;
            this.indValue1.MinValue = 0D;
            this.indValue1.Name = "indValue1";
            this.indValue1.SuccessColor = System.Drawing.Color.LimeGreen;
            this.indValue1.UsualColor = System.Drawing.Color.Black;
            this.indValue1.Value = double.NaN;
            // 
            // grpBox_Value
            // 
            this.grpBox_Value.Controls.Add(this.IndicatorLeft);
            this.grpBox_Value.Controls.Add(this.IndicatorDown);
            this.grpBox_Value.Controls.Add(this.IndicatorUp);
            resources.ApplyResources(this.grpBox_Value, "grpBox_Value");
            this.grpBox_Value.Name = "grpBox_Value";
            this.grpBox_Value.TabStop = false;
            // 
            // IndicatorLeft
            // 
            this.IndicatorLeft.CalculationMethod = UserControlsKPK2.Enums.CalcMethod.MinMax;
            this.IndicatorLeft.CheckValueFunction = null;
            resources.ApplyResources(this.IndicatorLeft, "IndicatorLeft");
            this.IndicatorLeft.FractPart = 0;
            this.IndicatorLeft.Inactive = false;
            this.IndicatorLeft.MaxValue = 0D;
            this.IndicatorLeft.MinValue = 0D;
            this.IndicatorLeft.Name = "IndicatorLeft";
            this.IndicatorLeft.ParameterName = null;
            this.IndicatorLeft.UsbBaseNew = null;
            this.IndicatorLeft.ValueString = "0";
            this.IndicatorLeft.WholePart = 0;
            // 
            // IndicatorDown
            // 
            this.IndicatorDown.CalculationMethod = UserControlsKPK2.Enums.CalcMethod.MinMax;
            this.IndicatorDown.CheckValueFunction = null;
            resources.ApplyResources(this.IndicatorDown, "IndicatorDown");
            this.IndicatorDown.FractPart = 0;
            this.IndicatorDown.Inactive = false;
            this.IndicatorDown.MaxValue = 0D;
            this.IndicatorDown.MinValue = 0D;
            this.IndicatorDown.Name = "IndicatorDown";
            this.IndicatorDown.ParameterName = null;
            this.IndicatorDown.UsbBaseNew = null;
            this.IndicatorDown.ValueString = "0";
            this.IndicatorDown.WholePart = 0;
            // 
            // IndicatorUp
            // 
            this.IndicatorUp.CalculationMethod = UserControlsKPK2.Enums.CalcMethod.MinMax;
            this.IndicatorUp.CheckValueFunction = null;
            resources.ApplyResources(this.IndicatorUp, "IndicatorUp");
            this.IndicatorUp.FractPart = 0;
            this.IndicatorUp.Inactive = false;
            this.IndicatorUp.MaxValue = 0D;
            this.IndicatorUp.MinValue = 0D;
            this.IndicatorUp.Name = "IndicatorUp";
            this.IndicatorUp.ParameterName = null;
            this.IndicatorUp.UsbBaseNew = null;
            this.IndicatorUp.ValueString = "0";
            this.IndicatorUp.WholePart = 0;
            // 
            // IndicationPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpBox_Task);
            this.Name = "IndicationPanel";
            this.EnabledChanged += new System.EventHandler(this.IndicationPanel_EnabledChanged);
            this.grpBox_Task.ResumeLayout(false);
            this.grpBox_Speed.ResumeLayout(false);
            this.grpBox_Value.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    #endregion

    public System.Windows.Forms.GroupBox grpBox_Speed;
    public UserControlsKPK2.IndicatorValue indValue3;
    public UserControlsKPK2.IndicatorValue indValue2;
    public UserControlsKPK2.IndicatorValue indValue1;
    public System.Windows.Forms.GroupBox grpBox_Value;
    public System.Windows.Forms.Label lbl_Set_Task;
    public System.Windows.Forms.GroupBox grpBox_Task;
		public SegmentIndicatorControl IndicatorUp;
		public SegmentIndicatorControl IndicatorLeft;
		public SegmentIndicatorControl IndicatorDown;
	}
}
