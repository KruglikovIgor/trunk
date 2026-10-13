using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using KPK2.Properties;
using Sparc.KPK2.NFunctions;
using Sparc.KPK2.Hardware;
using Sparc.KPK2.Imitator;
using System.Threading;
using KPK2.MyControl.DISS.FrmDiss32;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using Sparc.KPK2.Imitator.Hardware;

namespace KPK2.MyControl
{
	public partial class BPK : UserControl
	{
        private const int _collectInterval = 500;
        
        private Func<string, double, bool> _manualRecordDoubleFunc;
		private int defaultBPKValue = -100;
		private bool _flightVSS = false;
		private bool _secondUSPS = false;
		private bool _saveFdPlus = false;
		private bool _saveFdMinus = false;
		private List<UserControlsKPK2.SegmentIndicatorControl[]> segmentIndicatorsFd;
        private List<MethodInvoker> _renewDelegateList = new List<MethodInvoker>();
        private Imitator.Device _device;
		private Button[] _buttonsControlTasks;
		private Task _taskFd;
		private CancellationTokenSource _cts;
		private int _countButton = 0;
		private double _fd1Plus, _fd2Plus, _fd3Plus;
		private double _fd1Minus, _fd2Minus, _fd3Minus;
		private ControlTasks _currentTask;

        public BPK(Func<string, double, bool> manualRecordDoubleFunc, bool flightVSS, bool secondUSPS)
		{
			_manualRecordDoubleFunc = manualRecordDoubleFunc;
			_flightVSS = flightVSS;
			_secondUSPS = secondUSPS;
			InitializeComponent();
			if (!secondUSPS)
			{
				gbPC2.Visible = false;
				grpBox_PC.Text = string.Format(Resources.DataFromIndicatorN, "УС ПС");
			}

			if (Globals.Emulation)
			{
				var diss32 = new Imitator.Diss32();
				diss32.SetBPKWork();
                _device = diss32;				
			}

        }

