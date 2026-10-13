using System;
using System.Windows.Forms;
using Diss15_32.MyClasse;
using System.Collections;
using System.Collections.Generic;

namespace Diss15_32.GeneralForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            ControlTask ct = new ControlTask();
            int number = 0;
            int amplitude = 0;
            int shift = 0;
            if (!ct.GetControlTask(ref number, ref amplitude, ref shift))
                MessageBox.Show("Ошибка");
            else
            {
                MessageBox.Show("ОК");
                label1.Text = number + " " + amplitude + " " + shift;
            }
            //DiscreteOutputs di = new DiscreteOutputs();
            //BitArray states = new BitArray(1);
            //if (!di.GetOutputs(ref states))                        
            //    MessageBox.Show("Ошибка");
            //else
            //{
            //    MessageBox.Show("ОК");
            //    label1.Text = "";
            //    for (int i = 0; i < states.Count; i++)
            //        label1.Text += states[i] ? "1 " : "0 ";
            //}
            //GenSinCos board = new GenSinCos();
            //if (!board.GeneratorOn())
            //    MessageBox.Show("Ошибка");
            //else
            //    MessageBox.Show("ОК");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ControlTask ct = new ControlTask();
            
            //DiscreteOutputs di = new DiscreteOutputs();
            //if (!di.SetChannelsOff())
            //    MessageBox.Show("Ошибка");
            //else
            //    MessageBox.Show("ОК");
            //GenSinCos board = new GenSinCos();
            //if (!board.GeneratorOff())
            //    MessageBox.Show("Ошибка");
            //else
            //    MessageBox.Show("ОК");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ControlTask ct = new ControlTask();            
            string[] data = textBox3.Text.Split(' ');
            if (!ct.SetControlTask(Convert.ToInt32(data[0]), Convert.ToInt32(data[1]), Convert.ToInt32(data[2])))
                MessageBox.Show("Ошибка");
            else
                MessageBox.Show("ОК");


            //DiscreteOutputs di = new DiscreteOutputs();
            //if (!di.SetOutput(Convert.ToInt32(textBox3.Text), true))
            //    MessageBox.Show("Ошибка");
            //else
            //    MessageBox.Show("ОК");

            //GenSinCos board = new GenSinCos();
            //if (!board.GeneratorOn((ushort)Convert.ToUInt16(textBox3.Text)))
            //    MessageBox.Show("Ошибка");
            //else
            //    MessageBox.Show("ОК");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

            //DiscreteInputs di = new DiscreteInputs();
            bool isGen = false;
            DateTime dt = new DateTime();


            //DiscreteOutputs di = new DiscreteOutputs();
            ControlTask ct = new ControlTask();
            if (!ct.GetVersion(ref isGen, ref dt))
                MessageBox.Show("Ошибка");
            else
                MessageBox.Show("ОК");
            label4.Text = (isGen ? "Задачи" : "Не задачи") + dt.ToShortDateString();

            //if (!di.GetVersion(ref isGen, ref dt))
            //    MessageBox.Show("Ошибка");
            //else
            //    MessageBox.Show("ОК");
            //label1.Text = (isGen ? "Входы" : "Не входы") + dt.ToShortDateString();

            //GenSinCos board = new GenSinCos();
            //bool isGen = false;
            //DateTime dt = new DateTime();
            //if (!board.GetVersion(ref isGen, ref dt))
            //    MessageBox.Show("Ошибка");
            //else
            //    MessageBox.Show("ОК");
            //label1.Text = (isGen ? "Генератор" : "Не генератор") + dt.ToShortDateString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DiscreteOutputs di = new DiscreteOutputs();
            BitArray states = new BitArray(textBox5.Text.Length);
            for (int i=0;i<textBox5.Text.Length;i++)
                states[i] = textBox5.Text[i] == '1';
            if (!di.SetOutputs(states))
                MessageBox.Show("Ошибка");
            else
                MessageBox.Show("ОК");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ControlTask ct = new ControlTask();
            List<KeyValuePair<int,int>> data = new List<KeyValuePair<int,int>>();
            if (!ct.GetFrequency(ref data))
                MessageBox.Show("Ошибка");
            else
            {
                MessageBox.Show("ОК");
                for (int i = 0; i < data.Count; i++)
                    label6.Text += data[i].Key + " " + data[i].Value + " ";
            }

            

            //DiscreteOutputs di = new DiscreteOutputs();
            //BitArray states = new BitArray(textBox6.Text.Length-2);
            //int group = Convert.ToInt32(textBox6.Text[0].ToString());
            //for (int i = 0; i < textBox6.Text.Length-2; i++)
            //    states[i] = textBox6.Text[i+2] == '1';
            //MessageBox.Show(!di.SetGroupOutputs(group, states) ? "Ошибка" : "ОК");
        }
    }
}