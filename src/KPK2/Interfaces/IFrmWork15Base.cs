using System.Windows.Forms;

namespace KPK2.Interfaces
{
    //Базовый интерфейс  (используется для доступа SVChFreqAndPower.cs, StabilizeSpeed.cs, PK.cs к свойствам и методам frmWork15 )
    public interface IFrmWork15Base
    {
        void ManualRecordDouble(string paramName, double value);
        double GetMeasuredValue(string paramName);
        void SetIndicatorLimits(int number, string paramName, bool reverseSign = false);
        void ManualRecord(string str, string text);
        void ManualRecordBool(string str, RadioButton rbtn_1, RadioButton rbtn_2);
        int TestId { get; }
    }
}