		public void CalculationBPK()
		{
			double bpkForvard = defaultBPKValue;
			double bpkLeft = defaultBPKValue;
			double bpkDown = defaultBPKValue;
			double bpkBack = defaultBPKValue;
			double bpkRight = defaultBPKValue;
			double bpkUp = defaultBPKValue;
			double bpkPc1 = defaultBPKValue;
			double bpkYc0 = defaultBPKValue;
			double bpkPc2 = defaultBPKValue;
			double bpkYc9 = defaultBPKValue;
			double bpkPc1_2 = defaultBPKValue;
			double bpkYc0_2 = defaultBPKValue;
			double bpkPc2_2 = defaultBPKValue;
			double bpkYc9_2 = defaultBPKValue;

			// ВПЕРЕД
			ColorTextBoxBPK(txtBox_Forward, ref bpkForvard, 14.5, 19.5, 17.0);
			// ВЛЕВО
			ColorTextBoxBPK(txtBox_Left, ref bpkLeft, 14.5, 19.5, 17.0);
			// ВНИЗ
			ColorTextBoxBPK(txtBox_Down, ref bpkDown, 2.6, 3.6, 3.1);
			// НАЗАД
			ColorTextBoxBPK(txtBox_Back, ref bpkBack, 14.5, 19.5, 17.0);
			// ВПРАВО
			ColorTextBoxBPK(txtBox_Right, ref bpkRight, 14.5, 19.5, 17.0);
			// ВВЕРХ
			ColorTextBoxBPK(txtBox_Up, ref bpkUp, 2.6, 3.6, 3.1);
			// ПС 1
			if (_flightVSS)
				ColorTextBoxBPK(txtBox_PC1, ref bpkPc1, 123.5, 130.5, 127.0);
			else
				ColorTextBoxBPK(txtBox_PC1, ref bpkPc1, 124.5, 129.5, 127.0);
			// УС 0
			ColorTextBoxBPK(txtBox_YC0, ref bpkYc0, -1.0, 1.0, 0);
			// ПС 2
			if (_flightVSS)
				ColorTextBoxBPK(txtBox_PC2, ref bpkPc2, 253, 263, 258);
			else
				ColorTextBoxBPK(txtBox_PC2, ref bpkPc2, 254.5, 261.5, 258);
			// УС 9,5
			ColorTextBoxBPK(txtBox_YC9, ref bpkYc9, 8.5, 10.5, 9.5);

			// ПС 1
			if (_flightVSS)
				ColorTextBoxBPK(tbPC1_2, ref bpkPc1_2, 123.5, 130.5, 127.0);
			else
				ColorTextBoxBPK(tbPC1_2, ref bpkPc1_2, 124.5, 129.5, 127.0);
			// УС 0
			ColorTextBoxBPK(tbYC0_2, ref bpkYc0_2, -1.0, 1.0, 0);
			// ПС 2
			if (_flightVSS)
				ColorTextBoxBPK(tbPC2_2, ref bpkPc2_2, 253, 263, 258);
			else
				ColorTextBoxBPK(tbPC2_2, ref bpkPc2_2, 254.5, 261.5, 258);
			// УС 9,5
			ColorTextBoxBPK(tbYC9_2, ref bpkYc9_2, 8.5, 10.5, 9.5);


			if(Mode.GetMode().ModeApplication == ModeApplication.RepairCheck)
            {
				
                RecordDouble("BPK_Forward_32_RepairTest", bpkForvard - 17, 17);
                RecordDouble("BPK_Left_32_RepairTest", bpkLeft - 17, 17);
                RecordDouble("BPK_Down_32_RepairTest", bpkDown - 3, 3);
                RecordDouble("BPK_Back_32_RepairTest", bpkBack - 17, 17);
                RecordDouble("BPK_Right_32_RepairTest", bpkRight - 17, 17);
                RecordDouble("BPK_Up_32_RepairTest", bpkUp - 3, 3);
                RecordDouble("BPK_YC0_32_RepairTest", bpkYc0);
                RecordDouble("BPK_YC9_32_RepairTest", bpkYc9 - 9.5, 9.5);
				if (_flightVSS)
				{
					RecordDouble("BPK_PC2_32_FlightVSS_RepairTest", bpkPc2 - 258, 258);
					RecordDouble("BPK_PC1_32_FlightVSS_RepairTest", bpkPc1 - 127, 127);
				}
				else
				{
                    RecordDouble("BPK_PC1_32_RepairTest", bpkPc1 - 127, 127);
                    RecordDouble("BPK_PC2_32_RepairTest", bpkPc2 - 258, 258);
                }

                double defFd1PlusValue = 250;
				double defFd2PlusValue = 500;
                double defFd3PlusValue = 250;
                double defFd1MinusValue = 250;
                double defFd2MinusValue = 500;
                double defFd3MinusValue = 250;

				_fd1Plus = segmentIndicatorsFd[0][0].Value;
				_fd2Plus = segmentIndicatorsFd[0][1].Value;
				_fd3Plus = segmentIndicatorsFd[0][2].Value;
				_fd1Minus = segmentIndicatorsFd[1][0].Value;
				_fd2Minus =segmentIndicatorsFd[1][1].Value;
				_fd3Minus = segmentIndicatorsFd[1][2].Value;
				
				double fd1Plus = (_fd1Plus - defFd1PlusValue) / defFd1PlusValue * 100.0;
				double fd2Plus = (_fd2Plus - defFd2PlusValue) /  defFd2PlusValue * 100.0;
                double fd3Plus = (_fd3Plus - defFd3PlusValue) / defFd3PlusValue * 100.0;
                double fd1Minus = (_fd1Minus- defFd1MinusValue) / defFd1MinusValue * 100.0;
                double fd2Minus = (_fd2Minus - defFd2MinusValue) / defFd2MinusValue * 100.0;
                double fd3Minus = (_fd3Minus - defFd3MinusValue) / defFd3MinusValue * 100.0;
				if (_saveFdPlus)
				{
					_manualRecordDoubleFunc("Plus_Fd1", Math.Abs(fd1Plus));
					_manualRecordDoubleFunc("Plus_Fd2", Math.Abs(fd2Plus));
					_manualRecordDoubleFunc("Plus_Fd3", Math.Abs(fd3Plus));
				}
				if (_saveFdMinus)
				{
					_manualRecordDoubleFunc("Minus_Fd1", Math.Abs(fd1Minus));
					_manualRecordDoubleFunc("Minus_Fd2", Math.Abs(fd2Minus));
					_manualRecordDoubleFunc("Minus_Fd3", Math.Abs(fd3Minus));
				}
            }
			else
            {
                RecordDouble("BPK_Forward_32", bpkForvard);
                RecordDouble("BPK_Left_32", bpkLeft);
                RecordDouble("BPK_Down_32", bpkDown);
                RecordDouble("BPK_Back_32", bpkBack);
                RecordDouble("BPK_Right_32", bpkRight);
                RecordDouble("BPK_Up_32", bpkUp);
                RecordDouble("BPK_PC1_32", bpkPc1);
                RecordDouble("BPK_PC1_32_FlightVSS", bpkPc1);
                RecordDouble("BPK_YC0_32", bpkYc0);
                RecordDouble("BPK_PC2_32", bpkPc2);
                RecordDouble("BPK_PC2_32_FlightVSS", bpkPc2);
                RecordDouble("BPK_YC9_32", bpkYc9);

                RecordDouble("BPK_PC1_2_32", bpkPc1_2);
                RecordDouble("BPK_PC1_2_32_FlightVSS", bpkPc1_2);
                RecordDouble("BPK_YC0_2_32", bpkYc0_2);
                RecordDouble("BPK_PC2_2_32", bpkPc2_2);
                RecordDouble("BPK_PC2_2_32_FlightVSS", bpkPc2_2);
                RecordDouble("BPK_YC9_2_32", bpkYc9_2);

            }
        }

