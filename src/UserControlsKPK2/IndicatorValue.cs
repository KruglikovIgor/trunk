using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


namespace UserControlsKPK2
{
    public partial class IndicatorValue : UserControl
    {
        public IndicatorValue()
        {
            InitializeComponent();
        }

        private Color _successColor = Color.LimeGreen;
        private Color _failColor = Color.Red;
        private Color _usualColor = Color.Black;

        public Color UsualColor
        {
            get { return _usualColor; }
            set { _usualColor = value; }
        }

        public Color SuccessColor
        {
            get { return _successColor; }
            set { _successColor = value; }
        }
        public Color FailColor
        {
            get { return _failColor; }
            set { _failColor = value; }
        }

        [Localizable(true)]
        public string UpSign
        {
            get { return lUpSign.Text; }
            set { lUpSign.Text = value;
                DoInvalidate(); }
        }

        [Localizable(true)]
        public string DownSign
        {
            get { return lDownSign.Text; }
            set { lDownSign.Text = value;
                DoInvalidate(); }
        }

        public double Value
        {
            get { return Functions.MoveToDouble(tbValue.Text); }
            set
            {
                if (double.IsNaN(value))
                {
                    tbValue.Text = String.Empty;
                    return;
                }
                tbValue.Text = value.ToString();
            }
        }

        private double _maxValue = 0;

        /// <summary>
        /// Максимальное значение, которое может принимать индикатор
        /// </summary>
        public double MaxValue
        {
            get { return _maxValue; }
            set
            {
                //if (value <= _minValue)
                //  return;
                _maxValue = value;
            }
        }

        private double _minValue = 0;

        /// <summary>
        /// Минимальное значение, которое может принимать индикатор
        /// </summary>
        public double MinValue
        {
            get { return _minValue; }
            set
            {
                //if (value >= _maxValue)
                //  return;
                _minValue = value;
            }
        }

        private double _maxGreenValue = 0;
        /// <summary>
        /// Максимальное значение для "зелёного" диапазона
        /// </summary>
        public double MaxGreenValue { get { return _maxGreenValue; }
            set
            {

                //if ((value < _minGreenValue) || (value < _minValue))
                //  return;
                _maxGreenValue = value;
                DoInvalidate();
            }
        }

        private double _minGreenValue = 0;
        /// <summary>
        /// Минимальное значение для "зелёного" диапазона
        /// </summary>
        public double MinGreenValue
        {
            get { return _minGreenValue; }
            set
            {
                //if ((value > _maxGreenValue) || (value > _maxValue))
                //  return;
                _minGreenValue = value;
                DoInvalidate();
            }
        }

        /// <summary>
        /// Описание для подстановки в сообщение об ошибке
        /// </summary>
        public string Description { get; set; }

        public new bool Validate 
        { 
            get 
            { 
                if(tbValue.Text == "")
                {
                    return false;
                }
                return Value >= MinGreenValue && Value <= MaxGreenValue; 
            } 
        }

    public delegate void ErrorDiapazon (string name, double min, double max, double value) ;

    public Action<string> ErrorInputValue;
    public ErrorDiapazon ErrorDiapazonValue;

    public void ResetErrorInputValue()
    {
      ErrorInputValue = null;
    }

    public void ResetErrorDiapazonValue()
    {
      ErrorDiapazonValue = null;
    }

    private void ErrorInputValueFunc()
    {
      try
      {
        if (ErrorInputValue != null)
          ErrorInputValue(tbValue.Text);
      }
      catch
      {
      }
    }

    private void ErrorDizpazonValueFunc()
    {
      try
      {
        if (ErrorDiapazonValue != null)
          ErrorDiapazonValue(Description, MinValue, MaxValue, Value);
      }
      catch
      {
      }
    }

    private void DoInvalidate()
    {
      Invalidate();
    }

    private void tbValue_TextChanged(object sender, EventArgs e)
    {
      ForeColor = _usualColor;
      tbValue.Text = tbValue.Text.Trim();
    }

    private void tbValue_Validating(object sender, CancelEventArgs e)
    {
      if (String.IsNullOrEmpty(tbValue.Text))
        return;
      string strValue = tbValue.Text;
      double value = Functions.MoveToDouble(strValue);
      if (double.IsNaN(value))
      {
        ErrorInputValueFunc();
        tbValue.Focus();
        return;
      }
      if (MinValue != MaxValue)
        if ((value < MinValue) || (value > MaxValue))
        {
          ErrorDizpazonValueFunc();
          tbValue.Focus();
          return;
        }
      if (double.IsNaN(MinGreenValue) || double.IsNaN(MaxGreenValue))
        return;
      if(MinGreenValue == 0 && MaxGreenValue == 0)
            {
                tbValue.ForeColor = Color.Black;
                return;
            }
      tbValue.ForeColor = (value >= MinGreenValue && value <= MaxGreenValue) ? _successColor : _failColor;
    }
  }
}
