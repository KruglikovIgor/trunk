namespace KPK2.Users
{
    partial class frmAddUser
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
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddUser));
      this.btnClose = new System.Windows.Forms.Button();
      this.btn_AddPicture = new System.Windows.Forms.Button();
      this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
      this.grpBox = new System.Windows.Forms.GroupBox();
      this.lbl7 = new System.Windows.Forms.Label();
      this.cbPost = new System.Windows.Forms.ComboBox();
      this.postBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.KPK2DataSet = new KPK2.KPK2DataSet();
      this.chckBox = new System.Windows.Forms.CheckBox();
      this.pictureBox = new System.Windows.Forms.PictureBox();
      this.ramka = new System.Windows.Forms.PictureBox();
      this.tbNote = new System.Windows.Forms.TextBox();
      this.tbLastName = new System.Windows.Forms.TextBox();
      this.tbName = new System.Windows.Forms.TextBox();
      this.tbSurname = new System.Windows.Forms.TextBox();
      this.tbNumber = new System.Windows.Forms.TextBox();
      this.lbl6 = new System.Windows.Forms.Label();
      this.lbl5 = new System.Windows.Forms.Label();
      this.lLastName = new System.Windows.Forms.Label();
      this.lName = new System.Windows.Forms.Label();
      this.lSurname = new System.Windows.Forms.Label();
      this.lbl1 = new System.Windows.Forms.Label();
      this.lbl = new System.Windows.Forms.Label();
      this.btnAdd = new System.Windows.Forms.Button();
      this.postTableAdapter = new PostTableAdapter();
      this.executorsTableAdapter = new ExecutorsTableAdapter();
      this.errProvider = new System.Windows.Forms.ErrorProvider(this.components);
      this.grpBox.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.postBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.KPK2DataSet)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.ramka)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.errProvider)).BeginInit();
      this.SuspendLayout();
      // 
      // btnClose
      // 
      resources.ApplyResources(this.btnClose, "btnClose");
      this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnClose.Name = "btnClose";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
      // 
      // btn_AddPicture
      // 
      resources.ApplyResources(this.btn_AddPicture, "btn_AddPicture");
      this.btn_AddPicture.Name = "btn_AddPicture";
      this.btn_AddPicture.UseVisualStyleBackColor = true;
      this.btn_AddPicture.Click += new System.EventHandler(this.button1_Click);
      // 
      // openFileDialog
      // 
      resources.ApplyResources(this.openFileDialog, "openFileDialog");
      this.openFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog_FileOk);
      // 
      // grpBox
      // 
      this.grpBox.Controls.Add(this.lbl7);
      this.grpBox.Controls.Add(this.cbPost);
      this.grpBox.Controls.Add(this.chckBox);
      this.grpBox.Controls.Add(this.pictureBox);
      this.grpBox.Controls.Add(this.btn_AddPicture);
      this.grpBox.Controls.Add(this.ramka);
      this.grpBox.Controls.Add(this.tbNote);
      this.grpBox.Controls.Add(this.tbLastName);
      this.grpBox.Controls.Add(this.tbName);
      this.grpBox.Controls.Add(this.tbSurname);
      this.grpBox.Controls.Add(this.tbNumber);
      this.grpBox.Controls.Add(this.lbl6);
      this.grpBox.Controls.Add(this.lbl5);
      this.grpBox.Controls.Add(this.lLastName);
      this.grpBox.Controls.Add(this.lName);
      this.grpBox.Controls.Add(this.lSurname);
      this.grpBox.Controls.Add(this.lbl1);
      resources.ApplyResources(this.grpBox, "grpBox");
      this.grpBox.Name = "grpBox";
      this.grpBox.TabStop = false;
      // 
      // lbl7
      // 
      resources.ApplyResources(this.lbl7, "lbl7");
      this.lbl7.Name = "lbl7";
      // 
      // cbPost
      // 
      this.cbPost.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.postBindingSource, "PoistId", true));
      this.cbPost.DisplayMember = "PoistId";
      this.cbPost.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cbPost.DropDownWidth = 357;
      this.cbPost.FormattingEnabled = true;
      resources.ApplyResources(this.cbPost, "cbPost");
      this.cbPost.Name = "cbPost";
      this.cbPost.ValueMember = "PoistId";
      // 
      // postBindingSource
      // 
      this.postBindingSource.DataMember = "Post";
      this.postBindingSource.DataSource = this.KPK2DataSet;
      // 
      // KPK2DataSet
      // 
      this.KPK2DataSet.DataSetName = "KPK2DataSet";
      this.KPK2DataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
      // 
      // chckBox
      // 
      this.chckBox.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
      this.chckBox.FlatAppearance.CheckedBackColor = System.Drawing.Color.Lime;
      this.chckBox.ForeColor = System.Drawing.SystemColors.ControlText;
      resources.ApplyResources(this.chckBox, "chckBox");
      this.chckBox.Name = "chckBox";
      this.chckBox.UseVisualStyleBackColor = true;
      this.chckBox.CheckedChanged += new System.EventHandler(this.chckBox_CheckedChanged);
      // 
      // pictureBox
      // 
      this.pictureBox.BackColor = System.Drawing.Color.White;
      resources.ApplyResources(this.pictureBox, "pictureBox");
      this.pictureBox.Name = "pictureBox";
      this.pictureBox.TabStop = false;
      // 
      // ramka
      // 
      resources.ApplyResources(this.ramka, "ramka");
      this.ramka.Name = "ramka";
      this.ramka.TabStop = false;
      // 
      // tbNote
      // 
      this.tbNote.BackColor = System.Drawing.SystemColors.ButtonHighlight;
      resources.ApplyResources(this.tbNote, "tbNote");
      this.tbNote.Name = "tbNote";
      // 
      // tbLastName
      // 
      this.tbLastName.BackColor = System.Drawing.SystemColors.ButtonHighlight;
      resources.ApplyResources(this.tbLastName, "tbLastName");
      this.tbLastName.Name = "tbLastName";
      // 
      // tbName
      // 
      this.tbName.BackColor = System.Drawing.SystemColors.ButtonHighlight;
      resources.ApplyResources(this.tbName, "tbName");
      this.tbName.Name = "tbName";
      this.tbName.TextChanged += new System.EventHandler(this.tb_TextChanged);
      // 
      // tbSurname
      // 
      this.tbSurname.BackColor = System.Drawing.SystemColors.ButtonHighlight;
      resources.ApplyResources(this.tbSurname, "tbSurname");
      this.tbSurname.Name = "tbSurname";
      this.tbSurname.TextChanged += new System.EventHandler(this.tb_TextChanged);
      // 
      // tbNumber
      // 
      this.tbNumber.BackColor = System.Drawing.SystemColors.ButtonHighlight;
      resources.ApplyResources(this.tbNumber, "tbNumber");
      this.tbNumber.Name = "tbNumber";
      this.tbNumber.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
      this.tbNumber.Validating += new System.ComponentModel.CancelEventHandler(this.txtBox_Number_Validating);
      // 
      // lbl6
      // 
      resources.ApplyResources(this.lbl6, "lbl6");
      this.lbl6.Name = "lbl6";
      // 
      // lbl5
      // 
      resources.ApplyResources(this.lbl5, "lbl5");
      this.lbl5.Name = "lbl5";
      // 
      // lLastName
      // 
      resources.ApplyResources(this.lLastName, "lLastName");
      this.lLastName.Name = "lLastName";
      // 
      // lName
      // 
      resources.ApplyResources(this.lName, "lName");
      this.lName.Name = "lName";
      // 
      // lSurname
      // 
      resources.ApplyResources(this.lSurname, "lSurname");
      this.lSurname.Name = "lSurname";
      // 
      // lbl1
      // 
      resources.ApplyResources(this.lbl1, "lbl1");
      this.lbl1.Name = "lbl1";
      // 
      // lbl
      // 
      resources.ApplyResources(this.lbl, "lbl");
      this.lbl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lbl.Name = "lbl";
      // 
      // btnAdd
      // 
      resources.ApplyResources(this.btnAdd, "btnAdd");
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
      // 
      // postTableAdapter
      // 
      this.postTableAdapter.ClearBeforeFill = true;
      // 
      // executorsTableAdapter
      // 
      this.executorsTableAdapter.ClearBeforeFill = true;
      // 
      // errProvider
      // 
      this.errProvider.ContainerControl = this;
      // 
      // frmAddUser
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnClose;
      this.Controls.Add(this.btnAdd);
      this.Controls.Add(this.lbl);
      this.Controls.Add(this.grpBox);
      this.Controls.Add(this.btnClose);
      this.DoubleBuffered = true;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "frmAddUser";
      this.ShowIcon = false;
      this.Load += new System.EventHandler(this.frmAddUser_Load);
      this.grpBox.ResumeLayout(false);
      this.grpBox.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.postBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.KPK2DataSet)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.ramka)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.errProvider)).EndInit();
      this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btn_AddPicture;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.GroupBox grpBox;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.PictureBox ramka;
        private System.Windows.Forms.TextBox tbNote;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.TextBox tbSurname;
        private System.Windows.Forms.TextBox tbNumber;
        private System.Windows.Forms.Label lbl6;
        private System.Windows.Forms.Label lbl5;
        private System.Windows.Forms.Label lName;
        private System.Windows.Forms.Label lSurname;
        private System.Windows.Forms.Label lbl1;
        private System.Windows.Forms.Label lbl;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.CheckBox chckBox;
        private System.Windows.Forms.ComboBox cbPost;
        private System.Windows.Forms.Label lbl7;
        private KPK2DataSet KPK2DataSet;
        private PostTableAdapter postTableAdapter;
        private ExecutorsTableAdapter executorsTableAdapter;
        private System.Windows.Forms.BindingSource postBindingSource;
        private System.Windows.Forms.Label lLastName;
        private System.Windows.Forms.TextBox tbLastName;
        private System.Windows.Forms.ErrorProvider errProvider;
    }
}