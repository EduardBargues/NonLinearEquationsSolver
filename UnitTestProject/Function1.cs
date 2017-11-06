using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using NonLinearEquationsSolver;

namespace UnitTestProject
{
    public class Function1 : IFunction
    {
        public Matrix<double> GetTangentMatrix(Vector<double> u)
        {
            return new DenseMatrix(2, 2)
            {
                [0, 0] = 1,
                [1, 1] = 1
            };
        }

        public Vector<double> GetImage(Vector<double> u)
        {
            return new DenseVector(2)
            {
                [0] = u[0],
                [1] = u[1]
            };
        }
    }
}
