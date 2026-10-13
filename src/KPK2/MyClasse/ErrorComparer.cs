using System;
using System.Collections.Generic;

namespace KPK2.MyClasse
{
    public class ErrorComparer : IComparer<double>
    {
        private double _average;

        public ErrorComparer(double average)
        {
            _average = average;
        }

        public int Compare(double x, double y)
        {
            if (Math.Abs(x - _average) > Math.Abs(y - _average))
                return -1;
            if (Math.Abs(x - _average) < Math.Abs(y - _average))
                return 1;
            return 0;
        }
    }
}
