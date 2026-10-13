using System;
using KPK2.Common.DissFunctions.Diss32;

namespace KPK2.Common.TokKFunction.OneChannel.Events
{
	public class EventImageTok : EventArgs
	{
		public string Name { get; set; }
		public CheckType CheckType { get; set; }
	}
}
