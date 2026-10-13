using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using KPK2.Common;
using Sparc.KPK2.NFunctions;
using Res = Sparc.KPK2.Resources.Properties.Resources;

namespace KPK2.GeneralForms.ServiceForm
{
  public partial class frmSettingsFile : Form
  {
    List<Type> fieldsType = new List<Type>();
    private bool onLoad = true;

    public frmSettingsFile()
    {
      InitializeComponent();
    }

    private void bClose_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void frmSettingsFile_Load(object sender, EventArgs e)
    {
      Assembly ass = Assembly.GetAssembly(typeof(CommonSettings));
      Type tp = GetTypeByStr(ass.GetTypes(), "CommonSettings");
      foreach (FieldInfo fi in tp.GetFields())
      {
        if (!fi.IsNotSerialized)
        {
          dgvSettings.Rows.Add(false, fi.Name, fi.GetValue(CommonSettings.Default));
          fieldsType.Add(fi.FieldType);
        }
      }

      onLoad = false;
    }

    private Type GetTypeByStr(Type[] collection, string name)
    {
      foreach (Type t in collection)
        if (name == t.Name)
          return t;
      return null;
    }

    private int ColValueIndex = 2;
    private int ColNameIndex = 1;
    private int ColSelectIndex = 0;

    private void dgvSettings_CellLeave(object sender, DataGridViewCellEventArgs e)
    {
      
    }

    private object ConvertTo(string value, Type curType)
    {
      string typeName = curType.Name.ToLower();

      if (typeName == "string")
        return value;

      if (typeName == "boolean")
      {
        bool bresult;
        if (!bool.TryParse(value, out bresult))
          return null;
        return bresult;
      }

      if (typeName == "double")
      {
        double dresult = Functions.MoveToDouble(value);
        if (double.IsNaN(dresult))
          return null;
        return dresult;
      }

      throw new Exception("UnknownType!");
    }

    private void dgvSettings_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      
    }

    private void dgvSettings_CurrentCellChanged(object sender, EventArgs e)
    {
      
    }

    private void dgvSettings_SelectionChanged(object sender, EventArgs e)
    {
      
    }

    private void dgvSettings_EditModeChanged(object sender, EventArgs e)
    {
    }

    private void dgvSettings_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
    {
      if (onLoad)
        return;
      try
      {
        if (e.ColumnIndex == ColValueIndex)
        {
          dgvSettings.EndEdit();
          Type curType = fieldsType[e.RowIndex];
          string val = dgvSettings.Rows[e.RowIndex].Cells[ColValueIndex].Value.ToString();
          var value = ConvertTo(val, curType);
          if (value == null)
          {
            MessageBox.Show(string.Format(Res.ErrorInputNValue, val));
            dgvSettings.BeginEdit(false);
            e.Cancel = true;
          }
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.ToString());
      }
    }

    private void bSave_Click(object sender, EventArgs e)
    {
      SaveFileDialog sfd = new SaveFileDialog();
      sfd.ShowDialog();
      string path = sfd.FileName;
      try
      {
        Save(path);
      }
      catch (Exception ex)
      {
        MessageBox.Show(Res.Error + " " + ex);
      }
    }

    private void Save(string filePath)
    {
      XmlDocument xDocument = new XmlDocument();
      XmlNode rootNode = xDocument.CreateNode(XmlNodeType.Element, SettingsSaver.MainSettingsSection, String.Empty);
      xDocument.AppendChild(rootNode);
      foreach (DataGridViewRow row in dgvSettings.Rows)
      {
        if ((bool) row.Cells[ColSelectIndex].Value)
        {
          XmlNode inNode = xDocument.CreateNode(XmlNodeType.Element, row.Cells[ColNameIndex].Value.ToString(),String.Empty);
          XmlNode typeNode = xDocument.CreateNode(XmlNodeType.Attribute, "type", string.Empty);
          XmlNode valueNode = xDocument.CreateNode(XmlNodeType.Attribute, "value", string.Empty);
          typeNode.Value = fieldsType[row.Index].Name;
          if (row.Cells[ColValueIndex].Value == null)
            valueNode.Value = String.Empty;
          else
            valueNode.Value = row.Cells[ColValueIndex].Value.ToString();
          inNode.Attributes.Append((XmlAttribute)typeNode);
          inNode.Attributes.Append((XmlAttribute)valueNode);
          //inNode.Value = row.Cells[ColValueIndex].Value.ToString();
          rootNode.AppendChild(inNode);
        }
      }
      SettingsSaver.Save(xDocument, filePath);
    }
  }
}
