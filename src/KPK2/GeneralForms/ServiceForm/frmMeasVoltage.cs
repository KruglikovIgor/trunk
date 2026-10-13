using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using KPK2.Common.MeasureVoltage;
using Sparc.Hardware.NAkip1142;
using Sparc.KPK2.Hardware;
using Sparc.KPK2.Hardware.LTR;
using Sparc.KPK2.Hardware.MyClasseUSB;

namespace KPK2.GeneralForms.ServiceForm
{
    public partial class frmMeasVoltage : Form
    {
        #region закрытые поля

        private const int DirectCurrentWait = 315; //задержка по времени


        /// <summary>
        ///  допустимая погрешность отклонения показания U эталонного вольтметра
        /// от установленного значения U источника питания, при превышении которого
        /// необходимо повторное измерение
        /// </summary>
        private const double SetDirectVoltageError = 0.200;

        /// <summary>
        /// коэфф. для вычисления допустимой погрешности, составляющей 5% от установленного U при малых напряжениях
        /// </summary>
        private const double CoefAlowError = 0.10;

        private Channel _currentChannel = Channel.C1;
        private readonly Measure _measWorker = new Measure();
        private readonly ManualResetEvent _mreAuto = new ManualResetEvent(false);

        private IPowerSupply _ps;

        private readonly Color _failColor = Color.LightCoral;
        private readonly Color _successColor = Color.LightGreen;

        private Thread _autoThread;

        /// <summary>
        /// напряжение постоянного тока, установленное источником питания
        /// </summary>
        private double _setVoltage;
        /// <summary>
        /// напряжение, измеряемое платой
        /// </summary>
        private double _adcVoltage;
        
        private readonly MeasuredChannel _measuredChannel;
        private bool _isPSOn;
        private bool _lastStepActivateResult;
        private int _nStep;
        private bool _autoCheck;

        /// <summary>
        /// массив точек измерения напряжения для текущего измерительного канала
        /// </summary>
        private Voltages _cpv;
        

        #endregion

        /// <summary>
        ///   конструктор
        /// </summary>
        /// <param name="channel">канал ИП</param>
        /// <param name="mc">объект, содержащий имя канала и название сигнала</param>
        /// <param name="namesInterface">имя интерфейса подключения: USB или COM</param>
        /// <param name="isAkip">если АКИП 1142, по умолчанию true</param>
        public frmMeasVoltage(MeasuredChannel mc)
        {
            _measuredChannel = mc;
            InitializeComponent();
        }

        #region Загрузка

        private void frmMeasVoltage_Load(object sender, EventArgs e)
        {

            FillFirstColumnDataGrid();
            _measWorker.ResultsReady += ShowResult;

            DGInit();

            labelNameIK.Text = _measuredChannel.Name;
            dgvValues.CellValueChanged += dgvValues_CellValueChanged;
        }

      

        /// <summary>
        ///   Заполнение таблицы значениями устанавливаемых напряжений на ИП
        /// </summary>
        private void FillFirstColumnDataGrid()
        {
            _autoCheck = true;
            try
            {
                _cpv = VoltagesArray.GetVoltages(_measuredChannel.Name);

                foreach (double voltage in _cpv.ControlPoints)
                {
                    object[] row = { voltage, DBNull.Value, DBNull.Value, DBNull.Value };
                    dgvValues.Rows.Add(row);
                }

            }
            catch (Exception e)
            {
                ShowMessage(e.Message);
            }

            _autoCheck = false;
        }
        private bool Activation()
        {
            try
            {
                InitPowerSupply();
                StartNewMeasure(_measuredChannel.Code, 0);
                return true;
            }
            catch (Exception e)
            {
                ShowMessage(e.Message);
                return false;
            }
        }

        /// <summary>
        ///   Инициализация и подключение источника питания
        /// </summary>
        private void InitPowerSupply()
        {
            _ps = new AkipPS(CurrentChannel);
            _ps.Connect();

            //string command = string.Format(Commands.SetChannel, (int)CurrentChannel);
            //_ps.SendCommand(string.Format(command));
            // Устанавливаем ограничение тока 0,03 А для обоих каналов
            _ps.SendCommand(string.Format(Commands.SetChannel, (int)Channel.C1));
            _ps.SetCurrent(0.03);
            _ps.SendCommand(string.Format(Commands.SetChannel, (int)Channel.C2));
            _ps.SetCurrent(0.03);
            // Возвращаем рабочий канал
            _ps.SendCommand(string.Format(Commands.SetChannel, (int)CurrentChannel));

            _ps.SetOutState(true);
            _isPSOn = true;
        }

