using System;
using System.Windows.Forms;
using DBInstaller.Interface;

namespace Sparc.DatabaseInstaller.Sources.Logger
{
  /// <summary>
  ///   Логгер в TextBox/RichTextBox.
  /// </summary>
  public class TextBoxLogger : ILogger
  {
    //--------------------------------------------------------------------------------------------------------------
    private readonly TextBoxBase _textBox;

    //--------------------------------------------------------------------------------------------------------------
    public TextBoxLogger(TextBoxBase textBox)
    {
      _textBox = textBox ?? throw new ArgumentNullException(nameof(textBox));
    }

    //--------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Записать сообщение в Лог.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    public void Write(string message)
    {
      _textBox.AppendText(message + Environment.NewLine);
      _textBox.ScrollToCaret();
      _textBox.Refresh();
    }
  }
}