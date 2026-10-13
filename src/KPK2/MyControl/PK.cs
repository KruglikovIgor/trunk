using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using KPK2.GeneralForms.DissForms;
using KPK2.Interfaces;
using KPK2.Properties;
using Sparc.KPK2.NFunctions;
using Globals = KPK2.Common.Globals;

namespace KPK2.MyControl
{
    public partial class PK : UserControl
    {
        private int[,] allowRange = {
                                           {0, 50}, // вперёд
                                           {-20, 20}, //вбок
                                           {0,10}, //вверх
                                           {0,25},  //назад
                                           {0,25}, //вправо
                                           {0,10}, //вверх кз-2
                                           {50,400}, //ПС
                                           {-46,46}, //УС
                                            {0,0}, //пустышка
                                           {-20,20}, //Дельта ПС
                                           {-20,20}, //Дельта УС
                                           {-20,20}, //время включения табло П
                                    };
        private double[,] greenRange = {
                                           {14.5, 19.5}, // вперёд
                                           {-2.5, 2.5}, //вбок
                                           {4,5}, //вверх
                                           {14.5,19.5}, //назад
                                           {14.5,19.5}, //вправо
                                           {2.5,3.5}, //вверх кз-2
                                           {132.5,139.5}, //ПС
                                           {-1,1}, //УС
                                           {0,0}, //пустышка
                                           {-4.5,4.5}, //Дельта ПС
                                           {-3.5,3.5}, //Дельта УС
                                           {0.5, 2.5}, //время включения табло П

                                       };

        private string[] strArray = {
                                        string.Format(Resources.SpeedN,Resources.Forward.ToUpper()),
                                        string.Format(Resources.SpeedN,Resources.Sideway.ToUpper()),
                                        string.Format(Resources.SpeedN,Resources.Up.ToUpper()),
                                        string.Format(Resources.SpeedN,Resources.Back.ToUpper()),
                                        string.Format(Resources.SpeedN,Resources.Right.ToUpper()),
                                        string.Format(Resources.SpeedN,Resources.Up.ToUpper()),
                                        string.Format(Resources.PS, ""),
                                        string.Format(Resources.US, ""),
                                        string.Empty,
                                        ((Char)0x0394)+ string.Format(Resources.PS, ""),
                                        ((Char)0x0394)+ string.Format(Resources.US, ""),
                                        Resources.TimeWhilePOn
    };

        private IFrmWork15Base _frm;
        public PK(IFrmWork15Base frm)
        {
            _frm = frm;
            InitializeComponent();
        }

        private void txtBox_TextChanged(object sender, EventArgs e)
        {
            TextBox tbCurrent = (TextBox) sender;
            tbCurrent.ForeColor = SystemColors.WindowText;
            tbCurrent.Text = tbCurrent.Text.Trim();
        }        

        private void txtBox_Validating(object sender, CancelEventArgs e)
        {
            TextBox tbCurrent = (TextBox) sender;
            int numTb = (Convert.ToInt32(tbCurrent.Name.Substring(tbCurrent.Name.Length - 3, 1)) - 1)*3 + (Convert.ToInt32(tbCurrent.Name.Substring(tbCurrent.Name.Length - 1, 1)) - 1);            
            if (tbCurrent.Text != "")
            {
                double d = Functions.MoveToDouble(tbCurrent.Text);
                if (double.IsNaN(d))
                {
                  Globals.ErrorInputTextBox(tbCurrent);
                  return;
                }
                if (!(d >= allowRange[numTb, 0] && d <= allowRange[numTb, 1]))
                {
                    Globals.ErrorCorridorTextBox(tbCurrent, allowRange[numTb, 0], allowRange[numTb, 1], strArray[numTb]);
                    return;
                }
                // красный/зеленый коридор
                tbCurrent.ForeColor = greenRange[numTb, 0] <= d && d <= greenRange[numTb, 1] ? Color.LimeGreen : Color.Red;
            }
        }

        private void rbtn_Work_Click(object sender, EventArgs e)
        {
            grpBox_BPK_6.Enabled = true;
        }

        private void grpBox_BPK_Enter(object sender, EventArgs e)
        {

        }

        private void PK_Load(object sender, EventArgs e)
        {
            txtBox_1_1.Text = txtBox_1_2.Text = txtBox_1_3.Text = txtBox_2_1.Text =
                txtBox_2_2.Text = txtBox_2_3.Text = txtBox_3_1.Text = txtBox_3_2.Text =
                txtBox__4_1.Text = txtBox__4_2.Text = "";
            txtBox_1_1.Focus();
        }

        internal void Save()
        {
            _frm.ManualRecord("BPK_15_Forward", txtBox_1_1.Text); // Вперед 
            _frm.ManualRecord("BPK_15_Side", txtBox_1_2.Text); // Вбок
            _frm.ManualRecord("BPK_15_Up", txtBox_1_3.Text); // Вверх
            _frm.ManualRecord("BPK_15_Back", txtBox_2_1.Text); // Назад 
            _frm.ManualRecord("BPK_15_Right", txtBox_2_2.Text); // Вправо 
            _frm.ManualRecord("BPK_15_Up_2", txtBox_2_3.Text); // Вверх (положение 2)
            _frm.ManualRecord("BPK_15_W", txtBox_3_1.Text); // Путевая скорость 
            _frm.ManualRecord("BPK_15_Alfa", txtBox_3_2.Text); // Угол сноса 
            _frm.ManualRecord("BPK_15_Delta_W", txtBox__4_1.Text); // Путевая скорость (положение 4)
            _frm.ManualRecord("BPK_15_Delta_Alfa", txtBox__4_2.Text); // Угол сноса (положение 4)
            _frm.ManualRecord("BPK_15_Time", txtBox__4_3.Text); // время вкл.табло П
            _frm.ManualRecordBool("BPK_15_Signal", rbtn_3_Yes, rbtn_3_No); // W>=50
            _frm.ManualRecordBool("BPK_15_Work", rbtn_Work_Yes, rbtn_Work_No); // Положение РАБОТА 
            _frm.ManualRecordBool("BPK_15_Memory", rbtn_4_Yes, rbtn_4_No); // Положение 4
        }         

        private void PK_VisibleChanged(object sender, EventArgs e)
        {
            
        }

        private void PK_EnabledChanged(object sender, EventArgs e)
        {
          if (!Enabled)
          {
            Save();
          }
        }
    }
}
