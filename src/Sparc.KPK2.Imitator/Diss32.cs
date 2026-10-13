using System;
using System.Timers;

namespace Sparc.KPK2.Imitator
{
    public partial class Imitator
    {
	    public class Diss32 : Device
	    {
		    private const double bigSpeedKoef = -39.84;
		    private const double smallSpeedKoef = -3.59;
		    private const double US_Koef = -2.2;

		    private Timer tHigh = new Timer();
		    private const int Task1Channel = 8;
		    private const int Task2Channel = 9;
		    private const int Task3Channel = 10;
		    private const int Task4Channel = 11;

		    private const int TokKChannel = 16;
		    private const int HighChannel = 17;

		    private const int Task1BaseX = 17;
		    private const int Task1BaseZ = 17;
		    private const int Task1BaseY = 3;

		    private int[] _arrAddr = { 139, 75, 203 };

			public Diss32()
		    {
			    Hardware.HImitator.DiscretInputs.SetInputState(21, true);
			    tHigh.Interval = 10000;
			    tHigh.AutoReset = false;
			    tHigh.Elapsed += tHigh_Elapsed;
			    tHigh.Start();
			    Hardware.HImitator.PowerSupply115.OutputChanged += OutputChanged;
			    Hardware.HImitator.PowerSupply115.VoltageChanged += VoltageChanged;
			    Hardware.HImitator.PowerSupply40.VoltageChanged += PowerSupply40OnVoltageChanged;
			    Hardware.HImitator.DiscretOutputs.ReleStateChanged += DiscretOutputsOnReleStateChanged;
			    Hardware.HImitator.GenSinCos.AngleChanged += GenSinCosOnAngleChanged;
			    Hardware.HImitator.ControlTask.ControlTaskChanged += ControlTaskOnControlTaskChanged;
		    }

		    private void ControlTaskOnControlTaskChanged(int i)
		    {
			    Hardware.HImitator.DiscretInputs.SetInputState(3, i != 0);
		    }

		    private void OutputChanged()
		    {
			    VoltageChanged();
		    }

		    private void GenSinCosOnAngleChanged(ushort @ushort)
		    {

		    }


