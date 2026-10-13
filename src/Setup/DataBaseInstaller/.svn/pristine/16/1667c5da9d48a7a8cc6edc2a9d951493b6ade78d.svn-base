using System;
using System.Collections.Generic;
using System.IO;
using Sparc.DatabaseInstaller.Sources.Tools.LinesNavigator;


namespace Sparc.DatabaseInstaller.Sources.DatabaseInstaller.DbBackuper
{
	/// <summary>
	/// Индекс резервной копии БД.
	/// </summary>
	class	BackupIndex
	{
		//--------------------------------------------------------------------------------------------------------------
		private		const	string		Version_NA	=	"NA";

		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Версия БД.
		/// </summary>
		public		Version				Db_Version		{ get; set; }

		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Перечень БД резервной копии.
		/// </summary>
		public		List<string>		Db_Names		{ get; private set; }


		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Конструктор.
		/// </summary>
		public							BackupIndex ()
		{
			Db_Names	=	new List<string> ();
		}

		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Создать Индекс резервной копии БД на основе данных загруженных из файла.
		/// </summary>
		/// <param name="file_name">Имя файла.</param>
		/// <returns>Индекс резервной копии БД.</returns>
		public static BackupIndex		FromFile (string file_name)
		{
			var		bkp_idx		=	new BackupIndex ();
			bkp_idx.Load (file_name);

			return	bkp_idx;
		}

		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Загрузить Индекс резервной копии БД.
		/// </summary>
		/// <param name="file_name">Имя файла.</param>
		public		void				Load (string file_name)
		{
			if (file_name == null)
				throw	new ArgumentNullException ("file_name");


			var		text_lines	=	File.ReadAllLines	(file_name);
			var		lines		=	new LinesNavigator	(text_lines)
			{
				SkipCondition	=	s => string.IsNullOrWhiteSpace (s)
			};

			if ( !lines.Next () )
				throw	new Exception (string.Format ("File '{0}' is corrupted.", file_name));

			if (lines.Current.Trim () == Version_NA)
				throw	new Exception (string.Format ("File '{0}' does not contain database version.", file_name));

			Db_Version	=	new Version (lines.Current);

			Db_Names.Clear ();

			while (lines.Next ())
				Db_Names.Add (lines.Current);
		}

		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Сохранить Индекс резервной копии БД.
		/// </summary>
		/// <param name="file_name">Имя файла.</param>
		public		void				Save (string file_name)
		{
			if (file_name == null)
				throw	new ArgumentNullException ("file_name");

			var lines	=	new List<string> ();

			lines.Add		(Version_String (Db_Version));
			lines.AddRange	(Db_Names);

			File.WriteAllLines (file_name, lines);
		}

		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Получить строковое представление версии БД.
		/// </summary>
		/// <param name="version">Версия БД.</param>
		/// <returns>Строковое представление версии БД.</returns>
		public	static	string			Version_String (Version version)
		{
			return	(version == null)
						?	Version_NA
						:	version.ToString ();
		}
	}
}
