using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Windows.Forms;
using KPK2.Common;
using Sparc.KPK2.NFunctions;
using Res = Sparc.KPK2.Resources.Properties.Resources;

namespace KPK2.GeneralForms.ServiceForm
{
  public partial class frmCalibrateAttenuations : Form
  {
    Dictionary<string, double> _baseValues = new Dictionary<string, double>();
    private List<string> _changeList = new List<string>();

    public frmCalibrateAttenuations()
    {
      InitializeComponent();
      SetValue(tbKVPDiss13, CommonSettings.Default.KVPDiss13DbmKoef);
      SetValue(tbCabelDiss13, CommonSettings.Default.CabelDbmKoef);
      SetValue(tbAdapterDiss13, CommonSettings.Default.AdapterDbmKoef);

      SetValue(tbVP, CommonSettings.Default.VPDbmKoef);
      SetValue(tbCabelDiss15, CommonSettings.Default.CabelDiss15DbmKoef);
      SetValue(tbAdapterDiss15, CommonSettings.Default.AttenDbmKoef);
      SetValue(tbKVPDiss15, CommonSettings.Default.KVPDiss15DbmKoef);
    }

    private void SetValue(TextBox tb, double value)
    {
      AddValueToDictionary(tb, value);
      tb.Text = value.ToString();
    }

    private void AddValueToDictionary(TextBox tb, double value)
    {
      if (!_baseValues.ContainsKey(tb.Name))
        _baseValues.Add(tb.Name, value);
      else
        _baseValues[tb.Name] = value;
    }

    private void bClose_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void tb_TextChanged(object sender, EventArgs e)
    {
      TextBox tbCurrent = (TextBox) sender;
      CheckValue(tbCurrent);
    }

    /// <summary>
    /// Проверить значение на изменение
    /// </summary>
    /// <param name="tbCurrent"></param>
    private void CheckValue(TextBox tbCurrent)
    {
      double result = Functions.MoveToDouble(tbCurrent.Text);
      double original = double.NaN;
      if (_baseValues.ContainsKey(tbCurrent.Name))
        original = _baseValues[tbCurrent.Name];
      if (result != original)
        ValueChanged(tbCurrent.Name);
      else
        RemoveValueChanged(tbCurrent.Name);
    }

    /// <summary>
    /// Убрать изменение по значению
    /// </summary>
    /// <param name="name"></param>
    private void RemoveValueChanged(string name)
    {
      if (_changeList.Contains(name))
        _changeList.Remove(name);
      bSave.Enabled = _changeList.Count > 0;
    }

    /// <summary>
    /// Добавить изменение по значению
    /// </summary>
    /// <param name="name"></param>
    private void ValueChanged(string name)
    {
      if (!_changeList.Contains(name))
        _changeList.Add(name);
      bSave.Enabled = true;
    }

    private void bSave_Click(object sender, EventArgs e)
    {
      //foreach (string name in _changeList)
      //{
      //  ChangeValue(name);
      //}

      try
      {
        CommonSettings.Default.Save();
        _changeList.Clear();
        bSave.Enabled = false;
      }
      catch
      {
        MessageBox.Show(Res.Error);
      }
    }

    private bool ChangeValue(string name)
    {
      double result;
      if (name == tbKVPDiss13.Name)
      {
        result = Functions.MoveToDouble(tbKVPDiss13.Text);
        if (double.IsNaN(result))
          return false;
        CommonSettings.Default.KVPDiss13DbmKoef = result;
      }

      if (name == tbAdapterDiss13.Name)
      {
        result = Functions.MoveToDouble(tbAdapterDiss13.Text);
        if (double.IsNaN(result))
          return false;
        CommonSettings.Default.AdapterDbmKoef = result;
      }

      if (name == tbCabelDiss13.Name)
      {
        result = Functions.MoveToDouble(tbCabelDiss13.Text);
        if (double.IsNaN(result))
          return false;
        CommonSettings.Default.CabelDbmKoef = result;
      }

      if (name == tbKVPDiss15.Name)
      {
        result = Functions.MoveToDouble(tbKVPDiss15.Text);
        if (double.IsNaN(result))
          return false;
        CommonSettings.Default.KVPDiss15DbmKoef = result;
      }

      if (name == tbVP.Name)
      {
        result = Functions.MoveToDouble(tbVP.Text);
        if (double.IsNaN(result))
          return false;
        CommonSettings.Default.VPDbmKoef = result;
      }

      if (name == tbAdapterDiss15.Name)
      {
        result = Functions.MoveToDouble(tbAdapterDiss15.Text);
        if (double.IsNaN(result))
          return false;
        CommonSettings.Default.AttenDbmKoef = result;
      }

      if (name == tbCabelDiss15.Name)
      {
        result = Functions.MoveToDouble(tbCabelDiss15.Text);
        if (double.IsNaN(result))
          return false;
        CommonSettings.Default.CabelDiss15DbmKoef = result;
      }
      return true;
    }

    private void tb_Leave(object sender, EventArgs e)
    {
      TextBox tbCurrent = (TextBox) sender;
      if (!ChangeValue(tbCurrent.Name))
      {
        MessageBox.Show(string.Format(Res.ErrorInputNValue, tbCurrent.Text));
        tbCurrent.Focus();
      }
    }
  }
}
