using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Hardware.LfGenerator.Akip._3410;
using Hardware.LfGenerator.Akip._3410.Common;
using KPK2.Common;
using Sparc.Hardware.Transport.UsbCh375;
using Sparc.KPK2.Hardware;
using Sparc.KPK2.Hardware.LTR;
using Sparc.KPK2.Hardware.MyClasseUSB;
using USBVoltmeter;

namespace KPK2.MyClasse
{
	public class AutoAttestationAlgorithm
	{
		private string _curChannel;
		private USBBaseNew _curUsbBaseNew;
		private Measure _measWorker;
		private double _measureResult;
		private DataTable _dtCheckTable;
		private ILfGeneratorAkip3410 _akip3410;
		private double _measureEt;

		public double MeasuredEt
		{
			get { return _measureEt; }
		}

		public double MeasuredIn
		{
			get { return _measureResult; }
		}

		public DataTable CheckTable
		{
			get { return _dtCheckTable; }
		}

		public bool IsUNChChannel
		{
			get { return _curChannel.Contains("UNCh"); }
		}

		public AutoAttestationAlgorithm(string channel)
		{
			_curChannel = channel;
			_curUsbBaseNew = new USBBaseNew(0, _curChannel, 1);
			PrepareMeasurer(_curUsbBaseNew);
			_dtCheckTable = AutoAttestationTableGenerator.GenerateTable(IsUNChChannel);
		}

		private void PrepareMeasurer(USBBaseNew measChannel)
		{
			_measWorker = new Measure();
			_measWorker.ResultsReady += MeasWorkerOnResultsReady;
			_measWorker.AddMeasure(measChannel);
			_measWorker.Init();
		}

		private void MeasWorkerOnResultsReady(List<USBBaseNew> usblist)
		{
			lock (_curUsbBaseNew)
			{
				if (usblist[0] == _curUsbBaseNew)
					_measureResult = usblist[0].Result;
				else
					throw new Exception("Invalid channel!");
			}
			_measureEt = Convert.ToDouble(V7_VoltmeterManagement.GetVoltmeter(0).Measure(), CultureInfo.InvariantCulture);
			RenewRow(_currentRow);
		}

		public void Start(Action<int> selectRow, Action<int> renewRow)
		{
			SelectRow = selectRow;
			RenewRow = renewRow;
			cToken = new CancellationToken();
			CheckDevices(IsUNChChannel);
			_measWorker.StartMeasuring();
			Task doTask = new Task(MainAlgorithmThread);
			doTask.Start();
		}

		private int _currentRow = 0;
		private Action<int> SelectRow;
		private Action<int> RenewRow;
		private CancellationToken cToken;


		private void MainAlgorithmThread()
		{
			while (!cToken.IsCancellationRequested)
			{
				var row = _dtCheckTable.Rows[_currentRow];
				SelectRow(_currentRow);
				if (row == null)
					break;
				if (IsUNChChannel)
					CheckUNCh(row);
				CheckRow(row);
				RenewRow(_currentRow);
			}
		}

		private void CheckRow(DataRow row)
		{
			double voltage = Globals.MoveToDouble(row[0].ToString());
			_akip3410.Voltage.SetA(voltage);
			Thread.Sleep(2000);
			_measureEt = Convert.ToDouble(V7_VoltmeterManagement.GetVoltmeter(0).Measure(), CultureInfo.InvariantCulture);
		}

		private void CheckUNCh(DataRow row)
		{
			double frequency = Globals.MoveToDouble(row[0].ToString());
			double voltage = Globals.MoveToDouble(row[1].ToString());
			_akip3410.Frequency.SetA(frequency);
			_akip3410.Voltage.SetA(voltage);
			Thread.Sleep(2000);
			_measureEt = Convert.ToDouble(V7_VoltmeterManagement.GetVoltmeter(0).Measure(), CultureInfo.InvariantCulture);
		}

		private void CheckDevices(bool isUnChChannel)
		{
			if (V7_VoltmeterManagement.VoltmetersCount <= 0)
				throw new Exception("Voltmeter not found!");
			if (!isUnChChannel)
				V7_VoltmeterManagement.GetVoltmeter(0).SetMeasureVoltageDC();
			else
				V7_VoltmeterManagement.GetVoltmeter(0).SetMeasureVoltageAC();

			try
			{
				_akip3410 = new LfGeneratorAkip3410(new UsbCh375Port(0, TimeSpan.Zero));
				_akip3410.Connect();
				_akip3410.Function.SetA(FunctA.Sin);
				_akip3410.Voltage.SetA(0.05);
				_akip3410.Output.SetA(true);
			}
			catch
			{
				throw new Exception("Generator not found!");
			}
		}

		public void Stop()
		{
			_measWorker.StopMeasuring(false);
		}
	}
}
