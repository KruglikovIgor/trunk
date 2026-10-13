using System;
using System.Drawing;
using System.Windows.Forms;
using KPK2.GeneralForms.DissForms;
using KPK2.Properties;
using Sparc.KPK2.NFunctions;
using KPK2.Interfaces;

namespace KPK2.MyControl
{
  public partial class StabilizeSpeed : UserControl
  {
    private Timer tmrSpeed = new Timer();
    private DateTime _startTime;
    private Button[] arButtonSpeed = new Button[5];
    private int deltaV;
    private double _speed1;
    private string strSpeed;
    private IFrmWork15Base _frm;
    private const int UpDownMaxTime = 40; //40 секунд для индикатора ВВЕРХ-ВНИЗ
    private const int LeftRightMaxTime = 20; //20 секунд для индикатора ВЛЕВО-ВПРАВО и ВПЕРЁД-НАЗАД и индикатора Угла Сноса
    private const int SpeedMaxTime = 50; //50 секунд для индикатора скорости
    private int _currentMaxTime = 0;

    public StabilizeSpeed(IFrmWork15Base frm)
    {
      _frm = frm;
      InitializeComponent();
    }

    // "Скорости отработки" - 0,1 сек
    private void tmrSpeed_Tick(object sender, EventArgs e)
    {
      TimeSpan interval = DateTime.Now - _startTime;
      siTime.Value = interval.TotalSeconds;
      if (interval.TotalSeconds > _currentMaxTime) 
      {
        tmrSpeed.Stop();
        _speed1 = deltaV / (siTime.Value);
        // вывод полученной скорости отработки 
        siSpeed.Value = _speed1;
        // возвращаем все в исходное 
        for (int i = 0; i < arButtonSpeed.Length; i++)
        {
          if (arButtonSpeed[i].Enabled == false) continue;
          ImageSpeedInZero(arButtonSpeed[i]);
        }
      }
    }

    private int curSpeed;

    private readonly string[] _speed1Tasks = new[] {"Task_W_1_15", "Task_W_3_15"};
    private readonly string[] _speed2Tasks = new[] {"Task_W_2_15", "Task_W_1_15"};
    private readonly string[] _speed3Tasks = new[] {"Task_W_2_15", "Task_W_6_15"};
    private readonly string[] _speed4Tasks = new[] {"Task_W_6_15", "Task_W_5_15"};
    private readonly string[] _speed5Tasks = new[] {"Task_W_6_15", "Task_W_1_15"};

    private readonly string[] _text1Speed = new[]
      {
        Resources.Up, Resources.Back, Resources.Right, string.Format(Resources.US, "=-18"),
        string.Format(Resources.PS, "")
      };

    private string GetButtonTask(string buttonName, int taskNum)
    {
      int buttonNum = -1;
      try
      {
        buttonNum = Convert.ToInt32(buttonName.Substring(10, 1));
      }
      catch
      {
      }

      if ((buttonNum < 0) || (buttonNum > 5))
        return "";
      if ((taskNum < 1) || (taskNum > 2))
        return "";
      string[] current;
      switch (buttonNum)
      {
        case 1:
          current = _speed1Tasks;
          break;
        case 2:
          current = _speed2Tasks;
          break;
        case 3:
          current = _speed3Tasks;
          break;
        case 4:
          current = _speed4Tasks;
          break;
        default:
          current = _speed5Tasks;
          break;
      }
      return current[taskNum - 1];
    }

    private void btn_Speed_Click(object sender, EventArgs e)
    {
      Button current = (Button) sender;
      if (current.FlatStyle == FlatStyle.Standard)
      {
        if (!frmWork15.SetTask(true, GetButtonTask(current.Name, 1)))
          return;
        PressButtonSpeed(current);
        ImageSpeed(current);
        curSpeed = 1;
        SetInformationMessage(current.Name);
        siSpeed.Value = 0;
        siSpeed.Inactive = true;
      }
      else
      {
        if (curSpeed == 1)
        {
          //frmWork15.SetTask(false,""); // сняли КЗ   Убрано не понятно зачем снимать КЗ
          // запустили следующую 
          if (!frmWork15.SetTask(true, GetButtonTask(current.Name, 2)))
          {
            ImageSpeedInZero(current);
            return;
          }
          _startTime = DateTime.Now;
          _currentMaxTime = current == btn_Speed_1
                              ? UpDownMaxTime
                              : current == btn_Speed_5 ? SpeedMaxTime : LeftRightMaxTime;
          tmrSpeed.Start();
          current.Text = Resources.StopTimer;
          curSpeed = 2;
          return;
        }
        if (curSpeed == 2) // Остановить таймер 
        {
          tmrSpeed.Stop();
          CalculateSpeed();
          current.Text = Resources.Save;
          curSpeed = 3;
          ClearInformationMessage();
          return;
        }
        if (curSpeed == 3) // Сохранить результат 
        {
          frmWork15.SetTask(false, "");
          current.Text = _text1Speed[Convert.ToInt32(current.Name.Substring(10, 1)) - 1];
          curSpeed = 0;
          // сохранять если что-нибудь посчитали, см. tmrSpeed
          _frm.ManualRecordDouble(strSpeed, _speed1);
          ImageSpeedInZero(current);
          return;
        }
        // на всякий случай
        tmrSpeed.Stop();
        frmWork15.SetTask(false, "");
        ImageSpeedInZero(current);
      }
    }

    private void CalculateSpeed()
    {
      _speed1 = deltaV / (siTime.Value);
      // вывод полученной скорости отработки 
      siSpeed.Value = _speed1;
    }

