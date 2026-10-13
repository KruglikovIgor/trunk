namespace Sparc.KPK2.Hardware.PS115V
{
    public interface IPower115V
    {
        bool Connect();
        bool SetVoltageSimple(double voltage);
        bool SetVoltageInner(double voltage);
        bool SetPhase(double phase);
        bool GetOutputState();
        double GetVoltage(bool correct);
        double GetVoltage();
        double GetSettedVoltage();
        bool GetStatus();
        double GetCurrent();
        bool SetOnOff(bool state);
        bool SetPermitOnOff(bool state);
        bool SetCurrentLimit(double current);
        bool SetMaxVoltageRange(bool state);
        bool SetFrequency(double frequency);
        double GetFrequency();
        bool IsConnected();
        double GetCurrentLimit();
        double LineResistance { get; set; }
        void SetVoltageWOCorrection(double voltage);
        void Disconnect();
        string GetIdentString();
    }
}