using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Sparc.KPK2.Imitator
{
    public partial class Imitator
    {
        public class Diss15 : Device
        {
            private Timer tHigh = new Timer();
            private bool _timeHighPast = false;

            private static Diss15 _currentInstance = null;

            public static Diss15 CurrentInstance
            {
                get { return _currentInstance; }
            }

            public Diss15()
            {
                Hardware.HImitator.DiscretInputs.SetInputState(21, true);
                Hardware.HImitator.DiscretInputs.SetInputState(1, true);
                tHigh.Interval = 10000;
                tHigh.AutoReset = false;
                tHigh.Elapsed += tHigh_Elapsed;
                Hardware.HImitator.PowerSupply115.VoltageChanged += VoltageChanged;
                Hardware.HImitator.ControlTask.ControlTaskChanged += ControlTaskOnControlTaskChanged;
                Hardware.HImitator.PowerSupply40.VoltageChanged += PowerSupply40OnVoltageChanged;
                Hardware.HImitator.PowerSupply40.StateChanged += PowerSupply40OnStateChanged;
                Hardware.HImitator.DiscretOutpus1760X.OutputStateChanged += DiscretOutpus1760XOnOutputStateChanged;
                Hardware.HImitator.DiscretOutputs.ReleStateChanged += DiscretOutputsOnReleStateChanged;
                _currentInstance = this;
            }

            private void DiscretOutputsOnReleStateChanged(int i)
            {
                if (_timeHighPast)
                    Hardware.HImitator.DiscretInputs.SetInputState(20,
                        IsPower() && !Hardware.HImitator.DiscretOutputs.GetOutput(4));
                ControlTaskOnControlTaskChanged(Hardware.HImitator.ControlTask.GetCurrentTask());
            }

            public void SetMapCorner(bool state)
            {
                Thread.Sleep(250);
                Hardware.HImitator.DiscretInputs.SetInputState(0, state);
            }

            private void DiscretOutpus1760XOnOutputStateChanged(int i)
            {
                ControlTaskOnControlTaskChanged(Hardware.HImitator.ControlTask.GetCurrentTask());
            }

            private void PowerSupply40OnStateChanged(bool state)
            {
                ReSetPS40V();
            }

            private void PowerSupply40OnVoltageChanged(double voltage)
            {
                ReSetPS40V();
            }

            private void ReSetPS40V()
            {
                Hardware.HImitator.LTRCards.SetVoltage(11, 8, Hardware.HImitator.PowerSupply40.GetVoltage());
                if (!IsPower())
                    _timeHighPast = false;
            }

            private void ControlTaskOnControlTaskChanged(int taskNum)
            {
                if (!IsPower())
                {
                    Hardware.HImitator.LTRCards.SetVoltage(10, 1, 0);
                    Hardware.HImitator.LTRCards.SetVoltage(10, 2, 0);
                    Hardware.HImitator.LTRCards.SetVoltage(10, 3, 0);
                    Hardware.HImitator.LTRCards.SetVoltage(10, 4, 0);
                    Hardware.HImitator.LTRCards.SetVoltage(10, 5, 0);
                    Hardware.HImitator.LTRCards.SetVoltage(10, 6, 0);
                    Hardware.HImitator.LTRCards.SetVoltage(10, 13, 0);
                    Hardware.HImitator.LTRCards.SetVoltage(10, 14, 0);
                    return;
                }

                SetStaticVoltagesUNCH();
                double voltageX7;
                switch (taskNum)
                {
                    case 1:
                        Hardware.HImitator.LTRCards.SetVoltage(10, 1, 17);
                        Hardware.HImitator.LTRCards.SetVoltage(10, 2, 5);
                        Hardware.HImitator.LTRCards.SetVoltage(10, 12, 6.5);
                        voltageX7 = 0;
                        if (Hardware.HImitator.DiscretOutpus1760X.GetRelayState(4))
                            voltageX7 = -6;
                        Hardware.HImitator.LTRCards.SetVoltage(13, 7, voltageX7);
                        Hardware.HImitator.LTRCards.SetVoltage(12, 3, -0.116); //СУ
                        Hardware.HImitator.LTRCards.SetVoltage(12, 4, -3.2); //СУ
                        Hardware.HImitator.LTRCards.SetVoltage(12, 5, -2.84); //СУ
                        break;
                    case 2:
                        Hardware.HImitator.LTRCards.SetVoltage(10, 1, 17);
                        Hardware.HImitator.LTRCards.SetVoltage(10, 3, 17);

                        Hardware.HImitator.LTRCards.SetVoltage(10, 4, 0);
                        Hardware.HImitator.LTRCards.SetVoltage(10, 5, 0);
                        Hardware.HImitator.LTRCards.SetVoltage(10, 6, 0);
                        Hardware.HImitator.LTRCards.SetVoltage(12, 5, 0.145); //СУ
                        Hardware.HImitator.LTRCards.SetVoltage(10, 10, -0.65); //Ландыш
                        Hardware.HImitator.LTRCards.SetVoltage(10, 11, 0.65); //Ландыш
                        voltageX7 = 0;
                        if (Hardware.HImitator.DiscretOutpus1760X.GetRelayState(5))
                            voltageX7 = -9;
                        Hardware.HImitator.LTRCards.SetVoltage(13, 7, voltageX7);
                        break;
                    case 3:
                        SetSignalsTask3();
                        break;
                    case 4:
                        SetSignalsTask4();
                        break;
                    case 5:
                        SetSignalsTask5();
                        break;
                    case 6:
                        Hardware.HImitator.LTRCards.SetVoltage(10, 13, 215);
                        Hardware.HImitator.LTRCards.SetVoltage(10, 14, -18);
                        Hardware.HImitator.LTRCards.SetVoltage(11, 13, -17.6);
                        Hardware.HImitator.LTRCards.SetVoltage(12, 1, -6.4);
                        Hardware.HImitator.LTRCards.SetVoltage(12, 3, -0.112); //СУ
                        Hardware.HImitator.LTRCards.SetVoltage(12, 5, 0.145); //СУ
                        voltageX7 = 0;
                        if (Hardware.HImitator.DiscretOutpus1760X.GetRelayState(3))
                            voltageX7 = -0.75;
                        Hardware.HImitator.LTRCards.SetVoltage(13, 7, voltageX7);
                        Hardware.HImitator.LTRCards.SetVoltage(10, 10, 8); //Ландыш
                        Hardware.HImitator.LTRCards.SetVoltage(10, 11, -2.4); //Ландыш
                        break;
                }
            }

            private void SetSignalsTask3()
            {
                double voltageX7;
                Hardware.HImitator.LTRCards.SetVoltage(10, 2, 5);
                Hardware.HImitator.LTRCards.SetVoltage(10, 12, -6.5);
                Hardware.HImitator.LTRCards.SetVoltage(12, 4, -3.309); //СУ
                voltageX7 = 0;
                if (Hardware.HImitator.DiscretOutpus1760X.GetRelayState(4))
                    voltageX7 = 6;
                else if (Hardware.HImitator.DiscretOutpus1760X.GetRelayState(5))
                    voltageX7 = 0;
                Hardware.HImitator.LTRCards.SetVoltage(13, 7, voltageX7);
                Hardware.HImitator.LTRCards.SetVoltage(10, 12, 6.5); //САУ
            }

            private void SetSignalsTask5()
            {
                double voltageX7 = 0;
                Hardware.HImitator.LTRCards.SetVoltage(10, 13, 215);
                Hardware.HImitator.LTRCards.SetVoltage(10, 14, 18);
                Hardware.HImitator.LTRCards.SetVoltage(12, 1, 6.4);
                voltageX7 = 0;
                if (Hardware.HImitator.DiscretOutpus1760X.GetRelayState(3))
                    voltageX7 = -0.75;
                Hardware.HImitator.LTRCards.SetVoltage(13, 7, voltageX7);
                Hardware.HImitator.LTRCards.SetVoltage(10, 11, 2.4); //Ландыш
            }

            private void SetSignalsTask4()
            {
                Hardware.HImitator.LTRCards.SetVoltage(10, 13, 136);
                Hardware.HImitator.LTRCards.SetVoltage(10, 14, 0);
                Hardware.HImitator.LTRCards.SetVoltage(11, 13, -11.5);
                Hardware.HImitator.LTRCards.SetVoltage(12, 1, 0);
                Hardware.HImitator.LTRCards.SetVoltage(12, 4, 0); //СУ
                Hardware.HImitator.LTRCards.SetVoltage(10, 10, 5.5); //Ландыш
                Hardware.HImitator.LTRCards.SetVoltage(10, 11, 0); //Ландыш        
            }

            private void VoltageChanged()
            {
                if (IsPower())
                {
                    Hardware.HImitator.DiscretInputs.SetInputState(21, true);
                    Hardware.HImitator.DiscretInputs.SetInputState(4, true);
                    if (_timeHighPast)
                        Hardware.HImitator.DiscretInputs.SetInputState(20, !Hardware.HImitator.DiscretOutputs.GetOutput(4));
                    else
                        tHigh.Start();
                    Hardware.HImitator.DiscretInputs.SetInputState(13, true);
                }
                else
                    _timeHighPast = false;
                SetStaticVoltages(IsPower());
            }

            private void SetStaticVoltagesUNCH()
            {
                int num = Hardware.HImitator.ControlTask.GetCurrentTask();
                if (num != 1)
                {
                    double voltage = 0.06;
                    if (Hardware.HImitator.DiscretOutputs.GetOutput(2))
                        voltage = 0.07;
                    //Шум УНЧ
                    Hardware.HImitator.LTRCards.SetVoltage(14, 1, voltage);
                    Hardware.HImitator.LTRCards.SetVoltage(14, 2, voltage);
                    Hardware.HImitator.LTRCards.SetVoltage(14, 3, voltage);
                    Hardware.HImitator.LTRCards.SetVoltage(14, 4, voltage);
                    if (Hardware.HImitator.DiscretOutputs.GetOutput(2))
                        voltage -= 0.01;
                    Hardware.HImitator.LTRCards.SetVoltage(15, 1, voltage);
                    Hardware.HImitator.LTRCards.SetVoltage(15, 2, voltage);
                }
                else
                {
                    //Сигнал УНЧ
                    Hardware.HImitator.LTRCards.SetVoltage(14, 1, 1.2);
                    Hardware.HImitator.LTRCards.SetVoltage(14, 2, 1.2);
                    Hardware.HImitator.LTRCards.SetVoltage(14, 3, 1.2);
                    Hardware.HImitator.LTRCards.SetVoltage(14, 4, 1.2);
                    Hardware.HImitator.LTRCards.SetVoltage(15, 1, 1.2);
                    Hardware.HImitator.LTRCards.SetVoltage(15, 2, 1.2);
                }
            }

            private void SetStaticVoltages(bool state)
            {
                //вторичные ИП
                Hardware.HImitator.LTRCards.SetVoltage(11, 11, 27);
                Hardware.HImitator.LTRCards.SetVoltage(11, 4, 12.6);
                Hardware.HImitator.LTRCards.SetVoltage(11, 5, -12.6);
                Hardware.HImitator.LTRCards.SetVoltage(11, 3, -6.3);
                Hardware.HImitator.LTRCards.SetVoltage(11, 6, -20);
                Hardware.HImitator.LTRCards.SetVoltage(11, 9, -27);
                Hardware.HImitator.LTRCards.SetVoltage(11, 12, -27);

                //магнетрон и ТКМ
                Hardware.HImitator.LTRCards.SetVoltage(11, 7, -50);
                Hardware.HImitator.LTRCards.SetVoltage(12, 12, 71);
                Hardware.HImitator.LTRCards.SetVoltage(12, 13, -3.6);
                Hardware.HImitator.LTRCards.SetVoltage(12, 14, 3.6);
                Hardware.HImitator.LTRCards.SetVoltage(11, 15, 2.75);

                Hardware.HImitator.LTRCards.SetVoltage(13, 1, 0.95);
                Hardware.HImitator.LTRCards.SetVoltage(13, 2, -0.95);
                Hardware.HImitator.LTRCards.SetVoltage(13, 3, 0.95);
                Hardware.HImitator.LTRCards.SetVoltage(13, 4, -0.95);
                Hardware.HImitator.LTRCards.SetVoltage(13, 5, 0.95);
                Hardware.HImitator.LTRCards.SetVoltage(13, 6, -0.95);


                SetStaticVoltagesUNCH();
            }

            private bool IsPower()
            {
                return Hardware.HImitator.PowerSupply115.GetVoltage() == 115;
            }

            private void tHigh_Elapsed(object sender, ElapsedEventArgs e)
            {
                Hardware.HImitator.DiscretInputs.SetInputState(20, true);
                tHigh.Stop();
                _timeHighPast = true;
            }

            public void SetControl(bool state)
            {
                if (state)
                {
                    Hardware.HImitator.LTRCards.SetVoltage(10, 13, 306);
                    Hardware.HImitator.LTRCards.SetVoltage(10, 14, -15);
                }
                else
                {
                    Hardware.HImitator.LTRCards.SetVoltage(10, 13, 215);
                    Hardware.HImitator.LTRCards.SetVoltage(10, 14, -18);
                }
            }

            public void SetSeaMode(bool state)
            {
                Hardware.HImitator.LTRCards.SetVoltage(10, 13, state ? 219 : 215);
            }

            private const int task2BaseX = 17;
            private const int task2BaseZ = 17;

            public void SetKren(bool state, bool plus)
            {
                if (state)
                    Hardware.HImitator.LTRCards.SetVoltage(10, 3, task2BaseZ + 2 * (plus ? 1 : -1));
                else
                    Hardware.HImitator.LTRCards.SetVoltage(10, 3, task2BaseZ);
            }

            public void SetTangazh(bool state, bool plus)
            {
                if (state)
                    Hardware.HImitator.LTRCards.SetVoltage(10, 1, task2BaseX + 2 * (plus ? -1 : 1));
                else
                    Hardware.HImitator.LTRCards.SetVoltage(10, 1, task2BaseX);
            }
        }
    }
}
