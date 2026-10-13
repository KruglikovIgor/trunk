namespace KPK2.GeneralForms.ServiceForm
{
  partial class frmCalibrateAttenuations
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
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.tbKVPDiss13 = new System.Windows.Forms.TextBox();
      this.tbCabelDiss13 = new System.Windows.Forms.TextBox();
      this.tbAdapterDiss13 = new System.Windows.Forms.TextBox();
      this.bSave = new System.Windows.Forms.Button();
      this.bClose = new System.Windows.Forms.Button();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.label7 = new System.Windows.Forms.Label();
      this.tbVP = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.tbAdapterDiss15 = new System.Windows.Forms.TextBox();
      this.tbKVPDiss15 = new System.Windows.Forms.TextBox();
      this.tbCabelDiss15 = new System.Windows.Forms.TextBox();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(6, 23);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(111, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Затухание на КВП:";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(6, 51);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(129, 14);
      this.label2.TabIndex = 1;
      this.label2.Text = "Затухание на кабеле:";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(6, 78);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(163, 14);
      this.label3.TabIndex = 2;
      this.label3.Text = "Затухание на аттенюаторе:";
      // 
      // tbKVPDiss13
      // 
      this.tbKVPDiss13.Location = new System.Drawing.Point(176, 20);
      this.tbKVPDiss13.Name = "tbKVPDiss13";
      this.tbKVPDiss13.Size = new System.Drawing.Size(100, 22);
      this.tbKVPDiss13.TabIndex = 3;
      this.tbKVPDiss13.TextChanged += new System.EventHandler(this.tb_TextChanged);
      this.tbKVPDiss13.Leave += new System.EventHandler(this.tb_Leave);
      // 
      // tbCabelDiss13
      // 
      this.tbCabelDiss13.Location = new System.Drawing.Point(176, 48);
      this.tbCabelDiss13.Name = "tbCabelDiss13";
      this.tbCabelDiss13.Size = new System.Drawing.Size(100, 22);
      this.tbCabelDiss13.TabIndex = 4;
      this.tbCabelDiss13.TextChanged += new System.EventHandler(this.tb_TextChanged);
      this.tbCabelDiss13.Leave += new System.EventHandler(this.tb_Leave);
      // 
      // tbAdapterDiss13
      // 
      this.tbAdapterDiss13.Location = new System.Drawing.Point(176, 75);
      this.tbAdapterDiss13.Name = "tbAdapterDiss13";
      this.tbAdapterDiss13.Size = new System.Drawing.Size(100, 22);
      this.tbAdapterDiss13.TabIndex = 5;
      this.tbAdapterDiss13.TextChanged += new System.EventHandler(this.tb_TextChanged);
      this.tbAdapterDiss13.Leave += new System.EventHandler(this.tb_Leave);
      // 
      // bSave
      // 
      this.bSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.bSave.Enabled = false;
      this.bSave.Location = new System.Drawing.Point(12, 276);
      this.bSave.Name = "bSave";
      this.bSave.Size = new System.Drawing.Size(132, 30);
      this.bSave.TabIndex = 6;
      this.bSave.Text = "Сохранить";
      this.bSave.UseVisualStyleBackColor = true;
      this.bSave.Click += new System.EventHandler(this.bSave_Click);
      // 
      // bClose
      // 
      this.bClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.bClose.Location = new System.Drawing.Point(187, 276);
      this.bClose.Name = "bClose";
      this.bClose.Size = new System.Drawing.Size(132, 30);
      this.bClose.TabIndex = 7;
      this.bClose.Text = "Закрыть";
      this.bClose.UseVisualStyleBackColor = true;
      this.bClose.Click += new System.EventHandler(this.bClose_Click);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.Controls.Add(this.label2);
      this.groupBox1.Controls.Add(this.label3);
      this.groupBox1.Controls.Add(this.tbAdapterDiss13);
      this.groupBox1.Controls.Add(this.tbKVPDiss13);
      this.groupBox1.Controls.Add(this.tbCabelDiss13);
      this.groupBox1.Location = new System.Drawing.Point(12, 12);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(307, 105);
      this.groupBox1.TabIndex = 8;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "ДИСС-013";
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.label7);
      this.groupBox2.Controls.Add(this.tbVP);
      this.groupBox2.Controls.Add(this.label4);
      this.groupBox2.Controls.Add(this.label5);
      this.groupBox2.Controls.Add(this.label6);
      this.groupBox2.Controls.Add(this.tbAdapterDiss15);
      this.groupBox2.Controls.Add(this.tbKVPDiss15);
      this.groupBox2.Controls.Add(this.tbCabelDiss15);
      this.groupBox2.Location = new System.Drawing.Point(12, 130);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(307, 130);
      this.groupBox2.TabIndex = 9;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "ДИСС-15";
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(6, 22);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(104, 14);
      this.label7.TabIndex = 6;
      this.label7.Text = "Затухание на ВП:";
      // 
      // tbVP
      // 
      this.tbVP.Location = new System.Drawing.Point(176, 19);
      this.tbVP.Name = "tbVP";
      this.tbVP.Size = new System.Drawing.Size(100, 22);
      this.tbVP.TabIndex = 7;
      this.tbVP.TextChanged += new System.EventHandler(this.tb_TextChanged);
      this.tbVP.Leave += new System.EventHandler(this.tb_Leave);
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(6, 48);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(111, 14);
      this.label4.TabIndex = 0;
      this.label4.Text = "Затухание на КВП:";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(6, 74);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(129, 14);
      this.label5.TabIndex = 1;
      this.label5.Text = "Затухание на кабеле:";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(6, 101);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(163, 14);
      this.label6.TabIndex = 2;
      this.label6.Text = "Затухание на аттенюаторе:";
      // 
      // tbAdapterDiss15
      // 
      this.tbAdapterDiss15.Location = new System.Drawing.Point(176, 98);
      this.tbAdapterDiss15.Name = "tbAdapterDiss15";
      this.tbAdapterDiss15.Size = new System.Drawing.Size(100, 22);
      this.tbAdapterDiss15.TabIndex = 5;
      this.tbAdapterDiss15.TextChanged += new System.EventHandler(this.tb_TextChanged);
      this.tbAdapterDiss15.Leave += new System.EventHandler(this.tb_Leave);
      // 
      // tbKVPDiss15
      // 
      this.tbKVPDiss15.Location = new System.Drawing.Point(176, 45);
      this.tbKVPDiss15.Name = "tbKVPDiss15";
      this.tbKVPDiss15.Size = new System.Drawing.Size(100, 22);
      this.tbKVPDiss15.TabIndex = 3;
      this.tbKVPDiss15.TextChanged += new System.EventHandler(this.tb_TextChanged);
      this.tbKVPDiss15.Leave += new System.EventHandler(this.tb_Leave);
      // 
      // tbCabelDiss15
      // 
      this.tbCabelDiss15.Location = new System.Drawing.Point(176, 71);
      this.tbCabelDiss15.Name = "tbCabelDiss15";
      this.tbCabelDiss15.Size = new System.Drawing.Size(100, 22);
      this.tbCabelDiss15.TabIndex = 4;
      this.tbCabelDiss15.TextChanged += new System.EventHandler(this.tb_TextChanged);
      this.tbCabelDiss15.Leave += new System.EventHandler(this.tb_Leave);
      // 
      // frmCalibrateAttenuations
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(331, 318);
      this.Controls.Add(this.groupBox2);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.bClose);
      this.Controls.Add(this.bSave);
      this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "frmCalibrateAttenuations";
      this.Text = "Калибровка затуханий";
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox tbKVPDiss13;
    private System.Windows.Forms.TextBox tbCabelDiss13;
    private System.Windows.Forms.TextBox tbAdapterDiss13;
    private System.Windows.Forms.Button bSave;
    private System.Windows.Forms.Button bClose;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.TextBox tbVP;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.TextBox tbAdapterDiss15;
    private System.Windows.Forms.TextBox tbKVPDiss15;
    private System.Windows.Forms.TextBox tbCabelDiss15;

  }
}