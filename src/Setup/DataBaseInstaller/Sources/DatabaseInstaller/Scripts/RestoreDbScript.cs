using System;

namespace Sparc.DatabaseInstaller.Sources.DatabaseInstaller.Scripts
{
	/// <summary>
	/// Скрипт для Восстановления Резервной копии БД.
	/// </summary>
	class RestoreDbScript : SimpleScript
	{
		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="db_name">Имя БД.</param>
		/// <param name="backup_file_name">Имя файла резервной копии БД.</param>
		public RestoreDbScript (string db_name, string backup_file_name)
		{
			if (string.IsNullOrEmpty(db_name))
				throw	new ArgumentException ("db_name");
			if (string.IsNullOrEmpty(backup_file_name))
				throw	new ArgumentException ("backup_file_name");
			
			Sql		=	string.Format (@"RESTORE DATABASE [{0}] FROM DISK = N'{1}' WITH REPLACE", db_name, backup_file_name);
		}
	}
}