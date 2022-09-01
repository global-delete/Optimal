using System;
using System.Collections.Generic;
using System.Text;

namespace Optimal
{
    class Ozu
    {
        public delegate Vector OZUFunc(Vector X);
        public static Vector OzuMethod(Vector X, double eps, double h, Vector Fn, Vector Fv, int[] funcType, OZUFunc f)
        {
            //m кол-во пробных шагов
            int n = X.GetSize(), m = 3 * X.GetSize(), Pmin = int.MinValue;
            Vector[] tempXp = new Vector[m];
            Vector xp = new Vector(m), fp = new Vector(m);
            double ft = OptimCritery(X, Fn, Fv, funcType, f), Fpmin = double.MaxValue;

            do
            {
                for (int i = 0; i < m; i++)
                {
                    tempXp[i] = Vector.RandomNorm(X.GetSize());

                    // норм  вектора
                    double val = 0;
                    for (int j = 0; j < n; j++)
                        val += (tempXp[i][j] - X[j]) * (tempXp[i][j] - X[j]);
                    val = Math.Sqrt(val);
                    for (int j = 0; j < n; j++)
                        tempXp[i][j] = X[j] + h * (tempXp[i][j] - X[j]) / val;
                }
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                        xp[j] = tempXp[i][j];
                    fp[i] = OptimCritery(xp, Fn, Fv, funcType, f);

                    if (fp[i] < Fpmin) // минимальное значение ф-ии
                    {
                        Fpmin = fp[i];
                        Pmin = i;
                    }
                }
                if (Fpmin < ft) // новое значение ф-ии меньше, чем исх
                {
                    X = tempXp[Pmin];
                    ft = Fpmin;
                    h *= 1.2; // увел шага
                }
                else // новое значение ф-ии больше, чем исх
                    h *= 0.5; // умен шага
            }
            while (h > eps); // проверка точн
            return X;
        }
        public static double OptimCritery(Vector X, Vector Fn, Vector Fv, int[] funcType, OZUFunc f) //поиск оптимаьного критерия
        {
            Vector fx = f(X), g = new Vector(fx.GetSize());
            int gMax = 0;
            double g1, g2;

            for (int i = 0; i < g.GetSize(); i++)
            {
                if (funcType[i] == 1)// ограничение снизу (одностороннее)
                {
                    if (Fn[i] > 0)
                        g[i] = 2 - fx[i] / Fn[i];
                    if (Fn[i] == 0)
                        g[i] = -fx[i] / Fn[i] + 1;
                    if (Fn[i] < 0)
                        g[i] = fx[i] / Fn[i];
                }
                if (funcType[i] == 2)// ограничение сверху (одностороннее)
                {
                    if (Fv[i] > 0)
                        g[i] = fx[i] / Fv[i];
                    if (Fv[i] == 0)
                        g[i] = fx[i] / Fv[i] + 1;
                    if (Fv[i] < 0)
                        g[i] = 2 - fx[i] / Fv[i];
                }
                if (funcType[i] == 3) // двухстороннее ограничение 
                {
                    g1 = (fx[i] - Fn[i]) / (Fv[i] - Fn[i]);
                    g2 = (Fv[i] - fx[i]) / (Fv[i] - Fn[i]);
                    if (g1 > g2)
                        g[i] = g1;
                    else
                        g[i] = g2;
                }
                if (g[i] > g[gMax]) // макс критерий
                    gMax = i;
            }
            return g[gMax];
        }
    }
}
