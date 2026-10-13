using System;
using System.Timers;

namespace Sparc.KPK2.Imitator
{
    public partial class Imitator
    {
        public class Diss3228M : Device
        {
          private int[] _arrAddr = { 139, 75, 203 };
          Timer tHigh = new Timer();
          private const int Task1Channel = 8;
          private const int Task2Channel = 9;
          private const int Task3Channel = 10;
          private const int Task4Channel = 11;

          private const int TokKChannel = 16;
          private const int HighChannel = 17;

          private bool _contrBSstate;
          private bool _contrVChstate;

          public Diss3228M()
          {
            Hardware.HImitator.DiscretInputs.SetInputState(21, true);
            tHigh.Interval = 10000;
            tHigh.AutoReset = false;
            tHigh.Elapsed += tHigh_Elapsed;
            tHigh.Start();
            Hardware.HImitator.PowerSupply115.SetExtResistance(409);
            //Hardware.HImitator.PowerSupply115.OutputChanged += OutputChanged;
            //Hardware.HImitator.PowerSupply115.VoltageChanged += VoltageChanged;
            Hardware.HImitator.PowerSupply40.VoltageChanged += PowerSupply40OnVoltageChanged;
            Hardware.HImitator.DiscretOutputs.ReleStateChanged += DiscretOutputsOnReleStateChanged;
            Hardware.HImitator.GenSinCos.AngleChanged += GenSinCosOnAngleChanged;
            RenewCodeArray();
            SetRefuses();
          }

          /// <summary>
          /// Отказы
          /// </summary>
          private void SetRefuses()
          {
              Hardware.HImitator.LTRCards.SetVoltage(11, 6, 12); //Отказ МЦО
              Hardware.HImitator.LTRCards.SetVoltage(11, 7, 12); //Отказ УСК
              Hardware.HImitator.LTRCards.SetVoltage(11, 10, 12); //Отказ МП
              Hardware.HImitator.LTRCards.SetVoltage(11, 9, 12); //Отказ МАО
          }

          private void RenewCodeArray()
          {
            if (!IsPower27Applied())
            {
              Hardware.HImitator.ControlTask.SetArrayValue(0, GetInvertedArray(0, 0));
              Hardware.HImitator.ControlTask.SetArrayValue(1, GetInvertedArray(1, 0));
              Hardware.HImitator.ControlTask.SetArrayValue(2, GetInvertedArray(0, 0));
              return;
            }
            if (ContrBSOn())
            {
              Hardware.HImitator.ControlTask.SetArrayValue(0, GetArray(0, _arrAddr[0], -500));
              Hardware.HImitator.ControlTask.SetArrayValue(1, GetArray(1, _arrAddr[1], 500));
              Hardware.HImitator.ControlTask.SetArrayValue(2, GetArray(2, _arrAddr[2], 250));
              return;
            }
            if (ContrVChOn())
            {
                Random  rnd = new Random(Environment.TickCount);
                Hardware.HImitator.ControlTask.SetArrayValue(0, GetArray(0, _arrAddr[0], rnd.Next(242, 258)));
                Hardware.HImitator.ControlTask.SetArrayValue(1, GetArray(1, _arrAddr[1], rnd.Next(492, 508)));
                Hardware.HImitator.ControlTask.SetArrayValue(2, GetArray(2, _arrAddr[2], rnd.Next(242, 258)));
                return;
            }
            Hardware.HImitator.ControlTask.SetArrayValue(0, GetInvertedArray(_arrAddr[0], 5));
            Hardware.HImitator.ControlTask.SetArrayValue(1, GetInvertedArray(_arrAddr[1], 10));
            Hardware.HImitator.ControlTask.SetArrayValue(2, GetInvertedArray(_arrAddr[2], 15));
          }

