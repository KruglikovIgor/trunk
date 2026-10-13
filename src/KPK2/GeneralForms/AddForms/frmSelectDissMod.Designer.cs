namespace KPK2.GeneralForms.AddForms
{
    partial class frmSelectDissMod
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
            this.btnOk = new System.Windows.Forms.Button();
            this.lblModDiss = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lbDissMods = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(13, 121);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(96, 31);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "Выбрать";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // lblModDiss
            // 
            this.lblModDiss.AutoSize = true;
            this.lblModDiss.Location = new System.Drawing.Point(5, 12);
            this.lblModDiss.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblModDiss.Name = "lblModDiss";
            this.lblModDiss.Size = new System.Drawing.Size(252, 19);
            this.lblModDiss.TabIndex = 1;
            this.lblModDiss.Text = "Выберите модификацию ДИСС:  ";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(375, 121);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(96, 31);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lbDissMods
            // 
            this.lbDissMods.FormattingEnabled = true;
            this.lbDissMods.ItemHeight = 19;
            this.lbDissMods.Location = new System.Drawing.Point(12, 34);
            this.lbDissMods.Name = "lbDissMods";
            this.lbDissMods.Size = new System.Drawing.Size(460, 80);
            this.lbDissMods.TabIndex = 4;
            // 
            // frmSelectDissMod
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 161);
            this.Controls.Add(this.lbDissMods);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblModDiss);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSelectDissMod";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmSelectDissMod_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label lblModDiss;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ListBox lbDissMods;
    }
}