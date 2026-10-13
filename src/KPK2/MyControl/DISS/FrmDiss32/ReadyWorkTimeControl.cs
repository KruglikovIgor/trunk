using KPK2.GeneralForms.DissForms.Setting;
using NLog;
using Sparc.KPK2.Hardware.LTR;
using Sparc.KPK2.Hardware.PS115V;
using Sparc.KPK2.Hardware.PS27V;
using Sparc.KPK2.Hardware;
using Sparc.KPK2.NFunctions;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using KPK2.Common.DissFunctions;
using Res = Sparc.KPK2.Resources.Properties.Resources;
using KPK2.Common.DissFunctions.Diss32;

namespace KPK2.MyControl.DISS.FrmDiss32
{
    public partial class ReadyWorkTimeControl : UserControl
    {
        private bool _task1 = false;
        private Measure _measure;
        private DateTime _workDT;
        private TestTasks _testTasks;
        private Task _task;
        private CancellationTokenSource _cts;
        private bool _isActiveCT = false;

        public event EventHandler EventHigh;

        public ReadyWorkTimeControl(TestTasks testTasks, Measure measure)
        {
            InitializeComponent();
            _testTasks = testTasks;
            _measure = measure;
            siUp.Inactive = true;
            siUS.Inactive = true;
            siUS.Visible = false;
        }

        public void SetValueIndicator(int number, double val)
        {
            if (number < 0)
                return;

            switch (number)
            {
                case (int)Globals.IndicatorNumber.Speed1:
                    siUp.Value = val;
                    break;
                case(int)Globals.IndicatorNumber.Speed2:
                    if(!_task1)
                        siUS.Value = val; break;
            }
        }

        public void Start()
        {
            if(_task == null)
            {
                _cts = new CancellationTokenSource();
                _task = new Task(() => { ReadyWorkTime(_cts.Token); }, _cts.Token);
            }
            _task.Start();
        }