        public void ActivateFd()
        {
			_cts = new CancellationTokenSource();
			_taskFd = new Task(() => ContrMonitor(_cts.Token), _cts.Token);
			_taskFd.Start();
        }

        public void DeactivateFd()
        {
			if (_cts != null)
			{
				_cts.Cancel();
                _taskFd.Wait();
                _cts.Dispose();
				_cts = null;
			}
			if (_taskFd != null)
			{
				_taskFd.Dispose();
				_taskFd = null;
			}
        }

        public void DisableButton()
        {
            if (_countButton > 0) // есть нажатые кнопки
            {
                for (int i = 0; i < _buttonsControlTasks.Length; i++)
                    if (_buttonsControlTasks[i].FlatStyle == FlatStyle.Flat)
                    {
						_buttonsControlTasks[i].PerformClick();
                    }
            }
        }

        private List<double> GetArray()
        {
            List<double> arrays = new List<double>();
			if (Globals.Emulation)
			{
				HImitator.ControlTask.GetFrequency(out arrays);
            }
			else
				ControlTask.Instance().GetFrequency(ref arrays);

            if (arrays == null)
                return null;
            
            return arrays;
        }

		private void ContrMonitor(CancellationToken cToken)
		{
			Statistics[] statisticsChannelFd = new Statistics[3];

			List<double> resultArrayFd;

            statisticsChannelFd[0] = new Statistics() { LimitationValue = 20};
            statisticsChannelFd[1] = new Statistics() { LimitationValue = 20 };
            statisticsChannelFd[2] = new Statistics() { LimitationValue = 20 };
			double fd1, fd2, fd3;

            while(true)//todo добавить ограничение на 20 значений
			{
				if (cToken.IsCancellationRequested)
				{
					statisticsChannelFd[0].Reset();
					statisticsChannelFd[1].Reset();
					statisticsChannelFd[2].Reset();
					statisticsChannelFd[0] = null;
					statisticsChannelFd[1] = null;
					statisticsChannelFd[2] = null;
					statisticsChannelFd = null;
					return;
				}

				resultArrayFd = GetArray();

                if (_currentTask == ControlTasks.Task1)
                {
					for (int i = 0; i < 250; i++)
					{
						if(resultArrayFd[0] > 50 && resultArrayFd[0] < 700)
							statisticsChannelFd[0].Add(resultArrayFd[0]);

						if(resultArrayFd[2] > 50 && resultArrayFd[2] < 700)
							statisticsChannelFd[1].Add(resultArrayFd[2]);

						if(resultArrayFd[4] > 50 && resultArrayFd[4] < 700)
                            statisticsChannelFd[2].Add(resultArrayFd[4]);
					}

                    fd1 = statisticsChannelFd[0].GetPrms().Mean;
                    fd2 = statisticsChannelFd[1].GetPrms().Mean;
                    fd3 = statisticsChannelFd[2].GetPrms().Mean;

					segmentIndicatorsFd[0][0].Value = fd1;
					segmentIndicatorsFd[0][1].Value = fd2;
                    segmentIndicatorsFd[0][2].Value = fd3;

					//statisticsChannelFd[0].Reset();
					//statisticsChannelFd[1].Reset();
					//statisticsChannelFd[2].Reset();
                }

                if (_currentTask == ControlTasks.Task2)
                {
					for (int i = 0; i < 250; i++)
					{
						if(resultArrayFd[1] > 50 && resultArrayFd[1] < 700)
                            statisticsChannelFd[0].Add(resultArrayFd[1]);

						if(resultArrayFd[3] > 50 && resultArrayFd[3] < 700)
                            statisticsChannelFd[1].Add(resultArrayFd[3]);

						if(resultArrayFd[5] > 50 && resultArrayFd[5] < 700)
                            statisticsChannelFd[2].Add(resultArrayFd[5]);
					}

                    fd1 = statisticsChannelFd[0].GetPrms().Mean;
                    fd2 = statisticsChannelFd[1].GetPrms().Mean;
                    fd3 = statisticsChannelFd[2].GetPrms().Mean;

                    segmentIndicatorsFd[1][0].Value = fd1;
                    segmentIndicatorsFd[1][1].Value = fd2;
                    segmentIndicatorsFd[1][2].Value = fd3;

					//statisticsChannelFd[0].Reset();
					//statisticsChannelFd[1].Reset();
					//statisticsChannelFd[2].Reset();
                }
				Thread.Sleep(2000);
            }
        }

