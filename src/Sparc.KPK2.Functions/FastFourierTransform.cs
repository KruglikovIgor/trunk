using System;

namespace Sparc.KPK2.NFunctions
{
    class FastFourierTransform
    {
        class Complex
        {
            private double _re, _im;         
            public double Re
            {
                get { return _re; }
                set { _re = value; }
            }

            public double Im
            {
                get { return _im; }
                set { _im = value; }
            }

            public Complex(double re, double im)
            {
                _re = re;
                _im = im;
            }

            public Complex()
            {                
            }            

            public static Complex operator +(Complex a, Complex b)
            {
                return new Complex(a._re + b._re, a._im + b._im);
            }

            public static Complex operator -(Complex a, Complex b)
            {
                return new Complex(a._re - b._re, a._im - b._im);
            }            

            public static Complex operator *(Complex a, Complex b)
            {
                return new Complex(a._re * b._re - a._im * b._im, a._re * b._im + a._im * b._re);
            }
            
            public static Complex operator /(Complex a, double div)
            {
                return new Complex(a._re/div, a._im/div);
            }            

        }        

       
        private static FastFourierTransform _instance;
        private byte[] _reverse;
        private Complex[] _W2n;
        

        public static FastFourierTransform Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new FastFourierTransform();
                return _instance;
            }
        }

        private FastFourierTransform()
        {
            GenerateReverse();
            GenerateW2N();
        }

        private void GenerateW2N()
        {
            _W2n = new Complex[32];
            for (int i=0; i < _W2n.Length; i++)
            {
                _W2n[i] = new Complex(Math.Cos(2 * Math.PI / Math.Pow(2, (i + 1))), -Math.Sin(2 * Math.PI / Math.Pow(2, (i+1))));
            }            
        }

        private void GenerateReverse()
        {
            _reverse = new byte[256];
            for (int i=0;i<_reverse.Length;i++)
            {                
                for (int j = 0; j < BitInByte; j++)
                {
                    _reverse[i] = (byte)(_reverse[i] | ((i >> j) & 1) << (BitInByte - j - 1));
                    
                }
            }
        }

        const int BitInByte = 8;
        private byte GetByte(uint number, int byteNum)
        {
            const byte bFirst = 0xFF;
            byte result = (byte) ((number >> (byteNum*BitInByte)) & bFirst);
            return result;
        }

        const int ByteInInt = 4;

        private void SetByte(ref uint number, int byteNum, byte bValue)
        {
            uint result = 0;
            for (int i = ByteInInt-1; i >= 0; i--)
            {
                if (byteNum == i)
                    result |= bValue;
                else
                    result |= GetByte(number, i);
                if ((i-1) >= 0)
                    result <<= BitInByte;
            }
            number = result;
        }

        //complement: false - normal (direct) transformation, true - reverse transformation
        private void FFT(ref Complex[] xArr, bool complement)
        {
            int T = (int)Math.Log(xArr.Length, 2);
            uint JJ = 0;
            Complex cTemp = new Complex();

            //first interchanging

            for (int i=1; i < xArr.Length - 1; i++)
            {
                for (int j=0; j < ByteInInt; j++)
                    SetByte(ref JJ, j, _reverse[GetByte((uint)i, ByteInInt - j - 1)]);
                JJ >>= (32 - T);
                if (i < JJ)
                {
                    cTemp = xArr[i];
                    xArr[i] = xArr[JJ];
                    xArr[JJ] = cTemp;
                 }
            }

            //rotation multiplier array allocation
            Complex[] WStore = new Complex[xArr.Length / 2];
            for (int i = 0; i < WStore.Length;i++ )
                WStore[i] = new Complex();
            WStore[0].Re = 1;
            WStore[0].Im = 0;
            int skew = xArr.Length >> 1;
            Complex WN = new Complex();
            Complex W = new Complex();            

            //main loop

            for (int N = 2, Nd2 = 1, pWN = 0; N <= xArr.Length; Nd2 = N, N+=N, pWN++, skew >>= 1)
            {
                WN = _W2n[pWN];
                if (complement)
                    WN.Im *= -1;
                for (int j=0, k=0; k<Nd2;k++, j+=skew)
                {
                    if ((k & 1) == 1)
                    {
                        W *= WN;
                        WStore[j] = W;
                    }
                    else
                        W = WStore[j];

                    for (int m = k; m < xArr.Length; m += N)
                    {
                        int mpNd2 = m + Nd2;
                        cTemp = W;
                        cTemp *= xArr[mpNd2];
                        xArr[mpNd2] = xArr[m];
                        xArr[mpNd2] -= cTemp;
                        xArr[m] += cTemp;
                    }
                }
                
            }            

            if (complement)
                for (int i=0;i<xArr.Length;i++)
                {
                    xArr[i] /= xArr.Length;
                }
        }

        public double RMS (double[] points, ref double shift)
        {
            Complex[] cArray = new Complex[points.Length];
            for(int i=0;i<points.Length;i++)
                cArray[i] = new Complex(points[i],0);
            FFT(ref cArray, true);
            double result = 0;            
            for (int i=0; i<cArray.Length;i++)
                result += Math.Pow(cArray[i].Re, 2) + Math.Pow(cArray[i].Im, 2);               

            result = Math.Sqrt(result);                        
            shift = cArray[0].Re;
            return result;
        }        
    }
}
