using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KPK2.GeneralForms.AddForms
{
  public partial class frmVSSType : Form
  {
    private int? _vssType = null;

    public frmVSSType()
    {
      InitializeComponent();
    }

    public int? VSSType { get { return _vssType; }  }

    private void bTehn_Click(object sender, EventArgs e)
    {
      _vssType = 0;
      Close();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      _vssType = 1;
      Close();
    }
  }
}
