using System.Drawing;
using System.Windows.Forms;
using ZedGraph;
using Res = Sparc.KPK2.Resources.Properties.Resources;

namespace KPK2.MyControl
{
  public partial class GraphControl : UserControl
  {
    PointPairList _currentPairList = new PointPairList();

    public GraphControl()
    {
      InitializeComponent();
    }

    public void RenewValues(PointPairList list)
    {
      _currentPairList = list;
      zgcMainGraph.GraphPane.CurveList.Clear();
      zgcMainGraph.GraphPane.AddCurve(string.Empty, _currentPairList, Color.DarkBlue, SymbolType.None);
      zgcMainGraph.AxisChange();
      zgcMainGraph.Invalidate();
    }

    public void SpectrumGraphPrepare()
    {
      //FontSpec fs = new FontSpec(Font.FontFamily.ToString(), Font.Size, ForeColor, Font.Bold, Font.Italic, Font.Underline);
      GraphPane gp = zgcMainGraph.GraphPane;
      gp.Title.Text = Res.SpectrumGraphTitle;
      //gp.Title.FontSpec = fs;
      gp.XAxis.Title.Text = string.Format(Res.FreqM, Res.GHz);
      //gp.XAxis.Title.FontSpec = fs;
      gp.YAxis.Title.Text = string.Format(Res.LevelN, Res.Dbm);
      //gp.YAxis.Title.FontSpec = fs;
      gp.XAxis.Scale.MaxAuto = true;
      gp.XAxis.Scale.MinAuto = true;
      gp.XAxis.Scale.MajorStepAuto = true;
      gp.XAxis.Scale.MinorStepAuto = true;
      gp.YAxis.Scale.MaxAuto = true;
      gp.YAxis.Scale.MinAuto = true;
      gp.YAxis.Scale.MajorStepAuto = true;
      gp.YAxis.Scale.MinorStepAuto = true;
    }
  }
}
