using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NonLinearEquationsSolver;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            IFunction function = new Function1();
            ISolver solver = new Solver();
            Vector<double> force = DenseVector.Create(2, 1);
            SolverInputs input = new SolverInputs
            {
                Force = force,
                Function = function,
                FirstLambdaValue = 1,
                LastLambdaValue = 1,
                MaxIncrements = 10,
                MaxIterations = 10,
                Tolerances = new Tolerances(1e-3, 1e-3, 1e-3, 1e-3),
                Beta = 1,
                InitialApproximation = DenseVector.Create(2, 0.1),
            };
            Vector<double> solution;
            IterationReport report = solver.Solve(input, out solution);

            Vector<double> realSolution = DenseVector.Create(2, 1);
            Assert.IsTrue((solution - realSolution).Norm(2) <= 1e-3);
        }
    }

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