        public void Stop() 
        {
            if (_task.Status == TaskStatus.Running)
            { 
                _cts.Cancel();
                _task.Wait();
                
                _cts.Dispose();
                _task.Dispose();

                _task = null;
                _cts = null;
            }
            if (_isActiveCT)
            {
                StopWorker(false);
                TaskOff("Task_3_32");
            }
        }
        private void ReadyWorkTime(CancellationToken cToken)
        {
            try
            {
                double maxValuePS = 127 + 10.0;
                double minValuePS = 127 - 10.0;
                double maxValueUS = 2;
                double minValueUS = -2;
                double differencePS = 0.5;
                double differenceUS = 0.1;
                Logger _logger = LogManager.GetLogger("KPK2Trace");

                if (_measure.IsWorking)
                {
                    StopWorker(false);
                }

                if (!TaskOff("Task_3_32"))
                {
                    _logger.Error(string.Format(Res.ErrorOnDevice, Res.ControlTaskBoard, Res.SetValue));
                }

                //BeginInvoke(new MethodInvoker(() => btnTimePreperation.Text = "Отмена операции"));

                _task1 = true;
                BeginInvoke(new MethodInvoker(InitializeTask1));
                if (!TaskOn("Task_W_1_32", "Task_1_32"))
                {
                    _logger.Error(string.Format(Res.ErrorOnDevice, Res.ControlTaskBoard, Res.SetValue));
                }
                //InitializeTask1(TestTasks.TypeTestTasks.Task1);
                StartWorker();

                _workDT = DateTime.Now;
                int timeTick = 0;
                int time = 30;
                int timeResult = time;
                ChangeLabelText("Включена КЗ-1");
                while (true)
                {
                    if (cToken.IsCancellationRequested)
                    {//если операцая отменилась, отменить КЗ-1 и запустить КЗ-3
                        StopWorker(false);
                        TaskOff("Task_1_32");
                        _logger.Trace("Отмена операции \"Время готовности\" во время работы КЗ-1");
                        _task1 = false;
                        return;
                    }

                    siUp.Inactive = false;

                    if (timeResult == 0)
                    {
                        if (!TaskOff("Task_1_32"))
                            _logger.Error(string.Format(Res.ErrorOnDevice, Res.ControlTaskBoard, Res.SetValue));
                        _task1 = false;
                        break;
                    }
                    TimeSpan ts = DateTime.Now - _workDT;
                    timeResult = time - ts.Seconds;
                    if(timeResult < 0)
                    {
                        timeResult = 0;
                    }
                    ChangeTimerSiWork(timeResult);
                    timeTick += 500;
                    Thread.Sleep(500);
                }

                bool isCT1throw = false;
                TaskOff("Task_1_32");
                BeginInvoke(new MethodInvoker(() =>
                {
                    isCT1throw = siUp.Value > 20.0;
                }));

                string messageOper = "КЗ-1 не включилась. Прервать проверку готовности?";
                if (isCT1throw)
                {
                    ChangeLabelText("КЗ-1 не включилось");
                    if(MessageBox.Show(
                        messageOper, 
                        "Информация", 
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        ChangeLabelText("Проверка прервана");
                        _logger.Trace(messageOper + " Ответ оператора: Да");
                        return;
                    }
                    else
                    {
                        _logger.Trace(messageOper + " Ответ оператора: Нет");
                    }
                }


                siUp.Inactive = true;
                StopWorker(false);

                ChangeLabelText("Питание ДИСС временно выключено.");
                GenSinCos.Instance().GeneratorOff();
                Power115V.Instance.SetOnOff(false);
                Power27V.Instance.SetOnOff(false);


                timeTick = 0;
                time = 7;
                timeResult = time;
                _workDT = DateTime.Now;

                double ps = 0;
                double us = 0;

                while (true)
                {
                    if (cToken.IsCancellationRequested)
                    {//если операцая отменилась, Запустить ДИСС и КЗ-3
                        DiscreteOutputs.Instance().SetOutput("High_32", false);
                        Power27V.Instance.SetOnOff(true);
                        Power115V.Instance.SetVoltage(SettingsObj.GetSettings().MeasValueInfo.Voltage115);
                        Power115V.Instance.SetOnOff(true);
                        DO176X.Instance.SetBVKRefuse(false); //специально так делается т.к. для плат 1762 другая логика
                        BeginInvoke(new MethodInvoker(() =>
                        {
                            //btnTimePreperation.Text = "Время готовности";
                            //WrungButtonTask(btnTimePreperation);
                        }));
                        ChangeLabelText("Операция отменена");
                        _logger.Trace("Отмена операции \"Время готовности\" во время запуска ДИСС");
                        return;
                    }

                    if (timeResult <= 0)
                        break;
                    TimeSpan ts = DateTime.Now - _workDT;
                    timeResult = time - ts.Seconds;
                    if(timeResult < 0)
                    {
                        timeResult = 0;
                    }
                    ChangeTimerSiWork(timeResult);
                    timeTick += 500;
                    Thread.Sleep(500);
                }

                DiscreteOutputs.Instance().SetOutput("High_32", false);
                Power27V.Instance.SetOnOff(true);
                Power115V.Instance.SetVoltage(SettingsObj.GetSettings().MeasValueInfo.Voltage115);
                Power115V.Instance.SetOnOff(true);
                DO176X.Instance.SetBVKRefuse(false); //специально так делается т.к. для плат 1762 другая логика

                if (!TaskOn("Task_W_1_32", "Task_3_32"))
                    _logger.Error(Globals.GetDeviceErrorString(Globals.Device.ControlTask, Globals.Operation.SetValue));
                ChangeLabelText("Включена КЗ-3. \r\nВремя готовности (с):");
                siUp.Inactive = false;
                BeginInvoke(new MethodInvoker(() =>
                {
                    siUp.NameLabelUp = "W, км/ч";
                    siUp.NameLabelDown = "ПС1";
                    siUS.NameLabelUp = "α, град"; //TODO дописать значения
                    siUS.NameLabelDown = "УС 0";
                    siUS.Visible = true;
                    siUS.Inactive = false;
                }));

                InitializeTask3();

                StartWorker();

                _workDT = DateTime.Now;
                double psOld = 0;
                double usOld = 0;
                BeginInvoke(new MethodInvoker(() =>
                {
                    psOld = siUp.Value;
                    usOld = siUS.Value;
                }));
                while (true)
                {
                    if (cToken.IsCancellationRequested)
                    {//отмена операции
                        ChangeLabelText("Операция отменена");
                        _logger.Trace("Отмена операции \"Время готовности\", во время проверки готовности к работе");
                        return;
                    }

                    TimeSpan ts = DateTime.Now - _workDT;
					timeResult = ts.Seconds + 60 * ts.Minutes;
					ChangeTimerSiWork(timeResult);
                    if (timeResult >= 240)
                    {
                        double result = (double)timeResult / 60.0;
                        DataManager.Instance.SaveParameter(SettingsObj.GetSettings().TestId, "ReadyTime", result);
                        break;
                    }

                    BeginInvoke(new MethodInvoker(() =>
                    {
                        ps = siUp.Value;
                        us = siUS.Value;
                    }));

                    if (Math.Round(ps, 1) >= Math.Round(minValuePS, 1) && 
						Math.Round(ps, 1) <= Math.Round(maxValuePS, 1) &&
                        Math.Round(us, 1) <= maxValueUS &&
                        Math.Round(us, 1) >= minValueUS &&
                        Math.Abs(us - usOld) <= differenceUS &&
                        Math.Abs(ps - psOld)  <= Math.Round(differencePS, 2))
                    {
                        double result = (double)timeResult / 60.0;
                        DataManager.Instance.SaveParameter(SettingsObj.GetSettings().TestId, "ReadyTime", result);
                        BeginInvoke(new MethodInvoker(() => { this.Enabled = true; }));
                        break;
                    }
                    Thread.Sleep(500);
                    time += 500;
                    BeginInvoke(new MethodInvoker(() => 
                    {
                        psOld = ps;
                        usOld = us;
                    }));
                };
                ChangeLabelText("Выключение КЗ-3");
                Thread.Sleep(1000);

                ChangeLabelText($"Время готовности к работе составило: {timeResult} с");
                _isActiveCT = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
            }
        }

        private void ChangeLabelText(string text)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() =>
                {
                    label1.Text = text;
                }));
            }
            else
            {
                label1.Text = text;
            }
        }

        private void ChangeTimerSiWork(int second)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => { siWorkTime.ValueString = second.ToString(); }));
            }
            else
            {
                siWorkTime.ValueString = second.ToString();
            }
        }

        private void StopWorker(bool b)
        {
            _measure.StopMeasuring(b);
            if (Power115V.Instance.IsAktor)
                _measure.StartMeasuring();
        }

        private void StartWorker()
        {
            _measure.StopMeasuring(false);
            _measure.StartWorkerWithList(SettingsObj.GetSettings().MeasList);
        }

        private bool TaskOff(string str)
        {
            bool result = true;
            result &= DiscreteOutputs.Instance().SetOutput(str, false);
            result &= ControlTask.Instance().SetControlTask(0, 0, 0);
            result &= Diss32Functions.SetAngle(0);
            result &= TaskRele(false);
            return result;
        }

        private bool TaskOn(string strCT, string strDOutput)
        {
            bool result = true;
            EventHigh?.Invoke(this, EventArgs.Empty);
            result &= TaskRele(true);
            result &= ControlTask.Instance().SetControlTask(strCT);
            result &= DiscreteOutputs.Instance().SetOutput(strDOutput, true);
            result &= Diss32Functions.SetAngle(0);
            return result;
        }

        private bool TaskRele(bool state)
        {
            bool result = true;
            result &= DiscreteOutputs.Instance().SetOutput("UNCh_32", state);
            //result &= DiscreteOutputs.Instance().SetOutput("Course", state);
            result &= DiscreteOutputs.Instance().SetOutput("Pitch", state);
            result &= DiscreteOutputs.Instance().SetOutput("Heel", state);
            return result;
        }

        private void InitializeTask3()
        {
            SettingsObj.GetSettings().MeasList.Clear();
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() =>
                {
                    siUp.SetValue(_testTasks.TypeTestTasks.Task3.Viewers[0]);
                    siUS.SetValue(_testTasks.TypeTestTasks.Task3.Viewers[1]);
                }));
            }
            else
            {
                siUp.SetValue(_testTasks.TypeTestTasks.Task3.Viewers[0]);
                siUS.SetValue(_testTasks.TypeTestTasks.Task3.Viewers[1]);
            }

            SettingsObj.GetSettings().MeasList.Clear();
            foreach (var indicatorViewer in _testTasks.TypeTestTasks.Task3.Viewers)
            {

                SettingsObj.GetSettings().MeasList.Add(indicatorViewer.UsbBaseNew);
            }

            _measure.AddMeasureList(SettingsObj.GetSettings().MeasList);
            StartWorker();
        }

        private void InitializeTask1()
        {
            siUp.SetValue(_testTasks.TypeTestTasks.Task1.Viewers[0]);
            SettingsObj.GetSettings().MeasList.Clear();
            SettingsObj.GetSettings().MeasList.Add(_testTasks.TypeTestTasks.Task1.Viewers[0].UsbBaseNew);
            StartWorker();
        }
    }
}
