using System;
using Microsoft.Win32;

namespace Sparc.DatabaseInstaller.Sources.Tools
{
	/// <summary>
	/// Читатель/Писатель Версии БД.
	/// </summary>
    static class	DbVersion
    {
		//--------------------------------------------------------------------------------------------------------------
		//private	const string	KeyPath		=	@"HKEY_LOCAL_MACHINE\Software\Sparc\KPK-6";
		//private	const string	ParamName	=	@"DatabaseVersion";

		//--------------------------------------------------------------------------------------------------------------
		public	static	Version		Read (string KeyPath, string ParamName)
		{
			try
			{
				var		verVal		=	Registry.GetValue (KeyPath, ParamName, null);
				if (verVal == null)
					return	null;

				return	new Version ((string) verVal);
			}
			catch (Exception e)
			{
				throw	new Exception ("Can't read database version", e);
			}
		}

		//--------------------------------------------------------------------------------------------------------------
		public	static	void		Write (Version ver, string KeyPath, string ParamName)
		{
			try
			{
				Registry.SetValue (KeyPath, ParamName, ver.ToString ());
			}
			catch (Exception e)
			{
				throw	new Exception ("Can't write database version", e);
			}
		}
	}
}