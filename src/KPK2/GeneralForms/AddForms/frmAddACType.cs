using System;
using System.Windows.Forms;
using Sparc.KPK2.NFunctions;
using Sparc.KPK2.Resources.Properties;

namespace KPK2.GeneralForms.AddForms
{
  public partial class frmAddACType : Form
  {

    public string NewACType { get; private set; }

    public frmAddACType(string newACType)
    {
      InitializeComponent();
      tbNewACType.Text = newACType;
    }

    private void frmAddACType_Load(object sender, EventArgs e)
    {
      lbExistingACTypes.Items.AddRange(DataManager.Instance.GetAllACTypes().ToArray());
    }

    private void bCLose_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void bAdd_Click(object sender, EventArgs e)
    {
      NewACType = tbNewACType.Text.Trim();
      if (String.IsNullOrEmpty(NewACType))
      {
        MessageBox.Show(Resources.EmptyField);
        return;
      }

      if (!DataManager.Instance.AddACType(NewACType))
      {
        MessageBox.Show(Resources.ErrorSavingDataOnServer);
        return;
      }
      Close();
    }

    private void lbExistingACTypes_SelectedIndexChanged(object sender, EventArgs e)
    {
      NewACType = lbExistingACTypes.SelectedItem.ToString();
      tbNewACType.Text = NewACType;
      Close();
    }
  }
}
