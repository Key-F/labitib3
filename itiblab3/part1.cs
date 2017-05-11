using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace itiblab3
{
    class part1
    {
        public static double[] net(double[,] W, double[] x, int first, int second) // first - количество нейронов в i слое; second - количество нейронов в i + 1 слое
        {
            double[] net = new double[second];
            for (int j = 0; j < second; j++)
            {
                net[j] += W[0, j]; // * 1
                for (int i = 1; i <= first; i++)
                {
                    net[j] += W[i, j] * x[i - 1];
                }
            }
            return net;
        }
        public static double fnet(double net)
        {
            return ((1 - Math.Exp(-net)) / (1 + Math.Exp(-net)));
        }
        public static double[] Out(double[] Out, double[] net, int kolneir) // kolneir - количество нейронов в слое, из которого считаем out
        {
            //Out[0] = 1;
            for (int j = 0; j < kolneir; j++)
            {
                Out[j] = Math.Round(fnet(net[j]), 2);
            }
            return Out;
        } 
    }
}
