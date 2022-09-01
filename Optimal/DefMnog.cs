using System;
using System.Collections.Generic;
using System.Text;

namespace Optimal
{
    class DefMnog
    {
        static int NP; //число аргументов функции
        double[,] simplex = new double[NP, NP + 1]; // NP + 1 - число вершин симплекса
        double[] FN = new double[NP + 1];//массив значений 

        // double[] X - Первая вершина начального симплекса (начальная точка)
        //Np число аргументов функции
        public delegate double Func(double[] X, int NP);


        // Создает из точки X регулярный симплекс с длиной ребра L и с NP + 1 вершиной
        // Формирует массив FN значений оптимизируемой функции F в вершинах симплекса
        //L - Начальная длина ребра симплекса
        //L_thres - Предельное значение длины ребра симплекса
        void MakeSimplex(Func F, double[,] simplex, double[] X, double L, int NP, bool first)
        {
            double qn, q2, r1, r2;
            int i, j;
            qn = Math.Sqrt(1.0 + NP) - 1.0;
            q2 = L / Math.Sqrt(2.0) * (double)NP;
            r1 = q2 * (qn + (double)NP);
            r2 = q2 * qn;
            for (i = 0; i < NP; i++)
            {
                simplex[i, 0] = X[i];
            }
            for (i = 1; i < NP + 1; i++)
            {
                for (j = 0; j < NP; j++)
                {
                    simplex[j, i] = X[j] + r2;
                }
            }
            for (i = 1; i < NP + 1; i++)
            {
                simplex[i - 1, i] = simplex[i - 1, i] - r2 + r1;
            }
            for (i = 0; i < NP + 1; i++)
            {
                for (j = 0; j < NP; j++) X[j] = simplex[j, i];
                FN[i] = F(X, NP); // Значения функции в вершинах начального симплекса
            }
            if (first)
            {
                Console.WriteLine("Значения функции в вершинах начального симплекса:");
                for (i = 0; i < NP + 1; i++) Console.WriteLine(FN[i]);
            }
        }
        double[] Centr(int k, int NP) // Центр тяжести симплекса
        {
            int i, j;
            double s;
            double[] xc = new double[NP];//центр тяжести 
            for (i = 0; i < NP; i++)
            {
                s = 0;
                for (j = 0; j < NP + 1; j++)
                {
                    s += simplex[i, j];
                }
                xc[i] = s;
            }
            for (i = 0; i < NP; i++)
            {
                xc[i] = (xc[i] - simplex[i, k]) / (double)NP;
            }
            return xc;
        }
        void Reflection(int k, double cR, int NP) // Отражение вершины с номером k относительно центра тяжести
        {
            double[] xc = Centr(k, NP); // cR – коэффициент отражения
            for (int i = 0; i < NP; i++)
            {
                simplex[i, k] = (1.0 + cR) * xc[i] - simplex[i, k];
            }
        }
        void Reduction(int k, double gamma, int NP) // Редукция симплекса к вершине k(уменьшение длины всех ребер симплекса)
        {
            int i, j; // gamma – коэффициент редукции
            double[] xk = new double[NP];//выбранная вершина в алгоритме
            for (i = 0; i < NP; i++)
            {
                xk[i] = simplex[i, k];
            }
            for (j = 0; j < NP; j++)
            {
                for (i = 0; i < NP; i++)
                {
                    simplex[i, j] = xk[i] + gamma * (simplex[i, j] - xk[i]);//определяем новые координаты симплекса
                }
            }
            for (i = 0; i < NP; i++)
            {
                simplex[i, k] = xk[i]; // Восстанавливаем симплекс в вершине k
            }
        }
        void Сompression(int k, double alpha_beta, int NP) //растяжение/сжатие симплекса. alpha_beta – коэффициент растяжения/сжатия
        {
            double[] xc = Centr(k, NP);
            for (int i = 0; i < NP; i++)
            {
                simplex[i, k] = xc[i] + alpha_beta * (simplex[i, k] - xc[i]);
            }
        }
        double FindL(double[] X2, int NP) // Длина ребра симплекса
        {
            double L = 0;
            for (int i = 0; i < NP; i++) L += X2[i] * X2[i];
            return Math.Sqrt(L);
        }
        double minVal(double[] F, int N1, ref int imi) // Минимальный элемент массива и его индекс
        {
            double fmi = double.MaxValue, f;
            for (int i = 0; i < N1; i++)
            {
                f = F[i];
                if (f < fmi)
                {
                    fmi = f;
                    imi = i;
                }
            }
            return fmi;
        }
        double maxVal(double[] F, int N1, ref int ima)//Максимальный элемент массива и его индекс
        {
            double fma = double.MinValue, f;
            for (int i = 0; i < N1; i++)
            {
                f = F[i];
                if (f > fma)
                {
                    fma = f;
                    ima = i;
                }
            }
            return fma;
        }
        void simplexRestore(Func F, int NP)//выполняется восстановление симплекса
        {
            int i, imi = -1, imi2 = -1;
            double fmi, fmi2 = double.MaxValue, f;
            double[] X = new double[NP], X2 = new double[NP];
            fmi = minVal(FN, NP + 1, ref imi);
            for (i = 0; i < NP + 1; i++)
            {
                f = FN[i];
                if (f != fmi && f < fmi2)
                {
                    fmi2 = f;
                    imi2 = i;
                }
            }
            for (i = 0; i < NP; i++)
            {
                X[i] = simplex[i, imi];
                X2[i] = simplex[i, imi] - simplex[i, imi2];
            }
            MakeSimplex(F, simplex, X, FindL(X2, NP), NP, false);

            //Func F, double[,] simplex, double[] X, double L, int NP, bool first
        }
        bool Stop(double L_thres, int NP) // Возвращает true, если длина хотя бы одного ребра симплекса превышает L_thres, или false - в противном случае
        {
            int i, j, k;
            double[] X = new double[NP], X2 = new double[NP];
            for (i = 0; i < NP; i++)
            {
                for (j = 0; j < NP; j++)
                {
                    X[j] = simplex[j, i];
                }
                for (j = i + 1; j < NP + 1; j++)
                {
                    for (k = 0; k < NP; k++)
                    {
                        X2[k] = X[k] - simplex[k, j];
                    }
                    if (FindL(X2, NP) > L_thres)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        // Выполняет поиск экстремума (минимума) функции F
        public void NelderMead(Func F, ref double[] X, int NP, double L, double L_thres, double cR, double alpha, double beta, double gamma)
        {
            int i, j2, imi = -1, ima = -1;
            int j = 0, kr = 0, jMx = 10000; // Предельное число шагов алгоритма 
            double[] X2 = new double[NP], X_R = new double[NP];
            double Fmi, Fma, F_R, F_S, F_E;
            int Finstep = 60; //число шагов алгоритма, после выполнения которых симплекс восстанавливается
            Console.WriteLine("L = " + L);
            Console.WriteLine("L_thres = " + L_thres);
            Console.WriteLine("cR = " + cR);
            Console.WriteLine("alpha = " + alpha);
            Console.WriteLine("beta = " + beta);
            Console.WriteLine("gamma = " + gamma);
            MakeSimplex(F, simplex, X, L, NP, true);
            while (Stop(L_thres, NP) && j < jMx)
            {
                j++; // Число итераций
                kr++;
                if (kr == Finstep)
                {
                    kr = 0;
                    simplexRestore(F, NP); // Восстановление симплекса
                }
                Fmi = minVal(FN, NP + 1, ref imi);
                Fma = maxVal(FN, NP + 1, ref ima); // ima - Номер отражаемой вершины
                for (i = 0; i < NP; i++)
                {
                    X[i] = simplex[i, ima];
                }
                Reflection(ima, cR, NP); // Отражение
                for (i = 0; i < NP; i++)
                {
                    X_R[i] = simplex[i, ima];
                }
                F_R = F(X_R, NP); // Значение функции в вершине ima симплекса после отражения
                if (F_R > Fma)
                {
                    Сompression(ima, beta, NP); // Сжатие
                    for (i = 0; i < NP; i++)
                    {
                        X2[i] = simplex[i, ima];
                    }
                    F_S = F(X2, NP); // Значение функции в вершине ima симплекса после его сжатия
                    if (F_S > Fma)
                    {
                        for (i = 0; i < NP; i++)
                        {
                            simplex[i, ima] = X[i];
                        }
                        Reduction(ima, gamma, NP); // Редукция
                        for (i = 0; i < NP + 1; i++)
                        {
                            if (i == ima) continue;
                            for (j2 = 0; j2 < NP; j2++)
                            {
                                X2[j2] = simplex[j2, i];
                            }
                            // Значения функций в вершинах симплекса после редукции. В вершине ima значение функции сохраняется
                            FN[i] = F(X2, NP);
                        }
                    }
                    else
                    {
                        FN[ima] = F_S;
                    }
                }
                else if (F_R < Fmi)
                {
                    Сompression(ima, alpha, NP); // Растяжение
                    for (j2 = 0; j2 < NP; j2++)
                    {
                        X2[j2] = simplex[j2, ima];
                    }
                    F_E = F(X2, NP); // Значение функции в вершине ima симплекса после его растяжения
                    if (F_E > Fmi)
                    {
                        for (j2 = 0; j2 < NP; j2++)
                        {
                            simplex[j2, ima] = X_R[j2];
                        }
                        FN[ima] = F_R;
                    }
                    else
                    {
                        FN[ima] = F_E;
                    }
                }
                else
                {
                    FN[ima] = F_R;
                }
            }
            Console.WriteLine("Число итераций: " + j);
        }
    }
}
