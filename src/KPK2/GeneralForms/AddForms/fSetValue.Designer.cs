namespace KPK2.GeneralForms.AddForms
{
    partial class fSetValue
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
            this.lText = new System.Windows.Forms.Label();
            this.nudValue = new System.Windows.Forms.NumericUpDown();
            this.bSetValue = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudValue)).BeginInit();
            this.SuspendLayout();
            // 
            // lText
            // 
            this.lText.AutoSize = true;
            this.lText.Location = new System.Drawing.Point(14, 20);
            this.lText.Name = "lText";
            this.lText.Size = new System.Drawing.Size(150, 14);
            this.lText.TabIndex = 0;
            this.lText.Text = "Изменить значение угла:";
            // 
            // nudValue
            // 
            this.nudValue.Location = new System.Drawing.Point(180, 18);
            this.nudValue.Name = "nudValue";
            this.nudValue.Size = new System.Drawing.Size(57, 22);
            this.nudValue.TabIndex = 1;
            // 
            // bSetValue
            // 
            this.bSetValue.Location = new System.Drawing.Point(244, 15);
            this.bSetValue.Name = "bSetValue";
            this.bSetValue.Size = new System.Drawing.Size(87, 25);
            this.bSetValue.TabIndex = 2;
            this.bSetValue.Text = "Изменить";
            this.bSetValue.UseVisualStyleBackColor = true;
            this.bSetValue.Click += new System.EventHandler(this.bSetValue_Click);
            // 
            // bCancel
            // 
            this.bCancel.Location = new System.Drawing.Point(337, 15);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(87, 25);
            this.bCancel.TabIndex = 3;
            this.bCancel.Text = "Отмена";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // fSetValue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 60);
            this.ControlBox = false;
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bSetValue);
            this.Controls.Add(this.nudValue);
            this.Controls.Add(this.lText);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "fSetValue";
            this.Text = "Установка значения";
            ((System.ComponentModel.ISupportInitialize)(this.nudValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lText;
        private System.Windows.Forms.NumericUpDown nudValue;
        private System.Windows.Forms.Button bSetValue;
        private System.Windows.Forms.Button bCancel;
    }
}