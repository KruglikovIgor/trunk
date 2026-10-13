using System;
using System.Collections.Generic;

namespace Sparc.DatabaseInstaller.Sources.DatabaseInstaller
{
	/// <summary>
	/// Информация об установленной Базе данных.
	/// </summary>
	public	class		DbInfo
	{
		/// <summary>
		/// Обнаруженные БД.
		/// </summary>
		public		IEnumerable<string>		Installed_Databases			{ get; set; }

		/// <summary>
		/// Признак: В целом БД считается установленной?
		/// </summary>
		public		bool					Is_Overall_Installed		{ get; set; }

		/// <summary>
		/// Текущая версия БД.
		/// </summary>
		public		Version					Current_Version				{ get; set; }
	}
}