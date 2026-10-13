using System;
using System.Threading;

namespace	Sparc.DatabaseInstaller.Sources.Tools
{
	/// <summary>
	/// Синхронный вызывальщик метода в отдельном потоке.
	/// </summary>
	public	class	SyncSeparateThreadCaller
	{
		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Данные Запуска метода в отдельном потоке.
		/// </summary>
		class		RunMethodData
		{
			public		Action			Method			{ get; private set; }
			public		Exception		RisedException	{ get; set; }

			public		RunMethodData (Action method)
			{
				if (method == null)
					throw	new ArgumentNullException ("method");

				Method				=	method;
				RisedException		=	null;
			}
		}

		//--------------------------------------------------------------------------------------------------------------
		private	readonly ApartmentState		_apartmentState;
		private	readonly bool				_isBackground;

		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="apartmentState">Состояние апартамента потока в котором будет вызван метод.</param>
		/// <param name="isBackground">Поток выполнения метода фоновый?</param>
		public								SyncSeparateThreadCaller (ApartmentState apartmentState, bool isBackground)
		{
			_apartmentState	=	apartmentState;
			_isBackground	=	isBackground;
		}

		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Выполнить метод в отдельном потоке.
		/// </summary>
		/// <param name="method">Выполняемый метод.</param>
		/// <remarks>
		/// Все исключения возникшие при исполнении метода в отдельном потоке
		/// транслируются в вызывающий поток.
		/// </remarks>
		public	void						RunMethod (Action method)
        {
			var		thread		=	new Thread (Run);

			thread.SetApartmentState (_apartmentState);
			thread.IsBackground	=	_isBackground;
			thread.Name			=	"SyncSeparateThreadCaller";

			var		rmd			=	new RunMethodData (method);
			thread.Start (rmd);

			thread.Join ();

			if (rmd.RisedException != null)
				throw	rmd.RisedException;
        }

		//--------------------------------------------------------------------------------------------------------------
		private	void						Run (object runMethodData)
        {
			var		rmd	=	(RunMethodData) runMethodData;

			try
			{
				rmd.Method ();
			}
			catch (Exception	exc)
			{
				rmd.RisedException	=	exc;
			}
		}
	}
}
