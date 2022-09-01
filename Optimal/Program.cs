using System;

namespace Optimal
{
    class Program
    {
        static void Main(string[] args)
        {  
            double xmin = Optimal.StepByStep(func, 0, 0.00001, 1);
            Console.WriteLine(xmin);
            double gr = Optimal.GoldenRatio(func, 0, 4, 0.00001);
            Console.WriteLine(gr);

            double[] a = new double[2] { 1.5, 1.5 };
            Vector xn = new Vector(a);
            Vector X1;
            X1 = Gradient.Gradient_descent(xn, 0.5, 0.001, xn => (3 * xn[0] * xn[0] + xn[1] * xn[1] - xn[0] * xn[1] + xn[0]));
            Console.WriteLine("GRADIENT: min = {0} \n F(min) = {1}", X1, ((3 * X1[0] * X1[0] + X1[1] * X1[1] - X1[0] * X1[1] + X1[0])));

        }

        static double func(double x)
        {
            return Math.Cos(x);
        }
    }
}
