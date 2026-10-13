namespace KPK2.Reports
{
    partial class frmForPrint
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmForPrint));
            this.crystalViewer = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.MainReport1 = new KPK2.Reports.MainReport();
            this.SuspendLayout();
            // 
            // crystalViewer
            // 
            this.crystalViewer.ActiveViewIndex = -1;
            this.crystalViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalViewer.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.crystalViewer, "crystalViewer");
            this.crystalViewer.Name = "crystalViewer";
            // 
            // frmForPrint
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.crystalViewer);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmForPrint";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmForPrint_FormClosing);
            this.Load += new System.EventHandler(this.frmForPrint_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalViewer;
        private MainReport MainReport1;
    }
}