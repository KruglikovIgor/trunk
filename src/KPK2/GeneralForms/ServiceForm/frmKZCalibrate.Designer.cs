namespace KPK2.GeneralForms.ServiceForm
{
    partial class frmKZCalibrate
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
            this.nudChannel = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.bPlus = new System.Windows.Forms.Button();
            this.bMinus = new System.Windows.Forms.Button();
            this.bAuto = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tbShift = new System.Windows.Forms.TextBox();
            this.bSave = new System.Windows.Forms.Button();
            this.bClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudChannel)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "Подстраиваемый канал:";
            // 
            // nudChannel
            // 
            this.nudChannel.Location = new System.Drawing.Point(175, 46);
            this.nudChannel.Maximum = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.nudChannel.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudChannel.Name = "nudChannel";
            this.nudChannel.Size = new System.Drawing.Size(48, 22);
            this.nudChannel.TabIndex = 1;
            this.nudChannel.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudChannel.ValueChanged += new System.EventHandler(this.nudChannel_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(248, 14);
            this.label2.TabIndex = 2;
            this.label2.Text = "Номинальное действующее значение - 3В";
            // 
            // bPlus
            // 
            this.bPlus.Location = new System.Drawing.Point(245, 75);
            this.bPlus.Name = "bPlus";
            this.bPlus.Size = new System.Drawing.Size(75, 23);
            this.bPlus.TabIndex = 3;
            this.bPlus.Text = "+";
            this.bPlus.UseVisualStyleBackColor = true;
            this.bPlus.Click += new System.EventHandler(this.bPlus_Click);
            // 
            // bMinus
            // 
            this.bMinus.Location = new System.Drawing.Point(245, 104);
            this.bMinus.Name = "bMinus";
            this.bMinus.Size = new System.Drawing.Size(75, 23);
            this.bMinus.TabIndex = 4;
            this.bMinus.Text = "-";
            this.bMinus.UseVisualStyleBackColor = true;
            this.bMinus.Click += new System.EventHandler(this.bMinus_Click);
            // 
            // bAuto
            // 
            this.bAuto.Enabled = false;
            this.bAuto.Location = new System.Drawing.Point(245, 133);
            this.bAuto.Name = "bAuto";
            this.bAuto.Size = new System.Drawing.Size(75, 23);
            this.bAuto.TabIndex = 5;
            this.bAuto.Text = "Авто";
            this.bAuto.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 14);
            this.label3.TabIndex = 6;
            this.label3.Text = "Текущий сдвиг:";
            // 
            // tbShift
            // 
            this.tbShift.Location = new System.Drawing.Point(175, 75);
            this.tbShift.Name = "tbShift";
            this.tbShift.ReadOnly = true;
            this.tbShift.Size = new System.Drawing.Size(48, 22);
            this.tbShift.TabIndex = 7;
            this.tbShift.Text = "0";
            // 
            // bSave
            // 
            this.bSave.Enabled = false;
            this.bSave.Location = new System.Drawing.Point(12, 177);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(144, 44);
            this.bSave.TabIndex = 8;
            this.bSave.Text = "Сохранить";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // bClose
            // 
            this.bClose.Location = new System.Drawing.Point(185, 177);
            this.bClose.Name = "bClose";
            this.bClose.Size = new System.Drawing.Size(144, 44);
            this.bClose.TabIndex = 9;
            this.bClose.Text = "Закрыть";
            this.bClose.UseVisualStyleBackColor = true;
            this.bClose.Click += new System.EventHandler(this.bClose_Click);
            // 
            // frmKZCalibrate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 231);
            this.ControlBox = false;
            this.Controls.Add(this.bClose);
            this.Controls.Add(this.bSave);
            this.Controls.Add(this.tbShift);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.bAuto);
            this.Controls.Add(this.bMinus);
            this.Controls.Add(this.bPlus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nudChannel);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "frmKZCalibrate";
            this.Text = "Калибровка платы МКЗ";
            this.Load += new System.EventHandler(this.frmKZCalibrate_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudChannel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudChannel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bPlus;
        private System.Windows.Forms.Button bMinus;
        private System.Windows.Forms.Button bAuto;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbShift;
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.Button bClose;
    }
}