		    private void SetCurrentControlTask()
		    {
			    //if (ControlTask.GetCurrentTask() != 9)
//            return;
			    int task = GetCurrentTask();

			    switch (task)
			    {
				    case 1:
					    Hardware.HImitator.DiscretOutpus1760X.SetOutput(4, false); //Навигация
					    Hardware.HImitator.LTRCards.SetVoltage(10, 1, -4.71); //Wx ПКВ
					    Hardware.HImitator.LTRCards.SetVoltage(10, 3, -4.71); //Wz ПКВ
					    Hardware.HImitator.LTRCards.SetVoltage(10, 2, -3.1); //Wy НКВ

					    Hardware.HImitator.LTRCards.SetVoltage(10, 4, 0.47); //Wx НКВ
					    Hardware.HImitator.LTRCards.SetVoltage(10, 6, 1.57); //Wz НКВ
					    Hardware.HImitator.LTRCards.SetVoltage(10, 13, 2);


					    Hardware.HImitator.LTRCards.SetVoltage(10, 7, -0.52); //Wx СПСВ
					    Hardware.HImitator.LTRCards.SetVoltage(10, 8, 2.65); //Wy СПСВ
					    Hardware.HImitator.LTRCards.SetVoltage(10, 9, -0.52); //Wz СПСВ

					    Hardware.HImitator.LTRCards.SetVoltage(10, 10, 0.8); //Wx Тестер
					    Hardware.HImitator.LTRCards.SetVoltage(10, 11, 2.07); //Wy Тестер
					    Hardware.HImitator.LTRCards.SetVoltage(10, 12, 3.47); //Wz Тестер
					    break;
				    case 2:
					    Hardware.HImitator.DiscretOutpus1760X.SetOutput(4, false); //Навигация
					    Hardware.HImitator.LTRCards.SetVoltage(10, 1, 4.71); //Wx ПКВ
					    Hardware.HImitator.LTRCards.SetVoltage(10, 3, 4.71); //Wz ПКВ
					    Hardware.HImitator.LTRCards.SetVoltage(10, 2, 3.1); //Wy НКВ

					    Hardware.HImitator.LTRCards.SetVoltage(10, 4, -0.47);
					    Hardware.HImitator.LTRCards.SetVoltage(10, 6, -1.57);
					    Hardware.HImitator.LTRCards.SetVoltage(10, 13, 2);


					    Hardware.HImitator.LTRCards.SetVoltage(10, 7, 0.52); //Wx СПСВ
					    Hardware.HImitator.LTRCards.SetVoltage(10, 8, -2.65); //Wy СПСВ
					    Hardware.HImitator.LTRCards.SetVoltage(10, 9, 0.52); //Wz СПСВ

					    Hardware.HImitator.LTRCards.SetVoltage(10, 10, 0.29); //Wx Тестер
					    Hardware.HImitator.LTRCards.SetVoltage(10, 11, 3.93); //Wy Тестер
					    Hardware.HImitator.LTRCards.SetVoltage(10, 12, 2.53); //Wz Тестер

                        Hardware.HImitator.LTRCards.SetVoltage(12, 16, 1.5); //Z ПКВ
                        Hardware.HImitator.LTRCards.SetVoltage(11, 16, 1.7); //Z ПНП

                        break;
				    case 3:
					    Hardware.HImitator.DiscretOutpus1760X.SetOutput(4, true); //Навигация
					    Hardware.HImitator.LTRCards.SetVoltage(10, 13, 127 / bigSpeedKoef);
					    Hardware.HImitator.LTRCards.SetVoltage(10, 14, 0);
					    Hardware.HImitator.LTRCards.SetVoltage(10, 5, 0.5);
					    Hardware.HImitator.LTRCards.SetVoltage(12, 12, 128 / bigSpeedKoef);
					    break;
				    case 4:
					    Hardware.HImitator.DiscretOutpus1760X.SetOutput(4, true); //Навигация
					    Hardware.HImitator.LTRCards.SetVoltage(10, 13, 258 / bigSpeedKoef);
					    Hardware.HImitator.LTRCards.SetVoltage(10, 14, 9.5 / US_Koef);
					    Hardware.HImitator.LTRCards.SetVoltage(12, 12, 259 / bigSpeedKoef);
					    Hardware.HImitator.LTRCards.SetVoltage(10, 5, 9.6 / US_Koef);
					    break;
				    default:
					    //При обнулении

					    Hardware.HImitator.LTRCards.SetVoltage(10, 1, -4.71 / 1000d); //Wx ПКВ
					    Hardware.HImitator.LTRCards.SetVoltage(10, 3, -4.71 / 1000d); //Wz ПКВ

                        // В допуске
                        Hardware.HImitator.LTRCards.SetVoltage(10, 4, 2.47 / 1000d); //Wx НКВ
                        Hardware.HImitator.LTRCards.SetVoltage(10, 6, 9.57 / 1000d); //Wz НКВ
                        Hardware.HImitator.LTRCards.SetVoltage(10, 2, -21.1 / 1000d); //Wy НКВ

                        Hardware.HImitator.LTRCards.SetVoltage(10, 13, 2.81 / 1000d); //W НКВ

                        //Не в допуске
                        //Hardware.HImitator.LTRCards.SetVoltage(10, 4, 10.2 / 1000d); //Wx НКВ
                        //Hardware.HImitator.LTRCards.SetVoltage(10, 6, 15.1 / 1000d); //Wz НКВ
                        //Hardware.HImitator.LTRCards.SetVoltage(10, 2, 30.7 / 1000d); //Wy НКВ

                        //Hardware.HImitator.LTRCards.SetVoltage(10, 13, 10.4 / 1000d); //W НКВ

                        break;
			    }

		    }

		    private int GetCurrentTask()
		    {
			    if (Hardware.HImitator.ControlTask.GetCurrentTask() == 0)
				    return 0;
			    if (Hardware.HImitator.DiscretOutputs.GetOutput(Task1Channel))
				    return 1;
			    if (Hardware.HImitator.DiscretOutputs.GetOutput(Task2Channel))
				    return 2;
			    if (Hardware.HImitator.DiscretOutputs.GetOutput(Task3Channel))
				    return 3;
			    if (Hardware.HImitator.DiscretOutputs.GetOutput(Task4Channel))
				    return 4;
			    return 0;
		    }

