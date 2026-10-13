using KPK2.GeneralForms.DissForms.Setting;
using Sparc.KPK2.Hardware.LTR;
using Sparc.KPK2.Hardware.MyClasseUSB;
using Sparc.KPK2.Hardware.PS115V;
using Sparc.KPK2.NFunctions.Enums;
using Sparc.KPK2.NFunctions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using KPK2.Common.DissFunctions.Diss32.MeasurementPerfomanceElement;
using UserControlsKPK2;

namespace KPK2.Common.DissFunctions.Diss32
{
	public class MeasurementPerformance
	{
		private const int _numberRow = 2;
		private const int _numberColumn = 8;

		private Modification _modification;
		private DevicesID _devicesID;
		private string _deviceNumb;
		private List<RadioButton> _chButtons;
		private IndicatorViewerCreator _indicatorViewerCreator;
		private Measure _measure;

		private int[,] _arVisibleWorkChecker =
		{
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, // 1-й индикатор 1-й ряд
			{ 0, 1, 1, 1, 1, 1, 1, 1, 0, 0 },
			{ 0, 0, 1, 0, 0, 0, 0, 0, 1, 1 },
			{ 0, 1, 0, 1, 1, 1, 1, 1, 0, 0 }, // 4-й индикатор
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
			{ 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 },
			{ 0, 1, 0, 1, 1, 1, 1, 1, 0, 0 }, // 7-й индикатор
			{ 0, 0, 0, 1, 1, 1, 1, 1, 0, 0 },
			{ 1, 1, 0, 1, 0, 0, 0, 1, 1, 1 }, // 1-й индикатор 2-й ряд
			{ 0, 1, 0, 0, 0, 0, 0, 1, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 0, 1, 1 },
			{ 0, 0, 0, 1, 0, 0, 0, 1, 0, 0 }, // 4-й индикатор 2-й ряд
			{ 1, 1, 0, 0, 0, 0, 0, 1, 1, 1 },
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 0, 1, 0, 1, 0, 0, 0, 1, 0, 0 }, // 7-й индикатор 2-й ряд
			{ 0, 1, 0, 0, 0, 0, 0, 1, 0, 0 }
		};

