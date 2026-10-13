using Sparc.KPK2.Hardware.Enums;
using System;
using System.Data;

namespace Sparc.KPK2.Hardware.MyClasseUSB 
{
    /// <summary>
    /// Базовый класс для проведения всех измерений через платы L-Card 
    /// </summary>
    public abstract class UsbBase 
    {
        #region Определения 
        /// <summary>
        /// ID № проверки для записи в БД 
        /// </summary>
        protected int blockID;
        /// <summary>
        /// Имя сигнала 
        /// </summary>
        protected string name;
        /// <summary>
        /// для выхода отовсюду по Exception 
        /// </summary>
        protected bool sem;
        /// <summary>
        /// TRUE-остановка измерения, с главной формы 
        /// </summary>
        protected bool isStop;
        /// <summary>
        /// TRUE- что-то измеряем через USB 
        /// </summary>
        protected bool isWork;
        /// <summary>
        /// TRUE-данный сигнал последний в списке, после LTR.STOP - 
        /// закрыть LTR.Close(); 
        /// </summary>
        protected bool isClear;
        /// <summary>
        /// TRUE-возникла ошибка при измерении данного сигнала 
        /// </summary>
        protected bool isError;
        /// <summary>
        /// ID параметра который измеряется 
        /// </summary>
        protected int paramID;
        /// <summary>
        /// № слота платы 
        /// </summary>
        protected int nmbrSlot;
        /// <summary>
        /// № физического канала АЦП для данного измерения 
        /// </summary>
        protected int nmbrChanel;
        /// <summary>
        /// Тип платы LTR 
        /// </summary>
        protected string typePlate;
        /// <summary>
        /// Начальный диапазон АЦП 
        /// </summary>
        protected int rangeADC;
        /// <summary>
        /// Коэффициент для домножения для данного физ. канала 
        /// </summary>
        protected double koefLTR;
        /// <summary>
        /// Режим AC/DC+АС для плат типа LTR22 
        /// </summary>
        protected string strAC_DC;
        /// <summary>
        /// Max значение "зеленого коридора" 
        /// </summary>
        protected double max;
        /// <summary>
        /// Min значение "зеленого коридора" 
        /// </summary>
        protected double min;
        /// <summary>
        /// Измеренное значение 
        /// </summary>
        protected double result;
        ///// <summary>
        ///// Нажатая кнопка 
        ///// </summary>
        //protected Button btn;
        /// <summary>
        ///  командная строка из БД для одного сигнала 
        /// </summary>
        protected string strCommand;
        /// <summary>
        /// Индикатор для вывода  
        /// </summary>
        //protected IndicatorNumbers ind;
        /// <summary>
        /// Строка, которую вставляем в наш DataSet при первом измерении 
        /// </summary>
        protected DataRow insertRow;
        ///// <summary>
        ///// Наш DataSet для работы 
        ///// </summary>
        //protected KPK2DataSet ds;
        ///// <summary>
        ///// DescriptionParamTableAdapter
        ///// </summary>
        //protected KPK2.KPK2DataSetTableAdapters.DescriptionParamTableAdapter descriptionTA =
        //    new KPK2.KPK2DataSetTableAdapters.DescriptionParamTableAdapter();
        ///// <summary>
        ///// MeasuringParamTableAdapter 
        ///// </summary>
        //protected KPK2.KPK2DataSetTableAdapters.MeasuringParamTableAdapter measuringTA =
        //    new KPK2.KPK2DataSetTableAdapters.MeasuringParamTableAdapter();

