using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization; // Для работы с точкой в double (из textbox)

namespace itiblab3
{
    class formula
    {
        
        public static double ParseDouble1(string s)
        {
            double d;

            if (!Double.TryParse(s, NumberStyles.Float, CultureInfo.CurrentCulture, out d))
                Double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out d);

            return d;
        }
        public static double[,] SetW(double[,] W, int FirstInd, int SecondInt, double norm, double[] x, double[] Sigma)
        {
            for (int m = 0; m < SecondInt; m++)
            {
                W[0, m] += norm * 1 * Sigma[m]; // w0 заполняем
            }

            for (int j = 1; j <= FirstInd; j++)
            {
                for (int m = 0; m < SecondInt; m++)
                {
                    W[j, m] += norm * x[j - 1] * Sigma[m];
                }
            }
            return W;
        }
        public static double error(int tochnost, double[] t, double[] y, int M)
        {
            double Error = 0;
            for (int j = 0; j < M; j++)
            {
                Error += Math.Pow(Math.Round(t[j] - y[j], tochnost), 2);
            }
            return Math.Sqrt(Error);
        }
        public static List<double> converter(double[] x) // из массива в List
        {
            List<double> xc = new List<double>();
            for (int i = 0; i < x.Length; i++)
            {
                xc.Add(x[i]);
            }
            return xc;
        }
        
    }
}
