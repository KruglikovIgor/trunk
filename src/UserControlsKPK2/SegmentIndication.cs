using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using UserControlsKPK2.Enums;

namespace UserControlsKPK2
{
    public partial class SegmentIndication : UserControl
    {
        private const string Over = "OVER";
        /// <summary>
        /// Строковое значение величины, отображаемое индикатором.
        /// </summary>
        private string valueString = "000.00";
        private double _value;

        public delegate bool CheckValueDelegate(double value);

        public CheckValueDelegate CheckValueFunction;

        [Description("Method how to calculate numbers color")]
        [Browsable(true)]
        [Category("Appearance")]
        public CalcMethod CalculationMethod { get; set; } = CalcMethod.MinMax;

        [Description("The value to display.")]
        [Browsable(true)]
        [Category("Appearance")]
        public string ValueString
        {
            get
            {
                return valueString;
            }
          set
          {
            if (!DesignMode)
              Inactive = false;
            valueString = value.Replace(",", ".");
            if (!takeOnOffDisplay)
              return;

            double myValue;
            bool res = double.TryParse(valueString, out myValue);
            if (!res)
              res = double.TryParse(valueString.Replace(".", ","), out myValue);
            if (res)
              _isInvalid = !CheckValue(myValue);
            Invalidate();
          }
        }
        public double Value
        {
            get { return _value; }
          set
          {
            if (!DesignMode)
              Inactive = false;
            _value = value;
            double myValue = value;
            if (_numFract == 0)
              myValue = TrimBigValue(myValue);
            string formatValue = myValue.ToString();
            if (!double.IsNaN(myValue))
              if ((_numWhole != 0) || (_numFract != 0))
              {
                myValue = Math.Round(myValue, _numFract);
                formatValue = Math.Round(myValue, _numFract).ToString();
                int addZero = 0;
                if (_numWhole != 0)
                {
                  addZero = _numWhole -
                                (int) Math.Log10(((int) Math.Abs(myValue)) == 0 ? 1 : ((int) Math.Abs(myValue))) - 1;
                  if (addZero < 0)
                  {
                    if (_showOverValue)
                      valueString = value.ToString();
                    else
                      valueString = Over;
                    ValueChanged();
                    return;
                  }
                  if (_needAlign)
                    for (int i = 0; i < addZero; i++)
                      formatValue = formatValue.Insert(_value < 0 ? 1 : 0, " ");
                }
                int whole = (_numWhole == 0
                               ? ((int) Math.Abs(Convert.ToDouble(formatValue))).ToString().Length
                               : _numWhole);
                if ((_numFract > 0) && ((int) myValue == myValue))
                  formatValue += ".";
                int count = (_numFract > 0 ? _numFract + 1 + (myValue < 0 ? 1 : 0) : _numFract) + whole -
                            (formatValue.Length + (_needAlign? 0 : addZero));
                for (int i = 0; i < count; i++)
                  formatValue += "0";
              }
            ValueString = formatValue; //.Replace(" ","  ");
            _value = myValue;
            ValueChanged();
          }
        }

        private double TrimBigValue(double myValue)
        {
            int decimalNumbers = myValue.ToString().Length - ((int)myValue).ToString().Length;            
            double digitStep = 42 * this.CreateGraphics().DpiX * Font.SizeInPoints / 72 / 72;
            double fullSize = myValue.ToString().Length * digitStep;
            int norm = 1;         
            while (fullSize > ClientRectangle.Width)
            {
                fullSize -= digitStep;
                norm++;
            }
            if (norm == 0)
                return myValue;
            if (norm >= decimalNumbers)
                return Math.Round(myValue, 0);            
            return Math.Round(myValue, (decimalNumbers - norm) > 15 ? 15 : decimalNumbers - norm);
        }

        private bool CheckValue(double value)
        {
            if (CalculationMethod == CalcMethod.MinMax)
                return MinMaxCalculation(value);
            if (CalculationMethod == CalcMethod.Delegate)
                return CheckValueFunction(value);
            var minMax = MinMaxCalculation(value);
            var func = CheckValueFunction(value);
            if (CalculationMethod == CalcMethod.MinMaxAndDelegate)
                return minMax && func;
            if (CalculationMethod == CalcMethod.MinMaxOrDelegate)
                return minMax || func;
            return true;
        }

