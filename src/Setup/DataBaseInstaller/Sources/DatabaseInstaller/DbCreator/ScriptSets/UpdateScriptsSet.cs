using System;
using System.Collections.Generic;
using System.Linq;
using Sparc.DatabaseInstaller.Sources.DatabaseInstaller.Scripts;
using Sparc.DatabaseInstaller.Sources.ScriptLoader;


namespace	Sparc.DatabaseInstaller.Sources.DatabaseInstaller.DbCreator.ScriptSets
{
	/// <summary>
	/// Набор скриптов для обновления существующей БД.
	/// </summary>
	class	UpdateScriptsSet
	{
		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Скрипты.
		/// </summary>
		private	IEnumerable<UpdateScript>	_scripts;

		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Самая старшая версия до которой могут обновить скрипты БД.
		/// </summary>
		public	Version						HighVersion		{ get; private set; }
		
		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="scriptLoader">Загрузчик скриптов БД.</param>
		public								UpdateScriptsSet (IScriptLoader scriptLoader)
		{
			if (scriptLoader == null)
				throw	new ArgumentNullException (nameof(scriptLoader));

			var	updateScripts	=	scriptLoader.Load ().Select (si => new UpdateScript ("Apply patch " + si.Name, si.Text)).ToList ();
			if (updateScripts.Count == 0)
				throw	new Exception ("Update scripts not found");

			updateScripts.Sort (Comparison_by_FromVersion);
			Check_Version_Sequence (updateScripts);

			_scripts			=	updateScripts;
			HighVersion			=	updateScripts [updateScripts.Count - 1].To;
		}

		//--------------------------------------------------------------------------------------------------------------
		private static void					Check_Version_Sequence (List<UpdateScript>	updateScripts)
		{
			for (int i = 1; i < updateScripts.Count; ++i)
			{
				bool	isSameFromVer	=	updateScripts [i - 1].From == updateScripts [i].From;
				
				bool	isOverlap			=	updateScripts [i - 1].To > updateScripts [i].From;

				if (isSameFromVer || isOverlap)
					throw	new Exception ("Incorrect update scripts sequence");
			}
		}

		//--------------------------------------------------------------------------------------------------------------
		private static int					Comparison_by_FromVersion (UpdateScript x, UpdateScript y)
		{
			if (x.From < y.From)
				return	-1;

			if (x.From > y.From)
				return	1;

			return	0;
		}

		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Получить все скрипты для обновления БД от заданной версии.
		/// </summary>
		/// <param name="version">Версия БД.</param>
		/// <returns>Скрипты обновления БД.</returns>
		public	IEnumerable<IScript>		Get_Scripts_for_Update_From (Version version)
		{
			if (version == null)
				throw	new ArgumentNullException (nameof(version));

			return	_scripts.Where (s => s.From >= version);
		}
	}
}