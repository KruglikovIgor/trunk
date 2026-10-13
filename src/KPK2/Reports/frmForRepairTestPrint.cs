using System;
using System.Data;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Windows.Forms;
using KPK2.Common;
using KPK2.MyClasse;
using KPK2.Properties;
using Sparc.KPK2.NFunctions;
using Sparc.KPK2.NFunctions.Enums;
using Globals = KPK2.Common.Globals;

namespace KPK2.Reports 
{
  /// <summary>
  /// Выводим на печать результаты проверки 
  /// </summary>
  public partial class frmForRepairTestPrint : Form
  {    
    private readonly DataSet _ds;
    public bool TestResult;
    private bool _emptyReport = false;
    private ReportClass _report;
    private bool _JupReport = false;
        private Modification _modif;

    public frmForRepairTestPrint()
    {
      InitializeComponent();
    }

      public frmForRepairTestPrint(ReportClass report):this() 
      {
          _report = report;
          _JupReport = true;
          PrepareJupReport();
      }

        public frmForRepairTestPrint(DataSet dataSet, Modification modif, bool emptyReport = false)
      : this()
    {
      _ds = dataSet;
      _emptyReport = emptyReport;
      _JupReport = false;
            _modif = modif;
    }

    private void frmForRepairTestPrint_FormClosing(object sender, FormClosingEventArgs e)
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

    private void frmForRepairTestPrint_Load(object sender, EventArgs e)
    {
            if(!_JupReport)
                PrepareViewer();
    }

