using KPK2.Common.DissFunctions.Diss32;
using KPK2.DB;
using KPK2.GeneralForms;
using KPK2.GeneralForms.AddForms;
using KPK2.GeneralForms.DissForms;
using KPK2.GeneralForms.DissForms.Setting;
using KPK2.Properties;
using Sparc.KPK2.NFunctions;
using Sparc.KPK2.NFunctions.Enums;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace KPK2.Modes
{
	public class ModeDefaultCheck: ModeApplicationState
	{
		public Mode Mode { get; set; }

		public override void Initialization(frmMain frmMain)
		{
			base.Initialization(frmMain);
			ComponentResourceManager resources = new ComponentResourceManager(typeof(frmMain));
			// 
			// mnuDiss15
			// 
			ToolStripMenuItem mnuDiss15 = new ToolStripMenuItem();
			mnuDiss15.Name = "mnuDiss15";
			resources.ApplyResources(mnuDiss15, "mnuDiss15");
			mnuDiss15.Click += MenuItem_Work_15_Click;
			// 
			// mnuDISS15G
			// 
			var mnuDISS15G = new ToolStripMenuItem();
			mnuDISS15G.Name = "mnuDISS15G";
			resources.ApplyResources(mnuDISS15G, "mnuDISS15G");
			mnuDISS15G.Click += mnuDISS15G_Click;
			// 
			// mnuDiss32
			// 
			var mnuDiss32 = new ToolStripMenuItem();
			mnuDiss32.Name = "mnuDiss32";
			resources.ApplyResources(mnuDiss32, "mnuDiss32");
			mnuDiss32.Click += mnuDiss32_Click_1;
			// 
			// mnuDiss32_90
			// 
			var mnuDiss32_90 = new ToolStripMenuItem();
			mnuDiss32_90.Name = "mnuDiss32_90";
			resources.ApplyResources(mnuDiss32_90, "mnuDiss32_90");
			mnuDiss32_90.Click += mnuDiss32_Click;
			// 
			// mnuDiss32_90A
			// 
			var mnuDiss32_90A = new ToolStripMenuItem();
			mnuDiss32_90A.Name = "mnuDiss32_90A";
			resources.ApplyResources(mnuDiss32_90A, "mnuDiss32_90A");
			mnuDiss32_90A.Click += MenuItem_Work_32_Click;
			// 
			// mnuDiss13
			// 
			var mnuDiss13 = new ToolStripMenuItem();
			mnuDiss13.Name = "mnuDiss13";
			resources.ApplyResources(mnuDiss13, "mnuDiss13");
			mnuDiss13.Click += mnuDiss13_Click;
			// 
			// mnuDiss3228
			// 
			var mnuDiss3228 = new ToolStripMenuItem();
			mnuDiss3228.Name = "mnuDiss3228";
			resources.ApplyResources(mnuDiss3228, "mnuDiss3228");
			mnuDiss3228.Click += mnuDiss3228_Click;
			// 
			// mnuDiss3228_Monoblock
			// 
			var mnuDiss3228_Monoblock = new ToolStripMenuItem();
			mnuDiss3228_Monoblock.Name = "mnuDiss3228_Monoblock";
			resources.ApplyResources(mnuDiss3228_Monoblock, "mnuDiss3228_Monoblock");
			mnuDiss3228_Monoblock.Click += this.mnuDiss3228_Monoblock_Click;

			_mnuWork.DropDownItems.AddRange(new ToolStripItem[]
			{
				mnuDiss15,
				mnuDISS15G,
				mnuDiss32,
				mnuDiss32_90,
				mnuDiss32_90A,
				mnuDiss13,
				mnuDiss3228,
				mnuDiss3228_Monoblock
			});
		}

		public override void StartCheck(DevicesID deviceID, Modification modification)
		{
			string dissStr = "";
			dissStr = Globals.GetDissCaption(deviceID, modification);
            SettingsObj.GetSettings().DBKPK2 = new DataBaseKPK2(-1);
            DialogResult dResult = MessageBox.Show(string.Format(Resources.NeedToSaveTest, dissStr), Resources.ApplicationName,
				MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

			switch (dResult)
			{
				case DialogResult.Yes: //Делаем запись о новой проверки 
					frmNewCheck frm = new frmNewCheck(deviceID, modification, ModeApplication.DefaultCheck);
					if (frm.isShow)
						StartForm(frm);
					else
						Globals.CantOpenForm(frm);
					break;
				case DialogResult.No:
					switch (deviceID)
					{
						case DevicesID.Diss15:
							StartForm(new frmWork15(modification));
							break;
						case DevicesID.Diss32:
						case DevicesID.Diss32_90:
							StartForm(new frmWork32(new MeasurementPerformance(deviceID, modification), Mode));
							break;
						case DevicesID.Diss13:
							StartForm(new frmWork13());
							break;
						case DevicesID.Diss3228:
							StartForm(new frmWork3228());
							break;
						case DevicesID.Diss3228_Monoblock:
							StartForm(new frmWork3228_Monoblock());
							break;
					}
					break;
			}
		}

		// проверка ДИСС15
		private void MenuItem_Work_15_Click(object sender, EventArgs e)
		{
            StartCheck(DevicesID.Diss15, Modification.None);
		}

		private void mnuDiss3228_Monoblock_Click(object sender, EventArgs e)
		{
			StartCheck(DevicesID.Diss3228_Monoblock, Modification.None);
		}

		private void mnuDiss3228_Click(object sender, EventArgs e)
		{
			StartCheck(DevicesID.Diss3228, Modification.None);
		}

		private void mnuDiss13_Click(object sender, EventArgs e)
		{
			StartCheck(DevicesID.Diss13, Modification.None);
		}

		// проверка ДИСС32 
		private void MenuItem_Work_32_Click(object sender, EventArgs e)
		{
			StartCheck(DevicesID.Diss32_90, Modification.Diss32_90A);
		}

		private void mnuDiss32_Click(object sender, EventArgs e)
		{
			StartCheck(DevicesID.Diss32_90, Modification.None);
		}

		private void mnuDiss32_Click_1(object sender, EventArgs e)
		{
			StartCheck(DevicesID.Diss32, Modification.None);
		}

		private void mnuDISS15G_Click(object sender, EventArgs e)
		{
            //frmSelectDissMod selectDissMod = new frmSelectDissMod(new[] { "ДИСС-15Г", "ДИСС-15Д" });
            //selectDissMod.ShowDialog();
            //var selectedMod = selectDissMod.SelectedModification;
            //if (selectDissMod.Canceled)
            //    return;
            //StartCheck(DevicesID.Diss15, selectedMod);
            StartCheck(DevicesID.Diss15, Modification.Diss15G);
        }
	}
}
