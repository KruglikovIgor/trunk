using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using KPK2.Common;
using KPK2.GeneralForms.ReferenceForm;
using KPK2.Properties;
using Sparc.KPK2.NFunctions;
using Sparc.KPK2.NFunctions.Enums;
using Globals = KPK2.Common.Globals;

namespace KPK2.GeneralForms.ServiceForm
{
    public partial class frmSettings : Form
    {
        private readonly Color _changedColor = Globals.FailColor;
        private readonly Color _standardBackColor = Color.White;
        private readonly Color _standardForeColor = Color.Black;

        private readonly Dictionary<Control, string> linkSettings = new Dictionary<Control, string>();
        private readonly Dictionary<string, List<string>> linkACTypesToDevices = new Dictionary<string, List<string>>();
        private readonly List<string> changedList = new List<string>();

        private int iStatusChangeFlag;

        public frmSettings()
        {
            InitializeComponent();
            iStatusChangeFlag = 0;
        }

        private void bSave_Click(object sender, System.EventArgs e)
        {
            Enabled = false;
            SaveMainSettings();
            bSave.Enabled = false;
            Enabled = true;
            iStatusChangeFlag++;
            //MessageBox.Show($"Измененные настройки начнут действовать только после перезагрузки ПО КПК-2.","ВНИМАНИЕ!");
        }

        private void SaveMainSettings()
        {
            try
            {
                if (changedList.Contains(nudAddFlyDirection.Name))
                {
                    CommonSettings.Default.AddFlyDirectionDiss32 = (double)nudAddFlyDirection.Value;
                    RemoveChangeSimple(nudAddFlyDirection.Name);
                }

                if (changedList.Contains(nudAddAngleKZ1.Name))
                {
                    CommonSettings.Default.AddAngleKZ1Diss32 = (double)nudAddAngleKZ1.Value;
                    RemoveChangeSimple(nudAddAngleKZ1.Name);
                }

                foreach (KeyValuePair<Control, string> pair in linkSettings)
                    if (changedList.Contains(pair.Key.Name))
                    {
                        RemoveChangeSimple(pair.Key.Name);
                        if (pair.Key is CheckBox)
                        {
                            CheckBox cb = ((CheckBox)pair.Key);
                            SetSettingValue(pair.Value, cb.Checked);
                            cb.ForeColor = _standardForeColor;
                        }
                    }
                Settings.Default.Save();
                SaveEnableChecks();
                CommonSettings.Default.Save();
            }
            catch
            {
                MessageBox.Show(Resources.Error);
            }
        }

        Dictionary<string, DeviceAndMod> linkToEnableChecks = new Dictionary<string, DeviceAndMod>();

        private void SaveEnableChecks()
        {
            foreach (var item in clbChecks.Items)
                CommonSettings.Default.SetCheckEnable(linkToEnableChecks[item.ToString()], clbChecks.CheckedItems.Contains(item));
        }

        private void RemoveChange(string ctrlName)
        {
            RemoveChangeSimple(ctrlName);
            bSave.Enabled = changedList.Count > 0;
        }

        private void RemoveChange(Control ctrl)
        {
            if (ctrl is TextBox)
                ctrl.BackColor = _standardBackColor;
            else
                ctrl.ForeColor = _standardForeColor;
            RemoveChange(ctrl.Name);
        }

        private void RemoveChangeSimple(string ctrlName)
        {
            if (changedList.Contains(ctrlName))
                changedList.Remove(ctrlName);
        }

        private void AddChange(string ctrlName)
        {
            changedList.Add(ctrlName);
            bSave.Enabled = true;
        }

        private void MakeLinks()
        {
            linkSettings.Add(cbFirstRun, "FirstRun");
            linkSettings.Add(cbIsSilkway, "IsSilkWay");
            linkSettings.Add(cbPoverkaEnabled, "PoverkaEnabled");
            linkSettings.Add(cbShowDiss32, "ShowDiss32");
            linkSettings.Add(cbSelfTest, "SelfTest");
            linkSettings.Add(cbNSKS, "IsNSK");
            linkSettings.Add(cbSpectrumAnalyzer, "SpectrumAnalyzerActive");
            linkSettings.Add(cbJUPEnable, "JUPEnable");
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            FillChecks();
            FillACTypes();
            FillACTypesLink();
            MakeLinks();
            LoadSettings();
        }

