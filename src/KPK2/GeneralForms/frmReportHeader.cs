using System;
using System.IO;
using System.Windows.Forms;
using KPK2.Common;
using KPK2.Properties;

namespace KPK2.GeneralForms
{
  public partial class frmReportHeader : Form
  {

    private const int WM_DBLCLICK = 0x00A3;
    private const int HTCAPTION = 2;

    public frmReportHeader()
    {
      InitializeComponent();
    }

    private void bClose_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void rtbHeader_TextChanged(object sender, EventArgs e)
    {
      bSave.Enabled = true;
    }

    private void bSave_Click(object sender, EventArgs e)
    {
      bSave.Enabled = false;
      CommonSettings.Default.ReportHeader = rtbHeader.Text;
      CommonSettings.Default.RightReportHeader = rtbRightHeader.Text;
      CommonSettings.Default.Save();
    }

    private void frmReportHeader_Load(object sender, EventArgs e)
    {
      cbShowLogo.Enabled = false;
      rtbHeader.Text = CommonSettings.Default.ReportHeader;
      rtbRightHeader.Text = CommonSettings.Default.RightReportHeader;
      cbShowLogo.Checked = CommonSettings.Default.ShowReportLogo;
      cbShowLogo.Enabled = true;
    }

    private void cbShowLogo_CheckedChanged(object sender, EventArgs e)
    {
      if (!cbShowLogo.Enabled)
        return;
      if (((CheckBox) sender).Checked)
      {
        string ourLogo = Globals.PathToLogo;
        if (ofdOpenLogo.ShowDialog() == DialogResult.OK)
        {
          try
          {
            if (ourLogo != ofdOpenLogo.FileName)
            {
              if (File.Exists(ourLogo))
                File.Delete(ourLogo);
              File.Copy(ofdOpenLogo.FileName, ourLogo);
            }
            CommonSettings.Default.ShowReportLogo = true;
          }
          catch (Exception)
          {
            MessageBox.Show(Resources.Error);
            ((CheckBox) sender).Checked = false;
          }
        }
        else
        {
          if (!File.Exists(ourLogo))
            ((CheckBox) sender).Checked = false;
        }
      }
      else
        CommonSettings.Default.ShowReportLogo = false;
      Settings.Default.Save();

    }

    private void frmReportHeader_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (ParentForm != null)
        ((frmMain) ParentForm).EnableMenu(true);
    }

    protected override void WndProc(ref Message m)
    {
      switch (m.Msg)
      {
        case WM_DBLCLICK:
          {
            if (m.WParam.ToInt32() == HTCAPTION)
              return;
          }
          break;
      }
      base.WndProc(ref m);
    }
  }
}