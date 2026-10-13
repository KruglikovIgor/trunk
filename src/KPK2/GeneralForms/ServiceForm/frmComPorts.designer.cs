namespace KPK2.GeneralForms.ServiceForm
{
    partial class frmComPorts
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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      this.dgvPorts = new System.Windows.Forms.DataGridView();
      this.Protocol = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Com = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.panel1 = new System.Windows.Forms.Panel();
      this.bAuto = new System.Windows.Forms.Button();
      this.bClose = new System.Windows.Forms.Button();
      this.bSave = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.dgvPorts)).BeginInit();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // dgvPorts
      // 
      this.dgvPorts.AllowUserToAddRows = false;
      this.dgvPorts.AllowUserToDeleteRows = false;
      this.dgvPorts.AllowUserToResizeColumns = false;
      this.dgvPorts.AllowUserToResizeRows = false;
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dgvPorts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      this.dgvPorts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvPorts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Protocol,
            this.Com});
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Info;
      dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.dgvPorts.DefaultCellStyle = dataGridViewCellStyle2;
      this.dgvPorts.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dgvPorts.Location = new System.Drawing.Point(0, 0);
      this.dgvPorts.MultiSelect = false;
      this.dgvPorts.Name = "dgvPorts";
      this.dgvPorts.ReadOnly = true;
      this.dgvPorts.RowHeadersVisible = false;
      this.dgvPorts.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
      this.dgvPorts.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.dgvPorts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.dgvPorts.Size = new System.Drawing.Size(355, 266);
      this.dgvPorts.TabIndex = 0;
      // 
      // Protocol
      // 
      this.Protocol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.Protocol.HeaderText = "Протокол";
      this.Protocol.Name = "Protocol";
      this.Protocol.ReadOnly = true;
      // 
      // Com
      // 
      this.Com.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.Com.HeaderText = "Com-порт";
      this.Com.Name = "Com";
      this.Com.ReadOnly = true;
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.bAuto);
      this.panel1.Controls.Add(this.bClose);
      this.panel1.Controls.Add(this.bSave);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panel1.Location = new System.Drawing.Point(0, 202);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(355, 64);
      this.panel1.TabIndex = 1;
      // 
      // bAuto
      // 
      this.bAuto.Dock = System.Windows.Forms.DockStyle.Fill;
      this.bAuto.Enabled = false;
      this.bAuto.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.bAuto.Location = new System.Drawing.Point(97, 0);
      this.bAuto.Name = "bAuto";
      this.bAuto.Size = new System.Drawing.Size(161, 64);
      this.bAuto.TabIndex = 2;
      this.bAuto.Text = "Авто";
      this.bAuto.UseVisualStyleBackColor = true;
      this.bAuto.Click += new System.EventHandler(this.bAuto_Click);
      // 
      // bClose
      // 
      this.bClose.Dock = System.Windows.Forms.DockStyle.Right;
      this.bClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.bClose.Location = new System.Drawing.Point(258, 0);
      this.bClose.Name = "bClose";
      this.bClose.Size = new System.Drawing.Size(97, 64);
      this.bClose.TabIndex = 1;
      this.bClose.Text = "Закрыть";
      this.bClose.UseVisualStyleBackColor = true;
      this.bClose.Click += new System.EventHandler(this.bClose_Click);
      // 
      // bSave
      // 
      this.bSave.Dock = System.Windows.Forms.DockStyle.Left;
      this.bSave.Enabled = false;
      this.bSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.bSave.Location = new System.Drawing.Point(0, 0);
      this.bSave.Name = "bSave";
      this.bSave.Size = new System.Drawing.Size(97, 64);
      this.bSave.TabIndex = 0;
      this.bSave.Text = "Сохранить";
      this.bSave.UseVisualStyleBackColor = true;
      // 
      // frmComPorts
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(355, 266);
      this.ControlBox = false;
      this.Controls.Add(this.panel1);
      this.Controls.Add(this.dgvPorts);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
      this.Name = "frmComPorts";
      this.Text = "Com-порты";
      this.Load += new System.EventHandler(this.ComPorts_Load);
      ((System.ComponentModel.ISupportInitialize)(this.dgvPorts)).EndInit();
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvPorts;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.Button bClose;
        private System.Windows.Forms.Button bAuto;
        private System.Windows.Forms.DataGridViewTextBoxColumn Protocol;
        private System.Windows.Forms.DataGridViewTextBoxColumn Com;
    }
}