        private void FillACTypesLink()
        {
            var diss15Caption = Sparc.KPK2.NFunctions.Globals.GetDissCaption(DevicesID.Diss15, Modification.None);
            var diss15GCaption = Sparc.KPK2.NFunctions.Globals.GetDissCaption(DevicesID.Diss15, Modification.Diss15G);
            var diss32Caption = Sparc.KPK2.NFunctions.Globals.GetDissCaption(DevicesID.Diss32, Modification.None);
            var diss32_90Caption = Sparc.KPK2.NFunctions.Globals.GetDissCaption(DevicesID.Diss32_90, Modification.None);
            var diss32_90ACaption = Sparc.KPK2.NFunctions.Globals.GetDissCaption(DevicesID.Diss32_90, Modification.Diss32_90A);
            var diss32_28Caption = Sparc.KPK2.NFunctions.Globals.GetDissCaption(DevicesID.Diss3228, Modification.None);
            var diss32_28MCaption = Sparc.KPK2.NFunctions.Globals.GetDissCaption(DevicesID.Diss3228_Monoblock, Modification.None);
            var diss013Caption = Sparc.KPK2.NFunctions.Globals.GetDissCaption(DevicesID.Diss13, Modification.None);
            linkACTypesToDevices.Add("Ми-8", new List<string> { diss15Caption, diss15GCaption, diss32_90ACaption });
            linkACTypesToDevices.Add("Ми-17", new List<string> { diss15Caption, diss15GCaption, diss32_90ACaption });
            linkACTypesToDevices.Add("Ил-76", new List<string> { diss013Caption });
            linkACTypesToDevices.Add("Ми-24", new List<string> { diss15Caption, diss15GCaption, diss32_28Caption, diss32_28MCaption });
            linkACTypesToDevices.Add("Ми-28Н", new List<string> { diss32_28Caption, diss32_28MCaption });
            linkACTypesToDevices.Add("Ми-26", new List<string> { diss32_90Caption });
            linkACTypesToDevices.Add("Ка-27", new List<string> { diss32_28Caption, diss32_28MCaption, diss32Caption });
            linkACTypesToDevices.Add("Ка-28", new List<string> { diss32_28Caption, diss32_28MCaption, diss32Caption });
            linkACTypesToDevices.Add("Ка-29", new List<string> { diss32_28Caption, diss32_28MCaption, diss32Caption });
            linkACTypesToDevices.Add("Ка-31Р", new List<string> { diss32_28Caption, diss32_28MCaption, diss32Caption });
            linkACTypesToDevices.Add("Ка-52", new List<string> { diss32_28Caption, diss32_28MCaption, diss32Caption });
            linkACTypesToDevices.Add("Ка-226", new List<string>());
            linkACTypesToDevices.Add("Ми-35М", new List<string> { diss15Caption, diss15GCaption });
        }

        private void FillACTypes()
        {
            var acTypes = DataManager.Instance.GetACTypes();
            foreach (DataRow item in acTypes.Rows)
            {
                if (Convert.ToInt32(item[0]) < frmACType.MinModTypeId)
                    clbACTypes.Items.Add(item[1]);
            }
        }

        private void FillChecks()
        {
            clbChecks.Enabled = false;
            List<string> dissChecks = new List<string>();
            Dictionary<string, bool> enChecks = new Dictionary<string, bool>();

            // При добавлении пропускаются дублирующиеся ключи, запоминается только первый встреченный DeviceAndMod для каждого уникального названия. 
            foreach (var item in CommonSettings.Default.EnableChecks)
            {
                var caption = Sparc.KPK2.NFunctions.Globals.GetDissCaption(item.DeviceId, item.Modification);
                if (!linkToEnableChecks.ContainsKey(caption)) // проверка
                {
                    linkToEnableChecks.Add(caption, item);
                    dissChecks.Add(caption);
                    enChecks.Add(caption, item.CheckState);
                }
            }

            dissChecks.Sort();
            foreach (var item in dissChecks)
                clbChecks.Items.Add(item, enChecks[item] ? CheckState.Checked : CheckState.Unchecked);
            clbChecks.Enabled = true;
        }

        private void LoadSettings()
        {
            try
            {
                nudAddFlyDirection.Value = (ushort)CommonSettings.Default.AddFlyDirectionDiss32;
                nudAddAngleKZ1.Value = (ushort)CommonSettings.Default.AddAngleKZ1Diss32;
                foreach (KeyValuePair<Control, string> pair in linkSettings)
                {
                    if (SetSettingsControlValue(pair))
                        pair.Key.Enabled = true;
                }
            }
            catch
            {
                MessageBox.Show(Resources.LoadSettingsError, Resources.Error);
            }
        }

