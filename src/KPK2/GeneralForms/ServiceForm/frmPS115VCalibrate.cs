using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using KPK2.Properties;
using Sparc.KPK2.Hardware;
using Sparc.KPK2.Hardware.PS115V;
using Sparc.KPK2.NFunctions;
using Globals = KPK2.Common.Globals;
using Res = Sparc.KPK2.Resources.Properties.Resources;

namespace KPK2.GeneralForms.ServiceForm
{
    public partial class frmPS115VCalibrate : Form
    {

      private double _lineResistance = 0;
      private int _recordId = 0;
      private double _attentionResistance = 0.3;
      private int _wrongResistance = 1;

        public frmPS115VCalibrate()
        {
            InitializeComponent();
        }

        private void frmPS115VCalibrate_Load(object sender, EventArgs e)
        {
            if (!Power115V.Instance.IsConnected())
                MessageBox.Show(Res.NoConnectionPS115);
          DataTable dt = DataManager.Instance.GetCalibration(DataManager.PS115VModuleId, 0);
          if (dt.Rows.Count > 0)
          {
            _recordId = Convert.ToInt32(dt.Rows[0][0]);
            _lineResistance = Convert.ToDouble(dt.Rows[0][3]);
            nudRealVoltage.Value = (decimal)Convert.ToDouble(dt.Rows[0][4]);
            RenewLabelValue();
          }
        }

        private void bCalculate_Click(object sender, EventArgs e)
        {
            double voltage = Power115V.Instance.GetClearVoltage();
            double current = Power115V.Instance.GetCurrent();
            _lineResistance = Math.Round((voltage - (double)nudRealVoltage.Value/1000d) / current, 3);
          RenewLabelValue();
          if (_lineResistance > _attentionResistance)
            MessageBox.Show(
              string.Format( Resources.AttentionBigLineResistance, _lineResistance, _attentionResistance), Resources.Warning);
        }

        private void RenewLabelValue()
        {
          Globals.SetLabelValue(lLineResistance, ':', _lineResistance + " Ом");
          lLineResistance.ForeColor = _lineResistance > _attentionResistance ? Color.Red : Color.Black;
        }

      private void bSaveClose_Click(object sender, EventArgs e)
        {

            if (!Save())
                MessageBox.Show(Resources.CantSaveDbParam);
            else
            {
                Power115V.Instance.LineResistance = double.NaN;
                Close();
            }
        }

        private bool Save()
        {
          if (_lineResistance > _wrongResistance)
          {
            MessageBox.Show(
              string.Format(Resources.ErrorWrongLineResistance, _lineResistance, _wrongResistance), Resources.Error);
            return false;
          }

          if (_recordId == 0)
            return
              DataManager.Instance
                         .SaveCalibration(DataManager.PS115VModuleId, 0, _lineResistance, (int) nudRealVoltage.Value) != 0;
          return DataManager.Instance.UpdateCalibration(_recordId, _lineResistance, (int) nudRealVoltage.Value);
        }

        private void bSet_Click(object sender, EventArgs e)
        {                      
            Power115V.Instance.SetFrequency((double)nudFrequency.Value);
            Power115V.Instance.SetMaxCurrent((double)nudCurrentLimit.Value);
            Power115V.Instance.SetVoltage((double)nudVoltage.Value);
        }

        private void frmPS115VCalibrate_FormClosing(object sender, FormClosingEventArgs e)
        {
            Power115V.Instance.SetOnOff(false);
        }
    
    }
}
