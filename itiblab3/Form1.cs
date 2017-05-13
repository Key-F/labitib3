using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace itiblab3
{
    public partial class Form1 : Form
    {
        static int count = 0;
        public Form1()
        {
            InitializeComponent();
            label8.Visible = false;
        }

      
        private void button1_Click(object sender, EventArgs e)
        {
           
            label8.Visible = true;
            label8.Update();
        
            count = 0;

            int N = Convert.ToInt32(textBox1.Text);
            int J = Convert.ToInt32(textBox3.Text);
            int M = Convert.ToInt32(textBox2.Text);
            int kolX; // Количество значений X

            if (formula.ParseDouble1(textBox6.Text) == 0)
            {
                if (formula.ParseDouble1(textBox5.Text) == 0)
                {
                    if (formula.ParseDouble1(textBox4.Text) == 0)
                        kolX = 0;
                    else
                        kolX = 1;
                }
                else kolX = 2;
            }
            else kolX = 3;

            if (formula.ParseDouble1(textBox5.Text) == 0) // второй элемент не может быть нулевым
            {
                MessageBox.Show("Входной вектор не может быть нулевым", "Ошибка", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                label8.Visible = false;
                return;
            }

            double[] X = new double[kolX-1];
            for (int i = 5; i < 5 + X.Length; i++)
            {
                string controlName = "textBox" + i;
                var controls = this.Controls.Find(controlName, true); // http://stackoverflow.com/questions/32224320/how-is-the-number-one-pick-textbox-from-text-box
                var control = controls.FirstOrDefault();
                var textBox = (TextBox)control;
                if (control != null)
                    X[i - 5] = formula.ParseDouble1(textBox.Text); // Норм работает
            }

            
            int kol10t; // Количество значений 10t

            if (formula.ParseDouble1(textBox9.Text) == 0)
            {
                if (formula.ParseDouble1(textBox8.Text) == 0)
                {
                    if (formula.ParseDouble1(textBox7.Text) == 0)
                        kol10t = 0;
                    else
                        kol10t = 1;
                }
                else kol10t = 2;
            }
            else kol10t = 3;
            if (kol10t == 0)
            {
                MessageBox.Show("10t не может быть нулевым", "Ошибка", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                label8.Visible = false;
                return;
            }

            double[] t10 = new double[kol10t];
            for (int i = 7; i < 7 + t10.Length; i++)
            {
                string controlName = "textBox" + i;
                var controls = this.Controls.Find(controlName, true); // http://stackoverflow.com/questions/32224320/how-is-the-number-one-pick-textbox-from-text-box
                var control = controls.FirstOrDefault();
                var textBox = (TextBox)control;
                if (control != null)
                    t10[i - 7] = formula.ParseDouble1(textBox.Text); // Норм работает
            }

            //if(J < )
            

            double[] netSKR = new double[J + 1]; // + 1 для 1 
            double[] netVIH = new double[M + 1]; // комбинированные входы нейронов скрытого и выходного слоев
            double[] OutSKR = new double[J];
            double[] OutVIH = new double[M]; // выходные сигналы нейронов скрытого и выходного слоев;
            double[] SigmaSKR = new double[J + 1];
            double[] SigmaVIH = new double[M + 1]; // ошибки скрытого и выходного слоев.
            double[,] WSKR = new double[N + 1, J];
            double[,] WVIH = new double[J + 1, M]; // веса скрытого и выходного слоев
            var random = new Random();
            if (radioButton1.Checked == true)
            {
                double WW = formula.ParseDouble1(textBox10.Text);
                for (int j = 0; j < J; j++)
                    for (int i = 0; i < N + 1; i++)
                    {
                        WSKR[i, j] = WW;
                    }
                for (int j = 0; j < M; j++)
                    for (int i = 0; i < J + 1; i++)
                    {
                        WVIH[i, j] = WW;
                    }
            }
            int a, b;
            if (radioButton2.Checked == true)
            {
                a = Convert.ToInt32(textBox11.Text);
                b = Convert.ToInt32(textBox12.Text);
                for (int j = 0; j < J; j++)
                    for (int i = 0; i < N + 1; i++)
                    {
                        WSKR[i, j] = (float)random.Next(a, b) / 10f;
                    }
                for (int j = 0; j < M; j++)
                    for (int i = 0; i < J + 1; i++)
                    {
                        WVIH[i, j] = (float)random.Next(1, 10) / 10f;
                    }
            }
            for (int i = 0; i < t10.Length; i++)
            
                t10[i] = t10[i] * 0.1; 
            work(N, J, M, t10, X, netSKR, netVIH, OutSKR, OutVIH, SigmaSKR, SigmaVIH, WSKR, WVIH);
        }
        public  void work(int N, int J, int M, double[] t, double[] X1, double[] net1, double[] net2, double[] Out1, double[] Out2, double[] Sigma1, double[] Sigma2, double[,] W1, double[,] W2)
        {

            {
                               
                net1 = part1.net(W1, X1, N, J);
                Out1 = part1.Out(Out1, net1, J);
                net2 = part1.net(W2, Out1, J, M);
                Out2 = part1.Out(Out2, net2, M);

                Sigma1 = part2.sigmaJ(net1, J, M, W2, Sigma2);
                Sigma2 = part2.sigmaM(net2, M, t, Out2);
                
                W1 = formula.SetW(W1, N, J, 1, X1, Sigma1);
                W2 = formula.SetW(W2, J, M, 1, Out1, Sigma2);
                double err = formula.error(t, Out2, M);
                                 
                if (err > 0)
                {
                    richTextBox1.ScrollToCaret();
                    count++;
                    richTextBox1.AppendText("                   Номер эпохи: (" + count + ") ");
                    richTextBox1.AppendText(Environment.NewLine);
                    richTextBox1.AppendText("   Выход нейронов внешнего слоя: ");
                    
                    for (int i = 0; i < Out2.Count(); i++)
                    {
                        string vvv=Convert.ToString(Math.Round(Out2[i], 3));
                        richTextBox1.AppendText(vvv + " ");
                    }
                    richTextBox1.AppendText(Environment.NewLine);
                    richTextBox1.AppendText("  " + "  Веса скрытого слоя W1" + Environment.NewLine);
                    for (int i = 0; i < N + 1; i++)
                    {
                        for (int j = 0; j < J; j++)
                        {
                            richTextBox1.AppendText("   " + Convert.ToString(Math.Round(W1[i, j], 2)) + "   ");
                        }
                        richTextBox1.AppendText(Environment.NewLine);
                    }
                    richTextBox1.ScrollToCaret();
                    richTextBox1.AppendText("  Веса внешнего слоя W2" + (Environment.NewLine));
                    for (int i = 0; i < J; i++)
                    {
                        for (int j = 0; j < M; j++)
                        {
                            richTextBox1.AppendText("   " + Convert.ToString(Math.Round(W2[i, j], 2)) + "   ");
                        }
                        richTextBox1.AppendText(Environment.NewLine);
                    }
                    richTextBox1.AppendText("      Среднеквадратическая ошибка: ");
                    richTextBox1.AppendText(Convert.ToString(Math.Round(err, 3)) + Environment.NewLine);
                    richTextBox1.AppendText(Environment.NewLine);
                    richTextBox1.ScrollToCaret();
                    work(N, J, M, t, X1, net1, net2, Out1, Out2, Sigma1, Sigma2, W1, W2);

                }                    
                else
                {
                    count++;
                    richTextBox1.AppendText("                   Номер эпохи: (" + count + ") ");
                    richTextBox1.AppendText(Environment.NewLine);
                    richTextBox1.AppendText("   Выход нейронов внешнего слоя: ");

                    for (int i = 0; i < Out2.Count(); i++)
                    {
                        string vvv = Convert.ToString(Math.Round(Out2[i], 3));
                        richTextBox1.AppendText(vvv + " ");
                    }
                    richTextBox1.AppendText(Environment.NewLine);
                    richTextBox1.AppendText("   Веса скрытого слоя W1" + Environment.NewLine);
                    for (int i = 0; i < N + 1; i++)
                    {
                        for (int j = 0; j < J; j++)
                        {
                            richTextBox1.AppendText("   " + Convert.ToString(Math.Round(W1[i, j], 2)) + "   ");
                        }
                        richTextBox1.AppendText(Environment.NewLine);
                    }

                    richTextBox1.AppendText("  Веса внешнего слоя W2" + (Environment.NewLine));
                    for (int i = 0; i < J; i++)
                    {
                        for (int j = 0; j < M; j++)
                        {
                            richTextBox1.AppendText("   " + Convert.ToString(Math.Round(W2[i, j], 2)) + "   ");
                        }
                        richTextBox1.AppendText(Environment.NewLine);
                    }
                    richTextBox1.AppendText("      Среднеквадратическая ошибка: ");
                    richTextBox1.AppendText(Convert.ToString(Math.Round(err, 3)) + Environment.NewLine);
                    richTextBox1.AppendText(Environment.NewLine);
                    richTextBox1.ScrollToCaret();
                    label8.Visible = false;
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label8.Visible = true;
            label8.Update();
            string text = string.Empty;

            for (int i = 0; i < richTextBox1.Lines.Length; i++)
                text += richTextBox1.Lines[i] + Environment.NewLine;
            if (text == "") MessageBox.Show("Нечего копировать", "Ошибка", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            else
            {
                Clipboard.SetText(text);
                MessageBox.Show("Скопировано", "Сообщение", MessageBoxButtons.OK);
            }
            label8.Visible = false;
        }
    }
}