        private void TCollectParamsOnTick(object sender, EventArgs eventArgs)
        {
        }

		private void SetSettingsStatistick(Statistics[] statistics, bool setLimit)
		{

		}

        private void RecordDouble(string name, double value)
		{
			if (Math.Abs(value - defaultBPKValue) > double.Epsilon)
				_manualRecordDoubleFunc(name, value);
		}

        private void RecordDouble(string name, double value, double valueCorrection)
        {
            if (Math.Abs((value + valueCorrection) - defaultBPKValue) > double.Epsilon)
                _manualRecordDoubleFunc(name, value);
        }

        /// <summary>
        /// Выделяет цветом данные в TextBox-ах - "зеленый коридор"
        /// </summary>
        /// <param name="txtBox">проверяемый TextBox</param>
        /// <param name="var">переменная для БД</param>
        /// <param name="min">min "зеленого коридора"</param>
        /// <param name="max">max "зеленого коридора"</param>
        /// <param name="d">середина диапазона</param>
        private void ColorTextBoxBPK(TextBox txtBox, ref double outValue, double min, double max, double idealValue)
		{
			if (txtBox.Text != "")
			{
				double dCurrent = Functions.MoveToDouble(txtBox.Text);
				if (double.IsNaN(dCurrent))
					MessageBox.Show(string.Format(Resources.ErrorInputNValue, txtBox.Text));
				outValue = Math.Round(dCurrent, 2); // 2 знака после запятой 
				txtBox.Text = outValue.ToString();
				txtBox.ForeColor = min <= dCurrent && dCurrent <= max ? Color.LimeGreen : Color.Red;
			}
		}

		private void txtBox_Leave(object sender, EventArgs e)
		{
			TextBox curTb = ((TextBox)sender);
			curTb.Text = curTb.Text.Trim();
			if (String.IsNullOrEmpty(curTb.Text))
				return;
			double testValue = Functions.MoveToDouble(curTb.Text);
			if (double.IsNaN(testValue))
			{
				MessageBox.Show(string.Format(Resources.ErrorInputNValue, curTb.Text));
				curTb.Focus();
			}
		}

		private void BPK_Load(object sender, EventArgs e)
        {
            var mode = Mode.GetMode().ModeApplication;
			if (mode == ModeApplication.DefaultCheck)
				return;

			InitializeRepairCheck();
        }
		
		private void InitializeRepairCheck()
		{
            Size = new Size(950, 320);

			InitializeIndicatorsFd();
			MoveGrpBoxPC();
			InitializeButtonContolTasks();
        }

