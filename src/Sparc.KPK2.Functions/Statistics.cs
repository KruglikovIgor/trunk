/* File "Statistics.cs" (C#)
 * Created by Astakhov A.A., 06.2024
 * Last fixed in SVN  $Date:: $ by $Author:  $ 
 *****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;

namespace Sparc.KPK2.NFunctions
{
    /// <summary> Накапливает значения и вычисляет их статистку. 
    /// </summary>
    public class Statistics
    {
        // Накопленные значения.
        private List<double> _values = new List<double>();
        private double _min = double.MaxValue;
        private double _max = -double.MaxValue;
        private double _minAbs = double.MaxValue;
        private double _maxAbs = 0;
        private double _sum;
        private double _sum2; // Сумма квадратов.
        private double _dev = 0; // Ср. кв. отклонение.

        private readonly object _lockObject = new object();

        //----------------------------------------------------
        public bool IsLimit { get; set; } = false;
        public int LimitationValue { get; set; } = 5;
        public long Count { get { return _values.Count; } }

        //-----------------------------------------------------

        public Statistics()
        {
        }

        /// <summary>
        /// Вычисляет стат. пар-ры заданного перечня значений.
        /// </summary>
        public Statistics( System.Collections.IEnumerable values )
        {
            IEnumerable<double> doubleVals = values.OfType<double>();
            IEnumerable<int> intVals = values.OfType<int>();
            lock( _lockObject )
            {
                foreach( var value in doubleVals )
                    Add( value );
                foreach( var value in intVals )
                    Add( value );
            }
        }
        //---------------------------------------------------------------

        public void Add( double value )
        {
            lock( _lockObject )
            {
				if (IsLimit && _values.Count == LimitationValue)
				{
                    List<double> newList = new List<double>();
                    _sum -= _values[0];
                    _sum2 -= _values[0] * _values[0];
                    for (int i = 1; i < _values.Count; i++)
                    {
                        newList.Add(_values[i]);
                    }
                    _values = newList;
				}
                else if(IsLimit && Math.Abs(_values.Count - LimitationValue) > 1)
                {
                    Reset();
                }
                _min = Math.Min( _min, value );
                _max = Math.Max( _max, value );
                _minAbs = Math.Min( Math.Abs( _minAbs ), value );
                _maxAbs = Math.Max( Math.Abs( _maxAbs ), value );
                _sum += value;
                _sum2 += value*value;
                _dev = double.NaN;
                _values.Add( value );
            }
        }
        //-------------------------------------------------------

        public Prms GetPrms()
        {
            Prms prms;
            lock( _lockObject )
            {
                prms = new Prms()
                {
                    Count = _values.Count,
                    Min = _min,
                    Max = _max,
                    MinAbs = _minAbs,
                    MaxAbs = _maxAbs,
                    Mean = (_values.Count > 0) ? _sum / _values.Count : 0,
                    Rms = (_values.Count > 0) ? Math.Sqrt( _sum2 / _values.Count ) : 0,
                    Middle = (_values.Count > 0) ? 0.5*(_max + _min) : 0,
                    Pp = (_values.Count > 0) ? (_max - _min) : 0,
                };

                if( double.IsNaN( _dev ) ) // Еще не вычислялось?
                {
                    _dev = 0;
                    foreach( var value in _values )
                        _dev += (value - prms.Mean) * (value - prms.Mean);
                    _dev = Math.Sqrt( _dev / _values.Count );
                    //prms.Dev = Math.Sqrt( prms.Dev / (Count - 1) );
                }
                prms.Dev = _dev;
            }
            return prms;
        }
        //-------------------------------------------------------

        public void Reset()
        {
            lock( _lockObject )
            {
                _min = double.MaxValue;
                _max = -double.MaxValue;
                _minAbs = double.MaxValue;
                _maxAbs = 0;
                _sum = 0;
                _sum2 = 0;
                _dev = 0;
                _values.Clear();
            }
        }
    }
}
