using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimal
{
    class Gradient
    {
        public delegate double Funcp(Vector x); // Делегат для функции
        public delegate double Func(double x); // Делегат для функции 1 переменной
        public delegate double FuncOzu(Vector x);
        public static double Step(double xn, double h, double eps, Func func)
        {
            double fn = func(xn);

            while (Math.Abs(h) > eps)
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

        public static Vector Gradient_descent(Vector xn, double h, double eps, Funcp f)
        {

            int k = 0;
            while (true)
            {
                double fn = f(xn);
                Vector rez = new Vector(xn.GetSize());
                Vector bx;
                Vector xs;
                Vector help = new Vector(xn.GetSize());

                for (int i = 0; i < xn.GetSize(); i++)
                {
                    help[i] = eps * 0.26;
                    rez[i] = (f(xn + help) - fn) / (eps * 0.26);
                    help[i] = 0;
                }

                bx = -h * rez;
                xs = xn + bx;
                double fs = f(xs);
                if (fs < fn)
                    h *= 1.2;
                else
                {
                    h *= 0.5;
                }
                xn = xs;
                fn = fs;
                k++;
                if (bx.Norma1() < eps)
                {
                    return xn;
                }

            }

        }

        public static Vector Steepest_descent(Vector xn, double h, double eps, Funcp f1)
        {
            Vector x1 = xn;
            while (true)
            {
                double fn = f1(xn);
                Vector rez = new Vector(xn.GetSize());
                Vector xs = xn;
                double fs = fn;
                Vector help = new Vector(xn.GetSize());
                double hd = eps / 10.0;
                for (int i = 0; i < xn.GetSize(); i++)
                {
                    help[i] = hd;
                    rez[i] = (f1(xn + help) - fn) / hd;
                    help[i] = 0;
                }
                // поиск оптимального шага
                double dh = h;
                double hopt = Step(0.0, h, eps, x => f1(xn - x * rez));
                xs = xn - hopt * rez;
                fs = f1(xs);

                fn = f1(xn);
                xn = xs;
                fn = fs;

                Console.WriteLine("Current {0}  f {1} norma {2}", xn, fn, rez.Norma1());
                if (rez.Norma1() < eps)
                    break;

            }
            return xn;
        }

        public static Vector RandomSearch(Vector xn, double h, double eps, Funcp f)
        {
            int M = 3 * xn.GetSize();

            double minFxp;

            Vector d = new Vector(xn.GetSize());
            Vector[] xp = new Vector[M];
            double[] fxp = new double[M];

            int l = 0;

            Vector xs = xn;
            while (true)
            {
                l++;

                for (int j = 0; j < M; j++)
                {
                    d = Vector.NormalizeRandom(xs.GetSize());

                    xp[j] = xs + h * d;
                    fxp[j] = f(xp[j]);
                }
                minFxp = fxp.Min();

                if (minFxp > f(xs))
                    h = h * 0.5;
                else
                {
                    h *= 1.2;
                    xs = xp[Array.IndexOf(fxp, minFxp)];
                }

                if (h < eps)
                    break;

            }
            Console.WriteLine(l);
            return xs;
        }

     


    }
}