          private bool ContrVChOn()
          {
              return _contrVChstate;
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

          private bool ContrBSOn()
          {
              return _contrBSstate;
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

            private int SetBit(int result, int num, bool value)
            {
                if (!value)
                    return result;
                result |= 1 << num;
                return result;
            }

          private bool GetBit(int array, int i)
          {
              return (array >> i & 1) == 1;
          }

          private int GetArray(int addr, int correct)
          {
            int result = correct << 9 | addr;
            CalculateControlSum(ref result);
            return result;
          }

          private int GetInvertedArray(int addr, int correct)
          {
              int result = correct << 9 | addr;
              CalculateControlSum(ref result);
              return InvertArray(result);
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

          private void OutputChanged()
          {
            VoltageChanged();
            RenewCodeArray();
          }

          private void GenSinCosOnAngleChanged(ushort @ushort)
          {
            
          }


          private void SetCurrentControlTask()
          {
            if (Hardware.HImitator.ControlTask.GetCurrentTask() != 9)
              return;
            int task = 0;
            if (Hardware.HImitator.DiscretOutputs.GetOutput(Task1Channel))
              task = 1;
            if (Hardware.HImitator.DiscretOutputs.GetOutput(Task2Channel))
              task = 2;
            if (Hardware.HImitator.DiscretOutputs.GetOutput(Task3Channel))
              task = 3;
            if (Hardware.HImitator.DiscretOutputs.GetOutput(Task4Channel))
              task = 4;
            switch (task)
            {
              case 1:
              case 2:
                Hardware.HImitator.LTRCards.SetVoltage(10, 1, 17);
                Hardware.HImitator.LTRCards.SetVoltage(10, 3, 17);
                Hardware.HImitator.LTRCards.SetVoltage(10, 2, 3);

                //При обнулении
                Hardware.HImitator.LTRCards.SetVoltage(10, 4, 2);
                Hardware.HImitator.LTRCards.SetVoltage(10, 6, 2);
                Hardware.HImitator.LTRCards.SetVoltage(10, 13, 2);
                break;
              case 3:
                 Hardware.HImitator.LTRCards.SetVoltage(10, 13, 127);
                Hardware.HImitator.LTRCards.SetVoltage(10, 14, 0);
                Hardware.HImitator.LTRCards.SetVoltage(10, 5, 0.5);
                Hardware.HImitator.LTRCards.SetVoltage(12, 12, 128);
                break;
              case 4:
                Hardware.HImitator.LTRCards.SetVoltage(10, 13, 258);
                Hardware.HImitator.LTRCards.SetVoltage(10, 14, 9.5);
                Hardware.HImitator.LTRCards.SetVoltage(12, 12, 259);
                Hardware.HImitator.LTRCards.SetVoltage(10, 5, 9.6);
                break;
            }

          }

          private void DiscretOutputsOnReleStateChanged(int i)
          {
            SetCurrentK(IsPower27Applied());
            SetCurrentControlTask();
            VoltageChanged();
            RenewCodeArray();
            RenewIndicators();
          }

          private void RenewIndicators()
          {
            bool state = IsPower27Applied() && Hardware.HImitator.DiscretOutputs.GetOutput(3);
            Hardware.HImitator.DiscretInputs.SetInputState(0, state); // Захват1
            Hardware.HImitator.DiscretInputs.SetInputState(1, state); // Захват2
            Hardware.HImitator.DiscretInputs.SetInputState(2, state); //Захват3
            Hardware.HImitator.DiscretInputs.SetInputState(3, state); //Исправность
            Hardware.HImitator.DiscretInputs.SetInputState(12, state); //Захват
          }

          private bool IsPower27Applied()
          {
            double curVoltage = Hardware.HImitator.PowerSupply40.GetVoltage();
            curVoltage = Hardware.HImitator.PowerSupply40.isOn() ? curVoltage : 0;
            return  curVoltage >= 25 && curVoltage <= 29;
          }

          private void PowerSupply40OnVoltageChanged(double voltage)
          {
            Hardware.HImitator.LTRCards.SetVoltage(11,8, Hardware.HImitator.PowerSupply40.GetVoltage()/5);
            VoltageChanged();
          }

          private void VoltageChanged()
          {
            bool state = IsPower27Applied();
            SetIndicators(state);
            SetSecondaryPower(state);
            SetPM_MOD(state);
            SetSD(state);
            SetNoise(state);
            SetCurrentK(state);
            RenewCodeArray();
          }

          private void SetCurrentK(bool state)
          {
            double baseCurrent = 0.250;
            double baseCurrent2 = 0.251;
            int currentChannel = GetCurrentChannel(); //одноканальный ДИСС
            if (!Hardware.HImitator.DiscretOutputs.GetOutput(TokKChannel))
            {
              baseCurrent = 0;
              baseCurrent2 = 0;
            }
            if (!Hardware.HImitator.DiscretOutputs.GetOutput(HighChannel))
            {
              baseCurrent = 0.130 - currentChannel;
              baseCurrent2 = 0.130 + currentChannel;
            }
            if (currentChannel != 0)
              baseCurrent += currentChannel;
            for (int i = 6; i <= 11; i++)
              Hardware.HImitator.LTRCards.SetVoltage(13, i + 1, state ? (i%2==1? baseCurrent : baseCurrent2) : 0);
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
            double voltage = 1.5;
            if (ch2 && !ch1)
              voltage = 2.5;
            if (!ch2 && ch1)
              voltage = 1.5;
            if (ch2 && ch1)
              voltage = 2.5;
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
            for (int i = 0; i < 2; i++)
              Hardware.HImitator.LTRCards.SetVoltage(12, i + 1, state ? voltage : 0);
            for (int i = 2; i < 6; i++)
              Hardware.HImitator.LTRCards.SetVoltage(12, i + 1, state ? 1.1 : 0);
          }

          private void SetPM_MOD(bool state)
          {
            Hardware.HImitator.LTRCards.SetVoltage(12, 9, state ? 3 : 0);
            Hardware.HImitator.LTRCards.SetVoltage(12, 10, state ? 3 : 0);
            Hardware.HImitator.LTRCards.SetVoltage(12, 11, state ? -2.3 : 0);
            Hardware.HImitator.LTRCards.SetVoltage(12, 7, state ? 3.5 : 0);
            Hardware.HImitator.LTRCards.SetVoltage(12, 8, state ? -3.5 : 0);
          }

          private void SetSecondaryPower(bool state)
          {
            Hardware.HImitator.LTRCards.SetVoltage(11, 4, state? 12/2d : 0);
            Hardware.HImitator.LTRCards.SetVoltage(11, 5, state? -12/2d : 0);
            Hardware.HImitator.LTRCards.SetVoltage(11, 2, state? 6.3 : 0);
            Hardware.HImitator.LTRCards.SetVoltage(11, 3, state? -6.3 : 0);
            Hardware.HImitator.LTRCards.SetVoltage(10, 6, state? 9 : 0);
            Hardware.HImitator.LTRCards.SetVoltage(11, 13, state? 15/3d : 0);
            Hardware.HImitator.LTRCards.SetVoltage(11, 14, state? -15/3d : 0);
            Hardware.HImitator.LTRCards.SetVoltage(11, 15, state ? 5 : 0);
            Hardware.HImitator.LTRCards.SetVoltage(10, 16, state? 0.02 : 0);
            Hardware.HImitator.LTRCards.SetVoltage(11, 1, state? 5 : 0);
            //Hardware.HImitator.LTRCards.SetVoltage(11, 9, state? -27 : 0);
          }

          private void SetIndicators(bool state)
          {
            Hardware.HImitator.DiscretInputs.SetInputState(4, state);
            Hardware.HImitator.DiscretInputs.SetInputState(5, state);
            Hardware.HImitator.DiscretInputs.SetInputState(6, state);
            Hardware.HImitator.DiscretInputs.SetInputState(8, state);
            Hardware.HImitator.DiscretInputs.SetInputState(9, state);
            Hardware.HImitator.DiscretInputs.SetInputState(18, state);
            Hardware.HImitator.DiscretInputs.SetInputState(19, state);
            Hardware.HImitator.LTRCards.SetVoltage(11, 11, 3);
            Hardware.HImitator.LTRCards.SetVoltage(13, 6, 6); //Сигнал полёт
          }

          private void tHigh_Elapsed(object sender, ElapsedEventArgs e)
          {
                
          }

          public void SetContrBSState(bool state)
          {
              _contrBSstate = state;
              RenewCodeArray();
          }

          public void SetContrVChState(bool state)
          {
              _contrVChstate = state;
              RenewCodeArray();
          }
        }
    }
}
