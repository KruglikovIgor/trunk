using System;
using System.ComponentModel;
using System.Windows.Forms;
using Sparc.KPK2.Resources.Properties;

namespace KPK2.GeneralForms.ReferenceForm
{
  public partial class frmACTypeEdit : Form
  {
    public enum Operation
    {
      Add,
      Edit
    }

    private Operation _operation;
    public string CurrentACStr;

    public frmACTypeEdit(string acStr, string modStr = "")
    {
        InitializeComponent();
        if (String.IsNullOrEmpty(modStr))
            _operation = Operation.Add;
        else
            _operation = Operation.Edit;
        CurrentACStr = acStr;
        CurrentModStr = modStr;
        tbACType.Text = CurrentACStr;
        tbModification.Text = CurrentModStr;
    }

    public Operation CurrentOperation
    {
      get { return _operation; }
    }

    private void frmHelicopterEdit_Load(object sender, EventArgs e)
    {
        tbModification.Focus();
    }

    private bool ValidateForm()
    {
      txtHelicopter_Validating(null, new CancelEventArgs());
      if (errorProvider1.GetError(tbACType) == "")
      {
          CurrentModStr = tbModification.Text;
          return true;
      }
      return false;
    }

    private void txtHelicopter_Validating(object sender, CancelEventArgs e)
    {
      if (String.IsNullOrEmpty(tbModification.Text.Trim()))
        errorProvider1.SetError(tbACType, Resources.EmptyField);
      else
        errorProvider1.SetError(tbACType, "");
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      if (!ValidateForm())
        return;
      DialogResult = DialogResult.OK;
      Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }

    public int CurrentTypeId { get; set; }

    public string CurrentModStr { get; set; }
  }
}