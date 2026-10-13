using System;
using System.Drawing;
using System.Threading;
using KPK2.Properties;
using Sparc.KPK2.Hardware;
using Sparc.KPK2.Hardware.PS115V;
using Sparc.KPK2.Hardware.PS27V;
using Sparc.KPK2.NFunctions;

namespace KPK2.MyClasse 
{
    /// <summary>
    /// Для постоянного измерения питающих U и вывода на полоски; 
    /// для измерений в режиме "Источники питания" 
    /// </summary>
    public class PowerStrip 
    {
        Lambda27V _lambda27V = new Lambda27V();
        #region Определения 
        /// <summary>
        /// TRUE- ДИСС-15; FALSE- ДИСС-32 
        /// </summary>
        bool is15;
        /// <summary>
        /// для выхода отовсюду по Exception 
        /// </summary>
        //bool sem;        
        /// <summary>
        /// ID для записи в БД
        /// </summary>
        int blockID;        
        /// <summary>
        /// [3]полученные данные: [0]-U 27ВB; [1]-I 27B; [3]-U 115B; [4] - I 115В
        /// </summary>
        double[] arResult = new double[4];                
        //Button btn;        
        /// <summary>
        /// Цвет фона индикатора - остановили измерение(Color.Gray) 
        /// </summary>
        Color indStop = Color.Gray;        
        /// <summary>
        /// []Индикаторов: [3]-ДИСС-15; [2]-ДИСС-32 
        /// </summary>
        //IndicatorNumbers[] arIndNubr;                
        private ShowStripDelegate ShowStrip;
        private event Action<string> ErrorEvent;

        #endregion

        public double Get115V
        {
            get { return arResult[2]; }
        }

        public delegate void ShowStripDelegate(double[] result);

        public PowerStrip (bool isDiss15, int inBlockId, ShowStripDelegate dlg, Action<string> OnError)
        {
            blockID = inBlockId;
            ShowStrip = dlg;            
            is15 = isDiss15;
            string command = DataManager.Instance.GetParamCommand("115V_Read_U");
            //_phase115B = command.IndexOf("A") != -1 ? "A" :
            //    command.IndexOf("B") != -1 ? "B" : "C";
            ErrorEvent += OnError;
        }

        /// <summary>
        /// Измеряем U и I на БП 27В и U на БП 115В 
        /// </summary>
        public void Execute(ManualResetEvent mreStopMeasure) 
        {
            //WriteToDb();
            //sem = false;
            if (!Power_27(mreStopMeasure))
            {
                RaiseError(string.Format(Resources.ErrorOnDeviceN, string.Format(Resources.PowerSupplyN, 40)));
                return;
            }
            if (!Power115V.Instance.IsAktor)
                if (!Power115(mreStopMeasure))
                {
                    RaiseError(string.Format(Resources.ErrorOnDeviceN, string.Format(Resources.PowerSupplyN, 115)));                    
                    return;
                }            
            ShowStrip(arResult);
        }

        private void RaiseError(string message)
        {
            if (ErrorEvent != null)
                ErrorEvent.BeginInvoke(message, null, null);
        }
        /// <summary>
        /// Измеряем U и I на БП 27В-> arResult 
        /// </summary>
        private bool Power_27(ManualResetEvent mreStopMeasure) 
        {
            try
            {
                if (Power27V.Instance.GetOutputState() == null) //_lambda27V.GetOutputState() == null
                {
                    arResult[0] = arResult[1] = 0;
                    return false;
                }

                if (mreStopMeasure.WaitOne(0))
                    return true;
                arResult[0] = Globals.Process27Voltage(Power27V.Instance.GetVoltage()); //_lambda27V.GetVoltage()
                if (double.IsNaN(arResult[0]))
                    throw new Exception();
                if (mreStopMeasure.WaitOne(0))
                    return true;
                arResult[1] = Power27V.Instance.GetCurrent(); // _lambda27V.GetCurrent()
                if (double.IsNaN(arResult[1]))
                    throw new Exception();
            }
            catch
            {
                //sem = true; 
                // ошибка по I 27В 
                //if (is15 && btn.FlatStyle == FlatStyle.Flat)
                  //  arIndNubr[0].BackgroundIndicator = indStop;
                return false;
            }

            return true;
        }

        public bool Save()
        {
            return DataManager.Instance.SaveParameter(blockID, "27V_I", arResult[1]);
        }
        // <summary>
        // Измеряем U на БП 115В-> arResult 
        // </summary>
        private bool Power115(ManualResetEvent mreStopMeasure)
        {            
            try
            {
                arResult[2] = Globals.Process115Voltage(Power115V.Instance.GetVoltage());
                if (mreStopMeasure.WaitOne(0))
                    return true;
                arResult[3] = Power115V.Instance.GetCurrent();
                return true;
            }
            catch
            {
                return false;
            }
        }

        
    }
}
