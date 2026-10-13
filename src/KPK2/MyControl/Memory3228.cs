using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace KPK2.MyControl
{
  public partial class Memory3228 : UserControl
  {
    public Memory3228(bool isOneChannelDiss)
    {
      InitializeComponent();
      lCapture2.Visible = cb6.Visible = lCapture3.Visible = cb7.Visible = !isOneChannelDiss;
    }
  }
}
