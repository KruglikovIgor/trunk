using System.Data;

namespace KPK2.MyClasse
{
	public static class AutoAttestationTableGenerator
	{
		public static DataTable GenerateTable(bool isUNCh)
		{
			if (isUNCh)
				return GenerateUNChTable();
			return GenerateTable();
		}

		private static DataTable GenerateUNChTable()
		{
			DataTable dt = new DataTable();
			const int numCol = 5;
			for (int i = 0; i < numCol; i++)
				dt.Columns.Add("", typeof(string));

			double[] voltages = new[] {0.05, 0.1, 0.15, 0.2, 0.5, 1, 2, 3};
			double[] freqs = new[] {20d, 100, 1000, 6000};

			foreach(var freqItem in freqs)
			foreach (var voltageItem in voltages)
				dt.Rows.Add(freqItem, voltageItem, null, null, null);

			return dt;
		}

		private static DataTable GenerateTable()
		{
			DataTable dt = new DataTable();
			const int numCol = 4;
			for (int i = 0; i < numCol; i++)
				dt.Columns.Add("", typeof(string));

			double[] voltages = new[] { 0.05, 0.1, 0.5, 1, 10, 25, 49 };

			foreach (var voltageItem in voltages)
				dt.Rows.Add( voltageItem, null, null, null);

			for (int i=voltages.Length-1;i>= 0; i--)
				dt.Rows.Add(voltages[i], null, null, null);

			return dt;
		}


	}
}