        /// <summary>
        ///   Запускает измерения по кнопке "Измерить"
        /// </summary>
        /// <param name="nameSignal"></param>
        /// <param name="indNumber"></param>
        public void StartNewMeasure(string nameSignal, int indNumber)
        {
            USBBaseNew ub = new USBBaseNew(0, nameSignal, indNumber);
            _measWorker.AddMeasure(ub);
            _measWorker.Init();
            _measWorker.StartMeasuring();
        }

        private void DGInit()
        {
            _nStep = 0;
            btnAuto.Enabled = true;
            ActivateCurrentStep(false);

            GlobalMeasure.SetReadOnlyFields(dgvValues, ADCDirectColumn.pointV, ADCDirectColumn.errorV);
            foreach (DataGridViewColumn dc in dgvValues.Columns)
            {
               dc.DefaultCellStyle.Format = "N4";
            }

            dgvValues.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            foreach (DataGridViewColumn dc in dgvValues.Columns)
            {
                dc.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dc.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dc.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        #endregion

        private int CurrentStep
        {
            get { return _nStep; }
            set { SetCurrentStep(value); }
        }

        public Channel CurrentChannel
        {
            get { return _currentChannel; }
            set
            {
                if (_currentChannel != value)
                {
                    _currentChannel = value;
                    _ps = new AkipPS(_currentChannel);
                    if (_ps != null)
                        _ps.SetVoltage(Convert.ToDouble(dgvValues.Rows[_nStep].Cells[(int)ADCDirectColumn.pointV].Value)); // установка напряжения
                }
            }
        }

        /// <summary>
        /// Значение напряжения эталонного вольтметра.
        /// Устанавливается из кода родительской формы.
        /// </summary>
        public double IdealVoltage { get; set; }
        public double DiffVoltage { get; set; }

        #region Методы

        /// <summary>
        /// активация / деактивация кнопок
        /// </summary>
        /// <param name="isEnable"></param>
        private void SetButtonsEnabled(bool isEnable)
        {
            btnNextStep.Enabled = btnMeasure.Enabled = panel2.Enabled = isEnable;
        }

        /// <summary>
        /// Установка текущего шага
        /// </summary>
        /// <param name="value"></param>
        private void SetCurrentStep(int value)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<int>(SetCurrentStep), value);
                return;
            }

            LightStep(_nStep, false);
            CalculateError(_nStep);
            if (value >= dgvValues.Rows.Count)
            {
                value = 0;
            }

            _nStep = value;
            //_lastStepActivateResult = ActivateCurrentStep(true);
        }

