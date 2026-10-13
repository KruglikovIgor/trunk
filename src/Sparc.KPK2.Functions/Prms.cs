using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sparc.KPK2.NFunctions
{
    /// <summary> Статистические параметры. </summary>
    public class Prms
    {
        /// <summary> Кол-во накопленных значений. </summary>
        public long Count;
        /// <summary> Мин. значение. </summary>
        public double Min;
        /// <summary> Макс. значение. </summary>
        public double Max;
        /// <summary> Мин. абс. значение. </summary>
        public double MinAbs;
        /// <summary> Макс. абс. значение. </summary>
        public double MaxAbs;
        /// <summary> Ср. значение ( SUM( x[i])/N ). </summary>
        public double Mean;
        /// <summary> Ср. кв. значение. </summary>
        /// <remarks> SQRT( SUM( x[i]^2) / N )) </remarks>
        public double Rms;
        /// <summary> Середина разброса ( (max + min)/2 ). </summary>
        public double Middle;
        /// <summary> Рзброс (MAX - MIN). </summary>
        public double Pp;
        /// <summary> Ср. кв. отклонение. </summary>
        /// <remarks> SQRT( SUM( (;x[i] - M)^2) / N )) </remarks>
        public double Dev;
        //-------------------------------------------------------

        /// <summary> Форм-ет строку в виде
        /// XXXср XXXскз +- XXXско в NN значениях от XXX до XXX
        /// со зн-ями пар-ров соотв-но Mean, Rms, Dev, Min, Max.
        /// </summary>
        /// <param name="format"> 
        /// Желаемый формат (напр., F2) каждого выводимого стат. пар-ра. 
        /// </param>
        /// <returns></returns>
        public string String(string format = "F")
        {
            string f = string.Format(
            "{{0:{0}}}ср {{1:{0}}}скз +- {{2:{0}}}ско в {1} значениях от {{3:{0}}} до {{4:{0}}}",
            format, Count);
            string s = string.Format(f, Mean, Rms, Dev, Min, Max);
            return s;
        }
    }
}
