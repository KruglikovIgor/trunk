namespace KPK2.GeneralForms
{
    partial class frmReportHeader
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmReportHeader));
      this.panel1 = new System.Windows.Forms.Panel();
      this.cbShowLogo = new System.Windows.Forms.CheckBox();
      this.bSave = new System.Windows.Forms.Button();
      this.bClose = new System.Windows.Forms.Button();
      this.rtbHeader = new System.Windows.Forms.RichTextBox();
      this.rtbRightHeader = new System.Windows.Forms.RichTextBox();
      this.ofdOpenLogo = new System.Windows.Forms.OpenFileDialog();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.cbShowLogo);
      this.panel1.Controls.Add(this.bSave);
      this.panel1.Controls.Add(this.bClose);
      resources.ApplyResources(this.panel1, "panel1");
      this.panel1.Name = "panel1";
      // 
      // cbShowLogo
      // 
      resources.ApplyResources(this.cbShowLogo, "cbShowLogo");
      this.cbShowLogo.Name = "cbShowLogo";
      this.cbShowLogo.UseVisualStyleBackColor = true;
      this.cbShowLogo.CheckedChanged += new System.EventHandler(this.cbShowLogo_CheckedChanged);
      // 
      // bSave
      // 
      resources.ApplyResources(this.bSave, "bSave");
      this.bSave.Name = "bSave";
      this.bSave.UseVisualStyleBackColor = true;
      this.bSave.Click += new System.EventHandler(this.bSave_Click);
      // 
      // bClose
      // 
      resources.ApplyResources(this.bClose, "bClose");
      this.bClose.Name = "bClose";
      this.bClose.UseVisualStyleBackColor = true;
      this.bClose.Click += new System.EventHandler(this.bClose_Click);
      // 
      // rtbHeader
      // 
      resources.ApplyResources(this.rtbHeader, "rtbHeader");
      this.rtbHeader.Name = "rtbHeader";
      this.rtbHeader.TextChanged += new System.EventHandler(this.rtbHeader_TextChanged);
      // 
      // rtbRightHeader
      // 
      resources.ApplyResources(this.rtbRightHeader, "rtbRightHeader");
      this.rtbRightHeader.Name = "rtbRightHeader";
      // 
      // ofdOpenLogo
      // 
      resources.ApplyResources(this.ofdOpenLogo, "ofdOpenLogo");
      // 
      // frmReportHeader
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ControlBox = false;
      this.Controls.Add(this.rtbRightHeader);
      this.Controls.Add(this.rtbHeader);
      this.Controls.Add(this.panel1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
      this.Name = "frmReportHeader";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmReportHeader_FormClosing);
      this.Load += new System.EventHandler(this.frmReportHeader_Load);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button bClose;
        private System.Windows.Forms.RichTextBox rtbHeader;
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.RichTextBox rtbRightHeader;
        private System.Windows.Forms.CheckBox cbShowLogo;
        private System.Windows.Forms.OpenFileDialog ofdOpenLogo;
    }
}