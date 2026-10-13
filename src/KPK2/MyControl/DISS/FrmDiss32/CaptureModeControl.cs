using KPK2.Common.DissFunctions;
using KPK2.Common.DissFunctions.Diss32;
using KPK2.GeneralForms.DissForms;
using KPK2.GeneralForms.DissForms.Setting;
using NLog;
using Sparc.KPK2.Hardware;
using Sparc.KPK2.Hardware.LTR;
using Sparc.KPK2.Hardware.PS115V;
using Sparc.KPK2.NFunctions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KPK2.MyControl.DISS.FrmDiss32
{
    public partial class CaptureModeControl : UserControl
    {
        private Task _task;
        private CancellationTokenSource _cts;
        private Measure _measure;
        private TestTasks _testTasks;
        private bool _continueTask = false;
        private bool _isActiveCT = false;
        private bool _isOneChannel = false;

        public event EventHandler EventHigh;

        public bool IsActiveCT3 => _isActiveCT;

        public CaptureModeControl(TestTasks testTasks, Measure measure, bool isOneChannel)
        {
            InitializeComponent();
            si.Inactive = true;
            _measure = measure;
            _testTasks = testTasks;
            _isOneChannel = isOneChannel;
        }

        public void Start()
        {
            if (_task == null)
            {
                _cts = new CancellationTokenSource();
                _task = new Task(() => { ModeWorkCapture(_cts.Token); }, _cts.Token);
            }
            _task.Start();
        }

        public void Stop()
        {
            _cts.Cancel();
            if (_task.Status == TaskStatus.Running)
            {
                _task.Wait();
            }
            _cts.Dispose();
            _task.Dispose();

            _task = null;
            _cts = null;
        }

        public void SetValueIndicator(int number, double val)
        {
            if (number < 0)
                return;

            switch (number)
            {
                case (int)Globals.IndicatorNumber.Speed1:
                    si.Value = val;
                    break;
            }
        }

        private void ModeWorkCapture(CancellationToken cToken)
        {
            
            DateTime workDT;
            try
            {
                int time = 0;
                TimeSpan ts;
                Logger _logger = LogManager.GetLogger("KPK2Trace");
                //запуск КЗ-3
                ChangeLabelText("Запуск КЗ-3");
                TaskOn("Task_W_1_32", "Task_3_32");
                InitializeTask3();
                StartWorker();
                si.Inactive = false;
                Thread.Sleep(1000);
                ChangeLabelText("Включена КЗ-3");
                time = 20;
                int timeResult = time;
                workDT = DateTime.Now;
                frmWork32 frm32 = (frmWork32)ParentForm;
                while (true)
                {
                    if (cToken.IsCancellationRequested)
                    {
                        StopWorker(false);
                        TaskOff("Task_3_32");
                        return;
                    }
                    ts = DateTime.Now - workDT;
                    timeResult = time - ts.Seconds;
                    ChangeTimerSiWork(timeResult);
                    if (timeResult <= 0)
                    {
                        break;
                    }

                    Thread.Sleep(500);
                }

                //проверка сигналов ЗАХВАТ, 1, 2, 3
                bool isNotContinueTask = false;
                bool[] captureSignal;

                if (_isOneChannel) 
                {
                    captureSignal = new bool[]
                    {
                        frm32.TiCapture
                    };
                }
                else
                {
                    captureSignal = new bool[]
                    {
                        frm32.TiCapture,
                        frm32.TiCapture1,
                        frm32.TiCapture2,
                        frm32.TiCapture3
                    };
                }
                string messageNoCapture = "";
                int countNoCapture = 0;
                string messageLog;

                if (captureSignal == null)
                {
                    messageNoCapture = "При включении КЗ-3 отсутствуют сигналы ЗАХВАТ, ЗАХВАТ 1, ЗАХВАТ 2, ЗАХВАТ 3. \r\nПрервать проверку готовности?";
                    if(MessageBox.Show(messageNoCapture, "Информация", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        isNotContinueTask = MessageBox.Show(messageNoCapture, "Информация", MessageBoxButtons.YesNo) == DialogResult.Yes;
                        messageLog = isNotContinueTask ? "Да" : "Нет";
                        _logger.Trace($"{messageNoCapture}. Ответ оператора \"{messageLog}\".");
                    }
                }
                else
                {
                    for (int i = 0; i < captureSignal.Length; i++)
                    {
                        if (!captureSignal[i])
                        {
                            if (countNoCapture != 0)
                            {
                                messageNoCapture += ", ";
                            }

                            countNoCapture++;

                            if (i == 0)
                            {
                                messageNoCapture += "\"ЗАХВАТ\"";
                            }
                            else
                            {
                                messageNoCapture += $"\"ЗАХВАТ {i}\"";
                            }
                            _logger.Trace($"Сигналы {messageNoCapture} отсутствуют.");
                        }
                    }

                    if (countNoCapture > 1)
                    {
                        messageNoCapture = "При включении КЗ-3 отсутсвуют сигналы " + messageNoCapture + ". Прервать проверку?";
                        isNotContinueTask = MessageBox.Show(messageNoCapture, "Информация", MessageBoxButtons.YesNo) == DialogResult.Yes;
                        messageLog = isNotContinueTask ? "Да" : "Нет";
                        _logger.Trace($"{messageNoCapture}. Ответ оператора \"{messageLog}\".");
                    }
                    else if (countNoCapture == 1)
                    {
                        messageNoCapture = "При включении КЗ-3 отсутсвует сигнал " + messageNoCapture + ". Прервать проверку?"; ;
                        isNotContinueTask = MessageBox.Show(messageNoCapture, "Информация", MessageBoxButtons.YesNo) == DialogResult.Yes;
                        messageLog = isNotContinueTask ? "Да" : "Нет";
                        _logger.Trace($"{messageNoCapture}. Ответ оператора \"{messageLog}\".");
                    }
                }

                if (isNotContinueTask)
                {
                    StopWorker(false);
                    TaskOff("Task_3_32");
                    return;
                }

                //остановка КЗ-3
                StopWorker(false);
                TaskOff("Task_3_32");
                ChangeLabelText("КЗ-3 выключена");
                BeginInvoke(new MethodInvoker(() =>
                {
                    si.Inactive = true;
                }));

                workDT = DateTime.Now;

                Thread.Sleep(1500);

                //проверка сигналов память 1 и 2
                bool answerOper = false;
                bool notFinishProccesMemory1 = true;
                string messageOperator = "";
                ChangeLabelText("КЗ-3 выключена");
                Thread.Sleep(1500);

                workDT = DateTime.Now;
                while (true)
                {
                    if (cToken.IsCancellationRequested)
                    {
                        return;
                    }
                    ts = DateTime.Now - workDT;
                    ChangeTimerSiWork(ts.Seconds);
                    if(ts.Seconds >= 2 && notFinishProccesMemory1)
                    {
                        if (!frm32.TiMemory1)
                        {
                            _logger.Trace("отсутствует сигнал ПАМЯТЬ 1");
                        }
                        else
                        {
                            _logger.Trace("сигнал ПАМЯТЬ 1 есть");
                        }

                        string messageTabloP = "Горит ли на БПК табло ПОИСК?";
                        string answerOperStr;

                        var result = MessageBox.Show("Горит ли на БПК табло ПОИСК?", "Информация", MessageBoxButtons.YesNo);

                        answerOperStr = result == DialogResult.No ? "Нет" : "Да";
                        _logger.Trace($"{messageTabloP}. Ответ оператора \"{answerOperStr}\"");

                        if (result == DialogResult.No && !frm32.TiMemory1)
                        {
                            messageOperator = "При выключении КЗ-3 на БПК не горит табло ПОИСК и отсутствует сигнал ПАМЯТЬ 1. Прервать проверку готовности?";
                            answerOper = MessageBox.Show(messageOperator,
                                "Информация", MessageBoxButtons.YesNo) == DialogResult.Yes;

                            if (answerOper)
                            {
                                _logger.Trace($"{messageOperator}, Ответ Да");
                                return;
                            }
                            else
                            {
                                _logger.Trace($"{messageOperator}, Ответ Нет");
                            }
                        }
                        else if (result == DialogResult.No) 
                        {
                            messageOperator = "При выключении КЗ-3 на БПК не горит табло ПОИСК. Прервать проверку готовности?";
                            answerOper = MessageBox.Show(messageOperator,
                                "Информация", MessageBoxButtons.YesNo) == DialogResult.Yes;

                            if (answerOper)
                            {
                                _logger.Trace($"{messageOperator}, Ответ Да");
                                return;
                            }
                            else
                            {
                                _logger.Trace($"{messageOperator}, Ответ Нет");
                            }
                        }
                        else if (!frm32.TiMemory1)
                        {
                            messageOperator = "При выключении КЗ-3 отсутствует сигнал ПАМЯТЬ 1. Прервать проверку готовности?";
                            answerOper = MessageBox.Show(messageOperator,
                                "Информация", MessageBoxButtons.YesNo) == DialogResult.Yes;

                            if (answerOper)
                            {
                                _logger.Trace($"{messageOperator}, Ответ Да");
                                return;
                            }
                            else
                            {
                                _logger.Trace($"{messageOperator}, Ответ Нет");
                            }
                        }
                        notFinishProccesMemory1 = false;
                    }

                    if (ts.Seconds >= 10)
                    {
                        if (!frm32.TiMemory2)
                        {
                            _logger.Trace("отсутствует сигнал ПАМЯТЬ 2");
                        }
                        else
                        {
                            _logger.Trace("сигнал ПАМЯТЬ 2 есть");
                        }

                        string messageTabloP = "Горит ли на блоке УС ПС индикатор П?";
                        string answerOperStr;
                        var result = MessageBox.Show("Горит ли на блоке УС ПС индикатор П?", "Информация", MessageBoxButtons.YesNo);

                        answerOperStr = result == DialogResult.No ? "Нет" : "Да";
                        _logger.Trace($"{messageTabloP}. Ответ оператора \"{answerOperStr}\"");
                        
                        if (result == DialogResult.No && !frm32.TiMemory2)
                        {
                            messageOperator = "При выключении КЗ-3 на УСПС не горит индикатор П и отсутствует сигнал ПАМЯТЬ 2. Прервать проверку готовности?";
                            answerOper = MessageBox.Show(messageOperator,
                                "Информация", MessageBoxButtons.YesNo) == DialogResult.Yes;

                            if (answerOper)
                            {
                                _logger.Trace($"{messageOperator}, Ответ Да");
                                return;
                            }
                            else
                            {
                                _logger.Trace($"{messageOperator}, Ответ Нет");
                            }

                        }
                        else if (result == DialogResult.No)
                        {
                            messageOperator = "При выключении КЗ-3 на УСПС не горит индикатор П. Прервать проверку готовности?";
                            answerOper = MessageBox.Show(messageOperator,
                                "Информация", MessageBoxButtons.YesNo) == DialogResult.Yes;

                            if (answerOper)
                            {
                                _logger.Trace($"{messageOperator}, Ответ Да");
                                return;
                            }
                            else
                            {
                                _logger.Trace($"{messageOperator}, Ответ Нет");
                            }
                        }
                        else if (!frm32.TiMemory2)
                        {
                            messageOperator = "При выключении КЗ-3 отсутствует сигнал ПАМЯТЬ 2. Прервать проверку готовности?";
                            answerOper = MessageBox.Show(messageOperator,
                                "Информация", MessageBoxButtons.YesNo) == DialogResult.Yes;

                            if (answerOper)
                            {
                                _logger.Trace($"{messageOperator}, Ответ Да");
                                return;
                            }
                            else
                            {
                                _logger.Trace($"{messageOperator}, Ответ Нет");
                            }
                        }
                        break;
                    }

                    Thread.Sleep(500);
                }


                workDT = DateTime.Now;

                //проверка на шум
                int countInclusions = 0;
                string messageLbCountInclusions = "КЗ-3 выключена. \r\nПодсчет числа включения сигналов ЗАХВАТ: {0}";
                ChangeLabelText(string.Format(messageLbCountInclusions, countInclusions));
                bool[] capture = new bool[4];
                bool[] isCaptureAdd = new bool[4];
                TimeSpan[] timeEnableCapture = new TimeSpan[4];
                DateTime[] timeWorkCapture = new DateTime[4];
                int[] countEnableCapture = new int[4];
                int timeReadLogMillis = 0;
                bool isNotVisibleBtn = true;
                workDT = DateTime.Now;
                while (true)
                {
                    if (cToken.IsCancellationRequested)
                    {
                        ChangeLabelText("Проверка остановлена");
                        return;
                    }

                    if (_continueTask)
                    {
                        _logger.Trace("Оператор пропустил проверку на ШУМ");
                        break;
                    }

                    ts = DateTime.Now - workDT;
                    int secCurrent = 180 - (ts.Seconds + 60 * ts.Minutes);
                    if (secCurrent <= 0)
                    {
                        btnContinueTask.Visible = false;
                        DataManager.Instance.SaveParameter(SettingsObj.GetSettings().TestId, "NotTransitioFromMemoryToCapure", 1.0);
                        break;
                    }

                    ChangeTimerSiWork(secCurrent);
                    bool result = false;
                    var checkCapture = new bool[]
                    {
                        frm32.TiCapture,
                        frm32.TiCapture1,
                        frm32.TiCapture2,
                        frm32.TiCapture3
                    };
                    for (int i = 0; i < checkCapture.Length; i++)
                    {
                        if (checkCapture[i])
                        {
                            if (capture[i])
                            {
                                timeEnableCapture[i] = DateTime.Now - timeWorkCapture[i];
                                timeReadLogMillis += 250;
                                if (timeEnableCapture[i].TotalMilliseconds > 1500 && isCaptureAdd[i])
                                {
                                    isCaptureAdd[i] = false;
                                    countInclusions++;
                                    ChangeLabelText(string.Format(messageLbCountInclusions, countInclusions));
                                }
                            }
                            else
                            {
                                capture[i] = true;
                                isCaptureAdd[i] = true;
                                countEnableCapture[i]++;
                                timeWorkCapture[i] = DateTime.Now;
                                if (i == 0)
                                    _logger.Trace($"Сигнал ЗАХВАТ включился.");
                                else
                                    _logger.Trace($"Сигнал ЗАХВАТ {i} включился.");
                            }
                            //	_logger
                        }
                        else
                        {
                            if (capture[i])
                            {
                                capture[i] = false;
                                if (i == 0)
                                    _logger.Trace($"Сигнал ЗАХВАТ выключился.");
                                else
                                    _logger.Trace($"Сигнал ЗАХВАТ {i} выключился.");
                            }
                        }
                    }

                    if (timeReadLogMillis == 1000)
                    {
                        timeReadLogMillis = 0;
                        for (int i = 1; i < timeEnableCapture.Length; i++)
                        {
                            if (timeEnableCapture[i].Seconds != 0)
                            {
                                _logger.Trace($"Сигнал Захват {i} оставался включенным {timeEnableCapture[i].Seconds} сек.");
                                _logger.Trace($"Сигнал Захват {i} включался {countEnableCapture[i]} раз.");
                            }
                        }
                    }

                    if (countInclusions >= 3)
                    {
                        result = true;
                        break;
                    }

                    if (result && isNotVisibleBtn)
                    {
                        DataManager.Instance.SaveParameter(SettingsObj.GetSettings().TestId, "NotTransitioFromMemoryToCapure", 0.0);
                        _logger.Trace("Проверка на шум не прошла");
                        isNotVisibleBtn = false;
                        BeginInvoke(new MethodInvoker(() =>
                        {
                            btnContinueTask.Visible = true;
                        }));
                    }

                    Thread.Sleep(250);
                }

                TaskOn("Task_W_1_32", "Task_3_32");

                //Время перехода из памяти в захват
                InitializeTask3();
                StartWorker();
                si.Inactive = false;

                Thread.Sleep(500);
                bool captureT = false;
                bool serviceability = false;
                workDT = DateTime.Now;
                ChangeLabelText("КЗ-3 включена. \r\nОжидание перехода в режим ЗАХВАТ:");
                BeginInvoke(new MethodInvoker(() =>
                {
                    siWorkTime.MaxValue = 15;
                }));
                while (true)
                {
                    ts = DateTime.Now - workDT;
                    if (30 < ts.Seconds)
                    {
                        StopWorker(false);
                        TaskOff("Task_3_32");
                        BeginInvoke(new MethodInvoker(() =>
                        {
                            si.Inactive = true;
                        }));
                        DataManager.Instance.SaveParameter(SettingsObj.GetSettings().TestId, "TimeTransitioFromMemoryToCapure", ts.Seconds);
                        break;
                    }

                    ChangeTimerSiWork(ts.Seconds);

                    if (cToken.IsCancellationRequested)
                    {
                        StopWorker(false);
                        TaskOff("Task_3_32");
                        BeginInvoke(new MethodInvoker(() =>
                        {
                            si.Inactive = true;
                        }));
                        _logger.Trace("Проверка прервана");
                        return;
                    }

                    if (frm32.TiCapture && !captureT)
                    {
                        captureT = true;
                        _logger.Trace("Появился сигнал ЗАХВАТ");
                    }

                    if (frm32.TiGood && !serviceability)
                    {
                        serviceability = true;
                        _logger.Trace("Появился сигнал ИСПРАВНОСТЬ");
                    }

                    if (captureT && serviceability)
                    {
                        DataManager.Instance.SaveParameter(SettingsObj.GetSettings().TestId, "TimeTransitioFromMemoryToCapure", ts.Seconds);

                        ChangeLabelText($"Время перехода из режима ПАМЯТЬ в режим ЗАХВАТ = {ts.Seconds} с");
                        _isActiveCT = true;
                        break;
                    }
                    Thread.Sleep(250);//todo доьавить вкл КЗ-3
                };
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
        }

        private void InitializeTask3()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() =>
                {
                    si.SetValue(_testTasks.TypeTestTasks.Task3.Viewers[0]);
                }));
            }
            else
            {
                si.SetValue(_testTasks.TypeTestTasks.Task3.Viewers[0]);
            }

            SettingsObj.GetSettings().MeasList.Clear();
            SettingsObj.GetSettings().MeasList.Add(_testTasks.TypeTestTasks.Task3.Viewers[0].UsbBaseNew);
            
            _measure.AddMeasureList(SettingsObj.GetSettings().MeasList);
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
                
        private int GetNumChannel(Dictionary<int, string> chList, string value)
        {
            foreach (var item in chList)
            {
                if (item.Value == value)
                {
                    return item.Key;
                }
            }
            return -1;
        }

        private void btnContinueTask_Click(object sender, EventArgs e)
        {
            _continueTask = true;
            btnContinueTask.Visible = false;
        }
    }
}