		private void InitializeIndicatorsFd()
        {
            int positionX = 330;
            int positionY = 10;
            int intervalX = 95;
            int intervalY = 80;
			segmentIndicatorsFd = new List<UserControlsKPK2.SegmentIndicatorControl[]>
			{
				new[]
				{
					new UserControlsKPK2.SegmentIndicatorControl()
					{
						MaxValue = 251.5,
						MinValue = 248.5,
						NameLabelUp = "+Fd 1, Гц",
						NameLabelDown = "",
						Font = new Font("Microsoft Sans Serif", 9),
						Enabled = false,
						Inactive = true
					},
					new UserControlsKPK2.SegmentIndicatorControl()
					{
						MaxValue = 503,
						MinValue = 497,
						NameLabelUp = "+Fd 2, Гц",
						NameLabelDown = "",
						Font = new Font("Microsoft Sans Serif", 9),
						Enabled = false,
						Inactive = true
					},
					new UserControlsKPK2.SegmentIndicatorControl()
					{
						MaxValue = 251.5,
						MinValue = 248.5,
						NameLabelUp = "+Fd 3, Гц",
						NameLabelDown = "",
						Font = new Font("Microsoft Sans Serif", 9),
						Enabled = false,
						Inactive = true
					}
				},
				new[]
				{
					new UserControlsKPK2.SegmentIndicatorControl()
					{
						MaxValue = 251.5,
						MinValue = 248.5,
						NameLabelUp = "-Fd 1, Гц",
						NameLabelDown = "",
						Font = new Font("Microsoft Sans Serif", 9),
						Enabled = false,
						Inactive = true
					},
					new UserControlsKPK2.SegmentIndicatorControl()
					{
						MaxValue = 503,
						MinValue = 497,
						NameLabelUp = "-Fd 2, Гц",
						NameLabelDown = "",
						Font = new Font("Microsoft Sans Serif", 9),
						Enabled = false,
						Inactive = true
					},
					new UserControlsKPK2.SegmentIndicatorControl()
					{
						MaxValue = 251.5,
						MinValue = 248.5,
						NameLabelUp = "-Fd 3, Гц",
						NameLabelDown = "",
						Font = new Font("Microsoft Sans Serif", 9),
						Enabled = false,
						Inactive = true
					}
				}
			};

            for (int i = 0; i < segmentIndicatorsFd.Count; i++)
            {
                for (int j = 0; j < segmentIndicatorsFd[i].Length; j++)
                {
                    segmentIndicatorsFd[i][j].Font = new Font("Microsoft Sans Serif", 8.25F);
                    Controls.Add(segmentIndicatorsFd[i][j]);
                    segmentIndicatorsFd[i][j].Location = new Point(positionX + intervalX * j, positionY + intervalY * i);
                }
            }
        }

		private void MoveGrpBoxPC()
		{
			grpBox_PC.Location = new Point(3, 150);
			grpBox_PC.Size = new Size(440, 80);
			
			label21.Location = new Point(label18.Location.X + 96, label18.Location.Y);
			txtBox_PC2.Location = new Point(txtBox_YC0.Location.X + (txtBox_YC0.Location.X - txtBox_PC1.Location.X), txtBox_YC0.Location.Y);
			label23.Location = new Point(label20.Location.X + 96, label20.Location.Y);

			label22.Location = new Point(label21.Location.X + 96, label21.Location.Y);
			txtBox_YC9.Location = new Point(txtBox_PC2.Location.X + (txtBox_PC2.Location.X - txtBox_YC0.Location.X), txtBox_PC2.Location.Y);
			label24.Location = new Point(label23.Location.X + 96, label23.Location.Y);
		}

		private void InitializeButtonContolTasks()
        {
            int positionX = 650;
			int positionY = 45;
            int intervalY = 60;

            Button btCT1 = InitializeButton("btCT1", "Измерение +Fd", ControlTask1FdPlus_Click);
			Button btCT2 = InitializeButton("btCT2", "Измерение -Fd", ControlTask2FdMinus_Click);
            //Button btCT3 = InitializeButton("btCT3", "КЗ-3", ControlTask3_Click);
			//Button btCT4 = InitializeButton("btCT4", "КЗ-4", ControlTask4_Click);

            _buttonsControlTasks = new Button[]
			{
				btCT1,
				btCT2
				//btCT3,
				//btCT4
			};

			for(int i = 0; i < _buttonsControlTasks.Length; i++)
			{
				Controls.Add(_buttonsControlTasks[i]);
				_buttonsControlTasks[i].Location = new Point(positionX, positionY + i * intervalY);
			}
		}

		private Button InitializeButton(string name, string text, EventHandler eventClick)
		{
			Button btCT = new Button();
            btCT.Name = name;
			btCT.Text = text;
			btCT.Font = new Font("Microsoft Sans Serif", 9);
			btCT.Click += eventClick;
			btCT.Size = new Size(btCT.Width + 50, btCT.Height);
			return btCT;
        }

