namespace KPK2.Reports
{
    partial class frmForRepairTestPrintDiss3290A
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmForRepairTestPrint));
            this.crystalViewer = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.MainReport1 = new KPK2.Reports.RepairReport_Diss32_90A();
            this.SuspendLayout();
            // 
            // crystalViewer
            // 
            this.crystalViewer.ActiveViewIndex = -1;
            this.crystalViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.crystalViewer, "crystalViewer");
            this.crystalViewer.Name = "crystalViewer";
            // 
            // frmForRepairTestPrint
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.crystalViewer);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmForRepairTestPrint";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmForRepairTestPrint_FormClosing);
            this.Load += new System.EventHandler(this.frmForRepairTestPrint_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalViewer;
        private RepairReport_Diss32_90A MainReport1;
    }
}