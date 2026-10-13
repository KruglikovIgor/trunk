using System.Data.Common;

namespace Sparc.DatabaseInstaller.Sources.DatabaseInstaller.Scripts
{
	/// <summary>
	/// Интерфейс скрипта БД.
	/// </summary>
	interface IScript
	{
		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Имя скрипта.
		/// </summary>
		string		Name		{ get; }
		
		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// SQL-код скрипта.
		/// </summary>
		string		Sql			{ get; }

		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Выполнить скрипт.
		/// </summary>
		/// <param name="connection">Соединение с БД на котором должен быть выполнен скрипт.</param>
		/// <param name="transaction">Транзакция в которой должен быть выполнен скрипт.</param>
		void		Execute (DbConnection connection, DbTransaction transaction);
	}
}