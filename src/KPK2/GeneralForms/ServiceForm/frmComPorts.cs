using System;
using System.Windows.Forms;
using Sparc.KPK2.NFunctions;

namespace KPK2.GeneralForms.ServiceForm
{
  public partial class frmComPorts : Form
  {
    private const int LambdaId = 2;
    private const int KZId = 4;
    private const int SKTId = 5;
    private const int DiscreteInputsId = 6;
    private const int DiscreteOutputsId = 7;

    public frmComPorts()
    {
      InitializeComponent();
    }

    private void bClose_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void bAuto_Click(object sender, EventArgs e)
    {

    }

    private void ComPorts_Load(object sender, EventArgs e)
    {
      dgvPorts.Rows.Add("ИП Lambda", DataManager.Instance.GetPortByModuleId(LambdaId));
      dgvPorts.Rows.Add("КЗ", DataManager.Instance.GetPortByModuleId(KZId));
      dgvPorts.Rows.Add("СКТ", DataManager.Instance.GetPortByModuleId(SKTId));
      dgvPorts.Rows.Add("Дискретные входы", DataManager.Instance.GetPortByModuleId(DiscreteInputsId));
      dgvPorts.Rows.Add("Дискретные выходы", DataManager.Instance.GetPortByModuleId(DiscreteOutputsId));
    }
  }
}