        /// <summary>
        /// Ошибки при работе с L-Card 
        /// </summary>
        //protected _LTRNative.LTRERROR err;
        #endregion 
        #region Свойства всяческие...
        /// <summary>
        /// Get: № слота платы 
        /// </summary>
        public int NmbrSlot
        {
            get { return this.nmbrSlot; }
        }
        /// <summary>
        /// Get: № физического канала 
        /// </summary>
        public int NmbrChanel
        {
            get { return this.nmbrChanel; }
        }
        /// <summary>
        /// Get: Начальный диапазон АЦП 
        /// </summary>
        public int NmbrRangeADC
        {
            get { return rangeADC; }
        }
        /// <summary>
        /// Get: Измеренное значение 
        /// </summary>
        public double Result
        {
            get { return this.result; }
        }
        /// <summary>
        /// Get: Имя сигнала 
        /// </summary>
        public string NameSignal
        {
            get { return this.name; }
        }
        /// <summary>
        /// Get: Режим AC/DC+АС для плат типа LTR22 
        /// </summary>
        public string StrAC_DC
        {
            get { return strAC_DC; }
        }
        /// <summary>
        /// Get: Тип платы L-Card 
        /// </summary>
        public string TypePlate
        {
            get { return this.typePlate; }
        }
        ///// <summary>
        ///// Get: Состояние кнопки: true-нажата 
        ///// </summary>
        //public bool BtnPress
        //{
        //    get { return this.btn.FlatStyle == FlatStyle.Flat; }
        //}
        /// <summary>
        /// Set, Get; TRUE-остановка измерения данного сигнала 
        /// </summary>
        public bool IsStop
        {
            get { return isStop; }
            set { isStop = value; }
        }
        /// <summary>
        /// Get: TRUE- что-то измеряем через USB в данный момент 
        /// </summary>
        public bool IsWork
        {
            get { return isWork; }
        }
        /// <summary>
        /// Set: TRUE-данный сигнал последний в списке, необходимо  
        /// закрыть LTR.Close(); 
        /// </summary>
        public bool IsClear
        {
            set { isClear = value; }
        }
        /// <summary>
        /// TRUE-возникла ошибка при измерении данного сигнала 
        /// </summary>
        public bool IsError
        {
            get { return isError; }
            set { isError = value; }
        }
        #endregion 
        public UsbBase() { }
        /// <summary>
        /// Параметризованый конструктор базового класса для проведения всех измерений для плат L-Card 
        /// </summary>
        /// <param name="id">ID № проверки</param>
        /// <param name="str">Имя сигнала</param>
        /// <param name="bt">Нажатая кнопка</param>
        /// <param name="indic">Индикатор для вывода</param>
        /// <param name="dataSet">DataSet для работы</param>
        //public UsbBase(int id, string str, Button bt, IndicatorNumbers indic,
        //    KPK2DataSet dataSet) 
        //{
        //    blockID = id;
        //    name = str;
        //    btn = bt;
        //    ind = indic;
        //    ds = dataSet;

        //    // строка из DescriptionParam 
        //    DataRow[] row = this.ds.Tables["DescriptionParam"].Select("ParamName = '" + name + "'");
        //    paramID = int.Parse(row[0].ItemArray[0].ToString());   // ID сигнала 
        //    min = double.Parse(row[0].ItemArray[3].ToString());    // min "зеленого коридора" 
        //    max = double.Parse(row[0].ItemArray[4].ToString());    // max "зеленого коридора" 

        //    TypePlateAndNmbrChanel(row[0].ItemArray[6].ToString());
        //    SlotAndRangeAndKoef(row[0].ItemArray[7].ToString());

        //    insertRow = ds.MeasuringParam.NewRow();
        //    insertRow["BlockID"] = blockID;
        //    insertRow["ParamID"] = paramID;
        //}