        /// <summary>
        /// установка подсветки шага
        /// </summary>
        /// <param name="step"></param>
        /// <param name="isLight"></param>
        private void LightStep(int step, bool isLight)
        {
            try
            {
                dgvValues.Rows[step].DefaultCellStyle.BackColor = isLight ? Color.Yellow : Color.White;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void CalculateError(int step)
        {
            try
            {
                DataGridViewRow row = dgvValues.Rows[step];
                if (row.Cells[(int)ADCDirectColumn.idealV].Value != DBNull.Value &&
                    row.Cells[(int)ADCDirectColumn.measV].Value != DBNull.Value)
                {
                    double deviation = Math.Round(Convert.ToDouble(row.Cells[(int)ADCDirectColumn.measV].Value) -
                                   Convert.ToDouble(row.Cells[(int)ADCDirectColumn.idealV].Value), 4); // отклонение измеренного напряжения платы от показаний эталонного вольтметра

                    row.Cells[(int)ADCDirectColumn.errorV].Value = deviation;
                    row.Cells[(int)ADCDirectColumn.errorV].Style.BackColor = CheckADCDirectError(step, deviation)
                      ? _successColor
                      : _failColor;
                }
            }
            catch
            {
                MessageBox.Show("Ошибка вычисления погрешности!");
            }
        }

        private bool ActivateCurrentStep(bool meas)
        {
            try
            {
                LightStep(_nStep, true);

                _setVoltage = Convert.ToDouble(dgvValues.Rows[CurrentStep].Cells[0].Value);


                if (_isPSOn)
                {
                    int baseVoltage = 0;
                    if (IsDelta15V(_measuredChannel.Name))
                        baseVoltage = 29;
                    if (IsDelta27V(_measuredChannel.Name))
                        baseVoltage = 49;

                    if (baseVoltage != 0)
                    {
                        _ps.SetVoltage(baseVoltage);
                        _ps.SetAddVoltage(Math.Abs(baseVoltage - Math.Abs(_setVoltage)));
                    }
                    else
                    {
                        if (Math.Abs(_setVoltage) > _ps.MaxVoltage())
                        {
                            _ps.SetVoltage(_ps.MaxVoltage());
                            _ps.SetAddVoltage(Math.Abs(Math.Abs(_setVoltage) - _ps.MaxVoltage()));
                        }
                        else
                        {
                            _ps.SetVoltage(Math.Abs(_setVoltage));
                            _ps.SetAddVoltage(0);
                        }
                    }
                }

                if (meas)
                {
                    Thread.Sleep(DirectCurrentWait);
                    Application.DoEvents();
                    if (!MeasureCurrentStep())
                    {
                        return false;
                    }
                }
            }
            catch
            {
                ShowMessage("Ошибка установки следующего шага");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Внесение измерений в таблицу
        /// </summary>
        /// <param name="row">номер строки</param>
        /// <param name="col">номер колонки</param>
        /// <param name="value">значение</param>
        private void SetGridValue(int row, int col, double value)
        {
            if (InvokeRequired)
            {
                Invoke(new SetGridValueDelegate(SetGridValue), row, col, value);
                return;
            }

            dgvValues.Rows[row].Cells[col].Value = value;
        }

        /// <summary>
        /// Вывод сообщений
        /// </summary>
        /// <param name="message">сообщение</param>
        private void ShowMessage(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(ShowMessage), message);
                return;
            }

            MessageBox.Show(message);
        }

        /// <summary>
        /// Проверяет ошибки
        /// </summary>
        /// <param name="step">номер шага</param>
        /// <param name="deviation">отклонение от напряжения, измеренного эталонным вольтметром</param>
        /// <returns></returns>
        private bool CheckADCDirectError(int step, double deviation)
        {
            DataGridViewRow row = dgvValues.Rows[step];
            var idealVoltage = Convert.ToDouble(row.Cells[(int)ADCDirectColumn.idealV].Value); // эталонное напряжение
            idealVoltage = -1.000499999; //todo убрать после отладки
            double allowDeviation = SearchAllowError(_cpv.ErrorsRanges, idealVoltage); // допустимая отклонение
            if (double.IsNaN(allowDeviation))
            {
                return false;
            }

            row.Cells[(int)ADCDirectColumn.errorV].ToolTipText = "±" + allowDeviation;
            return Math.Abs(deviation) <= allowDeviation;
        }

        /// <summary>
        /// поиск допустимых ошибок
        /// </summary>
        /// <param name="errorInterval">интервал напряжений и допустимая погрешность</param>
        /// <param name="idealVoltage">эталонное напряжение</param>
        /// <returns>допустимую погрешность для напряжения, измеренного эталонным вольтметром</returns>
        private double SearchAllowError(double[,] errorInterval, double idealVoltage)
        {
            if (errorInterval == null || errorInterval.GetLength(0) < 1)
            {
                return double.NaN;
            }

            for (int i = 0; i < errorInterval.GetLength(0); i++)
            {
                if (idealVoltage >= errorInterval[i, 0] && idealVoltage <= errorInterval[i, 1])
                {
                    return errorInterval[i, 2];
                }
            }

            return double.NaN;
        }

        #endregion Методы

        #region Перечисления

        private enum ADCDirectColumn
        {
            pointF = 4,
            pointV = 0,
            idealV = 1,
            measV = 2,
            errorV =3
        }

        #endregion

        private delegate void SetGridValueDelegate(int row, int col, double value);

        #region Циклическое измерение

        private void btnAuto_Click(object sender, EventArgs e)
        {
            var stop = "Остановить";

            if (btnAuto.Text == stop)
            {
                _mreAuto.Set();
                return;
            }

            btnAuto.Enabled = false;
            SetButtonsEnabled(false);

            btnAuto.Text = stop;
            _autoThread = new Thread(AutoThread) { IsBackground = true };

            _mreAuto.Reset();
            _autoThread.Start();
            btnAuto.Enabled = true;
        }

        private void AutoThread()
        {
            _autoCheck = true;

            if (!_isPSOn)
            {
                ShowMessage("Не подключено оборудование для формирования сигнала, " +
                            "автоматическая проверка невозможна!");
                _autoCheck = false;
                return;
            }

            int prevStep;
            do
            {
                Thread.Sleep(100);
                if (GetCurIdealVoltage() > 0 != _setVoltage > 0)
                {
                    ShowMessage("Пожалуйста, поменяйте полярность подключения ИП!");
                    if (CurrentStep > 1)
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(4));
                        break;
                    }
                }

                _lastStepActivateResult = ActivateCurrentStep(true);
                if (!_lastStepActivateResult)
                {
                    ShowMessage("Ошибка установки шага автоматической проверки, " +
                                "возможно не подключено оборудование.");
                    break;
                }

                prevStep = CurrentStep++;

            } while (prevStep < CurrentStep && !_mreAuto.WaitOne(0));

            if (CurrentStep != 0)
                CurrentStep--;

            if (_lastStepActivateResult && !_mreAuto.WaitOne(0))
            {
                do
                {
                    _lastStepActivateResult = ActivateCurrentStep(true);
                    if (!_lastStepActivateResult)
                    {
                        ShowMessage("Ошибка установки шага автоматической проверки, " +
                                    "возможно не подключено оборудование.");
                        break;
                    }
                    prevStep = CurrentStep++;
                } while (prevStep < CurrentStep && !_mreAuto.WaitOne(0));
            }

            _ps.SetVoltage(0);
            _ps.SetAddVoltage(0);
            UnlockAutoControls();
            _autoCheck = false;
        }

