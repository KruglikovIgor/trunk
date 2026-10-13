using System.Collections.Generic;

namespace KPK2.MyClasse
{
    public class ProcessControlTask
    {
        public const int MaxCountProceed = 8;
        public const int ReadyToRemoveCount = 3;
        ProcessUnstableValue frequency = new ProcessUnstableValue(MaxCountProceed, ReadyToRemoveCount);
        ProcessUnstableValue duration = new ProcessUnstableValue(MaxCountProceed, ReadyToRemoveCount);

        public void Reset()
        {}

        public List<double> AddFrequencies(List<double> freqs)
        {
            return frequency.AddValues(freqs);
        }

        public List<double> AddDurations(List<double> durations)
        {
            return duration.AddValues(durations);
        }
    }
}
