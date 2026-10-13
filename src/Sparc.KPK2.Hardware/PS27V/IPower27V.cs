namespace Sparc.KPK2.Hardware.PS27V
{
    public interface IPower27V
    {
        bool Init();
        bool? GetOutputState();
        bool OnOffState(bool state);
        double GetVoltage();
        double GetSettedVoltage();
        double GetCurrent();
        double GetSettedCurrent();
        void SetVoltage(double voltage);
        void SetCurrent(double current);
        /// <summary>
        /// Активна ли (включилась ли) какая-нибудь защита (стабилизация по току, превышение напряжения и т.п.)
        /// </summary>
        /// <returns></returns>
        bool IsProtectionActive();

        bool IsOverCurrent();

        bool IsOverVoltage();
        string GetIdentString();
    }
}
