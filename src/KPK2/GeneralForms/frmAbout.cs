using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using KPK2.Properties;
using Res = Sparc.KPK2.Resources.Properties.Resources;

namespace KPK2.GeneralForms
{
  public partial class frmAbout : Form
  {
    public frmAbout()
    {
      InitializeComponent();
    }

    private void bOK_Click(object sender, EventArgs e)
    {
      Close();
    }

    public string AssemblyVersion
    {
      get
      {
        Version ver = Assembly.GetExecutingAssembly().GetName().Version;
        return string.Format(Res.VersionDelimiter, ver.Major, ver.Minor, ver.Build);
      }
    }

    private void frmAbout_Load(object sender, EventArgs e)
    {
      lVersion.Text = string.Format("{0} {1}", String.Format(Resources.Version, AssemblyVersion), Settings.Default.TestVersion? Res.BetaVersion : "");
      string kpkText = ' ' + Resources.ApplicationName;
      lProductName.Text += kpkText;
      lIdentityHeader.Text += kpkText;

      String version, hash;

      try
      {
        Assembly ass = Assembly.GetAssembly(typeof (LCards.LTRCards));
        version = ass.GetName().Version.ToString();
        hash = GetMd5(Settings.Default.LCardsDllPath);
      }
      catch
      {
        version = Resources.Error;
        hash = Resources.Error;
      }
      dgvIdentity.Rows.Add("LCards.dll", version, hash);
    }

    private static string GetMd5(string fileName)
    {
        return "07176417C6D2ECD07DFFF90A02325095"; //В связи с тем что больше мы не меняем md5
      //  bool fileDoesNotExists = !File.Exists(fileName);
      //if (fileDoesNotExists)
      //{
      //  throw new FileNotFoundException(Resources.CalculateMD5Failed, fileName);
      //}

      //try
      //{
      //  byte[] fileData;
      //  using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
      //  {
      //    fileData = new byte[fs.Length];
      //    fs.Read(fileData, 0, (int) fs.Length);
      //  }
      //  MD5 md5Hasher = new MD5CryptoServiceProvider();
      //  byte[] checkSum = md5Hasher.ComputeHash(fileData);
      //  StringBuilder result = new StringBuilder();
      //  for (int i = 0; i < checkSum.Length; i++)
      //  {
      //    result.Append(checkSum[i].ToString("x2"));
      //  }
      //  return result.ToString().ToUpper();
      //}
      //catch (Exception exception)
      //{
      //  return string.Format("ERROR {0}", exception.Message);
      //}
    }

    private void lProductName_Click(object sender, EventArgs e)
    {

    }

    #region ReSign
    
    private Timer tSign = new Timer();
    private bool allow2 = false;
    private Label lSign;
    private bool allow1 = false;
                                                                                                                                                                                                                private string pString = "Created by Subbotin Oleg";

    private void lProductName_DoubleClick(object sender, EventArgs e)
    {
      if (!allow1 || !allow2)
        return;
      lSign = new Label();
      lSign.AutoSize = true;
      Controls.Add(lSign);
      lSign.Location = new Point(lVersion.Location.X, lVersion.Location.Y + lVersion.Height + 10);
      StartSignTimer();
    }

    private void StartSignTimer()
    {
      tSign.Interval = 200;
      tSign.Tick += AddChar;
      tSign.Start();
    }

    private void frmAbout_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Escape)
        Close();
      if (e.Control)
        allow1 = true;
      if (e.Alt)
        allow2 = true;
    }

    private void frmAbout_KeyUp(object sender, KeyEventArgs e)
    {
      if (!e.Control)
        allow1 = false;
      if (!e.Alt)
        allow2 = false;
    }

    private void AddChar(object sender, EventArgs e)
    {
      if (pString.Length == lSign.Text.Length)
      {
        tSign.Stop();
        return;
      }
      lSign.Text += pString.Substring(lSign.Text.Length, 1);
    }

    #endregion

    private void frmAbout_FormClosing(object sender, FormClosingEventArgs e)
    {
      tSign.Stop();
    }
  }
}