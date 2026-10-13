using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KPK2.Common.DissFunctions;

namespace KPK2.GeneralForms.ReferenceForm
{
  public partial class frmBlock5Type : Form
  {

    public Block5Type CurrentBlock5Type = Block5Type.None;

    public frmBlock5Type()
    {
      InitializeComponent();
    }

    private void bBlock5_Click(object sender, EventArgs e)
    {
      CurrentBlock5Type = Block5Type.Block5;
      Close();
    }

    private void bBlock5I_Click(object sender, EventArgs e)
    {
      CurrentBlock5Type = Block5Type.Block5I;
      Close();
    }
  }
}
