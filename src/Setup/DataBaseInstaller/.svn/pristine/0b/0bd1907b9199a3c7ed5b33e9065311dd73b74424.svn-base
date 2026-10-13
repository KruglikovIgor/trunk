using System;

namespace Sparc.DatabaseInstaller.Sources.Tools.LinesNavigator
{
	static		class		LinesNavigatorExtensions
	{
		//--------------------------------------------------------------------------------------------------------------
		public		static		string			Next_Line (this LinesNavigator	lines)
		{
			if (lines.Next ())
				return	lines.Current;

			throw	new InvalidOperationException ();
		}
	}
}
