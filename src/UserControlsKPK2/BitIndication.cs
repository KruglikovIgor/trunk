using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace UserControlsKPK2
{
  public partial class BitIndication : UserControl
  {
    public BitIndication()
    {
      InitializeComponent();
    }

    private Color _BGColor = Color.Black;

    public Color BGColor
    {
      get { return _BGColor; }
      set { 
        _BGColor = value;
        Invalidate();
      }
    }

    private Color _borderColor = Color.Gray;

    public Color BorderColor
    {
      get { return _borderColor; }
      set { 
        _borderColor = value;
        Invalidate();
      }
    }

    private int _numBits = 1;

    public int NumBits
    {
      get { return _numBits; }
      set
      {
        if (value > 0)
        {
          _numBits = value;
          Size = new Size(_baseWidth + _baseStep*_numBits, Height);
          Invalidate();
        }
      }
    }

    private int _startBitNumber = 1;
    public int StartBitNumber
    {
      get { return _startBitNumber; }
      set { _startBitNumber = value;
        Invalidate();
      }
    }

    private int _value;

    public int Value
    {
      get { return _value; }
      set { 
        _value = value; 
        Invalidate();
      }
    }

    private Color _trueColor = Color.LimeGreen;

    public Color TrueColor
    {
      get { return _trueColor; }
      set { _trueColor = value;
      Invalidate();}
    }

    private Color _failColor = Color.Red;

    public Color FailColor
    {
      get { return _failColor; }
      set { _failColor = value;
      Invalidate();}
    }

    private bool _okState = false;
    public bool OKState
    {
      get { return _okState; }
    }

    public class BitState
    {
      public enum State
      {
        One,
        Zero,
        Inactive,
        Formula
      }

      public delegate bool FormulaDelegate(BitArray ba);

      public State CurrentState = State.Inactive;
      public FormulaDelegate Formula = null;
    }

    private Dictionary<int, BitState> _mask = new Dictionary<int, BitState>();

    
    public void SetMask(Dictionary<int, BitState> mask)
    {
      _mask = mask;
    }

    private const  int _baseWidth = 10;
    private int _baseStep = 20;

    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);
      e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
      const int bordersWidth = 3;
      const int rectangleRoundRadius = 3;
      Graphics grfx = e.Graphics;
      RectangleF myClientRectangle = new RectangleF(ClientRectangle.X + bordersWidth / 2f, ClientRectangle.Y - ClientRectangle.Y/4 + bordersWidth / 2f, ClientRectangle.Width - bordersWidth, ClientRectangle.Height - ClientRectangle.Height/4 - bordersWidth);
      RectangleF rf = new RectangleF(myClientRectangle.X, myClientRectangle.Y, myClientRectangle.Width, myClientRectangle.Height);
      GraphicsPath gp = GetRoundedRect(rf, rectangleRoundRadius);
      Brush b = new SolidBrush(_BGColor);
      Pen p = new Pen(_borderColor, bordersWidth);
      //Заполнение фона
      grfx.FillPath(b, gp);
      //Отрисовка рамки 
      grfx.DrawPath(p, gp);
      DrawValue(e.Graphics);
    }

    private int _space = 0;

    /// <summary>
    /// Отрисовка значений и номеров битов
    /// </summary>
    /// <param name="g"></param>
    private void DrawValue(Graphics g)
    {
      BitArray ba = new BitArray(_numBits);
      int xBase = 0;
      int yBase = 0;
      bool result = true;
      for (int i = 0; i < ba.Length; i++)
      {
        int realNum = i + (_startBitNumber - 1);
        int checkValue = (int)Math.Pow(2, realNum);
        ba[i] = (checkValue & _value) == checkValue;
        string strToDraw = ba[i] ? "1" : "0";
        Size szText = TextRenderer.MeasureText(g, strToDraw, Font);
        if (xBase < szText.Width/4)
          xBase += szText.Width / 8 + _space;
        Color curColor = GetCurrentBitColor(i, ba, realNum);
        result &= curColor != _failColor;
        g.DrawString(strToDraw, Font, new SolidBrush(curColor), new Point(xBase, yBase), StringFormat.GenericTypographic);
        strToDraw = (realNum + 1).ToString();
        Size szPlace = TextRenderer.MeasureText(g, strToDraw, new Font(Font.FontFamily, Font.Size - 20));
        g.DrawString(strToDraw, new Font(Font.FontFamily, Font.Size - 20), new SolidBrush(ForeColor), xBase + szText.Width/4 - szPlace.Width/3, ClientRectangle.Height - szPlace.Height);
        xBase += szText.Width/2 + _space;
      }
      _okState = result;
    }

    private Color GetCurrentBitColor(int index, BitArray ba, int maskIndex)
    {
      if (!_mask.ContainsKey(maskIndex))
        return _trueColor;
      BitState bState = _mask[maskIndex];
      if (bState.CurrentState == BitState.State.Inactive)
        return _borderColor;
      if (bState.CurrentState == BitState.State.Formula)
        if (bState.Formula != null)
          return bState.Formula(new BitArray(BitConverter.GetBytes(_value))) == ba[index] ? _trueColor : _failColor;
      return (!(ba[index] ^ (bState.CurrentState == BitState.State.One))) ? _trueColor : _failColor;
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

    public void ChangeBitMask(int bitNum, bool state)
    {
      //_mask[bitNum] = state ? BitState.State.One : BitState.State.Zero;
    }
  }
}