		    private void DiscretOutputsOnReleStateChanged(int i)
		    {
			    SetCurrentK(IsPower115Applied());
			    SetCurrentControlTask();
			    VoltageChanged();
		    }

		    private bool IsPower115Applied()
		    {
			    double curVoltage = Hardware.HImitator.PowerSupply115.GetVoltage();
			    curVoltage = Hardware.HImitator.PowerSupply115.OutputState() ? curVoltage : 0;
			    return curVoltage >= 109 && curVoltage <= 121;
		    }

		    private void PowerSupply40OnVoltageChanged(double voltage)
		    {
			    Hardware.HImitator.LTRCards.SetVoltage(11, 8, Hardware.HImitator.PowerSupply40.GetVoltage());
		    }

		    private void VoltageChanged()
		    {
			    bool state = IsPower115Applied();
			    SetIndicators(state);
			    SetSecondaryPower(state);
			    SetGLPD_MOD(state);
			    SetSD(state);
			    SetNoise(state);
			    SetCurrentK(state);
		    }

		    private void SetCurrentK(bool state)
		    {
			    double baseCurrent = 0.249;
			    double baseCurrent2 = 0.25;
			    int currentChannel = GetCurrentChannel(); //одноканальный ДИСС
			    if (!Hardware.HImitator.DiscretOutputs.GetOutput(TokKChannel))
			    {
				    baseCurrent = 0;
				    baseCurrent2 = 0;
			    }

			    if (!Hardware.HImitator.DiscretOutputs.GetOutput(HighChannel))
			    {
				    baseCurrent = (0.13) - currentChannel;
				    baseCurrent2 = (0.13) + currentChannel;
			    }

			    if (currentChannel != 0)
				    baseCurrent += currentChannel;
			    for (int i = 6; i <= 11; i++)
				    Hardware.HImitator.LTRCards.SetVoltage(13, i + 1,
					    state ? (i % 2 == 1 ? baseCurrent : baseCurrent2) : 0);
		    }

		    private int GetCurrentChannel()
		    {
			    bool state1 = Hardware.HImitator.DiscretOutputs.GetOutput(15);
			    bool state2 = Hardware.HImitator.DiscretOutputs.GetOutput(14);
			    if (state1 && state2)
				    return 3;
			    if (state1)
				    return 2;
			    if (state2)
				    return 1;
			    return 0;
		    }

		    private void SetNoise(bool state)
		    {
			    bool ch2 = Hardware.HImitator.DiscretOutputs.GetOutput(14);
			    bool ch1 = Hardware.HImitator.DiscretOutputs.GetOutput(15);
			    double voltage = 0.04;
			    if (ch2 && !ch1)
				    voltage = 0.041;
			    if (!ch2 && ch1)
				    voltage = 0.042;
			    if (ch2 && ch1)
				    voltage = 0.043;
			    for (int i = 0; i < 4; i++)
				    Hardware.HImitator.LTRCards.SetVoltage(14, i + 1, state ? voltage : 0);
			    for (int i = 0; i < 2; i++)
				    Hardware.HImitator.LTRCards.SetVoltage(15, i + 1, state ? voltage : 0);
		    }

		    private void SetSD(bool state)
		    {
			    bool ch2 = Hardware.HImitator.DiscretOutputs.GetOutput(14);
			    bool ch1 = Hardware.HImitator.DiscretOutputs.GetOutput(15);
			    double voltage = 1.2;
			    if (ch2 && !ch1)
				    voltage = 1.3;
			    if (!ch2 && ch1)
				    voltage = 1.4;
			    if (ch2 && ch1)
				    voltage = 1.5;
			    for (int i = 0; i < 6; i++)
				    Hardware.HImitator.LTRCards.SetVoltage(12, i + 1, state ? voltage : 0);
		    }

