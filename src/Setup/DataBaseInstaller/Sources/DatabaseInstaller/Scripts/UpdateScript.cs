using System;
using System.Linq;
using Sparc.DatabaseInstaller.Sources.Tools.LinesNavigator;

namespace Sparc.DatabaseInstaller.Sources.DatabaseInstaller.Scripts
{
	/// <summary>
	/// Скрипт обновления БД.
	/// </summary>
	class UpdateScript : SimpleScript
	{
		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Символ комментария в скрипте.
		/// </summary>
		private const		string			Remark_Sign	=	"--";

		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Версия ОТ которой данный скрипт производит обновление.
		/// </summary>
		public	Version		From			{ get; private set; }

		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Версия ДО которой данный скрипт производит обновление.
		/// </summary>
		public	Version		To				{ get; private set; }


		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="name">Имя скрипта.</param>
		/// <param name="sql">SQL-код скрипта.</param>
		public UpdateScript (string name, string sql)
			:
			base (name, sql)
		{
			ParseHeader (sql);

			if (To <= From)
				throw	new Exception (string.Format ("Incorrect update script versions: From {0}, To {1}", From, To));
		}

		//--------------------------------------------------------------------------------------------------------------
		private void ParseHeader (string sql)
		{
			string[]	header_lines	=	sql.Split (new[] {'\n','\r'}, StringSplitOptions.RemoveEmptyEntries);

			try
			{
				var		lines		=	new LinesNavigator	(header_lines)
				{
					SkipCondition	=	s => string.IsNullOrWhiteSpace (s)
				};

				var patch_comment	=	Extract_Comment	(lines.Next_Line ());
				Name				+=	string.Format	(" '{0}'", patch_comment);

				From				=	Parse_Version	(lines.Next_Line (), "From");
				To					=	Parse_Version	(lines.Next_Line (), "To");
			}
			catch (Exception e)
			{
				string	header		=	header_lines.Aggregate ((res, cur) => res + Environment.NewLine + cur);

				throw	new Exception ("Incorrect update script header format: " + e.Message + Environment.NewLine + header, e);
			}
		}

		//--------------------------------------------------------------------------------------------------------------
		private string Extract_Comment (string str)
		{
			string	s	=	str.TrimStart ();

			if ( !s.StartsWith (Remark_Sign, StringComparison.Ordinal) )
				throw	new Exception ();

			return	s.Substring (Remark_Sign.Length, s.Length - Remark_Sign.Length).Trim ();
		}

		//--------------------------------------------------------------------------------------------------------------
		private Version Parse_Version (string str, string ver_name)
		{
			string		s		=	Extract_Comment (str);

			string[]	parts	=	s.Split (new[] {':'}, 2, StringSplitOptions.RemoveEmptyEntries);

			if (parts.Length != 2)
				throw	new Exception ();

			if (string.Compare (parts [0], ver_name, StringComparison.OrdinalIgnoreCase) != 0)
				throw	new Exception ();

			return	Version.Parse (parts [1]);
		}
	}
}