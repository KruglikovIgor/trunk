using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using NPlot;

namespace KPK2
{
    public partial class TestGraph : Form
    {
        public TestGraph()
        {
            InitializeComponent();
            for (int i=0;i<4;i++)
                fb[i] = new FloatBuffer();
        }

        private FloatBuffer[] fb = new FloatBuffer[4];
        

        public class FloatBuffer
        {
            private List<int> _buffer;
            private int _maxSize = 1000;

            public FloatBuffer()
            {
                _buffer = new List<int>();
            }
            
            public void AddData(int[] data)
            {
                int count = data.Length;
                int diff = (_buffer.Count + count) - _maxSize;
                for (int i = 0; i < count; i++)
                {
                    _buffer.Add(data[i]);
                    if (i > diff)
                        _buffer.RemoveAt(0);
                }
            }

            public List<int> GetData()
            {
                return _buffer;
            }
        }

        

        private int[] GetData(int count)
        {
            int[] data = new int[count];
            Random r = new Random();
            for (int i = 0; i < count; i++)
                data[i] = r.Next(0, 100);
            return data;
        }

        Timer tFillData = new Timer();
        Timer tShowData = new Timer();

        private void TestGraph_Load(object sender, EventArgs e)
        {
            //for (int i = 0; i < 4;i++ )
              //  fb[i] = new FloatBuffer();
            psUNCH.Add(new HorizontalLine(5, Color.LightBlue));
            psUNCH.Add(new VerticalLine(5, Color.LightBlue));
            psUNCH.XAxis1 = new Axis(0,100000);
            psUNCH.YAxis1 = new Axis(0,110);
            tFillData.Interval = 1000;
            tFillData.Tick += tFillData_Tick;
            tFillData.Start();
            tShowData.Interval = 2000;
            tShowData.Tick += tShowData_Tick;
        }

        private void tShowData_Tick(object sender, EventArgs e)
        {
            psUNCH.Clear();
            psUNCH.Add(new HorizontalLine(5, Color.LightBlue));
            psUNCH.Add(new VerticalLine(5, Color.LightBlue));
            for (int i = 0; i < 4; i++)
            {
                LinePlot lp = new LinePlot(fb[i].GetData());
                lp.Pen = new Pen(Color.LightGoldenrodYellow,1.0f);
                psUNCH.Add(lp);
            }            
        }

        private void tFillData_Tick(object sender, EventArgs e)
        {
            for (int i=0;i<4;i++)
                fb[i].AddData(GetData(50));
        }
    }
}