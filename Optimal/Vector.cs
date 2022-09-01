﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Optimal
{
    class Vector
    {
        public int size;
        public double[] data;
        static Random rnd = new Random();

        public Vector(int size)
        {
            this.size = size;
            data = new double[size];
        }
        public void Clone(Vector v)
        {
            for (var index = 0; index < this.size; ++index)
                v[index] = this[index];
        }
        public static Vector RandomNorm(int size)
        {
            Vector rez = new Vector(size);
            for (int i = 0; i < size; i++)
                rez.data[i] = (rnd.NextDouble() - 0.5) * 2.0;
            return rez.Normalize();
        }
        public Vector(double[] v)
        {
            this.size = v.Length;
            data = new double[size];
            for (int i = 0; i < size; i++) data[i] = v[i];
        }
        public Vector(Vector v)
        {
            this.size = v.size;
            data = new double[size];
            for (int i = 0; i < size; i++) data[i] = v.data[i];
        }
        public int GetSize()
        {
            return size;
        }
        public bool SetElement(double el, int index)
        {
            if (index < 0 || index >= size) return false;
            data[index] = el;
            return true;
        }
        public double this[int index]
        {
            get
            {
                if (index < 0 || index >= size) return default(double);
                return data[index];
            }
            set
            {
                if (index >= 0 && index < size)
                    data[index] = value;
            }
        }
        public double GetElement(int index)
        {
            if (index < 0 || index >= size) return default(double);
            return data[index];
        }
        public Vector Copy()
        {
            Vector rez = new Vector(data);
            return rez;
        }
        public override string ToString()
        {
            string s = "(";
            for (int i = 0; i < size - 1; i++)
                s = s + data[i].ToString() + "; ";
            s += data[size - 1].ToString() + ")";
            return s;
        }
        public static Vector operator *(double c, Vector v)
        {
            Vector rez = new Vector(v.size);
            for (int i = 0; i < v.size; i++) rez.data[i] = v.data[i] * c;
            return rez;

        }
        public static Vector operator *(Vector v, double c)
        {
            Vector rez = new Vector(v.size);
            for (int i = 0; i < v.size; i++) rez.data[i] = v.data[i] * c;
            return rez;

        }
        public static double operator *(Vector v, Vector c)
        {
            if (c.size != v.size) return Double.NaN;
            double rez = 0;
            for (int i = 0; i < v.size; i++) rez += v.data[i] * c.data[i];
            return rez;

        }
        public static Vector operator +(Vector u, Vector v)
        {
            if (u.size != v.size) return null;
            Vector rez = new Vector(v.size);
            for (int i = 0; i < v.size; i++) rez.data[i] = u.data[i] + v.data[i];
            return rez;

        }
        public Vector Plus(Vector c)
        {
            Vector rez = new Vector(size);
            for (int i = 0; i < size; i++) rez.data[i] = data[i] + c.data[i];
            return rez;
        }
        public static Vector operator -(Vector u, Vector v)
        {
            if (u.size != v.size) return null;
            Vector rez = new Vector(v.size);
            for (int i = 0; i < v.size; i++) rez.data[i] = u.data[i] - v.data[i];
            return rez;

        }
        public double Norma1()
        {
            double s = 0;
            for (int i = 0; i < size; i++)
                s += data[i] * data[i];
            return Math.Sqrt(s);
        }
        public double Norma2()
        {
            double s = 0;
            for (int i = 0; i < size; i++)
                if (Math.Abs(data[i]) > s) s = Math.Abs(data[i]);
            return s;
        }
        public double Norma3()
        {
            double s = 0;
            for (int i = 0; i < size; i++)
                s += Math.Abs(data[i]);
            return s;
        }
        public double ScalarMultiply(Vector b)
        {
            if (size != b.size) return 0;
            double s = 0;
            for (int i = 0; i < size; i++)
                s += data[i] * b.data[i];
            return s;
        }
        public Vector MultiplyScalar(double c)
        {
            Vector rez = new Vector(size);
            for (int i = 0; i < size; i++) rez.data[i] = data[i] * c;
            return rez;
        }
        public Vector Normalize()
        {
            Vector rez = new Vector(size);
            double d = Norma1();
            for (int i = 0; i < size; i++)
                if (d != 0) rez.data[i] = data[i] / d;
                else rez.data[i] = data[i];
            return rez;
        }
        public static Vector NormalizeRandom(int size)
        {
            Vector rez = new Vector(size);
            for (int i = 0; i < size; i++)
                rez.data[i] = (rnd.NextDouble() - 0.5) * 2.0;
            return rez.Normalize();
        }
        public Vector UMinus()
        {
            Vector rez = new Vector(size);
            for (int i = 0; i < size; i++) rez.data[i] = -data[i];
            return rez;
        }

        public Vector Minus(Vector c)
        {
            Vector rez = new Vector(size);
            for (int i = 0; i < size; i++) rez.data[i] = data[i] - c.data[i];
            return rez;
        }

    }
}
