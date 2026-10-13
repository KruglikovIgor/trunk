using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using DBInstaller.Interface;
using Sparc.DatabaseInstaller.Sources.DatabaseInstaller.Scripts;
using Sparc.DatabaseInstaller.Sources.Logger;
using Sparc.DatabaseInstaller.Sources.Tools;

namespace Sparc.DatabaseInstaller.Sources.DatabaseInstaller.DbBackuper
{
	/// <summary>
	/// Создатель резервных копий БД.
	/// </summary>
    class	DbBackuper
    {
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#region VARs, CTORs

		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Логгер.
		/// </summary>
		public ILogger								Logger
		{
			get		{	return	_logger;	}

			set		{	_logger	=	value ?? new NullLogger ();	}
		}
		ILogger										_logger		=	new NullLogger ();

		//--------------------------------------------------------------------------------------------------------------
		private		const string					_BAKUP_INDEX_FILE_NAME	=	"Bakup.idx";

		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Строка подключения БД.
		/// </summary>
		private		string							_connection_String;


		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="connection_string">Строка подключения БД.</param>
		public					DbBackuper (string connection_string)
		{
			if (connection_string == null)
				throw	new ArgumentNullException ("connection_string");

			_connection_String	=	connection_string;
		}
		
#endregion
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#region BACKUP

		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Создать резервную копию БД.
		/// </summary>
		/// <param name="db_names">Сохраняемые БД.</param>
		/// <param name="version">Версия БД.</param>
		/// <param name="folder_path">Путь к папке резервных копий БД.</param>
		/// <param name="folder_name">Имя папки с резервной копией БД.</param>
		/// <param name="ask_if_Fail_Backup_Target">Спрашивать пользователя при ошибках подготовки папки резервной копии БД?</param>
		public		void		Backup (IEnumerable<string> db_names, Version version, string folder_path, string folder_name, bool ask_if_Fail_Backup_Target)
		{
			if (db_names == null)
				throw	new ArgumentNullException ("db_names");
			if (db_names.Any (string.IsNullOrEmpty))
				throw	new ArgumentException ("db_names");

			if (folder_path == null)
				throw	new ArgumentNullException ("folder_path");
			if (folder_name == null)
				throw	new ArgumentNullException ("folder_name");


			string	real_folder	=	Prepare_Folder (folder_path, folder_name, ask_if_Fail_Backup_Target);

			Backup_Databases (db_names, real_folder);
			SaveBackupIndex	 (db_names, version, real_folder);
		}

		//--------------------------------------------------------------------------------------------------------------
		private		string		Prepare_Folder (string folder_path, string folder_name, bool ask_if_Fail_Backup_Target)
		{
			string	folder	=	null;

			while (true)
			{
				try
				{
					folder			=	Path.Combine (folder_path, folder_name);

					bool	is_ok	=	Try_Prepare_Folder (ref folder, ref folder_path, folder_name, ask_if_Fail_Backup_Target);
					if	(is_ok)
						return	folder;

					break;
				}
				catch (Exception exc)
				{
					if ( ask_if_Fail_Backup_Target )
					{
						string	msg		=	string.Format (	"Can not prepare folder:{0}{1}{0}{0}Details:{0}{2}{0}{0}Try select another folder.",
															Environment.NewLine,
															folder,
															exc.Message
															);
						MessageBox.Show (msg, "Database Installer", MessageBoxButtons.OK, MessageBoxIcon.Warning);

						folder_path		=	Select_Folder (folder_path);
						if (folder_path != null)
							continue;
					}

					throw	new ArgumentException (string.Format ("Fail to prepare folder:{0}{1}", Environment.NewLine, folder), exc);
				}
			}

			throw	new ArgumentException (string.Format ("Fail to prepare folder:{0}{1}", Environment.NewLine, folder));
		}

		//--------------------------------------------------------------------------------------------------------------
		private		bool		Try_Prepare_Folder (ref string folder, ref string folder_path, string folder_name, bool ask_if_Fail_Backup_Target)
		{
			while (true)
			{
				if ( !Directory.Exists (folder) )
				{
					var directoryInfo = Directory.CreateDirectory (folder);
					return	true;
				}

				if ( !ask_if_Fail_Backup_Target )
					throw	new ArgumentException ("Directory already exists.");

				var		dlg_res		=	MessageBox.Show
				(
					string.Format ("Directory already exist. All data in directory will be lost.{0}{0}Proceed?", Environment.NewLine),
					"Database Installer",
					MessageBoxButtons.YesNoCancel,
					MessageBoxIcon.Warning
				);

				if (dlg_res == DialogResult.Yes)
				{
					Directory.Delete (folder, true);
					Directory.CreateDirectory (folder);
					return	true;
				}

				if (dlg_res == DialogResult.No)
				{
					folder_path		=	Select_Folder (folder_path);
					if (folder_path != null)
					{
						folder		=	Path.Combine (folder_path, folder_name);
						continue;
					}
				}

				return	false;
			}
		}

