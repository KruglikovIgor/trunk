using System;
using System.Windows.Forms;
using KPK2.MyClasse;

namespace KPK2.GeneralForms.ServiceForm
{
	public partial class frmAutoAttestation : Form
	{
		private AutoAttestationAlgorithm _algorithm;

		public frmAutoAttestation(string channel)
		{
			InitializeComponent();
			_algorithm = new AutoAttestationAlgorithm(channel);
			lChannelName.Text = channel;
		}

		private void frmAutoAttestation_FormClosing(object sender, FormClosingEventArgs e)
		{
			_algorithm.Stop();
		}

		private void frmAutoAttestation_Load(object sender, EventArgs e)
		{
			dgvCheckTable.DataSource = _algorithm.CheckTable;
		}

		private void bStart_Click(object sender, EventArgs e)
		{
			_algorithm.Start(SelectRow, RenewRow);
		}

		private void RenewRow(int numRow)
		{
			if (InvokeRequired)
			{
				Invoke(new Action<int>(RenewRow), numRow);
				return;
			}

			var etMeas = _algorithm.MeasuredEt;
			var inMeas = _algorithm.MeasuredIn;
			if (_algorithm.IsUNChChannel)
			{
				dgvCheckTable.Rows[numRow].Cells[2].Value = etMeas.ToString();
				dgvCheckTable.Rows[numRow].Cells[3].Value = inMeas.ToString();
				dgvCheckTable.Rows[numRow].Cells[4].Value = etMeas - inMeas;
			}
			else
			{
				dgvCheckTable.Rows[numRow].Cells[1].Value = etMeas.ToString();
				dgvCheckTable.Rows[numRow].Cells[2].Value = inMeas.ToString();
				dgvCheckTable.Rows[numRow].Cells[3].Value = etMeas - inMeas;
			}
		}

		private void SelectRow(int numRow)
		{
			if (InvokeRequired)
			{
				Invoke(new Action<int>(SelectRow), numRow);
				return;
			}

			dgvCheckTable.Rows[numRow].Selected = true;
		}

		
	}
}
