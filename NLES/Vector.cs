using System;
using System.Collections;

namespace NLES
{
    public class Vector : IEnumerable
    {
        private readonly double[] array;

        public Vector(int dimension, double value = 0)
        {
            array = new double[dimension];
            for (int i = 0; i < dimension; i++)
            {
                array[i] = value;
            }
        }

        public int Dimension => array.Length;

        public double this[int index]
        {
            get => array[index];
            set => array[index] = value;
        }

        public double DotProduct(Vector vector)
        {
            double result = 0;
            for (int i = 0; i < Dimension; i++)
            {
                result += this[i] * vector[i];
            }

            return result;
        }

        public double Norm(double exp)
        {
            double result = 0;
            for (int i = 0; i < Dimension; i++)
            {
                result += Math.Pow(this[i], exp);
            }
            result = Math.Pow(result, 1.0 / exp);

            return result;
        }

        public IEnumerator GetEnumerator() => array.GetEnumerator();

        public static Vector operator +(Vector v1, Vector v2)
        {
            Vector result = new Vector(v1.Dimension);

            for (int i = 0; i < v1.Dimension; i++)
            {
                result[i] = v1[i] + v2[i];
            }

            return result;
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            Vector result = new Vector(v1.Dimension);

            for (int i = 0; i < v1.Dimension; i++)
            {
                result[i] = v1[i] - v2[i];
            }

            return result;
        }

        public static Vector operator *(double factor, Vector v)
        {
            Vector result = new Vector(v.Dimension);

            for (int i = 0; i < v.Dimension; i++)
            {
                result[i] = factor * v[i];
            }

            return result;
        }
    }
}
