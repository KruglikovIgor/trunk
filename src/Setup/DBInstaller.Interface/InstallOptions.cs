namespace DBInstaller.Interface
{
	/// <summary>
	/// Опции установки Базы данных.
	/// </summary>
	public	class		InstallOptions
	{
		/// <summary>
		/// Признак: Принудительная Переустановка чистой БД.
		/// </summary>
		public		bool			Is_Reinstall				{ get; set; }

		/// <summary>
		/// Признак: Создание резервной копии БД.
		/// </summary>
		public		bool			Is_Backup					{ get; set; }

		/// <summary>
		/// Папка для создания резервных копий БД.
		/// </summary>
		public		string			Backup_Folder				{ get; set; }
	}
}