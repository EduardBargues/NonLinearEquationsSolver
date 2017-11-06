using System;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using NonLinearEquationsSolver;

namespace UnitTestProject
{
    public class Function2 : IFunction
    {
        public Matrix<double> GetTangentMatrix(Vector<double> u)
        {
            return new DenseMatrix(2, 2)
            {
                [0, 0] = 2 * u[0],
                [1, 1] = 2 * u[1]
            };
        }

        public Vector<double> GetImage(Vector<double> u)
        {
            return new DenseVector(2)
            {
                [0] = Math.Pow(u[0], 2),
                [1] = Math.Pow(u[1], 2)
            };
        }
    }
}