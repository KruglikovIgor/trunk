using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using Sparc.KPK2.NFunctions;

namespace KPK2.MyClasse
{
    public class ProcessUnstableValue
    {
        private int _maxCountProceed;
        private int _readyToRemoveCount;
        Dictionary<int, List<double>> valueByChannel = new Dictionary<int, List<double>>();
        private static Logger _logger = LogManager.GetLogger("KPK2Info");

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="maxCountProceed">Количество точек для анализа</param>
        /// <param name="readyToRemoveCount">Количество точек которые потенциально могут быть "испорченными", если их больше половины, то алгоритм будет работать неправильно</param>
        public ProcessUnstableValue(int maxCountProceed, int readyToRemoveCount)
        {
            _maxCountProceed = maxCountProceed;
            _readyToRemoveCount = readyToRemoveCount;
        }

        public void Reset()
        {
            valueByChannel.Clear();
        }

        public List<double> AddValues(List<double> values)
        {
            for (int i = 0; i < values.Count; i++)
            {
                if (!valueByChannel.ContainsKey(i))
                    valueByChannel.Add(i, new List<double>());
                var curFBC = valueByChannel[i];
                curFBC.Add(values[i]);
                if (curFBC.Count > _maxCountProceed)
                    curFBC.RemoveAt(0);
            }

            var result = new List<double>();
            for (int i = 0; i < values.Count; i++)
                result.Add(GetResult(valueByChannel[i]));
            return result;
        }

        private double GetResult(List<double> chList)
        {
            LogThisList(chList);
            //chList = new List<double>();
            //chList.Add(10000);
            //chList.Add(10000);
            //chList.Add(10000);
            //chList.Add(10911.07);
            //chList.Add(10000);
            //chList.Add(10000);
            //chList.Add(10000);
            //chList.Add(10025.06);
            var channelList = new List<double>(chList);
            //Dictionary<int, int> dublicates = new Dictionary<int, int>();
            //for (int i = 0; i < channelList.Count; i++)
            //{
            //    int val = (int)channelList[i];
            //    if (!dublicates.ContainsKey(val))
            //        dublicates.Add(val, 1);
            //    else
            //        dublicates[val]++;
            //}
            //if (dublicates.Values.Max() < chList.Count/2)
            //    return double.NaN;
            var aver = channelList.Average();
            channelList.Sort(new ErrorComparer(aver));
            double maxError = 0;
            if (channelList.Count > _readyToRemoveCount)
                maxError = Math.Abs(channelList[_readyToRemoveCount] - aver);
            else
                maxError = Math.Abs(channelList[0]-aver);
            double sum = 0;
            int count = 0;
            for (int i = 0; i < channelList.Count; i++)
                if (Math.Abs(channelList[i] - aver) <= maxError)
                {
                    sum += channelList[i];
                    count++;
                }
            sum /= count;
            _logger.Trace("Result value: " + sum);
            return sum;
        }

        private void LogThisList(List<double> chList)
        {
            StringBuilder sb= new StringBuilder();
            sb.Append("Log Values: ");
            foreach (var item in chList)
                sb.Append(item+" ");
            _logger.Trace(sb.ToString);
        }
    }
}
