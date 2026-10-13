namespace DBInstallerGUI
{
    partial class MainWnd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWnd));
            this.rtb_Log = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chk_IsReinstall = new System.Windows.Forms.CheckBox();
            this.chk_IsBackup = new System.Windows.Forms.CheckBox();
            this.btn_Install = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btn_Check = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btn_Restore = new System.Windows.Forms.Button();
            this.btn_Backup = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_Backup_Folder = new System.Windows.Forms.Button();
            this.tbx_Backup_Folder = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // rtb_Log
            // 
            this.rtb_Log.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtb_Log.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rtb_Log.Location = new System.Drawing.Point(12, 380);
            this.rtb_Log.Name = "rtb_Log";
            this.rtb_Log.ReadOnly = true;
            this.rtb_Log.Size = new System.Drawing.Size(660, 270);
            this.rtb_Log.TabIndex = 4;
            this.rtb_Log.Text = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.chk_IsReinstall);
            this.groupBox1.Controls.Add(this.chk_IsBackup);
            this.groupBox1.Controls.Add(this.btn_Install);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(12, 79);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(660, 110);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Install";
            // 
            // chk_IsReinstall
            // 
            this.chk_IsReinstall.AutoSize = true;
            this.chk_IsReinstall.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chk_IsReinstall.ForeColor = System.Drawing.Color.Brown;
            this.chk_IsReinstall.Location = new System.Drawing.Point(10, 44);
            this.chk_IsReinstall.Name = "chk_IsReinstall";
            this.chk_IsReinstall.Size = new System.Drawing.Size(131, 17);
            this.chk_IsReinstall.TabIndex = 1;
            this.chk_IsReinstall.Text = "Reinstall database";
            this.chk_IsReinstall.UseVisualStyleBackColor = true;
            // 
            // chk_IsBackup
            // 
            this.chk_IsBackup.AutoSize = true;
            this.chk_IsBackup.ForeColor = System.Drawing.Color.Blue;
            this.chk_IsBackup.Location = new System.Drawing.Point(10, 21);
            this.chk_IsBackup.Name = "chk_IsBackup";
            this.chk_IsBackup.Size = new System.Drawing.Size(125, 17);
            this.chk_IsBackup.TabIndex = 0;
            this.chk_IsBackup.Text = "Backup database";
            this.chk_IsBackup.UseVisualStyleBackColor = true;
            this.chk_IsBackup.CheckedChanged += new System.EventHandler(this.chk_IsBackup_CheckedChanged);
            // 
            // btn_Install
            // 
            this.btn_Install.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btn_Install.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_Install.ForeColor = System.Drawing.Color.Red;
            this.btn_Install.Location = new System.Drawing.Point(242, 70);
            this.btn_Install.Name = "btn_Install";
            this.btn_Install.Size = new System.Drawing.Size(200, 23);
            this.btn_Install.TabIndex = 2;
            this.btn_Install.Text = "INSTALL";
            this.btn_Install.UseVisualStyleBackColor = true;
            this.btn_Install.Click += new System.EventHandler(this.btn_Install_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.btn_Check);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox4.Location = new System.Drawing.Point(12, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(660, 61);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Check";
            // 
            // btn_Check
            // 
            this.btn_Check.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btn_Check.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_Check.ForeColor = System.Drawing.Color.Green;
            this.btn_Check.Location = new System.Drawing.Point(242, 21);
            this.btn_Check.Name = "btn_Check";
            this.btn_Check.Size = new System.Drawing.Size(200, 23);
            this.btn_Check.TabIndex = 0;
            this.btn_Check.Text = "CHECK";
            this.btn_Check.UseVisualStyleBackColor = true;
            this.btn_Check.Click += new System.EventHandler(this.btn_Check_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.btn_Restore);
            this.groupBox5.Controls.Add(this.btn_Backup);
            this.groupBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox5.Location = new System.Drawing.Point(12, 253);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(660, 116);
            this.groupBox5.TabIndex = 3;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Backup / Restore";
            // 
            // btn_Restore
            // 
            this.btn_Restore.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btn_Restore.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_Restore.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btn_Restore.Location = new System.Drawing.Point(242, 76);
            this.btn_Restore.Name = "btn_Restore";
            this.btn_Restore.Size = new System.Drawing.Size(200, 23);
            this.btn_Restore.TabIndex = 1;
            this.btn_Restore.Text = "RESTORE";
            this.btn_Restore.UseVisualStyleBackColor = true;
            this.btn_Restore.Click += new System.EventHandler(this.btn_Restore_Click);
            // 
            // btn_Backup
            // 
            this.btn_Backup.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btn_Backup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_Backup.ForeColor = System.Drawing.Color.Blue;
            this.btn_Backup.Location = new System.Drawing.Point(242, 21);
            this.btn_Backup.Name = "btn_Backup";
            this.btn_Backup.Size = new System.Drawing.Size(200, 23);
            this.btn_Backup.TabIndex = 0;
            this.btn_Backup.Text = "BACKUP";
            this.btn_Backup.UseVisualStyleBackColor = true;
            this.btn_Backup.Click += new System.EventHandler(this.btn_Backup_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btn_Backup_Folder);
            this.groupBox2.Controls.Add(this.tbx_Backup_Folder);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox2.Location = new System.Drawing.Point(12, 195);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(660, 52);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Backup Folder";
            // 
            // btn_Backup_Folder
            // 
            this.btn_Backup_Folder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Backup_Folder.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_Backup_Folder.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_Backup_Folder.Location = new System.Drawing.Point(617, 18);
            this.btn_Backup_Folder.Name = "btn_Backup_Folder";
            this.btn_Backup_Folder.Size = new System.Drawing.Size(37, 23);
            this.btn_Backup_Folder.TabIndex = 1;
            this.btn_Backup_Folder.Text = "...";
            this.btn_Backup_Folder.UseVisualStyleBackColor = true;
            this.btn_Backup_Folder.Click += new System.EventHandler(this.btn_Backup_Folder_Click);
            // 
            // tbx_Backup_Folder
            // 
            this.tbx_Backup_Folder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbx_Backup_Folder.BackColor = System.Drawing.Color.Ivory;
            this.tbx_Backup_Folder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbx_Backup_Folder.ForeColor = System.Drawing.Color.Blue;
            this.tbx_Backup_Folder.Location = new System.Drawing.Point(6, 19);
            this.tbx_Backup_Folder.Name = "tbx_Backup_Folder";
            this.tbx_Backup_Folder.Size = new System.Drawing.Size(602, 20);
            this.tbx_Backup_Folder.TabIndex = 0;
            this.tbx_Backup_Folder.TextChanged += new System.EventHandler(this.tbx_Backup_Folder_TextChanged);
            // 
            // MainWnd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 662);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.rtb_Log);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(300, 500);
            this.Name = "MainWnd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Database Installer";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtb_Log;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_Install;
        private System.Windows.Forms.CheckBox chk_IsReinstall;
        private System.Windows.Forms.CheckBox chk_IsBackup;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btn_Check;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btn_Restore;
        private System.Windows.Forms.Button btn_Backup;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_Backup_Folder;
        private System.Windows.Forms.TextBox tbx_Backup_Folder;

    }
}