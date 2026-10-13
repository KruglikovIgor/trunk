namespace KPK2.GeneralForms.ReferenceForm
{
  partial class frmBlock5Type
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
      this.bBlock5 = new System.Windows.Forms.Button();
      this.bBlock5I = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.label1.Location = new System.Drawing.Point(22, 27);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(166, 18);
      this.label1.TabIndex = 0;
      this.label1.Text = "Выберите тип блока 5:";
      // 
      // bBlock5
      // 
      this.bBlock5.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.bBlock5.Location = new System.Drawing.Point(12, 71);
      this.bBlock5.Name = "bBlock5";
      this.bBlock5.Size = new System.Drawing.Size(82, 35);
      this.bBlock5.TabIndex = 1;
      this.bBlock5.Text = "Блок 5";
      this.bBlock5.UseVisualStyleBackColor = true;
      this.bBlock5.Click += new System.EventHandler(this.bBlock5_Click);
      // 
      // bBlock5I
      // 
      this.bBlock5I.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.bBlock5I.Location = new System.Drawing.Point(113, 71);
      this.bBlock5I.Name = "bBlock5I";
      this.bBlock5I.Size = new System.Drawing.Size(82, 35);
      this.bBlock5I.TabIndex = 2;
      this.bBlock5I.Text = "Блок 5И";
      this.bBlock5I.UseVisualStyleBackColor = true;
      this.bBlock5I.Click += new System.EventHandler(this.bBlock5I_Click);
      // 
      // frmBlock5Type
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(207, 118);
      this.ControlBox = false;
      this.Controls.Add(this.bBlock5I);
      this.Controls.Add(this.bBlock5);
      this.Controls.Add(this.label1);
      this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.Name = "frmBlock5Type";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button bBlock5;
    private System.Windows.Forms.Button bBlock5I;
  }
}