		private void ControlsEnableFromTask(ControlTasks ct)
		{
			switch (ct)
			{
				case (ControlTasks.Task1):
					EnableSegmentIndicator(segmentIndicatorsFd[1], false);
                    EnableSegmentIndicator(segmentIndicatorsFd[0], true);
					break;
				case (ControlTasks.Task2):
					EnableSegmentIndicator(segmentIndicatorsFd[0], false);
					EnableSegmentIndicator(segmentIndicatorsFd[1], true);
					break;
				case (ControlTasks.Task3):
					EnableSegmentIndicator(segmentIndicatorsFd[0], false);
					EnableSegmentIndicator(segmentIndicatorsFd[1], false);
					break;
				case (ControlTasks.Task4):
                    EnableSegmentIndicator(segmentIndicatorsFd[0], false);
                    EnableSegmentIndicator(segmentIndicatorsFd[1], false);
                    break;
				case (ControlTasks.NotTask):
                    EnableSegmentIndicator(segmentIndicatorsFd[0], false);
                    EnableSegmentIndicator(segmentIndicatorsFd[1], false);
					break;
			}
		}

		#region event buttons Task
        private void ControlTask1FdPlus_Click(object sender, EventArgs e)
        {
			Button btnTask1 = (Button)sender;
			if (btnTask1.FlatStyle == FlatStyle.Standard)
			{
				_countButton = 1;
				_saveFdPlus = true;
				//MessageBox.Show("Нажмите на БПК кнопку \"Вперед-17 Влево-17 Вниз-3\"", "Информация",
				//	MessageBoxButtons.OK, MessageBoxIcon.Information);
				_currentTask = ControlTasks.Task1;
				PressButton(btnTask1, false);
				ControlsEnableFromTask(_currentTask);
				ActivateFd();
			}
			else
			{
				_countButton = 0;
				_fd1Plus = segmentIndicatorsFd[0][0].Value;
				_fd2Plus = segmentIndicatorsFd[0][1].Value;
				_fd3Plus = segmentIndicatorsFd[0][2].Value;
				WrungButton(btnTask1);
				DeactivateFd();
				_currentTask = ControlTasks.NotTask;
                ControlsEnableFromTask(_currentTask);
            }			
		}

		private void ControlTask2FdMinus_Click(object sender, EventArgs eventArgs)
		{
			Button btnTask2 = (Button)sender;
			if (btnTask2.FlatStyle == FlatStyle.Standard)
            {
				_countButton = 1;
				_saveFdMinus = true;
                //MessageBox.Show("Нажмите на БПК кнопку \"Назад-17 Вправо-17 Вверх-3\"", "Информация",
                //    MessageBoxButtons.OK, MessageBoxIcon.Information);
				_currentTask = ControlTasks.Task2;
                PressButton(btnTask2, false);
                ControlsEnableFromTask(_currentTask);
                ActivateFd();
            }
			else
			{
				_countButton = 0;
				_fd1Minus = segmentIndicatorsFd[1][0].Value;
				_fd2Minus = segmentIndicatorsFd[1][1].Value;
				_fd2Minus = segmentIndicatorsFd[1][2].Value;
				WrungButton(btnTask2);
				DeactivateFd();
				_currentTask = ControlTasks.NotTask;
                ControlsEnableFromTask(_currentTask);
            }
		}
        #endregion

        private void EnableSegmentIndicator(IEnumerable<UserControlsKPK2.SegmentIndicatorControl> segmentIndicators, bool enable) //todo вынести в спомогательный метод работы с формой
		{
			foreach (var sic in segmentIndicators) 
			{ 
				sic.Enabled = enable;
				sic.Inactive = !enable;
			}
		}

        private void PressButton(Button btn, bool enableBtns)//todo вынести в спомогательный класс для работы с формами
        {
            _countButton = 1;
            btn.FlatStyle = FlatStyle.Flat;
            btn.BackColor = Color.FromArgb(153, 249, 164);
            Control.ControlCollection collButton = btn.Parent.Controls;
            for (int i = 0; i < collButton.Count; i++)
                if (collButton[i].GetType() == typeof(Button) &&
                        collButton[i].Name != btn.Name)
                    collButton[i].Enabled = enableBtns;
        }

        private void WrungButton(Button btn)
        {
            _countButton = 0;
            btn.FlatStyle = FlatStyle.Standard;
            btn.BackColor = Color.Transparent;
            Control.ControlCollection collButton = btn.Parent.Controls;
            for (int i = 0; i < collButton.Count; i++)
                if (collButton[i].GetType() == typeof(Button))
                    collButton[i].Enabled = true;
        }
    }

	public enum ControlTasks
    {
        NotTask = -1,
        Task1 = 1,
		Task2 = 2,
		Task3 = 3,
		Task4 = 4
	}
}
