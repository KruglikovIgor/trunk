namespace KPK2.GeneralForms.ServiceForm
{
	partial class frmAutoAttestation
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
			this.dgvCheckTable = new System.Windows.Forms.DataGridView();
			this.bStart = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.lChannelName = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.dgvCheckTable)).BeginInit();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// dgvCheckTable
			// 
			this.dgvCheckTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dgvCheckTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvCheckTable.Location = new System.Drawing.Point(14, 35);
			this.dgvCheckTable.Name = "dgvCheckTable";
			this.dgvCheckTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvCheckTable.Size = new System.Drawing.Size(698, 392);
			this.dgvCheckTable.TabIndex = 0;
			// 
			// bStart
			// 
			this.bStart.Location = new System.Drawing.Point(12, 9);
			this.bStart.Name = "bStart";
			this.bStart.Size = new System.Drawing.Size(181, 36);
			this.bStart.TabIndex = 1;
			this.bStart.Text = "Старт";
			this.bStart.UseVisualStyleBackColor = true;
			this.bStart.Click += new System.EventHandler(this.bStart_Click);
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.Location = new System.Drawing.Point(530, 9);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(181, 36);
			this.button2.TabIndex = 2;
			this.button2.Text = "Стоп";
			this.button2.UseVisualStyleBackColor = true;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.bStart);
			this.panel1.Controls.Add(this.button2);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 433);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(723, 52);
			this.panel1.TabIndex = 3;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(14, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 14);
			this.label1.TabIndex = 4;
			this.label1.Text = "Поверка канала:";
			// 
			// lChannelName
			// 
			this.lChannelName.AutoSize = true;
			this.lChannelName.Location = new System.Drawing.Point(120, 13);
			this.lChannelName.Name = "lChannelName";
			this.lChannelName.Size = new System.Drawing.Size(81, 14);
			this.lChannelName.TabIndex = 5;
			this.lChannelName.Text = "Неизвестный";
			// 
			// frmAutoAttestation
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(723, 485);
			this.Controls.Add(this.lChannelName);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.dgvCheckTable);
			this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmAutoAttestation";
			this.Text = "Автоматическая поверка";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAutoAttestation_FormClosing);
			this.Load += new System.EventHandler(this.frmAutoAttestation_Load);
			((System.ComponentModel.ISupportInitialize)(this.dgvCheckTable)).EndInit();
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView dgvCheckTable;
		private System.Windows.Forms.Button bStart;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lChannelName;
	}
}