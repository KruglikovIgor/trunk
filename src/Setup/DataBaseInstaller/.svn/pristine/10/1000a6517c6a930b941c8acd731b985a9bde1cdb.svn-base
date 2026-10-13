using System;

namespace Sparc.DatabaseInstaller.Sources.DatabaseInstaller.Scripts
{
	/// <summary>
	/// Скрипт для Создания Резервной копии БД.
	/// </summary>
	class BackupDbScript : SimpleScript
	{
		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="db_name">Имя БД.</param>
		/// <param name="backup_file_name">Имя файла резервной копии БД.</param>
		public BackupDbScript (string db_name, string backup_file_name)
		{
			if (string.IsNullOrEmpty(db_name))
				throw	new ArgumentException ("db_name");
			if (string.IsNullOrEmpty(backup_file_name))
				throw	new ArgumentException ("backup_file_name");

			Sql		=	string.Format (@"BACKUP DATABASE [{0}] TO DISK = N'{1}' WITH FORMAT", db_name, backup_file_name);
		}
	}
}