        private bool SetSettingsControlValue(KeyValuePair<Control, string> pair)
        {
            Control control = pair.Key;
            object value = GetSettingValue(pair.Value);

            if (control is CheckBox)
            {
                ((CheckBox)control).Checked = (bool)value;
                return true;
            }
            if (control is TextBox)
            {
                control.Text = value.ToString();
                return true;
            }
            return false;
        }

        private object GetSettingValue(string parameterName)
        {
            object value;
            try
            {
                value = Settings.Default[parameterName];
            }
            catch
            {
                value = CommonSettings.Default[parameterName];
            }

            return value;
        }

        private void SetSettingValue(string parameterName, object value)
        {
            try
            {
                Settings.Default[parameterName] = value;
            }
            catch
            {
                CommonSettings.Default[parameterName] = value;
            }
        }

        private void bClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cbParameterChanged_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox rb = (CheckBox)sender;
            try
            {
                rb.ForeColor = IsParameterChanged(rb.Name, rb.Checked) ? _changedColor : _standardForeColor;
            }
            catch
            {
                MessageBox.Show(Resources.Error);
            }
        }

        private bool IsParameterChanged(string name, object value)
        {
            bool result = false;

            foreach (KeyValuePair<Control, string> pair in linkSettings)
                if (pair.Key.Name == name)
                {
                    result = CompareParameter(pair.Value, value);
                    if (result)
                        AddChange(name);
                    else RemoveChange(name);
                    return result;
                }
            throw new ArgumentException();
        }

        private bool CompareParameter(string pName, object value)
        {
            bool result;
            try
            {
                if (Settings.Default[pName] is int)
                    result = !Settings.Default[pName].Equals(Convert.ToInt32(value));
                else
                    result = !Settings.Default[pName].Equals(value);
            }
            catch
            {
                if (CommonSettings.Default[pName] is int)
                    result = !CommonSettings.Default[pName].Equals(Convert.ToInt32(value));
                else
                    result = !CommonSettings.Default[pName].Equals(value);
            }

            return result;
        }

        private void nudAddFlyDirection_ValueChanged(object sender, EventArgs e)
        {
            AddChange(((Control)sender).Name);
        }

        private void nudAddAngleKZ1_ValueChanged(object sender, EventArgs e)
        {
            AddChange(((Control)sender).Name);
        }

        private void clbChecks_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (clbChecks.Enabled)
                AddChange(sender.ToString());

            bACApply.Enabled = IsAnyItemSelected(clbACTypes, e.Index, e.NewValue);
        }

        private void bACApply_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbChecks.Items.Count; i++)
                clbChecks.SetItemChecked(i, false);

            foreach (var item in clbACTypes.CheckedItems)
            {
                foreach (var dissItem in linkACTypesToDevices[item.ToString()])
                {
                    for (int i = 0; i < clbChecks.Items.Count; i++)
                        if (clbChecks.Items[i].ToString() == dissItem)
                        {
                            clbChecks.SetItemChecked(i, true);
                            break;
                        }
                }
            }
        }

        private void cb_JupEnable_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox rb = (CheckBox)sender;
            try
            {
                rb.ForeColor = IsParameterChanged(rb.Name, rb.Checked) ? _changedColor : _standardForeColor;
            }
            catch (Exception)
            {
                MessageBox.Show(Resources.Error);
            }
        }

        private void clbACTypes_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (clbACTypes.Enabled)
                AddChange(sender.ToString());
            bACApply.Enabled = IsAnyItemSelected(clbACTypes, e.Index, e.NewValue);
        }

        private bool IsAnyItemSelected(CheckedListBox checkedListBox, int changingIndex, CheckState newCheckState)
        {
            for (int i = 0; i < checkedListBox.Items.Count; i++)
            {
                bool isSelected;
                if (i == changingIndex)
                    isSelected = (newCheckState == CheckState.Checked);
                else
                    isSelected = checkedListBox.GetItemChecked(i);

                if (isSelected)
                    return true; // как только нашли выбранный — выходим
            }
            return false;
        }

        private void frmSettings_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (iStatusChangeFlag > 0) MessageBox.Show($"Для применения настроек после изменения требуется перезагрузка ПО КПК-2.", "ВНИМАНИЕ!");
        }
    }
}