    private void PrepareViewer()
    {
            //isPrepared = true;
            bool mestreNull, mestreBTKNull = false;
      PageView page = (crystalViewer.Controls[0] as PageView);
      Panel panel = new Panel();
      panel.Dock = DockStyle.Top;
      panel.Height = 22;
      if (page != null)
      {
        panel.Width = page.Width;
        page.Controls.Add(panel);
      }
      
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

            mestreNull = _ds.Tables["Header"].Rows[0]["MestreName"] == DBNull.Value;
            mestreBTKNull = _ds.Tables["Header"].Rows[0]["MestreBTKName"] == DBNull.Value;

            ImageDataSet.CreateDataSet();
      if (CommonSettings.Default.ShowReportLogo)
        ImageDataSet.AddImage(Globals.PathToLogo);
      //MainReport1.Subreports[sHeaderReport].SetDataSource(ImageDataSet.DSet);
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

      //MainReport1.SetParameterValue("Configuration", Resources.ReportConfiguration);
      MainReport1.SetParameterValue("AllPages", Resources.ReportAllPages);
      MainReport1.SetParameterValue("Page", Resources.ReportPage);
      MainReport1.SetParameterValue("Right", Resources.ReportOK);
      MainReport1.SetParameterValue("Wrong", Resources.ReportWrong);
            MainReport1.SetParameterValue("WrongSetVC", "не пригодно");
            MainReport1.SetParameterValue("RightSetVC", "пригодно");
            MainReport1.SetParameterValue("EmplyField", "____________");

            MainReport1.SetParameterValue("Note", Resources.ReportCompletedWorksAndNote);
      MainReport1.SetParameterValue("Conclusion", Resources.ReportConclusion);
      MainReport1.SetParameterValue("ParamName", Resources.ReportParamName);
      MainReport1.SetParameterValue("Units", Resources.ReportUnits);
      MainReport1.SetParameterValue("NTP", Resources.ReportNTP);
      MainReport1.SetParameterValue("MeasValue", Resources.ReportMeasValue);
      MainReport1.SetParameterValue("CheckDate", Resources.ReportTestDate);

            if (_emptyReport)
            {
                MainReport1.SetParameterValue("CheckDateValue", String.Empty);
                MainReport1.SetParameterValue("MakingDateValue", string.Empty);
            }
            else
            {
                if (_ds.Tables["Header"].Rows[0]["TestDate"] == DBNull.Value)
                    MainReport1.SetParameterValue("CheckDateValue", String.Empty);
                else
                {
                    DateTime dt = Convert.ToDateTime(_ds.Tables["Header"].Rows[0]["TestDate"]);
                    MainReport1.SetParameterValue("CheckDateValue", dt.ToString("dd.MM.yyyy"));
                }

                if (_ds.Tables["Header"].Rows[0]["MakingDate"] == DBNull.Value)
                    MainReport1.SetParameterValue("MakingDateValue", string.Empty);
                else
                {
                    DateTime dt = Convert.ToDateTime(_ds.Tables["Header"].Rows[0]["MakingDate"]);
                    MainReport1.SetParameterValue("MakingDateValue", dt.ToString("dd.MM.yyyy"));
                }
            }

        MainReport1.SetParameterValue("NotMeasured", Resources.ReportNotMeasured);
      MainReport1.SetParameterValue("ConclusionParameters", Resources.ReportConclusionParameters);
        MainReport1.SetParameterValue("Operator", Resources.ReportOperator);
        MainReport1.SetParameterValue("Controller", Resources.ReportController);
      MainReport1.SetParameterValue("FIO", Resources.ReportFIO);
      MainReport1.SetParameterValue("Subscribe", Resources.ReportSubscribe);
      //MainReport1.SetParameterValue("Header", CommonSettings.Default.ReportHeader, sHeaderReport);
      //MainReport1.SetParameterValue("RightHeader", CommonSettings.Default.RightReportHeader, sHeaderReport);
      //MainReport1.SetParameterValue("ShowLogo", CommonSettings.Default.ShowReportLogo, sHeaderReport);
      MainReport1.SetParameterValue("AutoResult", TestResult);
            MainReport1.SetParameterValue("MestreNull", mestreNull);
            MainReport1.SetParameterValue("MestreBTKNull", mestreBTKNull);

            //ист питания
            //IsOk
            MainReport1.SetParameterValue("27_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[1][4]));
            MainReport1.SetParameterValue("115V_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[2][4]));
            //value
            MainReport1.SetParameterValue("value27V", _ds.Tables["Parameters"].Rows[1][3]);
            MainReport1.SetParameterValue("value115V", _ds.Tables["Parameters"].Rows[2][3]);

            //вторич напряж
            //IsOk
            MainReport1.SetParameterValue("plus12_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[4][4]));
            MainReport1.SetParameterValue("minus12_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[5][4]));
            MainReport1.SetParameterValue("minus20_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[5][4]));
            MainReport1.SetParameterValue("plus6_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[7][4]));
            MainReport1.SetParameterValue("minus15_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[10][4]));
            MainReport1.SetParameterValue("plus15_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[9][4]));
            MainReport1.SetParameterValue("minus6_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[8][4]));
            MainReport1.SetParameterValue("minus27_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[12][4]));
            MainReport1.SetParameterValue("plus5_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[11][4]));
            //values
            MainReport1.SetParameterValue("value_plus12", _ds.Tables["Parameters"].Rows[4][3]);
            MainReport1.SetParameterValue("value_minus12", _ds.Tables["Parameters"].Rows[5][3]);
            MainReport1.SetParameterValue("value_minus20", _ds.Tables["Parameters"].Rows[6][3]);
            MainReport1.SetParameterValue("value_plus6", _ds.Tables["Parameters"].Rows[7][3]);
            MainReport1.SetParameterValue("value_minus6", _ds.Tables["Parameters"].Rows[8][3]);
            MainReport1.SetParameterValue("value_plus15", _ds.Tables["Parameters"].Rows[9][3]);
            MainReport1.SetParameterValue("value_minus15", _ds.Tables["Parameters"].Rows[10][3]);
            MainReport1.SetParameterValue("value_plus5", _ds.Tables["Parameters"].Rows[11][3]);
            MainReport1.SetParameterValue("value_minus27", _ds.Tables["Parameters"].Rows[12][3]);

            //ГЛПд
            //IsOk
            MainReport1.SetParameterValue("glpd1_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[14][4]));
            MainReport1.SetParameterValue("glpd2_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[15][4]));
            //values
            MainReport1.SetParameterValue("value_glpd1", _ds.Tables["Parameters"].Rows[14][3]);
            MainReport1.SetParameterValue("value_glpd2", _ds.Tables["Parameters"].Rows[15][3]);

            //контроль напряжение
            //IsOk
            MainReport1.SetParameterValue("I_GD_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[16][4]));
            //Values
            MainReport1.SetParameterValue("value_I_GD", _ds.Tables["Parameters"].Rows[16][3]);

            //Mod
            //ok
            MainReport1.SetParameterValue("mod1_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[18][4]));
            MainReport1.SetParameterValue("mod2_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[19][4]));
            //Values
            MainReport1.SetParameterValue("value_mod1", _ds.Tables["Parameters"].Rows[18][3]);
            MainReport1.SetParameterValue("value_mod2", _ds.Tables["Parameters"].Rows[19][3]);

            //разность напряжений
            //ok
            MainReport1.SetParameterValue("I_K_1_1_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[21][4]));
            MainReport1.SetParameterValue("I_K_1_2_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[22][4]));
            MainReport1.SetParameterValue("I_K_2_1_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[23][4]));
            MainReport1.SetParameterValue("I_K_2_2_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[24][4]));
            MainReport1.SetParameterValue("I_K_3_1_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[25][4]));
            MainReport1.SetParameterValue("I_K_3_2_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[26][4]));
            //values
            MainReport1.SetParameterValue("value_I_K_1_1", _ds.Tables["Parameters"].Rows[21][3]);
            MainReport1.SetParameterValue("value_I_K_1_2", _ds.Tables["Parameters"].Rows[22][3]);
            MainReport1.SetParameterValue("value_I_K_2_1", _ds.Tables["Parameters"].Rows[23][3]);
            MainReport1.SetParameterValue("value_I_K_2_2", _ds.Tables["Parameters"].Rows[24][3]);
            MainReport1.SetParameterValue("value_I_K_3_1", _ds.Tables["Parameters"].Rows[25][3]);
            MainReport1.SetParameterValue("value_I_K_3_2", _ds.Tables["Parameters"].Rows[26][3]);

            //I K ABS
            //IsOk
            MainReport1.SetParameterValue("I_K_1_ABS_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[28][4]));
            MainReport1.SetParameterValue("I_K_2_ABS_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[29][4]));
            MainReport1.SetParameterValue("I_K_3_ABS_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[30][4]));
            //Values
            MainReport1.SetParameterValue("value_I_K_1_ABS", _ds.Tables["Parameters"].Rows[28][3]);
            MainReport1.SetParameterValue("I_K_2_ABS", _ds.Tables["Parameters"].Rows[29][3]);
            MainReport1.SetParameterValue("I_K_3_ABS", _ds.Tables["Parameters"].Rows[30][3]);

            //sd
            MainReport1.SetParameterValue("SD_I_1_1_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[32][4]));
            MainReport1.SetParameterValue("SD_I_1_2_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[33][4]));
            MainReport1.SetParameterValue("SD_I_2_1_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[34][4]));
            MainReport1.SetParameterValue("SD_I_2_2_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[35][4]));
            MainReport1.SetParameterValue("SD_I_3_1_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[36][4]));
            MainReport1.SetParameterValue("SD_I_3_2_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[37][4]));
            //Values
            MainReport1.SetParameterValue("SD_I_1_1", _ds.Tables["Parameters"].Rows[32][3]);
            MainReport1.SetParameterValue("SD_I_1_2", _ds.Tables["Parameters"].Rows[33][3]);
            MainReport1.SetParameterValue("SD_I_2_1", _ds.Tables["Parameters"].Rows[34][3]);
            MainReport1.SetParameterValue("SD_I_2_2", _ds.Tables["Parameters"].Rows[35][3]);
            MainReport1.SetParameterValue("SD_I_3_1", _ds.Tables["Parameters"].Rows[36][3]);
            MainReport1.SetParameterValue("SD_I_3_2", _ds.Tables["Parameters"].Rows[37][3]);

            //UNCH
            MainReport1.SetParameterValue("UNCH_1_1_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[39][4]));
            MainReport1.SetParameterValue("UNCH_1_2_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[40][4]));
            MainReport1.SetParameterValue("UNCH_2_1_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[41][4]));
            MainReport1.SetParameterValue("UNCH_2_2_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[42][4]));
            MainReport1.SetParameterValue("UNCH_3_1_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[43][4]));
            MainReport1.SetParameterValue("UNCH_3_2_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[44][4]));
            //values
            MainReport1.SetParameterValue("UNCH_1_1", _ds.Tables["Parameters"].Rows[39][3]);
            MainReport1.SetParameterValue("UNCH_1_2", _ds.Tables["Parameters"].Rows[40][3]);
            MainReport1.SetParameterValue("UNCH_2_1", _ds.Tables["Parameters"].Rows[41][3]);
            MainReport1.SetParameterValue("UNCH_2_2", _ds.Tables["Parameters"].Rows[42][3]);
            MainReport1.SetParameterValue("UNCH_3_1", _ds.Tables["Parameters"].Rows[43][3]);
            MainReport1.SetParameterValue("UNCH_3_2", _ds.Tables["Parameters"].Rows[44][3]);

            //10V oporn
            MainReport1.SetParameterValue("10V_opor_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[45][4]));
            MainReport1.SetParameterValue("10Vopor", _ds.Tables["Parameters"].Rows[45][3]);

            //Fd
            //IsOk
            MainReport1.SetParameterValue("Fd1plus_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[47][4]));
            MainReport1.SetParameterValue("Fd2plus_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[48][4]));
            MainReport1.SetParameterValue("Fd3plus_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[49][4]));
            MainReport1.SetParameterValue("Fd1minus_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[50][4]));
            MainReport1.SetParameterValue("Fd2minus_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[51][4]));
            MainReport1.SetParameterValue("Fd3minus_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[52][4]));
            //values
            MainReport1.SetParameterValue("Fd1plus", _ds.Tables["Parameters"].Rows[47][3]);
            MainReport1.SetParameterValue("Fd2plus", _ds.Tables["Parameters"].Rows[48][3]);
            MainReport1.SetParameterValue("Fd3plus", _ds.Tables["Parameters"].Rows[49][3]);
            MainReport1.SetParameterValue("Fd1minus", _ds.Tables["Parameters"].Rows[50][3]);
            MainReport1.SetParameterValue("Fd2minus", _ds.Tables["Parameters"].Rows[51][3]);
            MainReport1.SetParameterValue("Fd3minus", _ds.Tables["Parameters"].Rows[52][3]);

            //VSS zero
            //IsOk
            MainReport1.SetParameterValue("NKV_Wxr_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[55][4]));
            MainReport1.SetParameterValue("NKV_Wzr_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[56][4]));
            MainReport1.SetParameterValue("NKV_Wyr_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[57][4]));
            MainReport1.SetParameterValue("PKV_Wxr_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[59][4]));
            MainReport1.SetParameterValue("PKV_Wzr_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[60][4]));
            MainReport1.SetParameterValue("PKV_Wyr_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[61][4]));
            //values
            MainReport1.SetParameterValue("NKV_Wxr", _ds.Tables["Parameters"].Rows[55][3]);
            MainReport1.SetParameterValue("NKV_Wzr", _ds.Tables["Parameters"].Rows[56][3]);
            MainReport1.SetParameterValue("NKV_Wyr", _ds.Tables["Parameters"].Rows[57][3]);
            MainReport1.SetParameterValue("PKV_Wxr", _ds.Tables["Parameters"].Rows[59][3]);
            MainReport1.SetParameterValue("PKV_Wzr", _ds.Tables["Parameters"].Rows[60][3]);
            MainReport1.SetParameterValue("PKV_Wyr", _ds.Tables["Parameters"].Rows[61][3]);

            //кз1 кз2
            //IsOk
            MainReport1.SetParameterValue("forward_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[63][4]));
            MainReport1.SetParameterValue("left_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[64][4]));
            MainReport1.SetParameterValue("down_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[65][4]));
            MainReport1.SetParameterValue("back_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[66][4]));
            MainReport1.SetParameterValue("right_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[67][4]));
            MainReport1.SetParameterValue("up_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[68][4]));
            //values
            MainReport1.SetParameterValue("forward", _ds.Tables["Parameters"].Rows[63][3]);
            MainReport1.SetParameterValue("left", _ds.Tables["Parameters"].Rows[64][3]);
            MainReport1.SetParameterValue("down", _ds.Tables["Parameters"].Rows[65][3]);
            MainReport1.SetParameterValue("back", _ds.Tables["Parameters"].Rows[66][3]);
            MainReport1.SetParameterValue("right_32", _ds.Tables["Parameters"].Rows[67][3]);
            MainReport1.SetParameterValue("up", _ds.Tables["Parameters"].Rows[68][3]);

            //кз3, 4
            //IsOk
            MainReport1.SetParameterValue("T3_PS1_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[70][4]));
            MainReport1.SetParameterValue("T4_PS2_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[71][4]));
            //values
            MainReport1.SetParameterValue("T3_PS1", _ds.Tables["Parameters"].Rows[70][3]);
            MainReport1.SetParameterValue("T4_PS2", _ds.Tables["Parameters"].Rows[71][3]);

            //кз3, 4 ручной ввод
            //IsOk
            MainReport1.SetParameterValue("T3_PS1_USP_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[73][4]));
            MainReport1.SetParameterValue("T4_PS2_USP_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[74][4]));
            //values
            MainReport1.SetParameterValue("T3_PS1_USP", _ds.Tables["Parameters"].Rows[73][3]);
            MainReport1.SetParameterValue("T4_PS2_USP", _ds.Tables["Parameters"].Rows[74][3]);

            //угол сноса
            //IsOk
            MainReport1.SetParameterValue("US_0_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[76][4]));
            MainReport1.SetParameterValue("US_9_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[77][4]));
            //values
            MainReport1.SetParameterValue("US_0", _ds.Tables["Parameters"].Rows[76][3]);
            MainReport1.SetParameterValue("US_9", _ds.Tables["Parameters"].Rows[77][3]);

            //угол сноса ручной ввод
            //IsOk
            MainReport1.SetParameterValue("US_0_UPS_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[79][4]));
            MainReport1.SetParameterValue("US_9_USP_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[80][4]));
            //values
            MainReport1.SetParameterValue("US_0_USP", _ds.Tables["Parameters"].Rows[79][3]);
            MainReport1.SetParameterValue("US_9_USP", _ds.Tables["Parameters"].Rows[80][3]);

            //разность пс и ус
            //IsOk
            MainReport1.SetParameterValue("delta_PS_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[82][4]));
            MainReport1.SetParameterValue("delta_US_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[83][4]));
            //Values
            MainReport1.SetParameterValue("delta_PS", _ds.Tables["Parameters"].Rows[82][3]);
            MainReport1.SetParameterValue("delta_US", _ds.Tables["Parameters"].Rows[83][3]);

            //режим М-С и М-Б
            //IsOk
            MainReport1.SetParameterValue("M_S_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[85][4]));
            MainReport1.SetParameterValue("M_B_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[86][4]));
            //values
            MainReport1.SetParameterValue("M_S", _ds.Tables["Parameters"].Rows[85][3]);
            MainReport1.SetParameterValue("M_B", _ds.Tables["Parameters"].Rows[86][3]);

            //БПК
            //IsOk
            MainReport1.SetParameterValue("forward_USP_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[88][4]));
            MainReport1.SetParameterValue("left_usp_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[89][4]));
            MainReport1.SetParameterValue("down_usp_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[90][4]));
            MainReport1.SetParameterValue("back_usp_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[91][4]));
            MainReport1.SetParameterValue("right_usp_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[92][4]));
            MainReport1.SetParameterValue("up_usp_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[93][4]));
            MainReport1.SetParameterValue("BPK_US1_USP_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[95][4]));
            MainReport1.SetParameterValue("BPK_PS_USP_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[96][4]));
            MainReport1.SetParameterValue("BPK_PS1_USP2_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[98][4]));
            MainReport1.SetParameterValue("BPK_US2_USP2_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[99][4]));
            MainReport1.SetParameterValue("BPK_US_9_USP_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[101][4]));
            MainReport1.SetParameterValue("BPK_US_0_USP_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[100][4]));
            //values
            MainReport1.SetParameterValue("forward_USP", _ds.Tables["Parameters"].Rows[88][3]);
            MainReport1.SetParameterValue("left_usp", _ds.Tables["Parameters"].Rows[89][3]);
            MainReport1.SetParameterValue("down_usp", _ds.Tables["Parameters"].Rows[90][3]);
            MainReport1.SetParameterValue("back_usp", _ds.Tables["Parameters"].Rows[91][3]);
            MainReport1.SetParameterValue("right_usp", _ds.Tables["Parameters"].Rows[92][3]);
            MainReport1.SetParameterValue("up_usp", _ds.Tables["Parameters"].Rows[93][3]);
            MainReport1.SetParameterValue("BPK_US1_USP", _ds.Tables["Parameters"].Rows[95][3]);
            MainReport1.SetParameterValue("BPK_PS_USP", _ds.Tables["Parameters"].Rows[96][3]);
            MainReport1.SetParameterValue("BPK_PS1_USP2", _ds.Tables["Parameters"].Rows[98][3]);
            MainReport1.SetParameterValue("BPK_US2_USP2", _ds.Tables["Parameters"].Rows[99][3]);
            MainReport1.SetParameterValue("BPK_US_9_USP", _ds.Tables["Parameters"].Rows[101][3]);
            MainReport1.SetParameterValue("BPK_US_0_USP", _ds.Tables["Parameters"].Rows[100][3]);

            //свитова сигна
            //IsOk
            MainReport1.SetParameterValue("navigation_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[103][4]));
            MainReport1.SetParameterValue("controlDiss_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[104][4]));
            MainReport1.SetParameterValue("Memory1_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[106][4]));
            MainReport1.SetParameterValue("Memory2_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[107][4]));
            MainReport1.SetParameterValue("Search_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[108][4]));
            MainReport1.SetParameterValue("Serviceability_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[109][4]));
            MainReport1.SetParameterValue("Fly_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[110][4]));
            MainReport1.SetParameterValue("refusal_VCH_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[111][4]));
            MainReport1.SetParameterValue("refusal_VSS_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[112][4]));
            //Values
            MainReport1.SetParameterValue("navigation", _ds.Tables["Parameters"].Rows[103][3]);
            MainReport1.SetParameterValue("controlDiss", _ds.Tables["Parameters"].Rows[104][3]);
            MainReport1.SetParameterValue("Memory1", _ds.Tables["Parameters"].Rows[106][3]);
            MainReport1.SetParameterValue("Memory2", _ds.Tables["Parameters"].Rows[107][3]);
            MainReport1.SetParameterValue("Search", _ds.Tables["Parameters"].Rows[108][3]);
            MainReport1.SetParameterValue("Serviceability", _ds.Tables["Parameters"].Rows[109][3]);
            MainReport1.SetParameterValue("Fly", _ds.Tables["Parameters"].Rows[110][3]);
            MainReport1.SetParameterValue("refusal_VCH", _ds.Tables["Parameters"].Rows[111][3]);
            MainReport1.SetParameterValue("refusal_VSS", _ds.Tables["Parameters"].Rows[112][3]);

            //переход из режима памть в захват
            //IsOk
            MainReport1.SetParameterValue("timeMemory_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[113][4]));
            //values
            MainReport1.SetParameterValue("timeMemory", _ds.Tables["Parameters"].Rows[113][3]);

            MainReport1.SetParameterValue("Memory_Noise_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[114][4]));
            MainReport1.SetParameterValue("Memory_Noise", _ds.Tables["Parameters"].Rows[114][3]);

            MainReport1.SetParameterValue("TimeReady_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[115][4]));
            MainReport1.SetParameterValue("TimeReady", _ds.Tables["Parameters"].Rows[115][3]);

            MainReport1.SetParameterValue("Freq_SVCH_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[116][4]));
            MainReport1.SetParameterValue("Freq_SVCH", _ds.Tables["Parameters"].Rows[116][3]);

            MainReport1.SetParameterValue("pow_SVCH_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[117][4]));
            MainReport1.SetParameterValue("pow_SVCH", _ds.Tables["Parameters"].Rows[117][3]);

            //resist isolation
            //IsOk
            MainReport1.SetParameterValue("VCH_H34_38_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[119][4]));
            MainReport1.SetParameterValue("VCH_H34_39_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[120][4]));
            MainReport1.SetParameterValue("VCH_H35_1_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[121][4]));
            MainReport1.SetParameterValue("VCH_H35_4_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[122][4]));
            MainReport1.SetParameterValue("VSS_H4_38_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[123][4]));
            MainReport1.SetParameterValue("VSS_H4_39_IsOk", !Convert.ToBoolean(_ds.Tables["Parameters"].Rows[124][4]));
            //values
            MainReport1.SetParameterValue("VCH_H34_38", _ds.Tables["Parameters"].Rows[119][3]);
            MainReport1.SetParameterValue("VCH_H34_39", _ds.Tables["Parameters"].Rows[120][3]);
            MainReport1.SetParameterValue("VCH_H35_1", _ds.Tables["Parameters"].Rows[121][3]);
            MainReport1.SetParameterValue("VCH_H35_4", _ds.Tables["Parameters"].Rows[122][3]);
            MainReport1.SetParameterValue("VSS_H4_38", _ds.Tables["Parameters"].Rows[123][3]);
            MainReport1.SetParameterValue("VSS_H4_39", _ds.Tables["Parameters"].Rows[124][3]);

            if(_modif == Modification.Diss32_90A)
            {
                MainReport1.SetParameterValue("BVK_H2_41", _ds.Tables["Parameters"].Rows[125][3]);
                MainReport1.SetParameterValue("BVK_H2_40", _ds.Tables["Parameters"].Rows[126][3]);
                MainReport1.SetParameterValue("8_H2_49", _ds.Tables["Parameters"].Rows[127][3]);
                MainReport1.SetParameterValue("8_H2_50", _ds.Tables["Parameters"].Rows[128][3]);

            }


            crystalViewer.Refresh();
      crystalViewer.ReportSource = MainReport1;
      crystalViewer.Zoom(100);
    }

      private void PrepareJupReport()
      {
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