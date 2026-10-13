namespace KPK2.GeneralForms
{
    partial class frmRassword
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRassword));
      this.lbl1 = new System.Windows.Forms.Label();
      this.txtBox = new System.Windows.Forms.TextBox();
      this.btn_Ok = new System.Windows.Forms.Button();
      this.btn_Cancel = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // lbl1
      // 
      resources.ApplyResources(this.lbl1, "lbl1");
      this.lbl1.Name = "lbl1";
      // 
      // txtBox
      // 
      resources.ApplyResources(this.txtBox, "txtBox");
      this.txtBox.Name = "txtBox";
      // 
      // btn_Ok
      // 
      resources.ApplyResources(this.btn_Ok, "btn_Ok");
      this.btn_Ok.Name = "btn_Ok";
      this.btn_Ok.UseVisualStyleBackColor = true;
      this.btn_Ok.Click += new System.EventHandler(this.btn_Ok_Click);
      // 
      // btn_Cancel
      // 
      resources.ApplyResources(this.btn_Cancel, "btn_Cancel");
      this.btn_Cancel.Name = "btn_Cancel";
      this.btn_Cancel.UseVisualStyleBackColor = true;
      this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
      // 
      // frmRassword
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.btn_Cancel);
      this.Controls.Add(this.btn_Ok);
      this.Controls.Add(this.txtBox);
      this.Controls.Add(this.lbl1);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "frmRassword";
      this.ShowIcon = false;
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl1;
        private System.Windows.Forms.TextBox txtBox;
        private System.Windows.Forms.Button btn_Ok;
        private System.Windows.Forms.Button btn_Cancel;
    }
}