        /// <summary>
        /// Определяем тип платы и № физического канала 
        /// </summary>
        /// <param name="str">Из столбца "СОМ порт"</param>
        protected void TypePlateAndNmbrChanel(string str) 
        {
            // Тип платы 
            typePlate = str.IndexOf("LTR114") != -1 ? "LTR114" :
                str.IndexOf("LTR11") != -1 ? "LTR11" :
                str.IndexOf("LTR22") != -1 ? "LTR22" : 
                ""; // a если попадётся СОМ100??????????????
            // № физического канала АЦП
            try
            {
                if (typePlate != "") // только для плат LTR 
                {
                    nmbrChanel = str.IndexOf(":") + 1;
                    nmbrChanel = int.Parse(str.Substring(nmbrChanel, str.Length - nmbrChanel));
                }
            }
            catch { throw new ApplicationException(name + " ошибка в № физического канала АЦП");}
        }
        /// <summary>
        /// Определяем № слота платы, начальный диапазон измерений и коэффициент для домножения 
        /// </summary>
        /// <param name="str">Из столбца "Команда"</param>
        protected void SlotAndRangeAndKoef(string str) 
        {
            if (typePlate == "")
                return; // СОМ порту здесь не место !!!!!!!!!!!!! 
            // № слота платы    "[usb:15]10;15;AC";
            int n = str.IndexOf(":") + 1;
            int n1 = str.IndexOf("]");
            try
            {            
                nmbrSlot = int.Parse(str.Substring(n, n1-n));
                nmbrSlot = (int)LTRSupport.GetLTRSlotFromConfigByNum(nmbrChanel);
                //nmbrSlot = (int)LTRSupport.GetLTRSlot((LTRCardsEnum)(nmbrChanel + 1))-1;
            }
            catch { throw new ApplicationException(name + " ошибка в № слота");}
            // Начальный диапазон 
            n = n1 + 1; // с "]"
            n1 = str.IndexOf(";");
            double dd ;
            try { dd = double.Parse(str.Substring(n, n1 - n)); }
            catch { throw new ApplicationException(name + " ошибка в начальном диапазоне");}
            RangeADC(dd);
            // Коэффициент для домножения (+ AC/DC+AC(LTR22)) 
            n = n1 + 1; // с ";"
            try
            {
                if (typePlate == "LTR22") // "[usb:15]10;15;AC"
                {
                    n1 = str.LastIndexOf(";") - n; // кол-во символов под коэффициент 
                    koefLTR = double.Parse(str.Substring(n, n1));
                    strAC_DC = str.IndexOf("AC") != -1 ? "AC" :
                        str.IndexOf("DC") != -1 ? "DC" : "";
                    if (strAC_DC == "")
                        throw new ApplicationException(name + " ошибка в режиме АС<->DC");
                }
                else// для LTR11 и LTR114 
                    koefLTR = double.Parse(str.Substring(n, str.Length - n));
            }
            catch { throw new ApplicationException(name + " непонятный коэффициент для домножения"); }
        }
        /// <summary>
        /// Определяем № начального диапазона АЦП 
        /// </summary>
        /// <param name="dd">Начальный диапазон в Вольтах</param>
        /// <returns></returns>
        protected void RangeADC(double dd) 
        {
            if (typePlate == "LTR11")
                rangeADC = dd == 10.0 ? 0 :
                        dd == 2.5 ? 1 :
                        dd == 0.6 ? 2 : 3; // dd == 0.15 -> 3
            if (typePlate == "LTR22")
                rangeADC = dd == 10.0 ? 4 :
                        dd == 3.0 ? 5 :
                        dd == 1.0 ? 0 :
                        dd == 0.3 ? 1 :
                        dd == 0.1 ? 2 : 3; // dd == 0.03 -> 3
            if (typePlate == "LTR114")
                rangeADC = dd == 10.0 ? 0 :
                    dd == 2.0 ? 1 : 2; // dd == 0.4 -> 2 
        }

        /// <summary>
        /// Получение, вычисление и вывод результата на индикатор 
        /// </summary>
        /// <param name="obj">ltr платы для измерения</param>
        //public virtual void Execute(object obj) 
        //{
        //    sem = false;
        //    if (ExitFrom()) 
        //        return; // уходим  - закончили измерение 
        //    ind.BackgroundIndicator = Color.FromArgb(64, 0, 0);
            
        //    isWork = true;
        //    try
        //    {
        //        switch (typePlate)
        //        {
        //            case "LTR114":
        //                Execute114((LTRCards.LTR114)obj);
        //                break;
        //            case "LTR11":
        //                Execute11((LTRCards.LTR11)obj);
        //                break;
        //            case "LTR22":
        //                Execute22((LTRCards.LTR22)obj);
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        sem = true;
        //        ind.BackgroundIndicator = Color.Gray;
        //        throw new ApplicationException(name + ": " + ex.Message);
        //    }
        //    if (ExitFrom())
        //        return;
        //    ind.strText = result.ToString();
        //    ind.IndicatorColor = min <= result && result <= max ? 
        //        Color.Lime : Color.Red;
        //}
        /// <summary>
        /// Проводим измерение на плате типа LTR114 
        /// </summary>
        /// <param name="ltr"></param>
        //protected virtual void Execute114(LTRCards.LTR114 ltr) 
        //{
        //    ltr.SetActiveChannels(1);
        //    ltr.SetChannelParams(0, LTRCards.LTR114.MeasMode.U, (uint)(nmbrChanel - 1),
        //        (LTRCards.LTR114.ChannelRange)rangeADC);
        //    ltr.InterframeInterval(1);
        //    ltr.SetOsr(15);
        //    ltr.SetFreqDiv(2);

        //    ltr.SetADC();
        //    List<double[]> buff = ltr.GetData(500); // получил 500 точек в ответ 

        //    if (buff == null)
        //        return;

        //    isWork = false; // пусть будет пока 

