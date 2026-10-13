using System;
using System.Windows.Forms;

namespace KPK2.MyControl
{
  public partial class ctrlTK17 : UserControl
  {
    public event MethodInvoker SaveClick;

    public int Smes1
    {
      get { return Convert.ToInt32(tbSmes1.Text); }
    }

    public int Smes2
    {
      get { return Convert.ToInt32(tbSmes2.Text); }
    }

    public int Geter
    {
      get { return Convert.ToInt32(tbGeter.Text); }
    }

    public int UNChOut
    {
      get { return Convert.ToInt32(tbUNChOut.Text); }
    }

    public int? Clistr
    {
      get { return cbOverload.Checked ? null : (int?) Convert.ToInt32(tbClistr.Text); }
    }

    public bool SaveEnabled
    {
      get { return bSave.Enabled; }
    }

    public ctrlTK17(bool showAll)
    {
      InitializeComponent();
      lGeter.Visible = tbGeter.Visible = showAll;
      lUNChOut.Visible = tbUNChOut.Visible = showAll;
      lClistr.Visible = tbClistr.Visible = showAll;
      cbOverload.Visible = showAll;
    }

    private void bSave_Click(object sender, EventArgs e)
    {
      Button bCurrent = ((Button) sender);
      bCurrent.Enabled = false;
      if (!CheckFields())
      {
        bCurrent.Enabled = true;
        return;
      }

      if (SaveClick != null)
        SaveClick();
      bCurrent.Enabled = true;
    }

    public bool CheckFields()
    {
      bool result = true;
      if (tbSmes1.Visible)
        result &= CheckIntAndSetError(tbSmes1);
      if (tbSmes2.Visible)
        result &= CheckIntAndSetError(tbSmes2);
      if (tbGeter.Visible)
        result &= CheckIntAndSetError(tbGeter);
      if (tbUNChOut.Visible)
        result &= CheckIntAndSetError(tbUNChOut);
      if (tbClistr.Visible)
        if (!cbOverload.Checked)
          result &= CheckIntAndSetError(tbClistr);
      return result;
    }

    private bool CheckIntAndSetError(TextBox tb)
    {
      int res;
      if (!int.TryParse(tb.Text, out res))
      {
        ep.SetError(tb, "Строка не является целым числом!");
        return false;
      }
      return true;
    }

    internal void Clear()
    {
      tbSmes1.Text = "";
      tbSmes2.Text = "";
      bSave.Enabled = true;
      ep.Clear();
    }

    internal void SetFocus()
    {
      tbSmes1.Focus();
    }

    internal void DisableSave()
    {
      if (InvokeRequired)
      {
        Invoke(new MethodInvoker(DisableSave));
        return;
      }
      bSave.Enabled = false;
    }

    internal void SetValues(double smes1, double smes2, double geter, double unch, double clistr)
    {
      tbSmes1.Text = smes1.ToString();
      tbSmes2.Text = smes2.ToString();
      tbGeter.Text = geter.ToString();
      tbUNChOut.Text = unch.ToString();
      if (Math.Abs(clistr + 1) < double.Epsilon)
        cbOverload.Checked = true;
      else
        tbClistr.Text = clistr.ToString();
    }
  }
}
