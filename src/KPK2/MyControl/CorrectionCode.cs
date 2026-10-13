using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace KPK2.MyControl
{
  public partial class CorrectionCode : UserControl
  {
    public CorrectionCode()
    {
      InitializeComponent();
    }

    private void mtbCode1_Validating(object sender, CancelEventArgs e)
    {
      nudCode1.Value = GetValueFromBitMask(mtbCode1.Text);
    }

    private decimal GetValueFromBitMask(string bitMask)
    {
      int value = 0;
      for (int i = 0; i < bitMask.Length; i++)
      {
        value += bitMask.Substring(bitMask.Length - i - 1, 1) == "1" ? (int)Math.Pow(2, i) : 0;
      }
      return value;
    }

    private void mtbCode2_Validating(object sender, CancelEventArgs e)
    {
      nudCode2.Value = GetValueFromBitMask(mtbCode2.Text);
    }

    private void mtbCode3_Validating(object sender, CancelEventArgs e)
    {
      nudCode3.Value = GetValueFromBitMask(mtbCode3.Text);
    }

    private void nudCode1_ValueChanged(object sender, EventArgs e)
    {
      mtbCode1.Text = FromValueToBitMask((int)nudCode1.Value);
    }

    const int maxCodeLength = 4;

    private string FromValueToBitMask(int value)
    {
      string text = "";
      for (int i = 0; i < maxCodeLength; i++)
        text = ((value & (int) Math.Pow(2, i)) == (int) Math.Pow(2, i) ? "1" : "0") + text;
      return text;
    }

    private void nudCode2_ValueChanged(object sender, EventArgs e)
    {
      mtbCode2.Text = FromValueToBitMask((int)nudCode2.Value);
    }

    private void nudCode3_ValueChanged(object sender, EventArgs e)
    {
      mtbCode3.Text = FromValueToBitMask((int) nudCode3.Value);
    }
  }
}
