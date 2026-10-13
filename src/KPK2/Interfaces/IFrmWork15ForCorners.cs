namespace KPK2.Interfaces
{
    // Используется для доступа Corners.cs к свойствам и методам frmWork15 (наследник IWork15Base)
    public interface IFrmWork15ForCorners : IFrmWork15Base
    {
        string strW_Back_15 { get; }
        string strW_Right_15 { get; }
        void SetIndicatorLimits(int number, double baseValue, string paramName);
    }
}