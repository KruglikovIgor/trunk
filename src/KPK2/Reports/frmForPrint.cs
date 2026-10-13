using System;
using System.Data;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Windows.Forms;
using KPK2.Common;
using KPK2.MyClasse;
using KPK2.Properties;
using Sparc.KPK2.NFunctions;
using Globals = KPK2.Common.Globals;

namespace KPK2.Reports 
{
  /// <summary>
  /// Выводим на печать результаты проверки 
  /// </summary>
  public partial class frmForPrint : Form
  {    
    private readonly DataSet _ds;
    public bool TestResult;
    private bool _emptyReport = false;
    private ReportClass _report;
    private bool _JupReport = false;
    

    public frmForPrint()
    {
      InitializeComponent();
    }

      public frmForPrint(ReportClass report, int testId):this() 
      {
          _report = report;
          _JupReport = true;
          PrepareJupReport(testId);
      }

        public frmForPrint(DataSet dataSet, bool emptyReport = false)
      : this()
    {
      _ds = dataSet;
      _emptyReport = emptyReport;
      _JupReport = false;
    }

    private void frmForPrint_FormClosing(object sender, FormClosingEventArgs e)
    {
      if(!_JupReport)
        _ds.Tables.Clear();
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
      if (keyData == Keys.Escape)
        Close();
      return base.ProcessCmdKey(ref msg, keyData);
    }

    public void InitReportViewer()
    {
      crystalViewer.InitReportViewer();
    }

    private void frmForPrint_Load(object sender, EventArgs e)
    {
            if(!_JupReport)
                PrepareViewer();
    }

    private void PrepareViewer()
    {
      //isPrepared = true;
      PageView page = (crystalViewer.Controls[0] as PageView);
      Panel panel = new Panel();
      panel.Dock = DockStyle.Top;
      panel.Height = 22;
      if (page != null)
      {
        panel.Width = page.Width;
        page.Controls.Add(panel);
      }

      const string sHeaderReport = "HeaderReport";
        if (_emptyReport)
        {
            DataSet newDs = _ds.Copy();
            newDs.Tables["Header"].Rows[0]["EmployeeName"] = "";
            newDs.Tables["Header"].Rows[0]["ProductNumber"] = "";
            for (int i = 0; i < newDs.Tables["Parameters"].Rows.Count; i++)
                newDs.Tables["Parameters"].Rows[i]["Value"] = "";
            MainReport1.SetDataSource(newDs);
        }
        else
            MainReport1.SetDataSource(_ds);
        ImageDataSet.CreateDataSet();
      if (CommonSettings.Default.ShowReportLogo)
        ImageDataSet.AddImage(Globals.PathToLogo);
      MainReport1.Subreports[sHeaderReport].SetDataSource(ImageDataSet.DSet);
      //Загружаем локализованные параметры
      MainReport1.SetParameterValue("BaseLineCheck", Resources.ReportBaseLineCheck);
      MainReport1.SetParameterValue("TypeCheck", Resources.ReportTypeCheck);
      MainReport1.SetParameterValue("TypeAircraft", Resources.ReportTypeAircraft);
      MainReport1.SetParameterValue("AircraftRegNo", Resources.ReportAircraftRegNo);
      MainReport1.SetParameterValue("AircraftSerNo", Resources.ReportAircraftSerNo);
      MainReport1.SetParameterValue("Equipment", Resources.ReportEquipment);
      MainReport1.SetParameterValue("ItemSerNo", Resources.ReportItemSerNo);
      MainReport1.SetParameterValue("TSN", Resources.ReportTSN);
      MainReport1.SetParameterValue("TBO", Resources.ReportTBO);

      MainReport1.SetParameterValue("Configuration", Resources.ReportConfiguration);
      MainReport1.SetParameterValue("AllPages", Resources.ReportAllPages);
      MainReport1.SetParameterValue("Page", Resources.ReportPage);
      MainReport1.SetParameterValue("Right", Resources.ReportOK);
      MainReport1.SetParameterValue("Wrong", Resources.ReportWrong);

      MainReport1.SetParameterValue("Note", Resources.ReportCompletedWorksAndNote);
      MainReport1.SetParameterValue("Conclusion", Resources.ReportConclusion);
      MainReport1.SetParameterValue("ParamName", Resources.ReportParamName);
      MainReport1.SetParameterValue("Units", Resources.ReportUnits);
      MainReport1.SetParameterValue("NTP", Resources.ReportNTP);
      MainReport1.SetParameterValue("MeasValue", Resources.ReportMeasValue);
      MainReport1.SetParameterValue("CheckDate", Resources.ReportTestDate);

        if (_emptyReport)
            MainReport1.SetParameterValue("CheckDateValue", String.Empty);
        else
        {
            if (_ds.Tables["Header"].Rows[0]["TestDate"] == DBNull.Value)
                MainReport1.SetParameterValue("CheckDateValue", String.Empty);
            else
            {
                DateTime dt = Convert.ToDateTime(_ds.Tables["Header"].Rows[0]["TestDate"]);
                MainReport1.SetParameterValue("CheckDateValue", dt.ToLongDateString());
            }
        }

        MainReport1.SetParameterValue("NotMeasured", Resources.ReportNotMeasured);
      MainReport1.SetParameterValue("ConclusionParameters", Resources.ReportConclusionParameters);
        MainReport1.SetParameterValue("Operator", Resources.ReportOperator);
        MainReport1.SetParameterValue("Controller", Resources.ReportController);
      MainReport1.SetParameterValue("FIO", Resources.ReportFIO);
      MainReport1.SetParameterValue("Subscribe", Resources.ReportSubscribe);
      MainReport1.SetParameterValue("Header", CommonSettings.Default.ReportHeader, sHeaderReport);
      MainReport1.SetParameterValue("RightHeader", CommonSettings.Default.RightReportHeader, sHeaderReport);
      MainReport1.SetParameterValue("ShowLogo", CommonSettings.Default.ShowReportLogo, sHeaderReport);
      MainReport1.SetParameterValue("AutoResult", TestResult);

      crystalViewer.Refresh();
      crystalViewer.ReportSource = MainReport1;
      crystalViewer.Zoom(100);
    }

      private void PrepareJupReport(int testId)
      {

	      dsReport ds = new dsReport();
	      DataManager.Instance.FillTesting(testId, 0, ds.Tables["Header"]);

	      try
	      {
		      _report.SetParameterValue("TOForm", ds.Tables["Header"].Rows[0]["FormTO"]);
	      }
	      catch
	      {
		      _report.SetParameterValue("TOForm", "");
	      }

	      try
	      {
		      _report.SetParameterValue("WorkingTime", ds.Tables["Header"].Rows[0]["TotalRun"]);
	      }
	      catch
	      {
		      _report.SetParameterValue("WorkingTime", "");
	      }


			PageView page = (crystalViewer.Controls[0] as PageView);
          Panel panel = new Panel();
          panel.Dock = DockStyle.Top;
          panel.Height = 22;
          if (page != null)
          {
              panel.Width = page.Width;
              page.Controls.Add(panel);
          }

          crystalViewer.Refresh();
          crystalViewer.ReportSource = _report;
          crystalViewer.Zoom(100);
      }
  }
}