    private void SetInformationMessage(string buttonName)
    {
      switch (buttonName)
      {
        case "btn_Speed_1":
          tbInfo.Text = string.Format(Resources.UpDownSpeedMessage, btn_Speed_1.Text);
          break;
        case "btn_Speed_2":
          tbInfo.Text = string.Format(Resources.ForwardBackwardSpeedMessage, btn_Speed_2.Text);
          break;
        case "btn_Speed_3":
          tbInfo.Text = string.Format(Resources.LeftRightSpeedMessage, btn_Speed_3.Text);
          break;
        case "btn_Speed_4":
          tbInfo.Text = string.Format(Resources.AngleSpeedMessage, btn_Speed_4.Text);
          break;
        case "btn_Speed_5":
          tbInfo.Text = string.Format(Resources.SpeedSpeedMessage, btn_Speed_5.Text);
          break;
      }
    }

    private void ClearInformationMessage()
    {
      tbInfo.Text = String.Empty;
    }

    /// <summary>
    /// Возвращаем в исходное вид кнопки и индикаторов при ошибках в
    /// управляющих командах 
    /// </summary>
    /// <param name="btn">нажатая кнопка</param>
    private void ImageSpeedInZero(Button btn)
    {
      frmWork15.SetTask(false, "");
      btn.FlatStyle = FlatStyle.Standard;
      btn.BackColor = Color.Transparent;
      btn.Text = btn == btn_Speed_1
                   ? Resources.Up
                   : btn == btn_Speed_2
                       ? Resources.Back
                       : btn == btn_Speed_3
                           ? Resources.Right
                           : btn == btn_Speed_4 ? string.Format(Resources.US, "=-18") : string.Format(Resources.PS, "");

      curSpeed = 0;
      for (int i = 0; i < arButtonSpeed.Length; i++)
        arButtonSpeed[i].Enabled = true;
      siSpeed.Inactive = siTime.Inactive = true;
    }

    /// <summary>
    /// Настройка вида индикаторов... "Скорости отработки"
    /// </summary>
    /// <param name="btn">нажатая кнопка</param>
    private void ImageSpeed(Button btn)
    {
      grpBox_Speed.Text = btn == btn_Speed_1
                            ? string.Format(Resources.MeasureSpeedWN, "Wy")
                            : btn == btn_Speed_2
                                ? string.Format(Resources.MeasureSpeedWN, "Wx")
                                : btn == btn_Speed_3
                                    ? string.Format(Resources.MeasureSpeedWN, "Wz")
                                    : btn == btn_Speed_4
                                        ? Resources.MeasureSpeedUS
                                        : Resources.MeasureSpeedPS;
      btn.Text = btn == btn_Speed_1
                   ? Resources.Down
                   : btn == btn_Speed_2
                       ? Resources.Forward
                       : btn == btn_Speed_3
                           ? string.Format(Resources.US, "=-18")
                           : btn == btn_Speed_4 ? string.Format(Resources.US, "=+18") : Resources.Forward;

      deltaV = btn == btn_Speed_1
                 ? 8
                 : btn == btn_Speed_2
                     ? 20
                     : btn == btn_Speed_3
                         ? 20
                         : btn == btn_Speed_4 ? 15 : 100;
      strSpeed = btn == btn_Speed_1
                   ? "Spd_Wy_15"
                   : btn == btn_Speed_2
                       ? "Spd_Wx_15"
                       : btn == btn_Speed_3 ? "Spd_Wz_15" : btn == btn_Speed_4 ? "Spd_YC_15" : "Spd_PC_15";


      lWorking3.Text = btn == btn_Speed_1
                         ? Resources.Vstab + "," + Resources.MSec + Resources.InSec
                         : btn == btn_Speed_2
                             ? Resources.Vstab + "," + Resources.KmH + Resources.InSec
                             : btn == btn_Speed_3
                                 ? Resources.Vstab + "," + Resources.KmH + Resources.InSec
                                 : btn == btn_Speed_4
                                     ? Resources.Vstab + " " + Resources.AlfaGrad + Resources.InSec
                                     : Resources.Vstab + "," + Resources.KmH + Resources.InSec;
      double min = 0, max = 0;
      if (!DataManager.Instance.GetParamLimitAndId(strSpeed, ref min, ref max))
        MessageBox.Show(Resources.Error);
      siSpeed.MaxValue = max;
      siSpeed.MinValue = min;

    }

    /// <summary>
    /// Блокируем все кнопки "Скорости отработки" 
    /// </summary>
    /// <param name="btn">кнопка, которую нажали</param>
    private void PressButtonSpeed(Button btn)
    {
      btn.FlatStyle = FlatStyle.Flat;
      btn.BackColor = Color.FromArgb(153, 249, 164);
      for (int i = 0; i < arButtonSpeed.Length; i++)
        arButtonSpeed[i].Enabled = arButtonSpeed[i].Name == btn.Name;
    }

    private void StabilizeSpeed_Load(object sender, EventArgs e)
    {
      ConfigureSpeedTimer();
      FillButtonArray();
      grpBox_Speed.Text = Resources.MeasureSpeedStabilize;
      
    }

    private void ConfigureSpeedTimer()
    {
      tmrSpeed.Interval = 100;
      tmrSpeed.Tick += tmrSpeed_Tick;
    }

    private void FillButtonArray()
    {
      arButtonSpeed[0] = btn_Speed_1;
      arButtonSpeed[1] = btn_Speed_2;
      arButtonSpeed[2] = btn_Speed_3;
      arButtonSpeed[3] = btn_Speed_4;
      arButtonSpeed[4] = btn_Speed_5;
    }

    private void StabilizeSpeed_EnabledChanged(object sender, EventArgs e)
    {
      if (!Enabled)
        siTime.Inactive = siSpeed.Inactive = true;
    }
  }
}