        private bool MinMaxCalculation(double value)
        {
            if ((Math.Abs(maxValue - minValue) > double.Epsilon) || (Math.Abs(maxValue) > double.Epsilon) || (Math.Abs(minValue) > double.Epsilon))
                return (value >= minValue) && (value <= maxValue);
            return true;
        }

        private void ValueChanged()
        {
          _isInvalid = !CheckValue(_value);
            Invalidate();
        }

        private int _numWhole;
        private int _numFract;

        public int WholePart
        {
            get { return _numWhole; }
            set { _numWhole = value; }
        }

        public int FractPart
        {
            get { return _numFract; }
            set { _numFract = value; }
        }

      private bool _needAlign = true;

      public bool NeedAlign
      {
        get { return _needAlign; }
        set { _needAlign = value; }
      }

        /// <summary>
        /// Признак включенного/выключенного дисплея.
        /// </summary>
        private bool takeOnOffDisplay = true;
        [DefaultValue(true)]
        [Description("Indicates display status.")]
        [Browsable(true)]
        [Category("Appearance")]
        public bool TakeOnOffDisplay
        {
            get
            {
                return takeOnOffDisplay;
            }
            set
            {
                takeOnOffDisplay = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Цвет фона индикатора.
        /// </summary>
        private Color _BGColor = Color.Black;
        [Description("The background color.")]
        [Browsable(true)]
        [Category("Appearance")]
        public Color BGColor
        {
            get
            {
              return _BGColor;
            }
            set
            {
                _BGColor = value;
                Invalidate();
            }
        }

      private Color currentBGColor
      {
        get
        {
          return _inactive ? _inactiveBGColor : _BGColor;
        }
      }

        /// <summary>
        /// Цвет показывающий неактивность индикатора в данный момент.
        /// </summary>
        private Color _inactiveBGColor = Color.Gray;
        [Description("The inactive background color.")]
        [Browsable(true)]
        [Category("Appearance")]
        public Color InactiveBGColor
        {
            get
            {
                return _inactiveBGColor;
            }
            set
            {
                _inactiveBGColor = value;
                CheckInactive();
            }
        }

        private bool _inactive;
        /// <summary>
        /// Свойство показывающее неактивность индикатора в данный момент.
        /// </summary>
        [Description("Is indicator inactive now?")] 
        [Browsable(true)] 
        [Category("Appearance")] 
        public bool Inactive
        {
            get { return _inactive; }
            set 
            {
                if (_inactive == value)
                    return;
                _inactive = value;
                CheckInactive();
            }
        }

        private void CheckInactive()
        {
            Invalidate();
        }

        

        /// <summary>
        /// Цвет показывающий ошибочное значение сегмента.
        /// </summary>
        private Color invalidColor = Color.Red;
        [Description("The invalid value color.")]
        [Browsable(true)]
        [Category("Appearance")]
        public Color InvalidColor
        {
            get
            {
                return invalidColor;
            }
            set
            {
                invalidColor = value;
                Invalidate();
            }
        }


        /// <summary>
        /// Цвет светлого сегмента.
        /// </summary>
        private Color lightColor = Color.Lime;
        private bool _isInvalid = false;

        [Description("The light segment color.")]
        [Browsable(true)]
        [Category("Appearance")]
        public Color LightColor
        {
            get
            {
              return lightColor;
            }
            set
            {
                lightColor = value;
                Invalidate();
            }
        }

      private Color currentLightColor
      {
        get
        {
          double val;
          double.TryParse(ValueString, out val);
          return !_isInvalid || (Inactive && Value == 0 && (double.IsNaN(val) || val == 0)) ? lightColor : invalidColor;
        }
      }

        /// <summary>
        /// Цвет темного сегмента.
        /// </summary>
        private Color darkColor = Color.Transparent;//Color.FromArgb(64, 64, 64);
        [Description("The dark segment color.")]
        [Browsable(true)]
        [Category("Appearance")]
        public Color DarkColor
        {
            get
            {
                return darkColor;
            }
            set
            {
                darkColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Указывает единицы измерения.
        /// </summary>
        private string unit = "";
        [Description("Measuring unit.")]
        [Browsable(true)]
        [Category("Appearance")]
        [Localizable(true)]
        public string Unit
        {
            get
            {
                return unit;
            }
            set
            {
                unit = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Максимально допустимое значение параметра.
        /// </summary>
        private double maxValue;
        [Description("Maximum value.")]
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(0.0f)]
        public double MaxValue
        {
            get
            {
                return maxValue;
            }
            set
            {
                maxValue = value;
            }
        }


        /// <summary>
        /// Максимально допустимое значение параметра.
        /// </summary>
        private double minValue;
        [Description("Maximum value.")]
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(0.0f)]
        public double MinValue
        {
            get
            {
                return minValue;
            }
            set
            {
                minValue = value;
            }
        }

        /// <summary>
        /// Допустимые границы параметра.
        /// </summary>
        private string limiting = "";
        [Description("Value limiting.")]
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        public string Limiting
        {
            get
            {
                return limiting;
            }
            set
            {
                limiting = value;
                Invalidate();
            }
        }

        public SegmentIndication()
        {
            DarkColor = Color.Transparent;
            InitializeComponent();
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            AutoScaleMode = AutoScaleMode.None;
            ResizeRedraw = true;
            //BackColor = Color.Tra;
            //Font = new Font(Font.FontFamily.Name, 48);
            FontChanged += SegmentIndication_FontChanged;
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
        }
        void SegmentIndication_FontChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        const int FrameWidth = 3;
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            base.OnPaint(e);
            const int bordersWidth = 3;
            const int rectangleRoundRadius = 3;            
            Graphics grfx = e.Graphics;
            RectangleF rf = new RectangleF(ClientRectangle.X + bordersWidth / 2f, ClientRectangle.Y + bordersWidth / 2f, ClientRectangle.Width - bordersWidth, ClientRectangle.Height - bordersWidth);
            GraphicsPath gp = GetRoundedRect(rf, rectangleRoundRadius);
            Brush b = new SolidBrush(currentBGColor);
            Pen p = new Pen(Color.Gray, bordersWidth);
            grfx.FillPath(b, gp);
            grfx.DrawPath(p, gp);
            SevenSegmentDisplay ssd = new SevenSegmentDisplay(grfx);            
            const float x = 8;
            float y = (Height + FrameWidth - Font.Height) / 2.0f;
            Brush brush = takeOnOffDisplay ? new SolidBrush(currentLightColor) : new SolidBrush(DarkColor);
            ssd.DrawString(valueString + " " + unit, Font, brush, new SolidBrush(darkColor), x, y);
            if (Limiting != "")
                ssd.DrawLimiting(Limiting, Font, brush, 0.0f, y);
        }

        #region GetRoundedRect

        // credit for the this function is given to Aaron Reginald http://www.codeproject.com/cs/media/ExtendedGraphics.asp
        protected static GraphicsPath GetRoundedRect(RectangleF baseRect, float _radius)
        {
            // if corner _radius is less than or equal to zero, 
            // return the original rectangle 
            if (_radius <= 0.0f)
            {
                GraphicsPath mPath = new GraphicsPath();
                mPath.AddRectangle(baseRect);
                mPath.CloseFigure();
                return mPath;
            }

            // if the corner _radius is greater than or equal to 
            // half the width, or height (whichever is shorter) 
            // then return a capsule instead of a lozenge 
            if (_radius >= (Math.Min(baseRect.Width, baseRect.Height)) / 2.0)
            {
                return GetCapsule(baseRect);
            }

            // create the arc for the rectangle sides and declare 
            // a graphics path object for the drawing 
            float diameter = _radius * 2.0f;
            SizeF sizeF = new SizeF(diameter, diameter);
            RectangleF arc = new RectangleF(baseRect.Location, sizeF);
            GraphicsPath path = new GraphicsPath();

            // top left arc 
            path.AddArc(arc, 180, 90);

            // top right arc 
            arc.X = baseRect.Right - diameter;
            path.AddArc(arc, 270, 90);

            // bottom right arc 
            arc.Y = baseRect.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // bottom left arc
            arc.X = baseRect.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        // credit for the this function is given to Aaron Reginald http://www.codeproject.com/cs/media/ExtendedGraphics.asp
        private static GraphicsPath GetCapsule(RectangleF baseRect)
        {
            RectangleF arc;
            GraphicsPath path = new GraphicsPath();

            try
            {
                float diameter;
                if (baseRect.Width > baseRect.Height)
                {
                    // return horizontal capsule 
                    diameter = baseRect.Height;
                    SizeF sizeF = new SizeF(diameter, diameter);
                    arc = new RectangleF(baseRect.Location, sizeF);
                    path.AddArc(arc, 90, 180);
                    arc.X = baseRect.Right - diameter;
                    path.AddArc(arc, 270, 180);
                }
                else if (baseRect.Width < baseRect.Height)
                {
                    // return vertical capsule 
                    diameter = baseRect.Width;
                    SizeF sizeF = new SizeF(diameter, diameter);
                    arc = new RectangleF(baseRect.Location, sizeF);
                    path.AddArc(arc, 180, 180);
                    arc.Y = baseRect.Bottom - diameter;
                    path.AddArc(arc, 0, 180);
                }
                else
                {
                    // return circle 
                    path.AddEllipse(baseRect);
                }
            }

            catch (Exception)
            {
                path.AddEllipse(baseRect);
            }

            finally
            {
                path.CloseFigure();
            }

            return path;
        }

        #endregion

      private bool _showOverValue;

        private void SegmentIndication_MouseHover(object sender, EventArgs e)
        {
          if (ValueString == Over)
          {
            ValueString = Value.ToString();
            _showOverValue = true;
          }
        }

        private void SegmentIndication_MouseLeave(object sender, EventArgs e)
        {
          if (_showOverValue)
          {
            Value = Value;
            _showOverValue = false;
          }
        }
    }


  public class SevenSegmentDisplay
  {
    private Graphics grfx;

    private static byte[,] bySegment =
      {
        {1, 1, 1, 0, 1, 1, 1}, //0
        {0, 0, 1, 0, 0, 1, 0}, //1
        {1, 0, 1, 1, 1, 0, 1}, //2
        {1, 0, 1, 1, 0, 1, 1}, //3
        {0, 1, 1, 1, 0, 1, 0}, //4
        {1, 1, 0, 1, 0, 1, 1}, //5
        {1, 1, 0, 1, 1, 1, 1}, //6
        {1, 0, 1, 0, 0, 1, 0}, //7
        {1, 1, 1, 1, 1, 1, 1}, //8
        {1, 1, 1, 1, 0, 1, 1}, //9
        {0, 0, 0, 1, 0, 0, 0}
      }; //- 


    private readonly Point[][] apt = new Point[7][];

    public SevenSegmentDisplay(Graphics grfx)
    {
      this.grfx = grfx;
      grfx.CompositingQuality = CompositingQuality.HighQuality;

      //инициализация массива
      apt[0] = new Point[] {new Point(3, 2), new Point(39, 2), new Point(31, 10), new Point(11, 10)};
      apt[1] = new Point[] {new Point(2, 3), new Point(10, 11), new Point(10, 31), new Point(2, 35)};
      apt[2] = new Point[] {new Point(40, 3), new Point(40, 35), new Point(32, 31), new Point(32, 11)};
      apt[3] = new Point[]
        {
          new Point(3, 36), new Point(11, 32), new Point(31, 32), new Point(39, 36), new Point(31, 40),
          new Point(11, 40)
        };
      apt[4] = new Point[] {new Point(2, 37), new Point(10, 41), new Point(10, 61), new Point(2, 69)};
      apt[5] = new Point[] {new Point(40, 37), new Point(40, 69), new Point(32, 61), new Point(32, 41)};
      apt[6] = new Point[] {new Point(11, 62), new Point(31, 62), new Point(39, 70), new Point(3, 70)};
    }

    public SizeF MeasureString(string str, Font font, string unit, string limit)
    {
      SizeF sizef = new SizeF(0, grfx.DpiY*font.SizeInPoints/72);
      for (int i = 0; i < str.Length; i++)
      {
        if (Char.IsDigit(str[i]))
          sizef.Width += 42*grfx.DpiX*font.SizeInPoints/72/72;
        else if (str[i] == ':' || str[i] == '.')
          sizef.Width += 12*grfx.DpiX*font.SizeInPoints/72/72;
      }
      float w1 = 0, w2 = 0;
      if (unit != "")
      {
        for (int i = 0; i < unit.Length; i++)
        {
          w1 += 12*grfx.DpiX*font.SizeInPoints/72/72;
          if (!Char.IsSymbol(unit[i]))
            w1 += 21*grfx.DpiX*font.SizeInPoints/72/72;
        }
      }
      if (limit != "")
      {
        w2 += 42*limit.Length*grfx.DpiX*font.SizeInPoints/5/72/72;
      }
      sizef.Width += Math.Max(w1, w2);
      return sizef;
    }

    public void DrawString(string str, Font font, Brush lightBrush, Brush darkBrush, float x, float y)
    {
      bool anySymbolStarted = false;
      for (int i = 0; i < str.Length; i++)
      {
        if ((str[i] == ' ') && (!anySymbolStarted))
          x = Number(0, font, lightBrush, darkBrush, x, y, true);
        else if (str[i] == '-')
        {
          x = Number(10, font, lightBrush, darkBrush, x, y, false);
          anySymbolStarted = true;
        }
        else if (Char.IsDigit(str[i]))
        {
          x = Number(str[i] - '0', font, lightBrush, darkBrush, x, y, false);
          anySymbolStarted = true;
        }
        else if (str[i] == ':')
        {
          x = Colon(font, lightBrush, x, y);
          anySymbolStarted = true;
        }
        else if (str[i] == '.')
        {
          x = Dot(font, lightBrush, x, y);
          anySymbolStarted = true;
        }
        else if (Char.IsLetter(str[i]))
        {
          x = Letter(str[i], font, lightBrush, x, y);
          anySymbolStarted = true;
        }
        else
        {
          x = Symbol(str[i], font, lightBrush, x, y);
          anySymbolStarted = true;
        }
      }
    }

    private float digitEnd = 0;

    private float Colon(Font font, Brush brush, float x, float y)
    {
      Point[][] apt = new Point[2][];
      apt[0] = new[] {new Point(2, 21), new Point(6, 17), new Point(10, 21), new Point(6, 25)};
      apt[1] = new[] {new Point(2, 51), new Point(6, 47), new Point(10, 51), new Point(6, 55)};
      for (int i = 0; i < apt.Length; i++)
      {
        Fill(apt[i], font, brush, x, y);
      }
      return x + 12*grfx.DpiX*font.SizeInPoints/72/72;
    }

    private float Dot(Font font, Brush brush, float x, float y)
    {
      Point[] apt = new Point[] {new Point(2, 70), new Point(10, 70), new Point(10, 62), new Point(2, 62)};
      for (int i = 0; i < apt.Length; i++)
      {
        Fill(apt, font, brush, x, y);
      }
      return x + 12*grfx.DpiX*font.SizeInPoints/72/72;
    }

    private float Number(int num, Font font, Brush lightBrush, Brush darkBrush, float x, float y, bool isEmpty)
    {
      if (!isEmpty)
      {
        for (int i = 0; i < apt.Length; i++)
          Fill(apt[i], font, bySegment[num, i] == 1 ? lightBrush : darkBrush, x, y);
      }
      return x + 42*grfx.DpiX*font.SizeInPoints/72/72;
    }

    private void Fill(Point[] apt, Font font, Brush brush, float x, float y)
    {
      PointF[] aptf = new PointF[apt.Length];
      for (int i = 0; i < apt.Length; i++)
      {
        aptf[i].X = x + apt[i].X*grfx.DpiX*font.SizeInPoints/72/72;
        aptf[i].Y = y + apt[i].Y*grfx.DpiY*font.SizeInPoints/72/72;
      }
      grfx.FillPolygon(brush, aptf);
    }

    public float Letter(Char ch, Font font, Brush brush, float x, float y)
    {
      grfx.DrawString(ch.ToString(), new Font(font.FontFamily.Name, font.Size/2), brush, x, y + font.Size/2.0f);
      return x + 28*grfx.DpiX*font.SizeInPoints/72/72;
    }

    public float Symbol(Char ch, Font font, Brush brush, float x, float y)
    {
      grfx.DrawString(ch.ToString(), new Font(font.FontFamily.Name, font.Size/2), brush, x, y + font.Size/2.0f);
      float size = 16f*grfx.DpiX*font.SizeInPoints/72f/72f;
      return x + size;
    }

    public float DrawLimiting(string lim, Font font, Brush brush, float x, float y)
    {
      grfx.DrawString(lim, new Font(font.FontFamily.Name, font.Size/5), brush, digitEnd, y);
      return x + 42*grfx.DpiX*font.SizeInPoints/5/72/72;
    }
  }
}