        //    double dd = 0.0;
        //    for (int i = 0; i < buff[0].Length; i++)
        //        dd += buff[0][i];
        //    dd /= buff[0].Length; // усреднили 
        //    result = Answer(dd);
        //    if (isClear)
        //        ltr.CloseConnection();
        //}
        /// <summary>
        /// Проводим измерение на плате типа LTR11 
        /// </summary>
        /// <param name="ltr"></param>
        //protected virtual void Execute11(LTRCards.LTR11 ltr)  
        //{
        //    ltr.SetupADC(LTRCards.LTR11.StartADCMode.INT,
        //        LTRCards.LTR11.INPMode.INT, 1, 299);
        //    ltr.SetupChannel(0, (uint)(nmbrChanel - 1),
        //        LTRCards.LTR11.ChannelWorkMode.b16Low,
        //        (LTRCards.LTR11.ChannelRange)rangeADC);
        //    ltr.SetADC();
        //    List<double[]> buff = ltr.GetData(1000);// получили 1000 отсчётов
        //    if (buff == null)
        //        return; 

        //    isWork = false; //пусть будет пока 
        //    double dd = 0.0;

        //    for (int i = 0; i < buff[0].Length; i++)
        //        dd += buff[0][i];
        //    dd /= buff[0].Length; // усреднили             
        //    result = Answer(dd);
        //    if (isClear)
        //        ltr.CloseConnection();
        //}
        /// <summary>
        /// Проводим измерение на плате типа LTR22 
        /// </summary>
        /// <param name="ltr"></param>
        //protected virtual void Execute22(LTRCards.LTR22 ltr) 
        //{
        //    for (int i = 0; i < 4; i++) // вкл. только канал кот. измеряем
        //        ltr.SetOnOffChannel((uint)i, i == (nmbrChanel - 1));
        //    ltr.SetADCRange((uint)(nmbrChanel - 1), (LTRCards.LTR22.ChannelRange)rangeADC);
        //    ltr.SetMeasureADCZero(false);
        //    ltr.SetACDCState(strAC_DC == "DC" ? true : false);
        //    ltr.SetFrequency(1, false);

        //    ltr.SetConfigNow();
        //    int k = strAC_DC == "DC" ? 100 : 50000;
        //    List<double[]> buff = ltr.GetData((uint)k);

        //    if (buff == null)
        //        return;

        //    isWork = false; // пусть будет пока 
        //    double dd = 0.0;

        //    for( int i=0; i < buff[0].Length; i++)
        //        dd += strAC_DC == "DC" ? buff[0][i] : buff[0][i] * buff[0][i];
        //    dd = strAC_DC == "DC" ? dd / k : Math.Sqrt(dd / k);
        //    result = Answer(dd);
        //    if (isClear)
        //        ltr.CloseConnection();
        //}
        /// <summary>
        /// Вычисление результирующего значения с учётом всяких коэффициентов 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        protected virtual double Answer(double d) 
        {
            return koefLTR * d;
        }
        ///// <summary>
        ///// Выходит из режима измерения по отжатой кнопке или из-за ошибки 
        ///// </summary>
        ///// <returns></returns>
        //protected virtual bool ExitFrom() 
        //{ 
        //    return sem || btn.FlatStyle == FlatStyle.Standard || isStop;
        //}
        ///// <summary>
        ///// Записываем полученные результаты в DataSet
        ///// </summary>
        //public virtual void WriteDb() 
        //{
        //    string strSQL = "BlockID = '" + blockID + "' AND ParamID = '" +
        //        paramID + "'";
        //    DataRow[] row = ds.Tables["MeasuringParam"].Select(strSQL);
        //    try
        //    {
        //        if (row.Length == 0) // такой записи нет в БД - делаем новую запись 
        //        {
        //            insertRow["Result"] = (float)result;
        //            ds.Tables["MeasuringParam"].Rows.Add(insertRow);
        //            measuringTA.InsertNewResult(blockID, paramID, (float)result);
        //        }
        //        else // запись есть - её обновляем 
        //            measuringTA.UpdateResultValue((float)result, blockID, paramID);
        //    }
        //    catch (Exception ex)
        //    { throw new ApplicationException(name + ": ошибка сохранения данных " + ex.Message); }

        //    //ind.BackgroundIndicator = Color.Gray; // измерение остановлено, шоб былО виднО
        //}

        ///// <summary>
        ///// Отображаем значение на индикаторе + логика обработки  
        ///// </summary>
        ///// <param name="res">Значение для отображения</param>
        //public virtual void 
        //    ShowResult(double res) 
        //{
        //    if (ExitFrom())
        //        return; // уходим  - закончили измерение 
        //    ind.BackgroundIndicator = Color.FromArgb(64, 0, 0);

        //    result = Answer(res);
        //    ind.strText = result.ToString();
        //    ind.IndicatorColor = min <= result && result <= max ?
        //        Color.Lime : Color.Red;
        //}
    }
}
