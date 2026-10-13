namespace KPK2.GeneralForms.ServiceForm
{
    partial class frmCheckComDevices
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
            this.dgvCheckDevices = new System.Windows.Forms.DataGridView();
            this.cPort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lCaption = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.bClose = new System.Windows.Forms.Button();
            this.bStart = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCheckDevices)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvCheckDevices
            // 
            this.dgvCheckDevices.AllowUserToAddRows = false;
            this.dgvCheckDevices.AllowUserToDeleteRows = false;
            this.dgvCheckDevices.AllowUserToResizeColumns = false;
            this.dgvCheckDevices.AllowUserToResizeRows = false;
            this.dgvCheckDevices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCheckDevices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCheckDevices.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cPort,
            this.cResult});
            this.dgvCheckDevices.Location = new System.Drawing.Point(0, 35);
            this.dgvCheckDevices.Name = "dgvCheckDevices";
            this.dgvCheckDevices.RowHeadersVisible = false;
            this.dgvCheckDevices.Size = new System.Drawing.Size(626, 405);
            this.dgvCheckDevices.TabIndex = 0;
            // 
            // cPort
            // 
            this.cPort.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cPort.HeaderText = "Порт";
            this.cPort.Name = "cPort";
            // 
            // cResult
            // 
            this.cResult.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cResult.HeaderText = "Результат";
            this.cResult.Name = "cResult";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lCaption);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(626, 37);
            this.panel1.TabIndex = 1;
            // 
            // lCaption
            // 
            this.lCaption.AutoSize = true;
            this.lCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lCaption.ForeColor = System.Drawing.Color.Black;
            this.lCaption.Location = new System.Drawing.Point(0, 5);
            this.lCaption.Name = "lCaption";
            this.lCaption.Size = new System.Drawing.Size(81, 25);
            this.lCaption.TabIndex = 0;
            this.lCaption.Text = "Готово";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.bClose);
            this.panel2.Controls.Add(this.bStart);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 439);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(626, 92);
            this.panel2.TabIndex = 2;
            // 
            // bClose
            // 
            this.bClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bClose.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bClose.Location = new System.Drawing.Point(382, 10);
            this.bClose.Name = "bClose";
            this.bClose.Size = new System.Drawing.Size(232, 73);
            this.bClose.TabIndex = 1;
            this.bClose.Text = "Закрыть";
            this.bClose.UseVisualStyleBackColor = true;
            this.bClose.Click += new System.EventHandler(this.bClose_Click);
            // 
            // bStart
            // 
            this.bStart.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bStart.Location = new System.Drawing.Point(12, 11);
            this.bStart.Name = "bStart";
            this.bStart.Size = new System.Drawing.Size(232, 73);
            this.bStart.TabIndex = 0;
            this.bStart.Text = "Повторить проверку";
            this.bStart.UseVisualStyleBackColor = true;
            this.bStart.Click += new System.EventHandler(this.bStart_Click);
            // 
            // frmCheckComDevices
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 531);
            this.ControlBox = false;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dgvCheckDevices);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "frmCheckComDevices";
            this.Text = "Проверка com-портов на наличие устройств";
            this.Load += new System.EventHandler(this.frmCheckComDevices_Load);
            this.Shown += new System.EventHandler(this.frmCheckComDevices_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCheckDevices)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvCheckDevices;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lCaption;
        private System.Windows.Forms.Button bClose;
        private System.Windows.Forms.Button bStart;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPort;
        private System.Windows.Forms.DataGridViewTextBoxColumn cResult;
    }
}