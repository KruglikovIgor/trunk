using System.Runtime.InteropServices;

namespace KPK2.MyClasse 
{
    class myTimer 
    {
        [DllImport("KERNEL32")]
        static extern bool QueryPerformanceCounter(out long lpPerformanceCount);
        [DllImport("Kernel32.dll")]
        static extern bool QueryPerformanceFrequency(out long lpFrequancy);

        long start, stop, frequency;
        long[] counters;
        int n;

        public myTimer(int n) 
        {
            this.n = n;
            this.counters = new long[this.n];
            if (QueryPerformanceFrequency(out this.frequency) == false) { }
        }

        public void Start(int num) 
        {
            QueryPerformanceCounter(out this.start);
            this.counters[num] = this.start;
        }

        public double Get(int num) 
        {
            QueryPerformanceCounter(out this.stop);
            return (((double)(this.stop - this.counters[num])) / (double)this.frequency);
        }
    }
}
