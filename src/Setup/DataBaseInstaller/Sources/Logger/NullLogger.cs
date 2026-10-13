using DBInstaller.Interface;

namespace Sparc.DatabaseInstaller.Sources.Logger
{
	/// <summary>
	/// "Пустой" Логгер.
	/// </summary>
	public class	NullLogger : ILogger
	{
		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Записать сообщение в Лог.
		/// </summary>
		/// <param name="message">Сообщение.</param>
		public void		Write (string message)
		{
			//	do nothing
		}
	}
}
