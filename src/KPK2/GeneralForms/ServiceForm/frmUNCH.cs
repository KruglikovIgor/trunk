using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Sparc.Hardware.Akip3418;
using KPK2.Common.MeasureVoltage;
using System.Threading;
using Sparc.KPK2.Hardware;
using Sparc.KPK2.Hardware.LTR;
using Sparc.KPK2.Hardware.MyClasseUSB;
using USBVoltmeter;

namespace KPK2.GeneralForms.ServiceForm
{
    public partial class frmUNCH : Form
    {
        public frmUNCH(MeasuredChannel mc)
        {
            InitializeComponent();
            _measuredChannel = mc;
        }

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

        private bool _isBandThree;

        private bool _isBandTwenty;

        private readonly Measure _measWorker = new Measure();
        private readonly ManualResetEvent _mreAuto = new ManualResetEvent(false);

        private IAkip3418 _ps;

        private readonly Color _failColor = Color.LightCoral;
        private readonly Color _successColor = Color.LightGreen;

        private Thread _autoThread;



        /// <summary>
        /// напряжение переменного тока, установленное генератором
        /// </summary>
        private double _setVoltage;
        /// <summary>
        /// Частота, установленное генератором
        /// </summary>
        private double _setFrequency;
        /// <summary>
        /// напряжение, измеряемое платой
        /// </summary>
        private double _adcVoltage;

        private Channel _currentChannel = Channel.C1;

        private readonly MeasuredChannel _measuredChannel;
        private bool _isPSOn;
        private bool _lastStepActivateResult;
        private int _nStep;
        private bool _autoCheck;

        /// <summary>
        /// массив точек измерения напряжения для текущего измерительного канала
        /// </summary>
        private UNCH _cpv;


        #endregion

        #region Загрузка

