using System.Runtime.InteropServices;
using Sparc.KPK2.NFunctions;

namespace KPK2.DB
{
	public class DataBaseKPK2
	{
		private int _checkerId = -1;

		public bool SaveResults
		{
			get { return (_checkerId != -1); }
		}

		public DataBaseKPK2(int testId)
		{
			_checkerId = testId;
		}

		public bool ManualRecordDouble(string str, double value)
		{
			if (double.IsNaN(value))
				return true;
			return DataManager.Instance.SaveParameter(_checkerId, str, value);
		}
	}
}