		    private void SetGLPD_MOD(bool state)
		    {
			    Hardware.HImitator.LTRCards.SetVoltage(12, 9, state ? 3 : 0);
			    Hardware.HImitator.LTRCards.SetVoltage(12, 10, state ? 3 : 0);
			    Hardware.HImitator.LTRCards.SetVoltage(12, 11, state ? -2.3 : 0);
			    Hardware.HImitator.LTRCards.SetVoltage(12, 7, state ? 3.5 : 0);
			    Hardware.HImitator.LTRCards.SetVoltage(12, 8, state ? -3.5 : 0);
		    }

		    private void SetSecondaryPower(bool state)
		    {
			    Hardware.HImitator.LTRCards.SetVoltage(11, 4, state ? 12.6/2 : 0);
			    Hardware.HImitator.LTRCards.SetVoltage(11, 5, state ? -12.6/2 : 0);
			    Hardware.HImitator.LTRCards.SetVoltage(11, 2, state ? 6.3 : 0);
			    Hardware.HImitator.LTRCards.SetVoltage(11, 3, state ? -6.3 : 0);
			    Hardware.HImitator.LTRCards.SetVoltage(11, 6, state ? -20/2.9 : 0);
			    Hardware.HImitator.LTRCards.SetVoltage(11, 13, state ? 15/3 : 0);
			    Hardware.HImitator.LTRCards.SetVoltage(11, 14, state ? -15/3 : 0);
			    Hardware.HImitator.LTRCards.SetVoltage(10, 16, state ? 0.005 : 0);
			    Hardware.HImitator.LTRCards.SetVoltage(11, 1, state ? 5 : 0);
			    Hardware.HImitator.LTRCards.SetVoltage(11, 9, state ? -27/4.9 : 0);
			    Hardware.HImitator.LTRCards.SetVoltage(11, 10, state ? 10.0/2 : 0);
		    }

		    private void SetIndicators(bool state)
		    {
			    int task = GetCurrentTask();
			    bool refuseVCH = Hardware.HImitator.DiscretOutputs.GetOutput(17) && state;
			    Hardware.HImitator.DiscretInputs.SetInputState(10, refuseVCH); //Отказ ВЧ

			    Hardware.HImitator.DiscretInputs.SetInputState(0, task != 0); //Захват 1
			    Hardware.HImitator.DiscretInputs.SetInputState(1, task != 0); //Захват 2
			    Hardware.HImitator.DiscretInputs.SetInputState(2, task != 0); //Захват 3
			    Hardware.HImitator.DiscretInputs.SetInputState(3, task != 0 && !refuseVCH); //Исправность
			    Hardware.HImitator.DiscretInputs.SetInputState(4, task == 0 && state); //Память1
			    Hardware.HImitator.DiscretInputs.SetInputState(5, task == 0 && state); //Память2
			    Hardware.HImitator.DiscretInputs.SetInputState(9, state && !refuseVCH); //Полет
			    Hardware.HImitator.DiscretInputs.SetInputState(12, task != 0); //Захват
			    Hardware.HImitator.DiscretInputs.SetInputState(18, state); //Дисс ВКЛ
			    Hardware.HImitator.DiscretInputs.SetInputState(19,
				    !Hardware.HImitator.DiscretOutputs.GetOutput(17) && state); //Высокое


			    Hardware.HImitator.DiscretOutpus1760X.SetOutput(4, task == 3 || task == 4); //Навигация
			    Hardware.HImitator.DiscretOutpus1760X.SetOutput(3, state); //Откл. коорд.
			    Hardware.HImitator.DiscretOutpus1760X.SetOutput(2, state); //Испр. БВК
			    Hardware.HImitator.DiscretOutpus1760X.SetOutput(1, state); //Откл. Блок 8
			    Hardware.HImitator.DiscretInputs.SetInputState(22, state); //Не отказ БВК

			    Hardware.HImitator.LTRCards.SetVoltage(11, 11, 3);
		    }

