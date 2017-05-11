using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace itiblab3
{
    class part2
    {
        public static double[] sigmaM(double[] net, int M, double[] t, double[] y)
        {
            double[] Sigma = new double[M];
            for (int i = 0; i < M; i++)
            {
                Sigma[i] = 0.5 * (1 - part1.fnet(net[i])) * (t[i] - y[i]);
            }
            return Sigma;
        }
        public static double[] sigmaJ(double[] net, int J, int M, double[,] W, double[] SigmaM)
        {
            double[] Sigma = new double[J];
            for (int j = 1; j <= J; j++)
            {
                double Sum = 0;
                for (int m = 0; m < M; m++)
                {
                    Sum += W[j, m] * SigmaM[m];
                }
                Sigma[j - 1] = 0.5 * (1 - part1.fnet(net[j - 1])) * Sum;
            }
            return Sigma;
        }
    }
}