        private double GetCurIdealVoltage()
        {
            if (IsDelta27V(_measuredChannel.Name) || IsDelta15V(_measuredChannel.Name))
                return DiffVoltage;
            return IdealVoltage;
        }

        private void UnlockAutoControls()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(UnlockAutoControls));
                return;
            }
            btnAuto.Text = "Авто";
            SetButtonsEnabled(true);
        }

        #endregion

        #region Пошаговое измерение

        private void btnNextStep_Click(object sender, EventArgs e)
        {
            SetButtonsEnabled(false);
            try
            {
                CurrentStep++;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            SetButtonsEnabled(true);
        }


        private void btnMeasure_Click(object sender, EventArgs e)
        {
            SetButtonsEnabled(false);
            try
            {
                //StartNewMeasure(_measuredChannel.Code, 0);
                MeasureCurrentStep();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            SetButtonsEnabled(true);
        }

        private bool MeasureCurrentStep()
        {
            try
            {
                if (!double.IsNaN(GetCurIdealVoltage()))
                {
                    if (Math.Abs(_setVoltage) > 0.05)
                        if (GetCurIdealVoltage() > 0 != _setVoltage > 0)
                            return true;

                    ValidAndRepMeas();

                    if (IsDelta15V(_measuredChannel.Name) || IsDelta27V(_measuredChannel.Name))
                    {
                        Thread.Sleep(2000);
                        Application.DoEvents();
                    }

                    var idealVoltage = Math.Round(GetCurIdealVoltage(), 4);
                    SetGridValue(CurrentStep, (int)ADCDirectColumn.idealV, idealVoltage);
                }
                _resultsReadyEvent.Reset();

                //if (double.IsNaN(_adcVoltage))
                // {
                //   throw new Exception(); todo для отладки
                // }
                while (!_resultsReadyEvent.WaitOne(50))
                    Application.DoEvents();
                var measVoltage = Math.Round(_adcVoltage, 4);
                SetGridValue(CurrentStep, (int)ADCDirectColumn.measV, measVoltage);

            }
            catch
            {
                ShowMessage("Ошибка измерения!");
                return false;
            }

            if (_autoCheck)
                CalculateError(CurrentStep);
            CheckScrollAndSet();
            Application.DoEvents();
            return true;
        }

        private bool IsDelta27V(string measuredChannelName)
        {
            return measuredChannelName == VoltagesArray.Delta27VName;
        }

        private bool IsDelta15V(string measuredChannelName)
        {
            return measuredChannelName == VoltagesArray.Delta15VName;
        }

        private void CheckScrollAndSet()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(CheckScrollAndSet));
                return;
            }
            if (dgvValues.Rows.Count > dgvValues.DisplayedRowCount(true))
                if ((CurrentStep - dgvValues.DisplayedRowCount(true) / 2) > 0)
                    dgvValues.FirstDisplayedScrollingRowIndex = CurrentStep - dgvValues.DisplayedRowCount(true) / 2;
        }

        /// <summary>
        /// проверка на допустимую погрешность показаний эталонного вольтметра,
        /// повторные снятия показаний
        /// </summary>
        private void ValidAndRepMeas()
        {
            var numCycles = 0;

            // допустимая погрешность
            double allowError = _setVoltage > 2.0 ? SetDirectVoltageError : _setVoltage * CoefAlowError;

            while (numCycles < 11) // ограничение количества циклов задержки
            {
                if (Math.Abs(GetCurIdealVoltage() - _setVoltage) < allowError)
                    break;
                Thread.Sleep(DirectCurrentWait);
                Application.DoEvents();
                numCycles++;
            }

            SetDelay();
        }

        private void SetDelay()
        {
            double allowDeviation = SearchAllowError(_cpv.ErrorsRanges, _setVoltage); // допустимое отклонение
            int numInterval = allowDeviation >= 0.01 ? 3 : 4;
            double idVolt = Math.Round(IdealVoltage, 4);
            int numCycles = 0;
            do
            {
                Thread.Sleep(DirectCurrentWait * 5);
                numCycles++;
                if (idVolt != Math.Round(IdealVoltage, numInterval)) idVolt = Math.Round(IdealVoltage, numInterval);
                else break;
            } while (numCycles < 8);
            Thread.Sleep(DirectCurrentWait * 5);
        }


        AutoResetEvent _resultsReadyEvent = new AutoResetEvent(false);

        /// <summary>
        ///   Возврат результат с LTR плат
        /// </summary>
        /// <param name="chList"></param>
        private void ShowResult(List<USBBaseNew> chList)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<List<USBBaseNew>>(ShowResult), new object[] { chList });
                return;
            }
            try
            {
                if (chList.Count > 0)
                {
                    _adcVoltage = chList[0].Result;
                    _resultsReadyEvent.Set();
                }
            }
            catch (Exception)
            {
                // todo для отладки
            }
        }
        #endregion Пошаговое измерение

        private void rbChannel2_CheckedChanged(object sender, EventArgs e)
        {
            CurrentChannel = rbChannel1.Checked ? Channel.C1 : Channel.C2;
        }

        private void dgvValues_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            EnableControls(false);
            try
            {
                if ((int)ADCDirectColumn.pointV == e.ColumnIndex)
                    CurrentStep = e.RowIndex;
                ActivateCurrentStep(true);
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }
            EnableControls(true);
        }

        private void EnableControls(bool isEnabled)
        {
            dgvValues.Enabled = btnAuto.Enabled = isEnabled;
            SetButtonsEnabled(isEnabled);
        }


        private void dgvValues_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (_autoCheck)
                return;
            if ((e.ColumnIndex != (int)ADCDirectColumn.idealV) && (e.ColumnIndex != (int)ADCDirectColumn.measV))
                return;
            try
            {
                CalculateError(e.RowIndex);
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }
        }

        private void frmMeasVoltage_FormClosing(object sender, FormClosingEventArgs e)
        {
            _measWorker.StopMeasuring(false);
        }

        private void frmMeasVoltage_Shown(object sender, EventArgs e)
        {
            if (!Activation())
            {
                Close();
                return;
            }
        }
    }
}