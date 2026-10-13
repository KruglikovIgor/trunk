using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace UserControlsKPK2
{    
    public partial class PowerIndicator : UserControl
    {
        #region Const
        private const int RectangleRoundRadius = 8;
        private const int BordersWidth = 3;
        private const int BarsHeightInterval = 1;
        private const int BarsWidthInterval = 3;        
        private const int BarsEndInterval = 6;
        private const int StartBarsPosition = 6;
        private const int VoidBarWidth = 1;        
        #endregion

        #region Events
        public event EventHandler ValueChanged;
        #endregion

        #region Additional
        [Serializable]
        public class Bar
        {            
            public static event MethodInvoker Changed;            
 
            public Bar()
            {                
            }

            public Bar(string name)
            {
                _name = name;                
            }

            public Bar(string name, string units)
            {
                _name = name;
                _units = units;
            }

            public Bar(string name, string units, float min, float max, float minLimit, float maxLimit)
            {
                _name = name;
                _units = units;
                _maxValue = max;
                _minValue = min;
                _minLimitValue = minLimit;
                _maxLimitValue = maxLimit;
            }

            public Bar(string name, string units, float min, float max)
            {
                _name = name;
                _units = units;
                _maxValue = max;
                _minValue = min;                
            }            

            private string _name;
            private float _maxValue = 100;
            private float _minValue;
            private float _value;
            private float _maxLimitValue;
            private float _minLimitValue;
            private string _units;

            public string FullString
            {
                get { return _name + _value + _units; }
            }

            [Localizable(true)]
            public string Units
            {
                get { return _units; }
                set { _units = value; }
            }

            [Localizable(true)]
            public string Name
            {
                set 
                {
                    _name = value;
                    ChangedEvent();
                }
                get { return _name;  }
            }

            /// <summary>
            /// Максимальное значение на индикаторе
            /// </summary>
            public float MaxValue
            {
                set
                {
                    _maxValue = value;
                    ChangedEvent();
                }
                get { return _maxValue; }
            }

            /// <summary>
            /// Минимальное значение на индикаторе
            /// </summary>
            public float MinValue
            {
                set
                {
                    _minValue = value;
                    ChangedEvent();
                }
                get { return _minValue; }
            }

            /// <summary>
            /// Минимальное значение зоны "зелёной" подсветки
            /// </summary>
            public float MinLimitValue
            {
                set
                {
                    _minLimitValue = value;
                    ChangedEvent();
                }
                get { return _minLimitValue; }
            }
            /// <summary>
            /// Максимальное значение зоны "зелёной" подсветки
            /// </summary>
            public float MaxLimitValue
            {
                set
                {
                    _maxLimitValue = value;
                    ChangedEvent();
                }
                get { return _maxLimitValue; }
            }

            public float Value
            {
                get { return _value; }
                set
                {
                    float prevValue = _value;
                    _value = value;
                    //if (value < MinValue)
                    //    _value = MinValue;
                    //else
                    //    if (value > MaxValue)
                    //        _value = MaxValue;
                    //    else
                    //        _value = value;
                    if (prevValue != _value)
                        ChangedEvent();
                }
            }

            public bool IsRightValue
            {
                get
                {
                    if (Math.Abs(_minLimitValue - MaxLimitValue) > double.Epsilon)
                        return (_value >= _minLimitValue) && (_value <= _maxLimitValue);
                    return true;
                }
            }    
        
            public float Percentage
            {
                get
                {
                    if (_maxValue == _minValue)
                        return 1;
                    return (_value - _minValue) / (_maxValue - _minValue);
                }
            }

            private static void ChangedEvent()
            {
                if (Changed != null)
                    Changed();
            }
        }
        #endregion

        #region
        private readonly List<Bar> _bars = new List<Bar>();
        private int _maxTextLength = 13;
        private Color _successBar = Color.Green;
        private Color _failBar = Color.Red;
        private Color _succeessText = Color.GreenYellow;
        #endregion

        #region Features

        [Description("Коллекция отображаемых полосок"), Localizable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<Bar> Bars
        {
            get { return _bars; }
        }

        [Description("Максимально прогнозируемая длина текста слева")]
        public int MaxTextLength
        {
            get { return _maxTextLength; }
            set 
            {
                _maxTextLength = value; 
                Invalidate();
            }
        }

        [Description("Цвет бара при значении в допуске")]
        public Color SuccessBarColor
        {
            get { return _successBar; }
            set
            {
                _successBar = value;
                Invalidate();
            }
        }

        [Description("Цвет бара при значении вне допуска")]
        public Color FailBarColor
        {
            get { return _failBar; }
            set
            {
                _failBar = value;
                Invalidate();
            }
        }

        #endregion

        private void ValueChangedFunction()
        {
            if (ValueChanged != null)
                ValueChanged(this,new EventArgs());
        }

        public PowerIndicator()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            InitializeComponent();
            Bar.Changed += Invalidate;
            Bar.Changed += ValueChangedFunction;
            SizeChanged += Changed;
            //if (_bars.Count <= 0)
            //{
            //    _bars.Add(new Bar("U(27В)=", "В", 21, 33, 21.6f, 32.4f));
            //    _bars.Add(new Bar("I(27В)=", "А", 0, 12, 0.1f,12));
            //    _bars.Add(new Bar("U(115В)=", "В", 0, 130, 105, 129));
            //    _bars.Add(new Bar("I(115В)=", "А", 0, 5, 0.1f, 5));
            //}
        }

        private void Changed(object o, EventArgs e)
        {
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            base.OnPaint(e);
            FillBordersAndBackground(e.Graphics);            
            DrawStringsAndBars(e.Graphics);            
        }

        private void DrawStringsAndBars(Graphics graph)
        {
            PointF drawPoint = new PointF(StartBarsPosition, StartBarsPosition);
            StringBuilder sb = new StringBuilder();
            for (int i=0;i<_maxTextLength;i++)
                sb.Append("U");
            Size maxTextSize = TextRenderer.MeasureText(sb.ToString(), Font);
            Brush b;
            Pen p;            
            foreach (Bar bar in _bars)
            {
                b = new SolidBrush(bar.IsRightValue ? _succeessText : ForeColor);
                p = new Pen(b, VoidBarWidth);
                Size nameSize = TextRenderer.MeasureText(bar.FullString, Font);
                graph.DrawString(bar.FullString,Font,b,drawPoint);
                PointF dp = new PointF(ClientRectangle.Width - BordersWidth - BarsEndInterval, drawPoint.Y + nameSize.Height/2f);
                float xStartBar = drawPoint.X + maxTextSize.Width + BarsWidthInterval;
                graph.DrawLine(p, xStartBar, dp.Y, dp.X - 1, dp.Y);//-1 Непонятная вещь, если ширина линии 1-ца, то она выходит дальше прямоугольника
                Brush barBrush = new SolidBrush(bar.IsRightValue ? _successBar : _failBar);                
                graph.FillRectangle(barBrush, xStartBar, drawPoint.Y, (dp.X-xStartBar)*bar.Percentage, nameSize.Height);
                drawPoint.Y += nameSize.Height + BarsHeightInterval;
            }
        }        

        private void FillBordersAndBackground(Graphics graph)
        {
            RectangleF rf = new RectangleF(ClientRectangle.X+BordersWidth/2f,ClientRectangle.Y+BordersWidth/2f,ClientRectangle.Width-BordersWidth,ClientRectangle.Height-BordersWidth);
            GraphicsPath gp = GetRoundedRect(rf, RectangleRoundRadius);
            Brush b = new SolidBrush(Color.Black);
            Pen p = new Pen(Color.Gray,BordersWidth);
            graph.FillPath(b,gp);
            graph.DrawPath(p,gp);
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
    }
}