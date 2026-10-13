using KPK2.Backup;
using KPK2.Properties;
using System;
using System.IO;
using System.Windows.Forms;

namespace KPK2.GeneralForms
{
  public partial class BackUPDataBase : Form
  {
    public BackUPDataBase()
    {
      InitializeComponent();
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
      if (keyData == Keys.Escape)
        Close();
      return base.ProcessCmdKey(ref msg, keyData);
    }

    // создаем копию
    private void bSave_Click(object sender, EventArgs e)
    {
      bSave.Enabled = bClose.Enabled = false;
      BackupDB(false);
      Close();
    }
      
    private void bClose_Click(object sender, EventArgs e)
    {
      Close();
    }

    internal void BackupDB(bool silence)
    {
        BackupAlgorithm bAlg = new BackupAlgorithm();
        string backupPath = "";
        bool backupResult = bAlg.BackupDB(ref backupPath);

        if (!silence)
        {
            progressBar1.Value = progressBar1.Maximum;
            if (backupResult)
            {
                backupPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "Sparc\\KPK2\\Database");
                MessageBox.Show(string.Format(Resources.CopyDbSuccessful, backupPath), Resources.ApplicationName,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show(Resources.CopyDbFailed, Resources.ErrorCaption, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
        }
    }
  }
}