namespace KPK2.MyControl.DISS.FrmDiss32
{
	partial class UICheckingControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UICheckingControl));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.pageWork = new System.Windows.Forms.TabPage();
            this.ivFrequency = new UserControlsKPK2.IndicatorValue();
            this.btnFreq = new System.Windows.Forms.Button();
            this.btnInsulationResist = new System.Windows.Forms.Button();
            this.gbAll = new System.Windows.Forms.GroupBox();
            this.pDefaultValues = new System.Windows.Forms.Panel();
            this.rtbIndicatorsBaseState = new System.Windows.Forms.RichTextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.grpBox_115V = new System.Windows.Forms.GroupBox();
            this.lbl_Edit_115V = new System.Windows.Forms.Label();
            this.btn_115_Plus = new System.Windows.Forms.Button();
            this.btn_115_Minus = new System.Windows.Forms.Button();
            this.grpBox_27V = new System.Windows.Forms.GroupBox();
            this.lbl_Edit_27V = new System.Windows.Forms.Label();
            this.btn_27_Plus = new System.Windows.Forms.Button();
            this.btn_27_Minus = new System.Windows.Forms.Button();
            this.btnNoise = new System.Windows.Forms.Button();
            this.btnSD = new System.Windows.Forms.Button();
            this.btn_Tok_K = new System.Windows.Forms.Button();
            this.btn_GLPD_MOD = new System.Windows.Forms.Button();
            this.bSecPitan = new System.Windows.Forms.Button();
            this.bFirstPitan = new System.Windows.Forms.Button();
            this.lbl1 = new System.Windows.Forms.Label();
            this.pageTask = new System.Windows.Forms.TabPage();
            this.measurementsControl1 = new KPK2.MyControl.DISS.FrmDiss32.MeasurementsControl();
            this.tabControl.SuspendLayout();
            this.pageWork.SuspendLayout();
            this.gbAll.SuspendLayout();
            this.pDefaultValues.SuspendLayout();
            this.grpBox_115V.SuspendLayout();
            this.grpBox_27V.SuspendLayout();
            this.pageTask.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.pageWork);
            this.tabControl.Controls.Add(this.pageTask);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabControl.ItemSize = new System.Drawing.Size(150, 18);
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(980, 475);
            this.tabControl.TabIndex = 2;
            this.tabControl.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControl_Selecting);
            this.tabControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl_Selected);
            this.tabControl.EnabledChanged += new System.EventHandler(this.tabControl_EnabledChanged);
            // 
            // pageWork
            // 
            this.pageWork.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.pageWork.Controls.Add(this.ivFrequency);
            this.pageWork.Controls.Add(this.btnFreq);
            this.pageWork.Controls.Add(this.btnInsulationResist);
            this.pageWork.Controls.Add(this.gbAll);
            this.pageWork.Controls.Add(this.grpBox_115V);
            this.pageWork.Controls.Add(this.grpBox_27V);
            this.pageWork.Controls.Add(this.btnNoise);
            this.pageWork.Controls.Add(this.btnSD);
            this.pageWork.Controls.Add(this.btn_Tok_K);
            this.pageWork.Controls.Add(this.btn_GLPD_MOD);
            this.pageWork.Controls.Add(this.bSecPitan);
            this.pageWork.Controls.Add(this.bFirstPitan);
            this.pageWork.Controls.Add(this.lbl1);
            this.pageWork.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.pageWork.Location = new System.Drawing.Point(4, 22);
            this.pageWork.Name = "pageWork";
            this.pageWork.Size = new System.Drawing.Size(972, 449);
            this.pageWork.TabIndex = 4;
            this.pageWork.Text = "Проведение измерений";
            // 
            // ivFrequency
            // 
            this.ivFrequency.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ivFrequency.Description = null;
            this.ivFrequency.DownSign = "Частота";
            this.ivFrequency.FailColor = System.Drawing.Color.Red;
            this.ivFrequency.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ivFrequency.ForeColor = System.Drawing.Color.Black;
            this.ivFrequency.Location = new System.Drawing.Point(16, 197);
            this.ivFrequency.MaxGreenValue = 0D;
            this.ivFrequency.MaxValue = 0D;
            this.ivFrequency.MinGreenValue = 0D;
            this.ivFrequency.MinValue = 0D;
            this.ivFrequency.Name = "ivFrequency";
            this.ivFrequency.Size = new System.Drawing.Size(150, 59);
            this.ivFrequency.SuccessColor = System.Drawing.Color.LimeGreen;
            this.ivFrequency.TabIndex = 28;
            this.ivFrequency.UpSign = "f, МГц";
            this.ivFrequency.UsualColor = System.Drawing.Color.Black;
            this.ivFrequency.Value = 0D;
            this.ivFrequency.Visible = false;
            // 
            // btnFreq
            // 
            this.btnFreq.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnFreq.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnFreq.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnFreq.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnFreq.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnFreq.Location = new System.Drawing.Point(711, 268);
            this.btnFreq.Name = "btnFreq";
            this.btnFreq.Size = new System.Drawing.Size(182, 40);
            this.btnFreq.TabIndex = 27;
            this.btnFreq.Tag = "3";
            this.btnFreq.Text = "Частота и Мощность СВЧ";
            this.btnFreq.UseVisualStyleBackColor = true;
            this.btnFreq.Click += new System.EventHandler(this.btnFreq_Click);
            // 
            // btnInsulationResist
            // 
            this.btnInsulationResist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnInsulationResist.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnInsulationResist.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnInsulationResist.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnInsulationResist.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnInsulationResist.Location = new System.Drawing.Point(711, 313);
            this.btnInsulationResist.Name = "btnInsulationResist";
            this.btnInsulationResist.Size = new System.Drawing.Size(182, 40);
            this.btnInsulationResist.TabIndex = 19;
            this.btnInsulationResist.Tag = "3";
            this.btnInsulationResist.Text = "Электрическое сопротивление изоляции ";
            this.btnInsulationResist.UseVisualStyleBackColor = true;
            this.btnInsulationResist.Click += new System.EventHandler(this.btnInsulationResist_Click);
            // 
            // gbAll
            // 
            this.gbAll.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbAll.Controls.Add(this.pDefaultValues);
            this.gbAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gbAll.Location = new System.Drawing.Point(10, 13);
            this.gbAll.Name = "gbAll";
            this.gbAll.Size = new System.Drawing.Size(936, 184);
            this.gbAll.TabIndex = 14;
            this.gbAll.TabStop = false;
            // 
            // pDefaultValues
            // 
            this.pDefaultValues.Controls.Add(this.rtbIndicatorsBaseState);
            this.pDefaultValues.Controls.Add(this.richTextBox1);
            this.pDefaultValues.Location = new System.Drawing.Point(6, 10);
            this.pDefaultValues.Name = "pDefaultValues";
            this.pDefaultValues.Size = new System.Drawing.Size(877, 168);
            this.pDefaultValues.TabIndex = 0;
            // 
            // rtbIndicatorsBaseState
            // 
            this.rtbIndicatorsBaseState.BackColor = System.Drawing.SystemColors.Control;
            this.rtbIndicatorsBaseState.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbIndicatorsBaseState.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rtbIndicatorsBaseState.Location = new System.Drawing.Point(435, 3);
            this.rtbIndicatorsBaseState.Name = "rtbIndicatorsBaseState";
            this.rtbIndicatorsBaseState.ReadOnly = true;
            this.rtbIndicatorsBaseState.Size = new System.Drawing.Size(409, 152);
            this.rtbIndicatorsBaseState.TabIndex = 2;
            this.rtbIndicatorsBaseState.TabStop = false;
            this.rtbIndicatorsBaseState.Text = resources.GetString("rtbIndicatorsBaseState.Text");
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.Control;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.richTextBox1.Location = new System.Drawing.Point(4, 3);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(318, 152);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.TabStop = false;
            this.richTextBox1.Text = "Исходные положения переключателей:\n\nна индикаторе УС ПС\n- переключатель “С, М-С, " +
    "М-Б” в положение “С”";
            // 
            // grpBox_115V
            // 
            this.grpBox_115V.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.grpBox_115V.Controls.Add(this.lbl_Edit_115V);
            this.grpBox_115V.Controls.Add(this.btn_115_Plus);
            this.grpBox_115V.Controls.Add(this.btn_115_Minus);
            this.grpBox_115V.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.grpBox_115V.Location = new System.Drawing.Point(254, 355);
            this.grpBox_115V.Name = "grpBox_115V";
            this.grpBox_115V.Size = new System.Drawing.Size(182, 76);
            this.grpBox_115V.TabIndex = 11;
            this.grpBox_115V.TabStop = false;
            this.grpBox_115V.Text = "Подстройка 115В";
            this.grpBox_115V.Visible = false;
            // 
            // lbl_Edit_115V
            // 
            this.lbl_Edit_115V.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_Edit_115V.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbl_Edit_115V.Location = new System.Drawing.Point(2, 21);
            this.lbl_Edit_115V.Name = "lbl_Edit_115V";
            this.lbl_Edit_115V.Size = new System.Drawing.Size(178, 13);
            this.lbl_Edit_115V.TabIndex = 2;
            this.lbl_Edit_115V.Text = "Максимальное значение БП";
            this.lbl_Edit_115V.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_Edit_115V.Visible = false;
            // 
            // btn_115_Plus
            // 
            this.btn_115_Plus.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_115_Plus.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_115_Plus.Location = new System.Drawing.Point(102, 43);
            this.btn_115_Plus.Name = "btn_115_Plus";
            this.btn_115_Plus.Size = new System.Drawing.Size(70, 23);
            this.btn_115_Plus.TabIndex = 1;
            this.btn_115_Plus.Text = "+0,25B";
            this.btn_115_Plus.UseVisualStyleBackColor = true;
            this.btn_115_Plus.Click += new System.EventHandler(this.btn_115_Plus_Click);
            // 
            // btn_115_Minus
            // 
            this.btn_115_Minus.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_115_Minus.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_115_Minus.Location = new System.Drawing.Point(10, 43);
            this.btn_115_Minus.Name = "btn_115_Minus";
            this.btn_115_Minus.Size = new System.Drawing.Size(70, 23);
            this.btn_115_Minus.TabIndex = 0;
            this.btn_115_Minus.Text = "-0,25B";
            this.btn_115_Minus.UseVisualStyleBackColor = true;
            this.btn_115_Minus.Click += new System.EventHandler(this.btn_115_Minus_Click);
            // 
            // grpBox_27V
            // 
            this.grpBox_27V.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.grpBox_27V.Controls.Add(this.lbl_Edit_27V);
            this.grpBox_27V.Controls.Add(this.btn_27_Plus);
            this.grpBox_27V.Controls.Add(this.btn_27_Minus);
            this.grpBox_27V.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.grpBox_27V.Location = new System.Drawing.Point(30, 355);
            this.grpBox_27V.Name = "grpBox_27V";
            this.grpBox_27V.Size = new System.Drawing.Size(182, 76);
            this.grpBox_27V.TabIndex = 10;
            this.grpBox_27V.TabStop = false;
            this.grpBox_27V.Text = "Подстройка 27В";
            this.grpBox_27V.Visible = false;
            // 
            // lbl_Edit_27V
            // 
            this.lbl_Edit_27V.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_Edit_27V.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbl_Edit_27V.Location = new System.Drawing.Point(2, 21);
            this.lbl_Edit_27V.Name = "lbl_Edit_27V";
            this.lbl_Edit_27V.Size = new System.Drawing.Size(178, 13);
            this.lbl_Edit_27V.TabIndex = 2;
            this.lbl_Edit_27V.Text = "Максимальное значение БП";
            this.lbl_Edit_27V.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_Edit_27V.Visible = false;
            // 
            // btn_27_Plus
            // 
            this.btn_27_Plus.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_27_Plus.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_27_Plus.Location = new System.Drawing.Point(102, 43);
            this.btn_27_Plus.Name = "btn_27_Plus";
            this.btn_27_Plus.Size = new System.Drawing.Size(70, 23);
            this.btn_27_Plus.TabIndex = 1;
            this.btn_27_Plus.Text = "+0,1B";
            this.btn_27_Plus.UseVisualStyleBackColor = true;
            this.btn_27_Plus.Click += new System.EventHandler(this.btn_27_Plus_Click);
            // 
            // btn_27_Minus
            // 
            this.btn_27_Minus.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_27_Minus.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_27_Minus.Location = new System.Drawing.Point(10, 43);
            this.btn_27_Minus.Name = "btn_27_Minus";
            this.btn_27_Minus.Size = new System.Drawing.Size(70, 23);
            this.btn_27_Minus.TabIndex = 0;
            this.btn_27_Minus.Text = "-0,1B";
            this.btn_27_Minus.UseVisualStyleBackColor = true;
            this.btn_27_Minus.Click += new System.EventHandler(this.btn_27_Minus_Click);
            // 
            // btnNoise
            // 
            this.btnNoise.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNoise.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnNoise.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnNoise.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnNoise.Location = new System.Drawing.Point(483, 313);
            this.btnNoise.Name = "btnNoise";
            this.btnNoise.Size = new System.Drawing.Size(182, 40);
            this.btnNoise.TabIndex = 9;
            this.btnNoise.Tag = "6";
            this.btnNoise.Text = "Шум УНЧ";
            this.btnNoise.UseVisualStyleBackColor = true;
            this.btnNoise.Click += new System.EventHandler(this.btnNoise_Click);
            // 
            // btnSD
            // 
            this.btnSD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSD.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSD.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnSD.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSD.Location = new System.Drawing.Point(254, 313);
            this.btnSD.Name = "btnSD";
            this.btnSD.Size = new System.Drawing.Size(182, 40);
            this.btnSD.TabIndex = 8;
            this.btnSD.Tag = "5";
            this.btnSD.Text = "СД";
            this.btnSD.UseVisualStyleBackColor = true;
            this.btnSD.Click += new System.EventHandler(this.btnSD_Click);
            // 
            // btn_Tok_K
            // 
            this.btn_Tok_K.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_Tok_K.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btn_Tok_K.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_Tok_K.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_Tok_K.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_Tok_K.Location = new System.Drawing.Point(30, 313);
            this.btn_Tok_K.Name = "btn_Tok_K";
            this.btn_Tok_K.Size = new System.Drawing.Size(182, 40);
            this.btn_Tok_K.TabIndex = 7;
            this.btn_Tok_K.Tag = "4";
            this.btn_Tok_K.Text = "ТОК К";
            this.btn_Tok_K.UseVisualStyleBackColor = true;
            this.btn_Tok_K.Click += new System.EventHandler(this.btn_Tok_K_Click);
            // 
            // btn_GLPD_MOD
            // 
            this.btn_GLPD_MOD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_GLPD_MOD.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btn_GLPD_MOD.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_GLPD_MOD.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_GLPD_MOD.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_GLPD_MOD.Location = new System.Drawing.Point(483, 268);
            this.btn_GLPD_MOD.Name = "btn_GLPD_MOD";
            this.btn_GLPD_MOD.Size = new System.Drawing.Size(182, 40);
            this.btn_GLPD_MOD.TabIndex = 6;
            this.btn_GLPD_MOD.Tag = "3";
            this.btn_GLPD_MOD.Text = "ГЛПД, МОД";
            this.btn_GLPD_MOD.UseVisualStyleBackColor = true;
            this.btn_GLPD_MOD.Click += new System.EventHandler(this.btn_GLPD_MOD_Click);
            // 
            // bSecPitan
            // 
            this.bSecPitan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bSecPitan.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.bSecPitan.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.bSecPitan.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bSecPitan.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.bSecPitan.Location = new System.Drawing.Point(254, 268);
            this.bSecPitan.Name = "bSecPitan";
            this.bSecPitan.Size = new System.Drawing.Size(182, 40);
            this.bSecPitan.TabIndex = 5;
            this.bSecPitan.Tag = "2";
            this.bSecPitan.Text = "Вторичные источники питания";
            this.bSecPitan.UseVisualStyleBackColor = true;
            this.bSecPitan.Click += new System.EventHandler(this.btn_SecPitan_Click);
            // 
            // bFirstPitan
            // 
            this.bFirstPitan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bFirstPitan.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.bFirstPitan.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.bFirstPitan.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bFirstPitan.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.bFirstPitan.Location = new System.Drawing.Point(30, 268);
            this.bFirstPitan.Name = "bFirstPitan";
            this.bFirstPitan.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.bFirstPitan.Size = new System.Drawing.Size(182, 40);
            this.bFirstPitan.TabIndex = 4;
            this.bFirstPitan.Tag = "1";
            this.bFirstPitan.Text = "Источники питания\r\n27В и 115В";
            this.bFirstPitan.UseVisualStyleBackColor = true;
            this.bFirstPitan.Click += new System.EventHandler(this.btn_FirstPitan_Click);
            // 
            // lbl1
            // 
            this.lbl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbl1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbl1.Location = new System.Drawing.Point(16, 260);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(899, 3);
            this.lbl1.TabIndex = 0;
            // 
            // pageTask
            // 
            this.pageTask.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.pageTask.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pageTask.Controls.Add(this.measurementsControl1);
            this.pageTask.Font = new System.Drawing.Font("Verdana", 9F);
            this.pageTask.Location = new System.Drawing.Point(4, 22);
            this.pageTask.Name = "pageTask";
            this.pageTask.Size = new System.Drawing.Size(972, 449);
            this.pageTask.TabIndex = 2;
            this.pageTask.Text = "Контрольные задачи";
            // 
            // measurementsControl1
            // 
            this.measurementsControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.measurementsControl1.Device = null;
            this.measurementsControl1.DevicesId = Sparc.KPK2.NFunctions.Enums.DevicesID.None;
            this.measurementsControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.measurementsControl1.IsOneChannel = false;
            this.measurementsControl1.Location = new System.Drawing.Point(0, 0);
            this.measurementsControl1.Margin = new System.Windows.Forms.Padding(0);
            this.measurementsControl1.Measure = null;
            this.measurementsControl1.MeasuringValueInfo = null;
            this.measurementsControl1.Modification = Sparc.KPK2.NFunctions.Enums.Modification.None;
            this.measurementsControl1.Name = "measurementsControl1";
            this.measurementsControl1.Size = new System.Drawing.Size(970, 447);
            this.measurementsControl1.TabIndex = 0;
            this.measurementsControl1.TestTasks = null;
            // 
            // UICheckingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Name = "UICheckingControl";
            this.Size = new System.Drawing.Size(980, 475);
            this.tabControl.ResumeLayout(false);
            this.pageWork.ResumeLayout(false);
            this.gbAll.ResumeLayout(false);
            this.pDefaultValues.ResumeLayout(false);
            this.grpBox_115V.ResumeLayout(false);
            this.grpBox_27V.ResumeLayout(false);
            this.pageTask.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.GroupBox gbAll;
		private System.Windows.Forms.Panel pDefaultValues;
		private System.Windows.Forms.RichTextBox rtbIndicatorsBaseState;
		private System.Windows.Forms.RichTextBox richTextBox1;
		private System.Windows.Forms.GroupBox grpBox_115V;
		private System.Windows.Forms.Label lbl_Edit_115V;
		private System.Windows.Forms.Button btn_115_Plus;
		private System.Windows.Forms.Button btn_115_Minus;
		private System.Windows.Forms.GroupBox grpBox_27V;
		private System.Windows.Forms.Label lbl_Edit_27V;
		private System.Windows.Forms.Button btn_27_Plus;
		private System.Windows.Forms.Button btn_27_Minus;
		private System.Windows.Forms.Label lbl1;
		private System.Windows.Forms.TabPage pageTask;
		public System.Windows.Forms.TabPage pageWork;
		public System.Windows.Forms.Button btnNoise;
		public System.Windows.Forms.Button btnSD;
		public System.Windows.Forms.Button btn_Tok_K;
		public System.Windows.Forms.Button btn_GLPD_MOD;
		public System.Windows.Forms.Button bSecPitan;
		public System.Windows.Forms.Button bFirstPitan;
		public System.Windows.Forms.Button btnInsulationResist;
        public System.Windows.Forms.Button btnFreq;
        public MeasurementsControl measurementsControl1;
        private UserControlsKPK2.IndicatorValue ivFrequency;
    }
}
