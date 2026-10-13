using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KPK2.GeneralForms.AddForms
{
    public partial class fSetValue : Form
    {
        private Action<int> _valueChanged;

        public fSetValue(int minAngle, int maxAngle, int currentValue, Action<int> valueChanged)
        {
            InitializeComponent();
            nudValue.Minimum = minAngle;
            nudValue.Maximum = maxAngle;
            nudValue.Value = currentValue;
            _valueChanged = valueChanged;
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void bSetValue_Click(object sender, EventArgs e)
        {
            var handler = _valueChanged;
            if (handler != null)
                handler((int)nudValue.Value);
            Close();
        }
    }
}
