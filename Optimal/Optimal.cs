using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Optimal
{
    public delegate double Func(double x);

    class Optimal
    {
        double eps;  // заданная точность
        double xn; 
        double h; // шаг

        double a, b;

        
        public static double StepByStep(Func func, double xn, double eps, double h)
        {
            double fn = func(xn);
            for (int i = 0; Math.Abs(h) > eps; i++)
            {
        
                double xs = xn + h;
                double fs = func(xs);

                if (fs < fn)
                {
                    xn = xs;
                    fn = fs;
                }
                else
                {
                    xn = xs;
                    fn = fs;
                    h = -h / 2;
                }
                
            }
            return xn;
        }
        public static double GoldenRatio(Func func, double a, double b, double eps)
        {
            double l = b - a; // Длина отрезка
            double v; // Длина левой части
            double w; // Длина правой части
            v = a + l * 0.382;
            w = a + l * 0.618;
            double fv = func(v);
            double fw = func(w);

            while (l > eps)
            {
                if (fv < fw)
                {
                    b = w;
                    w = v;
                    l = b - a;
                    fw = fv;
                    v = a + l * 0.382;
                    fv = func(v);

                }
                else
                {
                    a = v;
                    l = b - a;
                    v = w;
                    fv = fw;
                    w = a + l * 0.618;
                    fw = func(w);
                }

            }

            return (a+b)/2;
        }
        // QUADRATIC APPROXIMATION
        public static double QuadroApp(double xn, double h, double eps, Func func)
        {
            double c;
            double b;
            double fmax; // f максимальное
            double xx = 0; // x новое
            double x1 = xn;
            double x2 = xn + h;
            double x3;
            double xp; // x предыдущее
            double f1 = func(x1);
            double f2 = func(x2);

            if (f1 > f2)
            {
                x3 = x1 + 2 * h;

            }
            else
            {
                x3 = x1 - h;
            }

            double f3 = func(x3);
            do

            {
                b = (f2 - f1) / (x2 - x1);
                c = 1 / (x3 - x2) * (((f3 - f1) / (x3 - x1)) - b);

                xp = xx;
                xx = 0.5 * (x2 + x1) - (b / (2 * c));

                double fxx = func(xx);
                List<double> list = new List<double>() { f1, f2, f3, fxx };
                fmax = list.Max();

                if (fmax == f1)
                {
                    if (xx < x2)
                    {
                        f1 = fxx;
                        x1 = xx;
                    }
                    else
                    {
                        x1 = x2;
                        f1 = f2;
                        x2 = xx;
                        f2 = fxx;
                    }
                }
                if (fmax == f3)
                {
                    if (xx < x2)
                    {
                        f3 = f2;
                        f2 = fxx;
                        x3 = x2;
                        x2 = xx;
                    }
                    else
                    {
                        x3 = xx;
                        f3 = fxx;
                    }
                }

            }
            while (Math.Abs(xx - xp) > eps);

            return xx;
        }


    }
}
