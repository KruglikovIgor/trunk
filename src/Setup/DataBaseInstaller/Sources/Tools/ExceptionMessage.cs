using System;
using System.Text;

namespace	Sparc.DatabaseInstaller.Sources.Tools
{
	/// <summary>
	/// Формирователь сообщений из исключения.
	/// </summary>
	public	class	ExceptionMessage
	{
		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Короткое сообщение.
		/// </summary>
		public		string				Short		{ get; private set; }

		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Полное сообщение.
		/// </summary>
		public		string				Full		{ get; private set; }


		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="exception">Исключение.</param>
		public										ExceptionMessage (Exception exception)
		{
			if (exception == null)
				throw	new ArgumentNullException ("exception");

			Short	=	Create_Short_ErrorMessage (exception);
			Full	=	Create_Full_ErrorMessage  (exception);
		}

		//--------------------------------------------------------------------------------------------------------------
		private static string			Create_Short_ErrorMessage (Exception exc)
        {
            var sb	=	new StringBuilder ();

			sb.AppendLine ("ERROR:");
			sb.AppendLine (exc.Message);
			sb.AppendLine ();
			sb.AppendLine ("DETAILS:");

			var	curExc	=	exc.InnerException;
			if (curExc == null)
			{
				sb.AppendLine ("Not available");
			}
			else
			{
				while (curExc != null)
				{
					sb.AppendLine (curExc.Message);
					curExc	=	curExc.InnerException;
				}
			}

			return	sb.ToString ();
        }

		//--------------------------------------------------------------------------------------------------------------
		private static string			Create_Full_ErrorMessage (Exception exc)
        {
			return	string.Format ("{1}{0}DEBUG INFO:{0}{2}", Environment.NewLine, Create_Short_ErrorMessage (exc), exc);
		}
	}
}