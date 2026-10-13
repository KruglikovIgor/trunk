
namespace KPK2.Interfaces
{
    public interface IFrmWork32
    {
        bool stateMapCorner { get; }
        bool TiMemory1 { get; }
        bool TiMemory2 { get; }
        bool TiVCH { get; }
        bool TiVSS { get; }
        bool TiFlight { get; }
        bool TiDiss { get; }
        bool TiDpsm { get; }
        bool TiBPKOn { get; }
        bool TiGood { get; }
        bool TiNavigation { get; }
        bool TiPositionOff { get; }
        bool TiBVK { get; }
    }
}