        private void frmUNCH_Load(object sender, EventArgs e)
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
                _cpv = UNCHArray.GetUNCH(_measuredChannel.Name);
                foreach (int freq in _cpv.FreqPoints)
                {
                    foreach (double voltage in _cpv.ControlPoints)
                    {
                        object[] row = { freq, voltage, DBNull.Value, DBNull.Value, DBNull.Value };
                        dgvValues.Rows.Add(row);
                    }
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
                InitGenerator();
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
        ///   Инициализация и подключение генератора акип-3418
        /// </summary>
        private void InitGenerator()
        {
            _ps = new Akip3418();
            _ps.Connect("USB", "0xF4EC", "0xEE38");
            _ps.SetOutputState(CurrentChannel ,true);
            _ps.SetSignalType(CurrentChannel, SignalType.SINE);       // установка синусоидального сигнала на выходе  
            _ps.SetSignalPhase(CurrentChannel, 0);              // установка начальной фазы
            _ps.SetSignalOffset(CurrentChannel, 0);            // установка постоянного смещения
            _ps.SetSignalFrequency(CurrentChannel, 20);     // установка частоты
            _ps.SetSignalAmplitude(CurrentChannel, 0.05);   // установка напряжения
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
                if (dc.Index == 0) dc.DefaultCellStyle.Format = "N0";
                else dc.DefaultCellStyle.Format = "N4";
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

        public double ConvertIsVPPtomVPP(double number)
        {
            number = number * 2 * Math.Sqrt(2);
            return number;
        }

        public Channel CurrentChannel
        {
            get { return _currentChannel; }
            set
            {
                if (_currentChannel != value)
                {
                    _currentChannel = value;
                    _ps = new Akip3418();
                    if (_ps != null)
                    {
                        _ps.Connect("USB", "0xF4EC", "0xEE38");
                        if (CurrentChannel != Channel.C1)
                        {
                            _ps.SetOutputState(Channel.C1, false);
                            _ps.SetOutputState(Channel.C2, true);
                        }
                        else
                        {
                            _ps.SetOutputState(Channel.C2, false);
                            _ps.SetOutputState(Channel.C1, true);
                        }
                        _ps.SetSignalType(CurrentChannel, SignalType.SINE);
                        _ps.SetSignalPhase(CurrentChannel, 0);
                        _ps.SetSignalOffset(CurrentChannel, 0);
                        _ps.SetSignalFrequency(CurrentChannel, ConvertIsVPPtomVPP(Convert.ToDouble(dgvValues.Rows[_nStep].Cells[(int)ADCDirectColumn.pointF].Value))); // установка частоты
                        _ps.SetSignalAmplitude(CurrentChannel, Convert.ToDouble(dgvValues.Rows[_nStep].Cells[(int)ADCDirectColumn.pointV].Value)); // установка напряжения

                    }
                }
            }
        }

        /// <summary>
        /// Значение напряжения эталонного вольтметра.
        /// Устанавливается из кода родительской формы.
        /// </summary>
        public double IdealVoltage { get; set; }

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

                _setVoltage = ConvertIsVPPtomVPP(Convert.ToDouble(dgvValues.Rows[CurrentStep].Cells[(int) ADCDirectColumn.pointV].Value));
                _setFrequency = Convert.ToDouble(dgvValues.Rows[CurrentStep].Cells[(int) ADCDirectColumn.pointF].Value);

                if (_isPSOn)
                {
                    _ps.SetSignalAmplitude(CurrentChannel, _setVoltage);
                    _ps.SetSignalFrequency(CurrentChannel, _setFrequency);

                    if (_setFrequency > 100 && !_isBandTwenty)
                    {
                        V7_VoltmeterManagement.GetVoltmeter(0).SetBandwidthHigh();
                        _isBandThree = false;
                        _isBandTwenty = true;
                    }
                   

                    if (_setFrequency <= 100 && !_isBandThree)
                    {
                        V7_VoltmeterManagement.GetVoltmeter(0).SetBandwidthLow();
                        _isBandThree = true;
                        _isBandTwenty = false;
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
                if (errorInterval[i, 1] < errorInterval[i, 0])
                    throw new Exception("Max less than Min! Incorrect format or revert values!");

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
            pointF,
            pointV,
            idealV,
            measV,
            errorV
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
                if (IdealVoltage >= 0 != _setVoltage > 0)
                {
                    ShowMessage("Пожалуйста, поменяйте полярность подключения Генератора!");
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

            //удалил второй блок do-while, который повторно запускал проход по таблице

            _ps.SetSignalAmplitude(CurrentChannel,0);
            _ps.SetSignalFrequency(CurrentChannel,0);
             UnlockAutoControls();
            _autoCheck = false;
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
        private int CurrentStep
        {
            get { return _nStep; }
            set { SetCurrentStep(value); }
        }
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
                ActivateCurrentStep(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            SetButtonsEnabled(true);
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
        private bool MeasureCurrentStep()
        {
            try
            {
                _mreAuto.Reset();
                _mreAuto.WaitOne(5000);
                var idealVoltage = V7_VoltmeterManagement.GetVoltmeter(0).MeasureDouble();
                if (!double.IsNaN(idealVoltage))
                {
                    ValidAndRepMeas(idealVoltage);
                    SetGridValue(CurrentStep, (int) ADCDirectColumn.idealV, idealVoltage);
                }
                //_mreAuto.Set();
  
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
        private void ValidAndRepMeas(double idealVolt)
        {
            var numCycles = 0;

            // допустимая погрешность
            double allowError = _setVoltage > 2.0 ? SetDirectVoltageError : _setVoltage * CoefAlowError;

            while (numCycles < 11) // ограничение количества циклов задержки
            {
                if (Math.Abs(idealVolt - _setVoltage) < allowError)
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

        ///<summary>
        ///Возврат результат с LTR плат
        ///</summary>
        ///<param name="chList"></param>
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

        private void rbChannel1_CheckedChanged(object sender, EventArgs e)
        {
            CurrentChannel = rbChannel1.Checked ? Channel.C1 : Channel.C2;
        }

        private void frmUNCH_Shown(object sender, EventArgs e)
        {
            if (!Activation())
            {
                Close();
                return;
            }
        }

        private void frmUNCH_FormClosing(object sender, FormClosingEventArgs e)
        {
            V7_VoltmeterManagement.GetVoltmeter(0).SetBandwidthHigh();
            _measWorker.StopMeasuring(false);
            V7_VoltmeterManagement.StartMeasuringVoltmeters();
        }
    }
}