		private int[,] _arVisibleStandartChecker =
		{
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, // 1-й индикатор 1-й ряд
			{ 0, 1, 1, 1, 1, 1, 1, 1, 0, 0 },
			{ 0, 0, 1, 0, 0, 0, 0, 0, 1, 1 },
			{ 0, 1, 0, 1, 1, 1, 1, 1, 0, 0 }, // 4-й индикатор
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
			{ 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 },
			{ 0, 1, 0, 1, 1, 1, 1, 1, 0, 0 }, // 7-й индикатор
			{ 0, 0, 0, 1, 1, 1, 1, 1, 0, 0 },
			{ 0, 1, 0, 1, 0, 0, 0, 1, 1, 1 }, // 1-й индикатор 2-й ряд
			{ 0, 1, 0, 0, 0, 0, 0, 1, 0, 0 },
			{ 0, 1, 0, 0, 0, 0, 0, 0, 1, 1 },
			{ 0, 0, 0, 1, 0, 0, 0, 1, 0, 0 }, // 4-й индикатор 2-й ряд
			{ 0, 1, 0, 0, 0, 0, 0, 1, 1, 1 },
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 0, 1, 0, 1, 0, 0, 0, 1, 0, 0 }, // 7-й индикатор 2-й ряд
			{ 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 }
		};

		internal int[,] _arVisible { get; private set; }

		public event Action<object, double> EventIndicatorPowerU;
		public event Action<object, double> EventIndicatorPowerI;

		public Modification Modification
		{
			get => _modification;
		}

		public DevicesID DevicesId
		{
			get => _devicesID;
		}

		public string DeviceNumb
		{
			get => _deviceNumb;
		}

		public int ChannelState { get; set; } = 0;
		public SegmentIndicatorControl[] Indicators { get; } = new SegmentIndicatorControl[16];
		public double ChannelValue1 { get; set; } //Todo необходимость этого параметра тут?
		public double ChannelValue2 { get; set; } //Todo необходимость этого параметра тут?
		public string CurrentParam { get; set; } = "";
		public List<double> MeasResults { get; private set; }
		public List<double> DeltaResults { get; private set; }

		public MeasurementPerformance(DevicesID deviceID, Modification modification, string deviceNumber = "")
		{
			_devicesID = deviceID;
			_modification = modification;
			_deviceNumb = deviceNumber;
			_chButtons = new List<RadioButton>();
			_measure = SettingsObj.GetSettings().Measure = new Measure();
			var mode = SettingsObj.GetSettings().Mode = Mode.GetMode();
			if (mode.ModeApplication == ModeApplication.RepairCheck)
			{
				_arVisible = _arVisibleWorkChecker;
			}
			else
				_arVisible = _arVisibleStandartChecker;
		}

		public void Initialize(GroupBox groupBox)
		{
			const int baseX = 3;
			const int baseY = 12;
			const int stepX = 10;
            const int stepY = 2;

			const int baseWidth = 104;

			GroupBox grpB = groupBox;
			SegmentIndicatorControl indicator;
			for (int i = 0; i < _numberRow; i++)
				for (int j = 0; j < _numberColumn; j++)
				{
					indicator = new SegmentIndicatorControl();
					indicator.Name = $"indicator_{i}_{j}";
					indicator.Width = baseWidth;
					grpB.Controls.Add(indicator);
					indicator.Location = new Point(baseX + j * (stepX + indicator.Width),
						baseY + i * (stepY + indicator.Height));
					indicator.Visible = false;
					indicator.Font = new Font("Microsoft Sans Serif", 8.25f);
					Indicators[j + i * _numberColumn] = indicator;
				}
		}

		public void ViewIndicators(IndicatorViewerCreator creator, bool oneChannel = false)
		{
			_indicatorViewerCreator = creator;
			string[] isNotAdd =
			{
				"115V_Read_U",
				"Plus_115V_32(90A)_I",
				"Plus_115V_32_I",
				"Plus_27V_32(90A)_I",
				"Plus_27V_32_I"
			};
			SettingsObj.GetSettings().MeasList.Clear();

            var measureList = SettingsObj.GetSettings().MeasList;
			measureList.Clear();
			foreach (var iv in creator.Viewers)
			{
				var ind = GetIndicator(iv.IndicatorNumber);
				ind.SetValue(iv);
                ind.NameLabelUp = iv.NameLabelUp;
                ind.NameLabelDown = iv.NameLabelDown;
				if (isNotAdd.Contains(iv.ParameterName))
					continue;
				measureList.Add(iv.UsbBaseNew);
			}
		}

		public void HideIndicators()
		{
			for (int i = 0; i < Indicators.Length; i++)
				Indicators[i].Visible = false;
		}

		public void SetChannelIndicatorsVisible(int channel, bool state) //Todo необходимость данного метрода, возможно надо дбудет объяденить с SetIndicatorsVisibility
		{
			if (channel < 0)
				return;
			int ind = channel * 3;
			int ind2 = channel * 3 + 1;
			Indicators[ind].Visible = state;
			Indicators[ind2].Visible = state;
		}

		public void SetIndicatorsVisibility(CheckType checkType, bool oneChannelDiss)
		{
			HideIndicators();
			for (int i = 0; i < Indicators.Length; i++)
			{
				if (checkType == CheckType.Stop)
					break;
				if (oneChannelDiss && ((checkType == CheckType.Tok_K_On) || (checkType == CheckType.Tok_K_Off) ||
									   (checkType == CheckType.Tok_K) || (checkType == CheckType.SD) ||
									   (checkType == CheckType.Noise)))
					if (CheckIndNumber_OneChannel(i))
					{
						Indicators[i].Visible = false;
						continue;
					}

				Indicators[i].Visible = _arVisible[i, (int)checkType] == 1;
				Indicators[i].Inactive = true;
			}
		}

		public void SetIndicatorValue(int number, double val)
		{
			switch (number) //TODO пересмотреть конструкцию
			{
				case (int)Globals.IndicatorNumber.Power115_U:
					EventIndicatorPowerU?.Invoke(this, val);
					return;
				case (int)Globals.IndicatorNumber.Power115_I:
					EventIndicatorPowerI?.Invoke(this, val);
					return;
				case (int)Globals.IndicatorNumber.ChannelMeas1:
					ChannelValue1 = val;
					Indicators[0].Value = Indicators[3].Value = Indicators[6].Value = val;
					return;
				case (int)Globals.IndicatorNumber.ChannelMeas2:
					ChannelValue2 = val;
					Indicators[1].Value = Indicators[4].Value = Indicators[7].Value = val;
					return;
				case (int)Globals.IndicatorNumber.ChannelMeasCurrent1:
					ChannelValue1 = val;
					Indicators[8].Value = Indicators[11].Value = Indicators[14].Value = val;
					return;
				case (int)Globals.IndicatorNumber.ChannelMeasCurrent2:
					ChannelValue2 = val;
					Indicators[9].Value = Indicators[12].Value = Indicators[15].Value = val;
					return;
				default:
					if (number < 0 || number >= Indicators.Length)
						return;
					Indicators[number].Value = val;
					break;
			}
		}

		public virtual void ImageStop()
		{
			for (int i = 0; i < Indicators.Length; i++)
				Indicators[i].Inactive = true;
		}

		public double GetIndicatorValue(int number)
		{
			if (number < 0)
				return 0;
			return Indicators[number].Value;
		}

		public void SaveAll(List<USBBaseNew> measList)
		{
			MeasResults = new List<double>();
			for (int numResult = 0; numResult < measList.Count; numResult++)
			{
				MeasResults.Add(measList[numResult].Result);
			}
		}

		public void CalcDelta(List<USBBaseNew> measList)
		{
			DeltaResults = new List<double>();
			for (int numResult = 0; numResult < measList.Count; numResult++)
				DeltaResults.Add(Math.Abs(MeasResults[numResult]) - Math.Abs(measList[numResult].Result));
		}

		public void StartWorker()
		{
			_measure.StopMeasuring(false);
			_measure.StartWorkerWithList(SettingsObj.GetSettings().MeasList);
		}

		public void StopWorker()
		{
			_measure.StopMeasuring(SettingsObj.GetSettings().TestId != -1);
			if (Power115V.Instance.IsAktor)
				_measure.StartMeasuring();
		}

		public void StopWorker(bool save)
		{
			_measure.StopMeasuring(save);
			if (Power115V.Instance.IsAktor)
				_measure.StartMeasuring();
		}

		public void SaveToKByChannelStateAndNumParam(int numValue)
		{
			double tempValue = _measure.GetValue(string.Format(CurrentParam, 1, numValue));
			DataManager.Instance.SaveParameter(SettingsObj.GetSettings().TestId, string.Format(CurrentParam, ChannelState, numValue), tempValue);
		}

		public double[] GetValueChannel(int channel, int row)
		{
			int index = -1;
			if (channel == 1)
			{
				index = 0;
			}
			else if (channel == 2)
			{
				index = 3;
			}
			else if (channel == 3)
			{
				index = 6;
			}
			index += row == 1 ? 0 : 8;
			if (index == -1)
				return null;
			return new double[] { Indicators[index].Value, Indicators[index + 1].Value };
		}

		private SegmentIndicatorControl GetIndicator(int number)
		{
			int indexIndicatorOfChannel;
			if (ChannelState == 1)
				indexIndicatorOfChannel = 0;
			else if (ChannelState == 2)
				indexIndicatorOfChannel = 3;
			else if (ChannelState == 3)
				indexIndicatorOfChannel = 6;
			else
				indexIndicatorOfChannel = -1;
			switch ((Globals.IndicatorNumber)number)
			{
				case Globals.IndicatorNumber.ChannelMeas1:
					return Indicators[indexIndicatorOfChannel];
				case Globals.IndicatorNumber.ChannelMeas2:
					return Indicators[indexIndicatorOfChannel + 1];
				case Globals.IndicatorNumber.ChannelMeasCurrent1:
					return Indicators[indexIndicatorOfChannel + 8];
				case Globals.IndicatorNumber.ChannelMeasCurrent2:
					return Indicators[indexIndicatorOfChannel + 9];
			}
			if (number < Indicators.Length && number >= 0)
				return Indicators[number];
			return null;
		}

		/// <summary>
		/// Проверяет относится ли индикатор к текущему проверяемому каналу одноканального ДИСС
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		private bool CheckIndNumber_OneChannel(int i)
		{
			if (ChannelState == 0)
				return true;
			if ((ChannelState == 1) && (i != 0) && (i != 1) && (i != 8) && (i != 9))
				return true;
			if ((ChannelState == 2) && (i != 3) && (i != 4) && (i != 11) && (i != 12))
				return true;
			if ((ChannelState == 3) && (i != 6) && (i != 7) && (i != 14) && (i != 15))
				return true;
			return false;
		}
	}
}
