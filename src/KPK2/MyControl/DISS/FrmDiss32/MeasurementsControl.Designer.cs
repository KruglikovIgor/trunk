using System.Drawing;
using KPK2.MyControl;
using Sparc.KPK2.Hardware.LTR;
using Sparc.KPK2.NFunctions.Enums;
using UserControlsKPK2;

namespace KPK2.MyControl.DISS.FrmDiss32
{
	public partial class MeasurementsControl
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
            this.bBortSystems = new System.Windows.Forms.Button();
            this.btn_Light = new System.Windows.Forms.Button();
            this.panel_Task = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.siWorkTime = new UserControlsKPK2.SegmentIndication();
            this.cbShowBlock8 = new System.Windows.Forms.CheckBox();
            this.grpBoxPoint23 = new System.Windows.Forms.GroupBox();
            this.rbtn_Point23_2 = new System.Windows.Forms.RadioButton();
            this.rbtn_Point23_1 = new System.Windows.Forms.RadioButton();
            this.label37 = new System.Windows.Forms.Label();
            this.grpBox_Task = new System.Windows.Forms.GroupBox();
            this.lbl_Set_Task = new System.Windows.Forms.Label();
            this.MemoryPS = new UserControlsKPK2.IndicatorValue();
            this.grpBox_Speed = new System.Windows.Forms.GroupBox();
            this.indValue3 = new UserControlsKPK2.IndicatorValue();
            this.indValue2 = new UserControlsKPK2.IndicatorValue();
            this.indValue1 = new UserControlsKPK2.IndicatorValue();
            this.grpBox_Value = new System.Windows.Forms.GroupBox();
            this.IndicatorDown = new UserControlsKPK2.SegmentIndicatorControl();
            this.IndicatorLeft = new UserControlsKPK2.SegmentIndicatorControl();
            this.IndicatorUp = new UserControlsKPK2.SegmentIndicatorControl();
            this.MemoryUS = new UserControlsKPK2.IndicatorValue();
            this.btn_Mode_M_B = new System.Windows.Forms.Button();
            this.grpBox_Corner = new System.Windows.Forms.GroupBox();
            this.tiCorner = new UserControlsKPK2.TextIndicator();
            this.cmbBox_Corner = new System.Windows.Forms.ComboBox();
            this.rbtn_Kren = new System.Windows.Forms.RadioButton();
            this.rbtn_Tangazh = new System.Windows.Forms.RadioButton();
            this.rbtn_Zero = new System.Windows.Forms.RadioButton();
            this.cmbBox_3 = new System.Windows.Forms.ComboBox();
            this.cmbBox_2 = new System.Windows.Forms.ComboBox();
            this.cmbBox_1 = new System.Windows.Forms.ComboBox();
            this.btn_Corner = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_Mode_M_C = new System.Windows.Forms.Button();
            this.btn_Memory = new System.Windows.Forms.Button();
            this.btn_Zero = new System.Windows.Forms.Button();
            this.btn_BPK = new System.Windows.Forms.Button();
            this.btn_Task_4 = new System.Windows.Forms.Button();
            this.btn_Task_3 = new System.Windows.Forms.Button();
            this.btn_Task_2 = new System.Windows.Forms.Button();
            this.btn_Task_1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tmrBlock8 = new System.Windows.Forms.Timer(this.components);
            this.panel_Task.SuspendLayout();
            this.grpBoxPoint23.SuspendLayout();
            this.grpBox_Task.SuspendLayout();
            this.grpBox_Speed.SuspendLayout();
            this.grpBox_Value.SuspendLayout();
            this.grpBox_Corner.SuspendLayout();
            this.SuspendLayout();
            // 
            // bBortSystems
            // 
            this.bBortSystems.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.bBortSystems.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.bBortSystems.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.bBortSystems.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.bBortSystems.Location = new System.Drawing.Point(718, 373);
            this.bBortSystems.Name = "bBortSystems";
            this.bBortSystems.Padding = new System.Windows.Forms.Padding(30, 0, 30, 0);
            this.bBortSystems.Size = new System.Drawing.Size(196, 40);
            this.bBortSystems.TabIndex = 21;
            this.bBortSystems.Text = "Wxr, Wzr, Wyr в бортовые системы";
            this.bBortSystems.UseVisualStyleBackColor = true;
            this.bBortSystems.Visible = false;
            this.bBortSystems.Click += new System.EventHandler(this.bBortSystems_Click);
            // 
            // btn_Light
            // 
            this.btn_Light.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btn_Light.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_Light.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btn_Light.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_Light.Location = new System.Drawing.Point(482, 373);
            this.btn_Light.Name = "btn_Light";
            this.btn_Light.Padding = new System.Windows.Forms.Padding(30, 0, 30, 0);
            this.btn_Light.Size = new System.Drawing.Size(196, 40);
            this.btn_Light.TabIndex = 20;
            this.btn_Light.Text = "Световая сигнализация";
            this.btn_Light.UseVisualStyleBackColor = true;
            this.btn_Light.Click += new System.EventHandler(this.btn_Light_Click);
            // 
            // panel_Task
            // 
            this.panel_Task.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_Task.Controls.Add(this.label5);
            this.panel_Task.Controls.Add(this.siWorkTime);
            this.panel_Task.Controls.Add(this.cbShowBlock8);
            this.panel_Task.Controls.Add(this.grpBoxPoint23);
            this.panel_Task.Controls.Add(this.grpBox_Task);
            this.panel_Task.Controls.Add(this.btn_Mode_M_B);
            this.panel_Task.Controls.Add(this.grpBox_Corner);
            this.panel_Task.Controls.Add(this.btn_Mode_M_C);
            this.panel_Task.Controls.Add(this.btn_Memory);
            this.panel_Task.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.panel_Task.Location = new System.Drawing.Point(3, 3);
            this.panel_Task.Name = "panel_Task";
            this.panel_Task.Size = new System.Drawing.Size(914, 300);
            this.panel_Task.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(784, 216);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(114, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "Время работы:";
            this.label5.Visible = false;
            // 
            // siWorkTime
            // 
            this.siWorkTime.BackColor = System.Drawing.Color.Transparent;
            this.siWorkTime.BGColor = System.Drawing.Color.Black;
            this.siWorkTime.CalculationMethod = UserControlsKPK2.Enums.CalcMethod.MinMax;
            this.siWorkTime.DarkColor = System.Drawing.Color.Transparent;
            this.siWorkTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.siWorkTime.FractPart = 0;
            this.siWorkTime.Inactive = false;
            this.siWorkTime.InactiveBGColor = System.Drawing.Color.Gray;
            this.siWorkTime.InvalidColor = System.Drawing.Color.Red;
            this.siWorkTime.LightColor = System.Drawing.Color.Lime;
            this.siWorkTime.Location = new System.Drawing.Point(784, 237);
            this.siWorkTime.Margin = new System.Windows.Forms.Padding(8);
            this.siWorkTime.MaxValue = 120D;
            this.siWorkTime.MinValue = 0D;
            this.siWorkTime.Name = "siWorkTime";
            this.siWorkTime.NeedAlign = true;
            this.siWorkTime.Size = new System.Drawing.Size(114, 37);
            this.siWorkTime.TabIndex = 19;
            this.siWorkTime.Unit = "";
            this.siWorkTime.Value = 0D;
            this.siWorkTime.ValueString = "00:00:00";
            this.siWorkTime.Visible = false;
            this.siWorkTime.WholePart = 0;
            // 
            // cbShowBlock8
            // 
            this.cbShowBlock8.AutoSize = true;
            this.cbShowBlock8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbShowBlock8.Location = new System.Drawing.Point(415, 10);
            this.cbShowBlock8.Name = "cbShowBlock8";
            this.cbShowBlock8.Size = new System.Drawing.Size(64, 18);
            this.cbShowBlock8.TabIndex = 17;
            this.cbShowBlock8.Text = "Блок 8";
            this.cbShowBlock8.UseVisualStyleBackColor = true;
            this.cbShowBlock8.Visible = false;
            this.cbShowBlock8.CheckedChanged += new System.EventHandler(this.cbShowBlock8_CheckedChanged);
            // 
            // grpBoxPoint23
            // 
            this.grpBoxPoint23.Controls.Add(this.rbtn_Point23_2);
            this.grpBoxPoint23.Controls.Add(this.rbtn_Point23_1);
            this.grpBoxPoint23.Controls.Add(this.label37);
            this.grpBoxPoint23.Location = new System.Drawing.Point(497, 213);
            this.grpBoxPoint23.Name = "grpBoxPoint23";
            this.grpBoxPoint23.Size = new System.Drawing.Size(290, 65);
            this.grpBoxPoint23.TabIndex = 16;
            this.grpBoxPoint23.TabStop = false;
            this.grpBoxPoint23.Visible = false;
            // 
            // rbtn_Point23_2
            // 
            this.rbtn_Point23_2.AutoSize = true;
            this.rbtn_Point23_2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.rbtn_Point23_2.Location = new System.Drawing.Point(163, 37);
            this.rbtn_Point23_2.Name = "rbtn_Point23_2";
            this.rbtn_Point23_2.Size = new System.Drawing.Size(48, 18);
            this.rbtn_Point23_2.TabIndex = 2;
            this.rbtn_Point23_2.TabStop = true;
            this.rbtn_Point23_2.Text = "НЕТ";
            this.rbtn_Point23_2.UseVisualStyleBackColor = true;
            // 
            // rbtn_Point23_1
            // 
            this.rbtn_Point23_1.AutoSize = true;
            this.rbtn_Point23_1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.rbtn_Point23_1.Location = new System.Drawing.Point(75, 37);
            this.rbtn_Point23_1.Name = "rbtn_Point23_1";
            this.rbtn_Point23_1.Size = new System.Drawing.Size(41, 18);
            this.rbtn_Point23_1.TabIndex = 1;
            this.rbtn_Point23_1.TabStop = true;
            this.rbtn_Point23_1.Text = "ДА";
            this.rbtn_Point23_1.UseVisualStyleBackColor = true;
            // 
            // label37
            // 
            this.label37.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label37.Location = new System.Drawing.Point(6, 17);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(280, 17);
            this.label37.TabIndex = 0;
            this.label37.Text = "Отсутствуют сигналы П1, П2, П, ПОИСК";
            this.label37.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpBox_Task
            // 
            this.grpBox_Task.Controls.Add(this.lbl_Set_Task);
            this.grpBox_Task.Controls.Add(this.MemoryPS);
            this.grpBox_Task.Controls.Add(this.grpBox_Speed);
            this.grpBox_Task.Controls.Add(this.grpBox_Value);
            this.grpBox_Task.Controls.Add(this.MemoryUS);
            this.grpBox_Task.Location = new System.Drawing.Point(10, 10);
            this.grpBox_Task.Name = "grpBox_Task";
            this.grpBox_Task.Size = new System.Drawing.Size(326, 287);
            this.grpBox_Task.TabIndex = 0;
            this.grpBox_Task.TabStop = false;
            this.grpBox_Task.Text = "Контрольная задача КЗ-1";
            // 
            // lbl_Set_Task
            // 
            this.lbl_Set_Task.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_Set_Task.ForeColor = System.Drawing.Color.Red;
            this.lbl_Set_Task.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbl_Set_Task.Location = new System.Drawing.Point(2, 222);
            this.lbl_Set_Task.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.lbl_Set_Task.MaximumSize = new System.Drawing.Size(559, 78);
            this.lbl_Set_Task.Name = "lbl_Set_Task";
            this.lbl_Set_Task.Size = new System.Drawing.Size(315, 57);
            this.lbl_Set_Task.TabIndex = 0;
            this.lbl_Set_Task.Text = "Идет отработка указателей индикатора УС ПС";
            this.lbl_Set_Task.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MemoryPS
            // 
            this.MemoryPS.Description = null;
            this.MemoryPS.DownSign = "ПС 2";
            this.MemoryPS.FailColor = System.Drawing.Color.Red;
            this.MemoryPS.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MemoryPS.ForeColor = System.Drawing.Color.Black;
            this.MemoryPS.Location = new System.Drawing.Point(10, 220);
            this.MemoryPS.MaxGreenValue = 0D;
            this.MemoryPS.MaxValue = 0D;
            this.MemoryPS.MinGreenValue = 0D;
            this.MemoryPS.MinValue = 0D;
            this.MemoryPS.Name = "MemoryPS";
            this.MemoryPS.Size = new System.Drawing.Size(90, 59);
            this.MemoryPS.SuccessColor = System.Drawing.Color.LimeGreen;
            this.MemoryPS.TabIndex = 14;
            this.MemoryPS.UpSign = "";
            this.MemoryPS.UsualColor = System.Drawing.Color.Black;
            this.MemoryPS.Value = 0D;
            this.MemoryPS.Visible = false;
            // 
            // grpBox_Speed
            // 
            this.grpBox_Speed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBox_Speed.Controls.Add(this.indValue3);
            this.grpBox_Speed.Controls.Add(this.indValue2);
            this.grpBox_Speed.Controls.Add(this.indValue1);
            this.grpBox_Speed.Location = new System.Drawing.Point(4, 135);
            this.grpBox_Speed.Name = "grpBox_Speed";
            this.grpBox_Speed.Size = new System.Drawing.Size(313, 84);
            this.grpBox_Speed.TabIndex = 11;
            this.grpBox_Speed.TabStop = false;
            this.grpBox_Speed.Text = "Составляющие скорости с блока 6";
            // 
            // indValue3
            // 
            this.indValue3.Description = null;
            this.indValue3.DownSign = "Вниз";
            this.indValue3.FailColor = System.Drawing.Color.Red;
            this.indValue3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.indValue3.ForeColor = System.Drawing.Color.Black;
            this.indValue3.Location = new System.Drawing.Point(220, 19);
            this.indValue3.MaxGreenValue = 0D;
            this.indValue3.MaxValue = 0D;
            this.indValue3.MinGreenValue = 0D;
            this.indValue3.MinValue = 0D;
            this.indValue3.Name = "indValue3";
            this.indValue3.Size = new System.Drawing.Size(90, 59);
            this.indValue3.SuccessColor = System.Drawing.Color.LimeGreen;
            this.indValue3.TabIndex = 13;
            this.indValue3.UpSign = "V, м/с";
            this.indValue3.UsualColor = System.Drawing.Color.Black;
            this.indValue3.Value = 0D;
            // 
            // indValue2
            // 
            this.indValue2.Description = null;
            this.indValue2.DownSign = "Влево";
            this.indValue2.FailColor = System.Drawing.Color.Red;
            this.indValue2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.indValue2.ForeColor = System.Drawing.Color.Black;
            this.indValue2.Location = new System.Drawing.Point(114, 19);
            this.indValue2.MaxGreenValue = 0D;
            this.indValue2.MaxValue = 0D;
            this.indValue2.MinGreenValue = 0D;
            this.indValue2.MinValue = 0D;
            this.indValue2.Name = "indValue2";
            this.indValue2.Size = new System.Drawing.Size(90, 59);
            this.indValue2.SuccessColor = System.Drawing.Color.LimeGreen;
            this.indValue2.TabIndex = 12;
            this.indValue2.UpSign = "V, км/ч";
            this.indValue2.UsualColor = System.Drawing.Color.Black;
            this.indValue2.Value = 0D;
            // 
            // indValue1
            // 
            this.indValue1.Description = null;
            this.indValue1.DownSign = "Вперед";
            this.indValue1.FailColor = System.Drawing.Color.Red;
            this.indValue1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.indValue1.ForeColor = System.Drawing.Color.Black;
            this.indValue1.Location = new System.Drawing.Point(6, 19);
            this.indValue1.MaxGreenValue = 0D;
            this.indValue1.MaxValue = 0D;
            this.indValue1.MinGreenValue = 0D;
            this.indValue1.MinValue = 0D;
            this.indValue1.Name = "indValue1";
            this.indValue1.Size = new System.Drawing.Size(90, 59);
            this.indValue1.SuccessColor = System.Drawing.Color.LimeGreen;
            this.indValue1.TabIndex = 11;
            this.indValue1.UpSign = "V, км/ч";
            this.indValue1.UsualColor = System.Drawing.Color.Black;
            this.indValue1.Value = 0D;
            // 
            // grpBox_Value
            // 
            this.grpBox_Value.Controls.Add(this.IndicatorDown);
            this.grpBox_Value.Controls.Add(this.IndicatorLeft);
            this.grpBox_Value.Controls.Add(this.IndicatorUp);
            this.grpBox_Value.Location = new System.Drawing.Point(5, 27);
            this.grpBox_Value.Name = "grpBox_Value";
            this.grpBox_Value.Size = new System.Drawing.Size(312, 98);
            this.grpBox_Value.TabIndex = 0;
            this.grpBox_Value.TabStop = false;
            this.grpBox_Value.Text = "Составляющие скорости с выходов ПКВ";
            // 
            // IndicatorDown
            // 
            this.IndicatorDown.CalculationMethod = UserControlsKPK2.Enums.CalcMethod.MinMax;
            this.IndicatorDown.CheckValueFunction = null;
            this.IndicatorDown.FractPart = 0;
            this.IndicatorDown.Inactive = false;
            this.IndicatorDown.Location = new System.Drawing.Point(220, 16);
            this.IndicatorDown.Margin = new System.Windows.Forms.Padding(0);
            this.IndicatorDown.MaxValue = 0D;
            this.IndicatorDown.MinValue = 0D;
            this.IndicatorDown.Name = "IndicatorDown";
            this.IndicatorDown.NameLabelDown = "Вниз";
            this.IndicatorDown.NameLabelUp = "V, м/с";
            this.IndicatorDown.NameSegment = "SegmentIndication1";
            this.IndicatorDown.ParameterName = null;
            this.IndicatorDown.Size = new System.Drawing.Size(90, 80);
            this.IndicatorDown.TabIndex = 2;
            this.IndicatorDown.TestId = 0;
            this.IndicatorDown.UsbBaseNew = null;
            this.IndicatorDown.Value = 0D;
            this.IndicatorDown.ValueString = "0";
            this.IndicatorDown.WholePart = 0;
            // 
            // IndicatorLeft
            // 
            this.IndicatorLeft.CalculationMethod = UserControlsKPK2.Enums.CalcMethod.MinMax;
            this.IndicatorLeft.CheckValueFunction = null;
            this.IndicatorLeft.FractPart = 0;
            this.IndicatorLeft.Inactive = false;
            this.IndicatorLeft.Location = new System.Drawing.Point(114, 16);
            this.IndicatorLeft.Margin = new System.Windows.Forms.Padding(0);
            this.IndicatorLeft.MaxValue = 0D;
            this.IndicatorLeft.MinValue = 0D;
            this.IndicatorLeft.Name = "IndicatorLeft";
            this.IndicatorLeft.NameLabelDown = "Влево";
            this.IndicatorLeft.NameLabelUp = "V, м/с";
            this.IndicatorLeft.NameSegment = "SegmentIndication1";
            this.IndicatorLeft.ParameterName = null;
            this.IndicatorLeft.Size = new System.Drawing.Size(90, 80);
            this.IndicatorLeft.TabIndex = 1;
            this.IndicatorLeft.TestId = 0;
            this.IndicatorLeft.UsbBaseNew = null;
            this.IndicatorLeft.Value = 0D;
            this.IndicatorLeft.ValueString = "0";
            this.IndicatorLeft.WholePart = 0;
            // 
            // IndicatorUp
            // 
            this.IndicatorUp.CalculationMethod = UserControlsKPK2.Enums.CalcMethod.MinMax;
            this.IndicatorUp.CheckValueFunction = null;
            this.IndicatorUp.FractPart = 0;
            this.IndicatorUp.Inactive = false;
            this.IndicatorUp.Location = new System.Drawing.Point(3, 16);
            this.IndicatorUp.Margin = new System.Windows.Forms.Padding(0);
            this.IndicatorUp.MaxValue = 0D;
            this.IndicatorUp.MinValue = 0D;
            this.IndicatorUp.Name = "IndicatorUp";
            this.IndicatorUp.NameLabelDown = "Вперед";
            this.IndicatorUp.NameLabelUp = "V, м/с";
            this.IndicatorUp.NameSegment = "SegmentIndication1";
            this.IndicatorUp.ParameterName = null;
            this.IndicatorUp.Size = new System.Drawing.Size(90, 80);
            this.IndicatorUp.TabIndex = 0;
            this.IndicatorUp.TestId = 0;
            this.IndicatorUp.UsbBaseNew = null;
            this.IndicatorUp.Value = 0D;
            this.IndicatorUp.ValueString = "0";
            this.IndicatorUp.WholePart = 0;
            // 
            // MemoryUS
            // 
            this.MemoryUS.Description = null;
            this.MemoryUS.DownSign = "УС 9,5 Влево";
            this.MemoryUS.FailColor = System.Drawing.Color.Red;
            this.MemoryUS.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.MemoryUS.ForeColor = System.Drawing.Color.Black;
            this.MemoryUS.Location = new System.Drawing.Point(118, 220);
            this.MemoryUS.MaxGreenValue = 0D;
            this.MemoryUS.MaxValue = 0D;
            this.MemoryUS.MinGreenValue = 0D;
            this.MemoryUS.MinValue = 0D;
            this.MemoryUS.Name = "MemoryUS";
            this.MemoryUS.Size = new System.Drawing.Size(90, 59);
            this.MemoryUS.SuccessColor = System.Drawing.Color.LimeGreen;
            this.MemoryUS.TabIndex = 15;
            this.MemoryUS.UpSign = "";
            this.MemoryUS.UsualColor = System.Drawing.Color.Black;
            this.MemoryUS.Value = 0D;
            this.MemoryUS.Visible = false;
            // 
            // btn_Mode_M_B
            // 
            this.btn_Mode_M_B.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_Mode_M_B.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_Mode_M_B.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_Mode_M_B.Location = new System.Drawing.Point(360, 118);
            this.btn_Mode_M_B.Name = "btn_Mode_M_B";
            this.btn_Mode_M_B.Size = new System.Drawing.Size(123, 32);
            this.btn_Mode_M_B.TabIndex = 14;
            this.btn_Mode_M_B.Text = "Режим М-Б";
            this.btn_Mode_M_B.UseVisualStyleBackColor = true;
            this.btn_Mode_M_B.Click += new System.EventHandler(this.btn_Mode_M_B_Click);
            // 
            // grpBox_Corner
            // 
            this.grpBox_Corner.Controls.Add(this.tiCorner);
            this.grpBox_Corner.Controls.Add(this.cmbBox_Corner);
            this.grpBox_Corner.Controls.Add(this.rbtn_Kren);
            this.grpBox_Corner.Controls.Add(this.rbtn_Tangazh);
            this.grpBox_Corner.Controls.Add(this.rbtn_Zero);
            this.grpBox_Corner.Controls.Add(this.cmbBox_3);
            this.grpBox_Corner.Controls.Add(this.cmbBox_2);
            this.grpBox_Corner.Controls.Add(this.cmbBox_1);
            this.grpBox_Corner.Controls.Add(this.btn_Corner);
            this.grpBox_Corner.Controls.Add(this.label4);
            this.grpBox_Corner.Controls.Add(this.label3);
            this.grpBox_Corner.Controls.Add(this.label2);
            this.grpBox_Corner.Location = new System.Drawing.Point(497, 10);
            this.grpBox_Corner.Name = "grpBox_Corner";
            this.grpBox_Corner.Size = new System.Drawing.Size(415, 200);
            this.grpBox_Corner.TabIndex = 15;
            this.grpBox_Corner.TabStop = false;
            this.grpBox_Corner.Text = "Введение углов крена и тангажа";
            // 
            // tiCorner
            // 
            this.tiCorner.BackColor = System.Drawing.Color.Transparent;
            this.tiCorner.BorderColor = System.Drawing.Color.Gray;
            this.tiCorner.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold);
            this.tiCorner.Location = new System.Drawing.Point(261, 16);
            this.tiCorner.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tiCorner.Name = "tiCorner";
            this.tiCorner.OffColor = System.Drawing.Color.Gray;
            this.tiCorner.OnColor = System.Drawing.Color.LightGreen;
            this.tiCorner.Size = new System.Drawing.Size(147, 81);
            this.tiCorner.State = false;
            this.tiCorner.TabIndex = 20;
            this.tiCorner.TabStop = false;
            this.tiCorner.VisualText = " 0°";
            // 
            // cmbBox_Corner
            // 
            this.cmbBox_Corner.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBox_Corner.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.cmbBox_Corner.FormattingEnabled = true;
            this.cmbBox_Corner.Items.AddRange(new object[] {
            "+15°",
            "+11°",
            "+5°",
            " 0°",
            "-5°",
            "-11°",
            "-15°"});
            this.cmbBox_Corner.Location = new System.Drawing.Point(145, 29);
            this.cmbBox_Corner.Name = "cmbBox_Corner";
            this.cmbBox_Corner.Size = new System.Drawing.Size(100, 21);
            this.cmbBox_Corner.TabIndex = 3;
            // 
            // rbtn_Kren
            // 
            this.rbtn_Kren.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtn_Kren.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.rbtn_Kren.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(249)))), ((int)(((byte)(164)))));
            this.rbtn_Kren.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.rbtn_Kren.Location = new System.Drawing.Point(14, 65);
            this.rbtn_Kren.Name = "rbtn_Kren";
            this.rbtn_Kren.Size = new System.Drawing.Size(100, 32);
            this.rbtn_Kren.TabIndex = 1;
            this.rbtn_Kren.TabStop = true;
            this.rbtn_Kren.Text = "Крен";
            this.rbtn_Kren.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbtn_Kren.UseVisualStyleBackColor = true;
            this.rbtn_Kren.Click += new System.EventHandler(this.rbtn_Kren_Click);
            // 
            // rbtn_Tangazh
            // 
            this.rbtn_Tangazh.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtn_Tangazh.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.rbtn_Tangazh.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(249)))), ((int)(((byte)(164)))));
            this.rbtn_Tangazh.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.rbtn_Tangazh.Location = new System.Drawing.Point(14, 108);
            this.rbtn_Tangazh.Name = "rbtn_Tangazh";
            this.rbtn_Tangazh.Size = new System.Drawing.Size(100, 32);
            this.rbtn_Tangazh.TabIndex = 2;
            this.rbtn_Tangazh.TabStop = true;
            this.rbtn_Tangazh.Text = "Тангаж";
            this.rbtn_Tangazh.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbtn_Tangazh.UseVisualStyleBackColor = true;
            this.rbtn_Tangazh.Click += new System.EventHandler(this.rbtn_Tangazh_Click);
            // 
            // rbtn_Zero
            // 
            this.rbtn_Zero.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtn_Zero.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(249)))), ((int)(((byte)(164)))));
            this.rbtn_Zero.Checked = true;
            this.rbtn_Zero.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.rbtn_Zero.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(249)))), ((int)(((byte)(164)))));
            this.rbtn_Zero.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtn_Zero.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.rbtn_Zero.Location = new System.Drawing.Point(14, 22);
            this.rbtn_Zero.Name = "rbtn_Zero";
            this.rbtn_Zero.Size = new System.Drawing.Size(100, 32);
            this.rbtn_Zero.TabIndex = 0;
            this.rbtn_Zero.TabStop = true;
            this.rbtn_Zero.Text = "Обнуление";
            this.rbtn_Zero.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbtn_Zero.UseVisualStyleBackColor = false;
            this.rbtn_Zero.Click += new System.EventHandler(this.rbtn_Zero_Click);
            // 
            // cmbBox_3
            // 
            this.cmbBox_3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBox_3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmbBox_3.FormattingEnabled = true;
            this.cmbBox_3.Items.AddRange(new object[] {
            "Не изменяется",
            "Увеличивается",
            "Уменьшается"});
            this.cmbBox_3.Location = new System.Drawing.Point(276, 152);
            this.cmbBox_3.Name = "cmbBox_3";
            this.cmbBox_3.Size = new System.Drawing.Size(125, 22);
            this.cmbBox_3.TabIndex = 7;
            this.cmbBox_3.SelectedIndexChanged += new System.EventHandler(this.cmbBox_3_SelectedValueChanged);
            // 
            // cmbBox_2
            // 
            this.cmbBox_2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBox_2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmbBox_2.FormattingEnabled = true;
            this.cmbBox_2.Items.AddRange(new object[] {
            "Не изменяется",
            "Увеличивается",
            "Уменьшается"});
            this.cmbBox_2.Location = new System.Drawing.Point(145, 152);
            this.cmbBox_2.Name = "cmbBox_2";
            this.cmbBox_2.Size = new System.Drawing.Size(125, 22);
            this.cmbBox_2.TabIndex = 6;
            this.cmbBox_2.SelectedIndexChanged += new System.EventHandler(this.cmbBox_2_SelectedValueChanged);
            // 
            // cmbBox_1
            // 
            this.cmbBox_1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBox_1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmbBox_1.FormattingEnabled = true;
            this.cmbBox_1.Items.AddRange(new object[] {
            "Не изменяется",
            "Увеличивается",
            "Уменьшается"});
            this.cmbBox_1.Location = new System.Drawing.Point(14, 152);
            this.cmbBox_1.Name = "cmbBox_1";
            this.cmbBox_1.Size = new System.Drawing.Size(125, 22);
            this.cmbBox_1.TabIndex = 5;
            this.cmbBox_1.SelectedIndexChanged += new System.EventHandler(this.cmbBox_1_SelectedValueChanged);
            // 
            // btn_Corner
            // 
            this.btn_Corner.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_Corner.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_Corner.Location = new System.Drawing.Point(145, 65);
            this.btn_Corner.Name = "btn_Corner";
            this.btn_Corner.Size = new System.Drawing.Size(100, 32);
            this.btn_Corner.TabIndex = 4;
            this.btn_Corner.Text = "Ввести угол";
            this.btn_Corner.UseVisualStyleBackColor = true;
            this.btn_Corner.Click += new System.EventHandler(this.btn_Corner_Click);
            // 
            // label4
            // 
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(273, 176);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(128, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "Вниз";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(142, 176);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(128, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "Влево";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(14, 176);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Вперед";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_Mode_M_C
            // 
            this.btn_Mode_M_C.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_Mode_M_C.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_Mode_M_C.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_Mode_M_C.Location = new System.Drawing.Point(360, 75);
            this.btn_Mode_M_C.Name = "btn_Mode_M_C";
            this.btn_Mode_M_C.Size = new System.Drawing.Size(123, 32);
            this.btn_Mode_M_C.TabIndex = 13;
            this.btn_Mode_M_C.Text = "Режим М-С";
            this.btn_Mode_M_C.UseVisualStyleBackColor = true;
            this.btn_Mode_M_C.Click += new System.EventHandler(this.btn_Mode_M_C_Click);
            // 
            // btn_Memory
            // 
            this.btn_Memory.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_Memory.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_Memory.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_Memory.Location = new System.Drawing.Point(360, 32);
            this.btn_Memory.Name = "btn_Memory";
            this.btn_Memory.Size = new System.Drawing.Size(123, 32);
            this.btn_Memory.TabIndex = 12;
            this.btn_Memory.Text = "Режим ПАМЯТЬ";
            this.btn_Memory.UseVisualStyleBackColor = true;
            this.btn_Memory.Click += new System.EventHandler(this.btn_Memory_Click);
            // 
            // btn_Zero
            // 
            this.btn_Zero.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btn_Zero.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_Zero.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btn_Zero.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_Zero.Location = new System.Drawing.Point(253, 373);
            this.btn_Zero.Name = "btn_Zero";
            this.btn_Zero.Padding = new System.Windows.Forms.Padding(30, 0, 30, 0);
            this.btn_Zero.Size = new System.Drawing.Size(196, 40);
            this.btn_Zero.TabIndex = 19;
            this.btn_Zero.Text = "Wxr, Wzr, Wyr при обнулении";
            this.btn_Zero.UseVisualStyleBackColor = true;
            this.btn_Zero.Click += new System.EventHandler(this.btn_Zero_Click);
            // 
            // btn_BPK
            // 
            this.btn_BPK.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btn_BPK.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_BPK.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btn_BPK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_BPK.Location = new System.Drawing.Point(29, 373);
            this.btn_BPK.Name = "btn_BPK";
            this.btn_BPK.Size = new System.Drawing.Size(196, 40);
            this.btn_BPK.TabIndex = 18;
            this.btn_BPK.Text = "БПК";
            this.btn_BPK.UseVisualStyleBackColor = true;
            this.btn_BPK.Click += new System.EventHandler(this.btn_BPK_Click);
            // 
            // btn_Task_4
            // 
            this.btn_Task_4.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btn_Task_4.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_Task_4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btn_Task_4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_Task_4.Location = new System.Drawing.Point(718, 313);
            this.btn_Task_4.Name = "btn_Task_4";
            this.btn_Task_4.Size = new System.Drawing.Size(196, 40);
            this.btn_Task_4.TabIndex = 17;
            this.btn_Task_4.Text = "КЗ-4";
            this.btn_Task_4.UseVisualStyleBackColor = true;
            this.btn_Task_4.Click += new System.EventHandler(this.btn_Task_4_Click);
            // 
            // btn_Task_3
            // 
            this.btn_Task_3.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btn_Task_3.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_Task_3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btn_Task_3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_Task_3.Location = new System.Drawing.Point(482, 313);
            this.btn_Task_3.Name = "btn_Task_3";
            this.btn_Task_3.Size = new System.Drawing.Size(196, 40);
            this.btn_Task_3.TabIndex = 16;
            this.btn_Task_3.Text = "КЗ-3";
            this.btn_Task_3.UseVisualStyleBackColor = true;
            this.btn_Task_3.Click += new System.EventHandler(this.btn_Task_3_Click);
            // 
            // btn_Task_2
            // 
            this.btn_Task_2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btn_Task_2.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_Task_2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btn_Task_2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_Task_2.Location = new System.Drawing.Point(253, 313);
            this.btn_Task_2.Name = "btn_Task_2";
            this.btn_Task_2.Size = new System.Drawing.Size(196, 40);
            this.btn_Task_2.TabIndex = 15;
            this.btn_Task_2.Text = "КЗ-2";
            this.btn_Task_2.UseVisualStyleBackColor = true;
            this.btn_Task_2.Click += new System.EventHandler(this.btn_Task_2_Click);
            // 
            // btn_Task_1
            // 
            this.btn_Task_1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btn_Task_1.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_Task_1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btn_Task_1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_Task_1.Location = new System.Drawing.Point(29, 313);
            this.btn_Task_1.Name = "btn_Task_1";
            this.btn_Task_1.Size = new System.Drawing.Size(196, 40);
            this.btn_Task_1.TabIndex = 14;
            this.btn_Task_1.Text = "КЗ-1";
            this.btn_Task_1.UseVisualStyleBackColor = true;
            this.btn_Task_1.Click += new System.EventHandler(this.btn_Task_1_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(21, 305);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(902, 5);
            this.label1.TabIndex = 13;
            // 
            // tmrBlock8
            // 
            this.tmrBlock8.Tick += new System.EventHandler(this.tmrBlock8_Tick);
            // 
            // MeasurementsControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.bBortSystems);
            this.Controls.Add(this.btn_Light);
            this.Controls.Add(this.panel_Task);
            this.Controls.Add(this.btn_Zero);
            this.Controls.Add(this.btn_BPK);
            this.Controls.Add(this.btn_Task_4);
            this.Controls.Add(this.btn_Task_3);
            this.Controls.Add(this.btn_Task_2);
            this.Controls.Add(this.btn_Task_1);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MeasurementsControl";
            this.Size = new System.Drawing.Size(926, 429);
            this.Load += new System.EventHandler(this.MeasurementsControl_Load);
            this.panel_Task.ResumeLayout(false);
            this.panel_Task.PerformLayout();
            this.grpBoxPoint23.ResumeLayout(false);
            this.grpBoxPoint23.PerformLayout();
            this.grpBox_Task.ResumeLayout(false);
            this.grpBox_Speed.ResumeLayout(false);
            this.grpBox_Value.ResumeLayout(false);
            this.grpBox_Corner.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button bBortSystems;
		private System.Windows.Forms.Button btn_Light;
		private System.Windows.Forms.Panel panel_Task;
		private System.Windows.Forms.CheckBox cbShowBlock8;
		private System.Windows.Forms.GroupBox grpBoxPoint23;
		private System.Windows.Forms.RadioButton rbtn_Point23_2;
		private System.Windows.Forms.RadioButton rbtn_Point23_1;
		private System.Windows.Forms.Label label37;
		private System.Windows.Forms.GroupBox grpBox_Task;
		private System.Windows.Forms.Label lbl_Set_Task;
		private System.Windows.Forms.GroupBox grpBox_Speed;
		private UserControlsKPK2.IndicatorValue indValue3;
		private UserControlsKPK2.IndicatorValue indValue2;
		private UserControlsKPK2.IndicatorValue indValue1;
		private System.Windows.Forms.GroupBox grpBox_Value;
		private System.Windows.Forms.Button btn_Mode_M_B;
		private System.Windows.Forms.GroupBox grpBox_Corner;
		private UserControlsKPK2.TextIndicator tiCorner;
		private System.Windows.Forms.ComboBox cmbBox_Corner;
		private System.Windows.Forms.RadioButton rbtn_Kren;
		private System.Windows.Forms.RadioButton rbtn_Tangazh;
		private System.Windows.Forms.RadioButton rbtn_Zero;
		private System.Windows.Forms.ComboBox cmbBox_3;
		private System.Windows.Forms.ComboBox cmbBox_2;
		private System.Windows.Forms.ComboBox cmbBox_1;
		private System.Windows.Forms.Button btn_Corner;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btn_Mode_M_C;
		private System.Windows.Forms.Button btn_Memory;
		private System.Windows.Forms.Button btn_Zero;
		private System.Windows.Forms.Button btn_BPK;
		private System.Windows.Forms.Button btn_Task_4;
		private System.Windows.Forms.Button btn_Task_3;
		private System.Windows.Forms.Button btn_Task_2;
		private System.Windows.Forms.Button btn_Task_1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Timer tmrBlock8;
		private SegmentIndicatorControl IndicatorDown;
		private SegmentIndicatorControl IndicatorLeft;
		private SegmentIndicatorControl IndicatorUp;
		private System.Windows.Forms.Label label5;
		private SegmentIndication siWorkTime;
        private IndicatorValue MemoryPS;
        private IndicatorValue MemoryUS;
    }
}
