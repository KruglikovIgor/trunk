using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sparc.Hardware.Akip3418;
using Sparc.Hardware.Akip3418.InnerFeatures;

namespace Sparc.KPK2.Imitator.Hardware
{
    public class Akip3418GeneratorMock : IAkip3418
    {
        public void Connect(string strId, string vendorId, string modelId)
        {
            IsConnected = true;
        }

        public string GetInfo()
        {
            return "Akip3418";
        }

        public void ClearAllStatus()
        {
            
        }

        public void SetOutputState(Channel channel, bool state)
        {
            
        }

        public bool GetOutputState(Channel channel)
        {
            return true;
        }

        public void SetOutputLoad(Channel channel, OutputLoad load)
        {
            
        }

        public void SetPolarity(Channel channel, Polarity polar)
        {
            
        }

        public void SetSignalType(Channel channel, SignalType sType)
        {
         
        }

        public SignalType GetSignalType(Channel channel)
        {
            return SignalType.SINE;
        }

        public void SetSignalFrequency(Channel channel, double frequency)
        {
            
        }

        public double GetSignalFrequency(Channel channel)
        {
            return 20000;
        }

        public void SetSignalPhase(Channel channel, double phase)
        {

        }

        public double GetSignalPhase(Channel channel)
        {
            return 0;
        }

        public void SetSignalAmplitude(Channel channel, double amplitude)
        {
            
        }

        public double GetSignalAmplitude(Channel channel)
        {
            return 0.05;
        }

        public void SetSignalOffset(Channel channel, double offset)
        {
            
        }

        public double GetSignalOffset(Channel channel)
        {
            return 0;
        }

        public void SetSignalDutyFactor(Channel channel, double duty)
        {
            
        }

        public double GetSignalDutyFactor(Channel channel)
        {
            return 0;
        }

        public void SetSignalSymmetry(Channel channel, double sym)
        {
            
        }

        public double GetSignalSymmetry(Channel channel)
        {
            return 0;
        }

        public void SetSignalPeriod(Channel channel, double period)
        {
            
        }

        public double GetSignalPeriod(Channel channel)
        {
            return 3333;
        }

        public void SetPulseWidth(Channel channel, double width)
        {
            
        }

        public double GetPulseWidth(Channel channel)
        {
            return 0.005;
        }

        public string SendRequest(string command)
        {
            return "";
        }

        public void SetClockSource(SourceType type)
        {
            
        }

        public SourceType GetClockSource()
        {
            return SourceType.INT;
        }

        public void Disconnect()
        {
            IsConnected = false;
        }

        public bool CheckDevice()
        {
            return true;
        }

        public bool IsConnected { get; set; }
    }
}
