using System;
using System.Collections.Generic;
using System.Linq;

using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

using NLES;
using NLES.Contracts;
using Xunit;

namespace NonLinearEquationsSolver
{
    public class TestSolverResults
    {
        readonly double tolerance = 1e-3;
        readonly int decimalsPrecision = 10;

        [Fact]
        public void SolveLinearFunction()
        {
            // ARRANGE
            static Vector<double> Function(Vector<double> u) => new DenseVector(2) { [0] = u[0], [1] = u[0] + 2 * u[1] };

            static Matrix<double> Stiffness(Vector<double> u) => new DenseMatrix(2, 2) { [0, 0] = 1, [1, 0] = 1, [1, 1] = 2 };
            DenseVector force = new DenseVector(2) { [0] = 1, [1] = 3 };
            Solver Solver = Solver.Builder
                .Solve(2, Function, Stiffness)
                .Under(force)
                .Build();

            // ACT
            List<LoadState> states = Solver.Broadcast().Take(2).ToList();

            // ASSERT
            AssertSolutionsAreCorrect(Function, force, states);
        }

        [Fact]
        public void SolveLinearFunctionSmallIncrements()
        {
            // ARRANGE
            static Vector<double> Function(Vector<double> u) => new DenseVector(2) { [0] = u[0], [1] = u[0] + 2 * u[1] };

            static Matrix<double> Stiffness(Vector<double> u) => new DenseMatrix(2, 2) { [0, 0] = 1, [1, 0] = 1, [1, 1] = 2 };
            DenseVector force = new DenseVector(2) { [0] = 1, [1] = 3 };
            double inc = 1e-2;
            Solver Solver = Solver.Builder
                .Solve(2, Function, Stiffness)
                .Under(force)
                .UsingStandardNewtonRaphsonScheme(inc)
                .Build();

            // ACT
            List<LoadState> states = Solver.Broadcast().TakeWhile(s => s.Lambda <= 1).ToList();

            // ASSERT
            Assert.Equal((int)(1 / inc) - 1, states.Count);
            AssertSolutionsAreCorrect(Function, force, states);
        }

        [Fact]
        public void SolveLinearFunctionArcLength()
        {
            // ARRANGE
            static Vector<double> Function(Vector<double> u) => new DenseVector(2) { [0] = u[0], [1] = u[0] + 2 * u[1] };

            static Matrix<double> Stiffness(Vector<double> u) => new DenseMatrix(2, 2) { [0, 0] = 1, [1, 0] = 1, [1, 1] = 2 };
            DenseVector force = new DenseVector(2) { [0] = 1, [1] = 3 };
            double radius = 1e-2;
            Solver Solver = Solver.Builder
                .Solve(2, Function, Stiffness)
                .Under(force)
                .UsingArcLengthScheme(radius)
                .Build();

            // ACT
            List<LoadState> states = Solver.Broadcast().TakeWhile(s => s.Lambda <= 1).ToList();

            // ASSERT
            AssertSolutionsAreCorrect(Function, force, states);
        }

        [Fact]
        public void SolveQuadraticFunction()
        {
            // ARRANGE
            static Vector<double> Function(Vector<double> u) => new DenseVector(2)
            {
                [0] = u[0] * u[0] + 2 * u[1] * u[1],
                [1] = 2 * u[0] * u[0] + u[1] * u[1]
            };

            static Matrix<double> Stiffness(Vector<double> u) => new DenseMatrix(2, 2)
            {
                [0, 0] = 2 * u[0],
                [0, 1] = 4 * u[1],
                [1, 0] = 4 * u[0],
                [1, 1] = 2 * u[1]
            };
            DenseVector force = new DenseVector(2) { [0] = 3, [1] = 3 };
            Solver Solver = Solver.Builder
                .Solve(2, Function, Stiffness)
                .Under(force)
                .WithInitialConditions(0.1, DenseVector.Create(2, 0), DenseVector.Create(2, 1))
                .Build();

            // ACT
            List<LoadState> states = Solver.Broadcast().TakeWhile(x => x.Lambda <= 1).ToList();

            // ASSERT
            AssertSolutionsAreCorrect(Function, force, states);
        }

        [Fact]
        public void SolveQuadraticFunctionSmallIncrements()
        {
            // ARRANGE
            static Vector<double> Function(Vector<double> u) => new DenseVector(2)
            {
                [0] = u[0] * u[0] + 2 * u[1] * u[1],
                [1] = 2 * u[0] * u[0] + u[1] * u[1]
            };

            static Matrix<double> Stiffness(Vector<double> u) => new DenseMatrix(2, 2)
            {
                [0, 0] = 2 * u[0],
                [0, 1] = 4 * u[1],
                [1, 0] = 4 * u[0],
                [1, 1] = 2 * u[1]
            };
            DenseVector force = new DenseVector(2) { [0] = 3, [1] = 3 };
            Solver Solver = Solver.Builder
                .Solve(2, Function, Stiffness)
                .Under(force)
                .WithInitialConditions(0.1, DenseVector.Create(2, 0), DenseVector.Create(2, 1))
                .UsingStandardNewtonRaphsonScheme(0.01)
                .Build();

            // ACT
            List<LoadState> states = Solver.Broadcast().TakeWhile(x => x.Lambda <= 1).ToList();

            // ASSERT
            AssertSolutionsAreCorrect(Function, force, states);
        }

        [Fact]
        public void SolveQuadraticFunctionArcLength()
        {
            // ARRANGE
            static Vector<double> Function(Vector<double> u) => new DenseVector(2)
            {
                [0] = u[0] * u[0] + 2 * u[1] * u[1],
                [1] = 2 * u[0] * u[0] + u[1] * u[1]
            };

            static Matrix<double> Stiffness(Vector<double> u) => new DenseMatrix(2, 2)
            {
                [0, 0] = 2 * u[0],
                [0, 1] = 4 * u[1],
                [1, 0] = 4 * u[0],
                [1, 1] = 2 * u[1]
            };
            DenseVector force = new DenseVector(2) { [0] = 3, [1] = 3 };
            Solver Solver = Solver.Builder
                .Solve(2, Function, Stiffness)
                .Under(force)
                .WithInitialConditions(0.1, DenseVector.Create(2, 0), DenseVector.Create(2, 1))
                .UsingArcLengthScheme(0.05)
                .NormalizeLoadWith(0.01)
                .Build();

            // ACT
            List<LoadState> states = Solver.Broadcast().TakeWhile(x => x.Lambda <= 10).ToList();

            // ASSERT
            AssertSolutionsAreCorrect(Function, force, states);
        }

        void AssertSolutionIsCorrect(Vector<double> solution)
        {
            double first = solution.First();
            foreach (double d in solution)
            {
                Assert.Equal(first, d, decimalsPrecision);
            }
        }

        void AssertSolutionsAreCorrect(Func<Vector<double>, Vector<double>> reaction, Vector<double> force, List<LoadState> states)
        {
            foreach (LoadState state in states)
            {
                AssertSolutionIsCorrect(state.Displacement);
                AssertAreCloseEnough(reaction(state.Displacement), state.Lambda * force, tolerance);
            }
        }

        void AssertAreCloseEnough(Vector<double> v1, Vector<double> v2, double tolerance) => Assert.True((v1 - v2).Norm(2) <= tolerance);
    }
}
