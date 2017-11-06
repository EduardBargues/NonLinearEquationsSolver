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
            ProblemDefinition input = new ProblemDefinition
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
            IterationPhaseReport phaseReport = solver.Solve(input, out solution);

            Vector<double> realSolution = DenseVector.Create(2, 1);
            Assert.IsTrue((solution - realSolution).Norm(2) <= 1e-3);
        }
        [TestMethod]
        public void TestMethod2()
        {
            IFunction function = new Function2();
            ISolver solver = new Solver();
            Vector<double> force = DenseVector.Create(2, 1);
            ProblemDefinition input = new ProblemDefinition
            {
                Force = force,
                Function = function,
                FirstLambdaValue = 1,
                LastLambdaValue = 1,
                MaxIncrements = 10,
                MaxIterations = 10,
                Tolerances = new Tolerances(1e-3, 1e-3, 1e-3, 1e-3),
                Beta = 1,
                InitialApproximation = DenseVector.Create(2, 0.1)
            };
            Vector<double> solution;
            IterationPhaseReport phaseReport = solver.Solve(input, out solution);
            Vector<double> realSolution = DenseVector.Create(2, 1);
            Assert.IsTrue((solution - realSolution).Norm(2) <= 1e-3);
        }
        [TestMethod]
        public void TestMethod3()
        {
            IFunction function = new Function3();
            ISolver solver = new Solver();
            Vector<double> force = DenseVector.Create(2, 2);
            ProblemDefinition input = new ProblemDefinition
            {
                Force = force,
                Function = function,
                FirstLambdaValue = 1,
                LastLambdaValue = 1,
                MaxIncrements = 10,
                MaxIterations = 10,
                Tolerances = new Tolerances(1e-3, 1e-3, 1e-3, 1e-3),
                Beta = 1,
                InitialApproximation = DenseVector.Create(2, 0.1)
            };
            Vector<double> solution;
            IterationPhaseReport phaseReport = solver.Solve(input, out solution);
            Vector<double> realSolution = DenseVector.Create(2, 1);
            Assert.IsTrue((solution - realSolution).Norm(2) <= 1e-3);
        }
        [TestMethod]
        public void TestMethod4()
        {
            IFunction function = new Function4();
            ISolver solver = new Solver();
            Vector<double> force = DenseVector.Create(2, 2);
            ProblemDefinition input = new ProblemDefinition
            {
                Force = force,
                Function = function,
                FirstLambdaValue = 1,
                LastLambdaValue = 1,
                MaxIncrements = 10,
                MaxIterations = 10,
                Tolerances = new Tolerances(1e-3, 1e-3, 1e-3, 1e-3),
                Beta = 1,
                InitialApproximation = DenseVector.Create(2, 0.1)
            };
            Vector<double> solution;
            IterationPhaseReport phaseReport = solver.Solve(input, out solution);
            Vector<double> realSolution = DenseVector.Create(2, 1);
            Assert.IsTrue((solution - realSolution).Norm(2) <= 1e-3);
        }
        [TestMethod]
        public void TestMethod5()
        {
            IFunction function = new Function4();
            ISolver solver = new Solver();
            Vector<double> force = DenseVector.Create(2, 2);
            ProblemDefinition input = new ProblemDefinition
            {
                Force = force,
                Function = function,
                FirstLambdaValue = 0,
                LastLambdaValue = 1,
                MaxIncrements = 10,
                MaxIterations = 10,
                Tolerances = new Tolerances(1e-3, 1e-3, 1e-3, 1e-3),
                Beta = 1,
                InitialApproximation = DenseVector.Create(2, 0.1)
            };
            Vector<double> solution;
            IterationPhaseReport phaseReport = solver.Solve(input, out solution);
            Vector<double> realSolution = DenseVector.Create(2, 1);
            Assert.IsTrue((solution - realSolution).Norm(2) <= 1e-3);
        }
    }
}