using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using KPK2.GeneralForms.DissForms;
using KPK2.GeneralForms;
using Sparc.KPK2.NFunctions.Enums;
using KPK2.GeneralForms.ReferenceForm;
using KPK2.GeneralForms.ServiceForm;
using KPK2.Users;

namespace KPK2.Modes
{
	public abstract class ModeApplicationState
	{
		internal ToolStripMenuItem _mnuWork;

		public event EventHandler ShowForm;
		public event EventHandler CloseForm;
		
		public frmMain ParentForm { get; private set; }

		public ModeApplicationState()
		{
		}

		public virtual void Initialization(frmMain main)
		{
			if(ParentForm == null)
				ParentForm = main;
			var menuStrip = (MenuStrip)ParentForm.Controls.Find("menuStrip", true)
				.FirstOrDefault(control => control.Name == "menuStrip");
			_mnuWork = (ToolStripMenuItem)menuStrip.Items.Find("mnuWork", true)
				.FirstOrDefault(toolStripItem => toolStripItem.Name == "mnuWork");
		}

		internal virtual void StartForm<T>(T f) where T : Form
		{
			f.Closed += CloseForm;
			f.MdiParent = ParentForm;
			f.Show();
			if (!(f is frmPost) && !(f is frmACType) && !(f is frmCalibrateAttenuations))
				f.WindowState = FormWindowState.Maximized;
			ShowForm?.Invoke(ParentForm, EventArgs.Empty);
		}

		public abstract void StartCheck(DevicesID deviceID, Modification modification);
	}
}
