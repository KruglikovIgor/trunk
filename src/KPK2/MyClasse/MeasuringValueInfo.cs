using System;

namespace KPK2.MyClasse
{
	public class MeasuringValueInfo
	{
		private const float _voltage115 = 115f;
		private const double _voltage27 = 27.5;
		private const double _specSignalKoef = 1000;

		public float Voltage115
		{
			get => _voltage115;
		}

		public double Voltage27
		{
			get => _voltage27;
		}

		public double ZoptPNPStartValue { get; set; }

		public double CalculateDelta15(double voltage1, double voltage2)
		{
			double result = Math.Abs(voltage1 - voltage2) * _specSignalKoef;
			return result;
		}
	}
}
