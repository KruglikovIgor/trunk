using System;
using System.ComponentModel;
using System.Windows.Forms;
using KPK2.Common.DissFunctions.Diss32;
using KPK2.DB;
using KPK2.GeneralForms;
using KPK2.GeneralForms.DissForms;
using KPK2.GeneralForms.DissForms.Setting;
using KPK2.Properties;
using Sparc.KPK2.NFunctions;
using Sparc.KPK2.NFunctions.Enums;

namespace KPK2.Modes
{
	internal class ModeWorkCheck: ModeApplicationState
	{
		public Mode Mode { get; set; }

		public override void Initialization(frmMain frmMain)
		{
			base.Initialization(frmMain);
			ComponentResourceManager resources = new ComponentResourceManager(typeof(frmMain));

			ToolStripMenuItem mnuDiss32WT = new ToolStripMenuItem();
			resources.ApplyResources(mnuDiss32WT, "mnuDiss32WT");
			mnuDiss32WT.Click += new System.EventHandler(this.mnuDiss32_Click_1);

			ToolStripMenuItem mnuDiss32_90AWT = new ToolStripMenuItem();
			mnuDiss32_90AWT.Name = "mnuDiss32_90A";
			resources.ApplyResources(mnuDiss32_90AWT, "mnuDiss32_90AWT");
			mnuDiss32_90AWT.Click += new System.EventHandler(MenuItem_Work_32_Click);

			_mnuWork.DropDownItems.AddRange(new ToolStripItem[]
			{
				mnuDiss32WT,
				mnuDiss32_90AWT
			});
		}

		public override void StartCheck(DevicesID deviceID, Modification modification)
		{
			string dissStr;
			dissStr = Globals.GetDissCaption(deviceID, modification);
            SettingsObj.GetSettings().DBKPK2 = new DataBaseKPK2(-1);
            DialogResult dResult = MessageBox.Show(string.Format(Resources.NeedToSaveTest, dissStr), Resources.ApplicationName,
				MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
			switch (dResult)
			{
				case DialogResult.Yes: //Делаем запись о новой проверки 
					frmNewCheck frm = new frmNewCheck(deviceID, modification, Sparc.KPK2.NFunctions.ModeApplication.RepairCheck);
					if (frm.isShow)
						StartForm(frm);
					else
						Globals.CantOpenForm(frm);
					break;
				case DialogResult.No:
					switch (deviceID)
					{
						case DevicesID.Diss32_WorkTest:
						case DevicesID.Diss32_90_WorkTest:
							Form formWork32 = new frmWork32(new MeasurementPerformance(deviceID, modification), Mode);
							StartForm(formWork32);
							break;
					}
					break;
			}
		}

		private void mnuDiss32_Click_1(object sender, EventArgs e)
		{
			StartCheck(DevicesID.Diss32_WorkTest, Modification.None);
		}

		private void MenuItem_Work_32_Click(object sender, EventArgs e)
		{
			StartCheck(DevicesID.Diss32_90_WorkTest, Modification.Diss32_90A);
		}
	}
}
