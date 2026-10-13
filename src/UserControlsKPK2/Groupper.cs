using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace UserControlsKPK2
{
  public partial class Groupper : UserControl
  {
    public Groupper()
    {
      InitializeComponent();
    }
    
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public override string Text
    {
      get
      {
        return base.Text;
      }
      set
      {
        base.Text = value;
        Invalidate();
      }
    }

    private int _lineWidth = 1;

    public int LineWidth
    {
      get { return _lineWidth; }
      set { _lineWidth = value;
      Invalidate();}
    }

    private bool _wrapByWord;

    public bool WrapByWord
    {
      get { return _wrapByWord; }
      set { _wrapByWord = value;
      Invalidate();}
    }

    private bool _ignoreAddWhiteSpaces;

    public bool IgnoreAddWhiteSpaces
    {
      get { return _ignoreAddWhiteSpaces; }
      set { _ignoreAddWhiteSpaces = value;
      Invalidate();}
    }

    private float XCenter
    {
      get { return ClientRectangle.Width/2f; }
    }

    private float YCenter
    {
      get { return ClientRectangle.Height / 2f; }
    }

    

    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);
      string[] sWrapped = new[] { Text };
      Size tSize = new Size();
      List<Size> tAllSizes = new List<Size>();
      float fOneSymbol = 0;
      if (_wrapByWord)
        sWrapped = Text.Split(' ');
      
      for (int i=0;i<sWrapped.Length;i++)
      {
        Size curSize = TextRenderer.MeasureText(e.Graphics, sWrapped[i], Font);
        if (IgnoreAddWhiteSpaces)
          curSize = new Size(curSize.Width - (curSize.Width/sWrapped[i].Length/2), curSize.Height);
        tAllSizes.Add(curSize);
        if (curSize.Width > tSize.Width)
        {
          tSize = curSize;
          fOneSymbol = tSize.Width / (sWrapped[i].Length + 1f);
        }
      }
      if (IgnoreAddWhiteSpaces)
        fOneSymbol = 0;
      Graphics g = e.Graphics;
      Brush bStr = new SolidBrush(ForeColor);
      float YStart = 0;
      PointF pMaxStrLocation = new PointF();
      for (int i = 0; i < sWrapped.Length; i++)
      {
        PointF pStrLocation = new PointF(XCenter - tAllSizes[i].Width / 2f + fOneSymbol / 2f, YStart);
        if (tAllSizes[i] == tSize)
          pMaxStrLocation = pStrLocation;
        g.DrawString(sWrapped[i], Font, bStr, pStrLocation);
        YStart += tAllSizes[i].Height;
      }

      if (sWrapped.Length > 1)
        tSize.Height *= sWrapped.Length;

      Pen pLines = new Pen(bStr, _lineWidth);
      PointF pleftLineStart  = new PointF(pMaxStrLocation.X-fOneSymbol, pMaxStrLocation.Y + tSize.Height / 2f);
      PointF pleftLineP2  = new PointF(pLines.Width, pleftLineStart.Y);
      PointF pleftLineP3 = new PointF(pleftLineP2.X, ClientRectangle.Height - pLines.Width);
      g.DrawLines(pLines, new[] { pleftLineStart, pleftLineP2, pleftLineP3});

      PointF prightLineStart = new PointF(pMaxStrLocation.X + tSize.Width, pMaxStrLocation.Y + tSize.Height / 2f);
      PointF prightLineP2 = new PointF(ClientRectangle.Width - pLines.Width, prightLineStart.Y);
      PointF prightLineP3 = new PointF(prightLineP2.X, ClientRectangle.Height - pLines.Width);

      g.DrawLines(pLines, new[] {prightLineStart, prightLineP2, prightLineP3});
      

    }
  }
}
