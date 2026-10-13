namespace KPK2.Interfaces
{
    //Используется для доступа Black8Check.cs к свойствам и методам frmWork15 (наследник IWork15Base)
    public interface IFrmWork15ForBlack8Check : IFrmWork15Base
    {
        string alfa_18minus { get; }
        string spd_task6_15 { get; }
        char _deltaChar { get; }
        void SetIndicatorLimitsPercent(int number, double startValue, double baseValue, string paramName, bool reverseSign = false);
        void SetupIndicator(int number, string upSide, string downSide, string startValue);
        void SetIndicatorValue(int number, double val);
        void DeletingAll();
        void InitializTask_6();
    }
}