		//--------------------------------------------------------------------------------------------------------------
		private		string		Select_Folder (string path)
		{
			string	selected_folder	=	null;

			new SyncSeparateThreadCaller (ApartmentState.STA, true)
			.RunMethod
			(
				() =>
				{
					selected_folder = Browse_Folder (path);
				}
			);

			return	selected_folder;
		}

		//--------------------------------------------------------------------------------------------------------------
		private		string		Browse_Folder (string path)
		{
			using (var dlg = new FolderBrowserDialog ())
			{
				dlg.Description		=	"Select backup folder";
				dlg.SelectedPath	=	path;

				var dlg_res			=	dlg.ShowDialog ();

				if (dlg_res == DialogResult.OK)
					return	dlg.SelectedPath;

				return	null;
			}
		}

		//--------------------------------------------------------------------------------------------------------------
		private		void		Backup_Databases (IEnumerable<string> db_names, string folder)
		{
			using (var conn = new SqlConnection (_connection_String))
			{
				conn.Open ();

				foreach (var db_nm in db_names)
				{
					string	file_name	=	Backup_Db_FileName (folder, db_nm);
					Backup_Db (db_nm, file_name, conn);
				}
			}
		}

		//--------------------------------------------------------------------------------------------------------------
		private		static		string		Backup_Db_FileName (string folder, string db_name)
        {
			return	Path.Combine (folder, db_name) + ".bak";
		}

		//--------------------------------------------------------------------------------------------------------------
		private		void		Backup_Db (string db_name, string file_name, DbConnection conn)
        {
			_logger.Write (string.Format ("Backup {0} to '{1}'", db_name, file_name));

            var script	=	new BackupDbScript (db_name, file_name);
			script.Execute (conn, null);
        }

		//--------------------------------------------------------------------------------------------------------------
		private void SaveBackupIndex (IEnumerable<string> db_names, Version version, string folder)
		{
			string	file_name	=	Backup_Index_FileName (folder);

			_logger.Write (string.Format ("Write backup index '{0}'", file_name));

			var		bkp_idx		=	new BackupIndex ();

			bkp_idx.Db_Version	=	version;
			bkp_idx.Db_Names.AddRange (db_names);

			bkp_idx.Save (file_name);
		}

		//--------------------------------------------------------------------------------------------------------------
		private		string		Backup_Index_FileName (string folder)
        {
			return	Path.Combine (folder, _BAKUP_INDEX_FILE_NAME);
		}

#endregion
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#region RESTORE

		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Восстановить БД из резервной копии.
		/// </summary>
		/// <param name="folder">Путь к резервной копии БД.</param>
		public		void		Restore (string folder)
		{
			if (folder == null)
				throw	new ArgumentNullException ("folder");

			_logger.Write (string.Format ("Restore form '{0}'", folder));

			var	file_name	=	Backup_Index_FileName (folder);
			_logger.Write (string.Format ("Read backup index '{0}'", file_name));

			var bkp_idx		=	BackupIndex.FromFile (file_name);

			Restore_Databases (folder, bkp_idx.Db_Names);
		}

		//--------------------------------------------------------------------------------------------------------------
		private		void		Restore_Databases (string folder, IEnumerable<string> db_names)
		{
			using (var conn = new SqlConnection (_connection_String))
			{
				conn.Open ();

				foreach (var db_nm in db_names)
					Restore_Db (folder, db_nm, conn);
			}
		}

		//--------------------------------------------------------------------------------------------------------------
		private		void		Restore_Db (string folder, string db_name, DbConnection conn)
        {
			string	file_name	=	Backup_Db_FileName (folder, db_name);

			_logger.Write (string.Format ("Restore {0} from '{1}'", db_name, file_name));
			
            var script	=	new RestoreDbScript (db_name, file_name);
			script.Execute (conn, null);
        }

#endregion
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	}
}
