using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace UserControlsKPK2
{
  public partial class TextIndicator : UserControl
  {

    private Color _offColor = Color.Gray;
    private Color _onColor = Color.LightGreen;
    private int _borderWidth = 3;
    private string _text;
    private bool _isOn;
    private Color _borderColor = Color.Gray;
    [Localizable(true)]
    public string VisualText
    {
      get { return _text; }
      set
      {
        _text = value;
        OnChangeProperties();
      }
    }

    public Color OnColor
    {
      get { return _onColor; }
      set
      {
        _onColor = value;
        OnChangeProperties();
      }
    }

    public Color OffColor
    {
      get { return _offColor; }
      set
      {
        _offColor = value;
        OnChangeProperties();
      }
    }

    public Color BorderColor
    {
      get { return _borderColor; }
      set
      {
        _borderColor = value;
        OnChangeProperties();
      }
    }

    public bool State
    {
      get { return _isOn; }
      set
      {
        if (_isOn != value)
        {
          _isOn = value;
          OnChangeProperties();
        }
      }
    }

    private Color TextColor
    {
      get { return _isOn ? _onColor : _offColor; }
    }

    private void OnChangeProperties()
    {
      Invalidate();
    }


    public TextIndicator()
    {
      InitializeComponent();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
      base.OnPaint(e);
      Pen p = new Pen(_borderColor, _borderWidth);
      Brush bText = new SolidBrush(TextColor);
      Brush bBackground = new SolidBrush(ForeColor);
      RectangleF rf = new RectangleF(ClientRectangle.X + _borderWidth/2, ClientRectangle.Y + _borderWidth/2,
                                     ClientRectangle.Width - _borderWidth, ClientRectangle.Height - _borderWidth);
      GraphicsPath gp = GetRoundedRect(rf, 4);
      e.Graphics.FillPath(bBackground, gp);
      e.Graphics.DrawPath(p, gp);
      if (String.IsNullOrEmpty(_text))
        return;
      Size s = TextRenderer.MeasureText(_text, Font);
      Regex reg = new Regex(@".*\d+.*");
      bool containNumber = reg.IsMatch(_text);
      e.Graphics.DrawString(_text, Font, bText, (int) (ClientRectangle.X + ClientRectangle.Width/2d - s.Width/2d) + (containNumber? 2 : 0),
                            (int) (ClientRectangle.Y + ClientRectangle.Height/2d - s.Height/2d));
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
      if (_radius >= (Math.Min(baseRect.Width, baseRect.Height))/2.0)
      {
        return GetCapsule(baseRect);
      }

      // create the arc for the rectangle sides and declare 
      // a graphics path object for the drawing 
      float diameter = _radius*2.0f;
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