		    public static void PushSearchButton(bool state)
		    {
			    Hardware.HImitator.DiscretInputs.SetInputState(0, !state); //Захват 1
			    Hardware.HImitator.DiscretInputs.SetInputState(1, !state); //Захват 2
			    Hardware.HImitator.DiscretInputs.SetInputState(2, !state); //Захват 3
			    Hardware.HImitator.DiscretInputs.SetInputState(4, state); //Память1
			    Hardware.HImitator.DiscretInputs.SetInputState(5, state); //Память2
			    Hardware.HImitator.DiscretInputs.SetInputState(12, !state); //Захват
		    }

		    private void tHigh_Elapsed(object sender, ElapsedEventArgs e)
		    {

		    }

		    /// <summary>
		    /// Установка крена
		    /// </summary>
		    /// <param name="p"></param>
		    /// <param name="isPlusCorner"></param>
		    public void SetHeel(bool state, bool isPlusCorner)
		    {
			    if (state)
			    {
				    Hardware.HImitator.LTRCards.SetVoltage(10, 3, Task1BaseZ + 2 * (isPlusCorner ? 1 : -1));
				    Hardware.HImitator.LTRCards.SetVoltage(10, 2, Task1BaseY + 2 * (isPlusCorner ? -1 : 1));
			    }
			    else
			    {
				    Hardware.HImitator.LTRCards.SetVoltage(10, 3, Task1BaseZ);
				    Hardware.HImitator.LTRCards.SetVoltage(10, 2, Task1BaseY);
			    }
		    }

		    /// <summary>
		    /// Утсановка тангажа
		    /// </summary>
		    /// <param name="p"></param>
		    /// <param name="isPlusCorner"></param>
		    public void SetPitch(bool state, bool isPlusCorner)
		    {
			    if (state)
			    {
				    Hardware.HImitator.LTRCards.SetVoltage(10, 1, Task1BaseX + 2 * (isPlusCorner ? 1 : -1));
				    Hardware.HImitator.LTRCards.SetVoltage(10, 2, Task1BaseY + 2 * (isPlusCorner ? -1 : 1));
			    }
			    else
			    {
				    Hardware.HImitator.LTRCards.SetVoltage(10, 1, Task1BaseX);
				    Hardware.HImitator.LTRCards.SetVoltage(10, 2, Task1BaseY);
			    }
		    }

		    public void SetMS(bool state)
		    {
			    Hardware.HImitator.LTRCards.SetVoltage(10, 13, state ? -6.6 : -6.5);
		    }

		    public void SetMB(bool state)
		    {
			    Hardware.HImitator.LTRCards.SetVoltage(10, 13, state ? -6.55 : -6.5);
		    }

		    public void SetBPKWork()
		    {
				Hardware.HImitator.ControlTask.SetFrequency(new System.Collections.Generic.List<int>() { 250, 250, 500, 500, 250, 250 });
		    }

			private int GetArray(int index, int addr, int frequency)
		    {
			    int result = GetArray(addr, 0);
			    result |= frequency << 15;
			    if (frequency < 0)
				    result |= 1 << 28;
			    result |= 1 << 8;
			    if (index == 0 && ContrBSOn())
				    result |= 15 << 27;
			    CalculateControlSum(ref result);
			    return InvertArray(result);
		    }

		    private int GetArray(int addr, int correct)
		    {
			    int result = correct << 9 | addr;
			    CalculateControlSum(ref result);
			    return result;
		    }

			private void CalculateControlSum(ref int result)
		    {
			    result &= ~(1 << 31);
			    int calc = result;
			    bool control = true;
			    while (calc != 0)
			    {
				    if ((calc & 1) == 1)
					    control = !control;
				    calc >>= 1;
			    }
			    if (control)
				    result |= 1 << 31;
		    }

		    private bool ContrBSOn()
		    {
			    return false;
		    }

			private int InvertArray(int array)
		    {
			    int result = 0;
			    for (int i = 0; i < 32; i++)
			    {
				    result = SetBit(result, 31 - i, GetBit(array, i));
			    }
			    return result;
		    }
		    private bool GetBit(int array, int i)
		    {
			    return (array >> i & 1) == 1;
		    }

			private int SetBit(int result, int num, bool value)
		    {
			    if (!value)
				    return result;
			    result |= 1 << num;
			    return result;
		    }
		}
    }
}
