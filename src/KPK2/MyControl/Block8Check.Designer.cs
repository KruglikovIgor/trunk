namespace KPK2.MyControl
{
    partial class Block8Check
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Block8Check));
      this.grpBox_Task6 = new System.Windows.Forms.GroupBox();
      this.b8 = new Block8();
      this.btn_Mode_M = new System.Windows.Forms.Button();
      this.btn_Mode_K = new System.Windows.Forms.Button();
      this.grpBox_Task6.SuspendLayout();
      this.SuspendLayout();
      // 
      // grpBox_Task6
      // 
      this.grpBox_Task6.Controls.Add(this.b8);
      resources.ApplyResources(this.grpBox_Task6, "grpBox_Task6");
      this.grpBox_Task6.Name = "grpBox_Task6";
      this.grpBox_Task6.TabStop = false;
      this.grpBox_Task6.Enter += new System.EventHandler(this.grpBox_Task6_Enter);
      // 
      // b8
      // 
      resources.ApplyResources(this.b8, "b8");
      this.b8.Name = "b8";
      // 
      // btn_Mode_M
      // 
      this.btn_Mode_M.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
      resources.ApplyResources(this.btn_Mode_M, "btn_Mode_M");
      this.btn_Mode_M.Name = "btn_Mode_M";
      this.btn_Mode_M.UseVisualStyleBackColor = true;
      this.btn_Mode_M.Click += new System.EventHandler(this.btn_Mode_M_Click);
      // 
      // btn_Mode_K
      // 
      this.btn_Mode_K.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
      resources.ApplyResources(this.btn_Mode_K, "btn_Mode_K");
      this.btn_Mode_K.Name = "btn_Mode_K";
      this.btn_Mode_K.UseVisualStyleBackColor = true;
      this.btn_Mode_K.Click += new System.EventHandler(this.btn_Mode_K_Click);
      // 
      // Block8Check
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
      this.Controls.Add(this.btn_Mode_M);
      this.Controls.Add(this.btn_Mode_K);
      this.Controls.Add(this.grpBox_Task6);
      this.Name = "Block8Check";
      this.Load += new System.EventHandler(this.Block8Check_Load);
      this.grpBox_Task6.ResumeLayout(false);
      this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBox_Task6;
        private System.Windows.Forms.Button btn_Mode_M;
        private System.Windows.Forms.Button btn_Mode_K;
        public Block8 b8;
    }
}
