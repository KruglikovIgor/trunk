using System;
using System.Windows.Forms;
using Sparc.KPK2.Hardware;
using Sparc.KPK2.NFunctions;
using Res = Sparc.KPK2.Resources.Properties.Resources;

namespace KPK2.GeneralForms.ServiceForm
{
    public partial class frmKZCalibrate : Form
    {
        private int currentChannelShift;

        private string currentShift
        {
            get
            {
                if (currentChannelShift > 0)
                    return "+" + currentChannelShift;
                return currentChannelShift.ToString();
            }
        }

        private int CurrentChannel
        {
            get { return (int) nudChannel.Value; }
        }

        private int FullAmplitude
        {
            get { return ControlTask.BaseAmplitudeAttestation + currentChannelShift; }
        }

        public frmKZCalibrate()
        {
            InitializeComponent();
        }

        private void bClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmKZCalibrate_Load(object sender, EventArgs e)
        {
            ControlTask.Instance().SetControlTaskWithCorrection(5, ControlTask.BaseAmplitudeAttestation,
                                                  ControlTask.BaseShiftAttestation);
            tbShift.Text = GetChannelShift();
        }

        private string GetChannelShift()
        {
            int shift = DataManager.Instance.GetKZChannelShift((int) nudChannel.Value);
            if (shift == -1)
                currentChannelShift = 0;
            else
                currentChannelShift = DataManager.Instance.GetKZChannelShift((int)nudChannel.Value) - ControlTask.BaseAmplitudeAttestation;
            return currentShift;
        }

        private void nudChannel_ValueChanged(object sender, EventArgs e)
        {
            tbShift.Text = GetChannelShift();
            bSave.Enabled = false;
        }

        private void bPlus_Click(object sender, EventArgs e)
        {
            currentChannelShift++;
            if (FullAmplitude == 255)
                bPlus.Enabled = false;
            if (!SetAmplitudeCorrection())
                MessageBox.Show(Res.Error);
            else
                tbShift.Text = currentShift;
            bSave.Enabled = true;
            bMinus.Enabled = true;
        }

        private bool SetAmplitudeCorrection()
        {
            return ControlTask.Instance().TunningAmplitude(CurrentChannel,
                                                    FullAmplitude);
        }

        private void bMinus_Click(object sender, EventArgs e)
        {
            currentChannelShift--;
            if (FullAmplitude == 0)
                bMinus.Enabled = false;
            if (!SetAmplitudeCorrection())
                MessageBox.Show(Res.Error);
            else
                tbShift.Text = currentShift;
            bSave.Enabled = true;
            bPlus.Enabled = true;
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            if (!DataManager.Instance.SaveKZChannelShift(CurrentChannel, FullAmplitude))
                MessageBox.Show(Res.Error);
            bSave.Enabled = false;
        }
    }
}