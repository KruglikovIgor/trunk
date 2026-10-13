using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace KPK2.MyControl
{
  public partial class IndicationPanel : UserControl
  {
    public IndicationPanel()
    {
      InitializeComponent();
    }

    private void IndicationPanel_EnabledChanged(object sender, EventArgs e)
    {
      IndicatorUp.Inactive = IndicatorLeft.Inactive = IndicatorDown.Inactive = !Enabled;
    }
  }
}
