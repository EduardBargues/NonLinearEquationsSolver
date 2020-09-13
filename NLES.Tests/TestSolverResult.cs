using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using NLES;
using NLES.Contracts;
using NLES.Tests;
using Xunit;
using Vector = NLES.Vector;

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
            static Vector Reaction(Vector u) => new Vector(2) { [0] = u[0], [1] = u[0] + 2 * u[1] };

            static ILinearSolver Stiffness(Vector u)
                => new LinearSolverForTesting(new DenseMatrix(2, 2) { [0, 0] = 1, [1, 0] = 1, [1, 1] = 2 });
            Vector force = new Vector(2) { [0] = 1, [1] = 3 };
            NonLinearSolver Solver = NonLinearSolver.Builder
                .Solve(2, Reaction, Stiffness)
                .Under(force)
                .Build();

            // ACT
            List<LoadState> states = Solver.Broadcast().Take(2).ToList();

            // ASSERT
            AssertSolutionsAreCorrect(Reaction, force, states);
        }

        [Fact]
        public void SolveLinearFunctionSmallIncrements()
        {
            // ARRANGE
            static Vector Reaction(Vector u) => new Vector(2) { [0] = u[0], [1] = u[0] + 2 * u[1] };

            static ILinearSolver Stiffness(Vector u)
                => new LinearSolverForTesting(new DenseMatrix(2, 2) { [0, 0] = 1, [1, 0] = 1, [1, 1] = 2 });
            Vector force = new Vector(2) { [0] = 1, [1] = 3 };
            double inc = 1e-2;
            NonLinearSolver Solver = NonLinearSolver.Builder
                .Solve(2, Reaction, Stiffness)
                .Under(force)
                .UsingStandardNewtonRaphsonScheme(inc)
                .Build();

            // ACT
            List<LoadState> states = Solver.Broadcast().TakeWhile(s => s.Lambda <= 1).ToList();

            // ASSERT
            Assert.Equal((int)(1 / inc) - 1, states.Count);
            AssertSolutionsAreCorrect(Reaction, force, states);
        }

        [Fact]
        public void SolveLinearFunctionArcLength()
        {
            // ARRANGE
            static Vector Reaction(Vector u) => new Vector(2) { [0] = u[0], [1] = u[0] + 2 * u[1] };

            static ILinearSolver Stiffness(Vector u)
                => new LinearSolverForTesting(new DenseMatrix(2, 2) { [0, 0] = 1, [1, 0] = 1, [1, 1] = 2 });
            Vector force = new Vector(2) { [0] = 1, [1] = 3 };
            double radius = 1e-2;
            NonLinearSolver Solver = NonLinearSolver.Builder
                .Solve(2, Reaction, Stiffness)
                .Under(force)
                .UsingArcLengthScheme(radius)
                .Build();

            // ACT
            List<LoadState> states = Solver.Broadcast().TakeWhile(s => s.Lambda <= 1).ToList();

            // ASSERT
            AssertSolutionsAreCorrect(Reaction, force, states);
        }

        [Fact]
        public void SolveQuadraticFunction()
        {
            // ARRANGE
            static Vector Reaction(Vector u) => new Vector(2)
            {
                [0] = u[0] * u[0] + 2 * u[1] * u[1],
                [1] = 2 * u[0] * u[0] + u[1] * u[1]
            };

            static ILinearSolver Stiffness(Vector u)
                => new LinearSolverForTesting(new DenseMatrix(2, 2)
                {
                    [0, 0] = 2 * u[0],
                    [0, 1] = 4 * u[1],
                    [1, 0] = 4 * u[0],
                    [1, 1] = 2 * u[1]
                });
            Vector force = new Vector(2) { [0] = 3, [1] = 3 };
            NonLinearSolver Solver = NonLinearSolver.Builder
                .Solve(2, Reaction, Stiffness)
                .Under(force)
                .WithInitialConditions(0.1, new Vector(2, 0), new Vector(2, 1))
                .Build();

            // ACT
            List<LoadState> states = Solver.Broadcast().TakeWhile(x => x.Lambda <= 1).ToList();

            // ASSERT
            AssertSolutionsAreCorrect(Reaction, force, states);
        }

        [Fact]
        public void SolveQuadraticFunctionSmallIncrements()
        {
            // ARRANGE
            static Vector Reaction(Vector u) => new Vector(2)
            {
                [0] = u[0] * u[0] + 2 * u[1] * u[1],
                [1] = 2 * u[0] * u[0] + u[1] * u[1]
            };

            static ILinearSolver Stiffness(Vector u)
                => new LinearSolverForTesting(new DenseMatrix(2, 2)
                {
                    [0, 0] = 2 * u[0],
                    [0, 1] = 4 * u[1],
                    [1, 0] = 4 * u[0],
                    [1, 1] = 2 * u[1]
                });
            Vector force = new Vector(2) { [0] = 3, [1] = 3 };
            NonLinearSolver Solver = NonLinearSolver.Builder
                .Solve(2, Reaction, Stiffness)
                .Under(force)
                .WithInitialConditions(0.1, new Vector(2, 0), new Vector(2, 1))
                .UsingStandardNewtonRaphsonScheme(0.01)
                .Build();

            // ACT
            List<LoadState> states = Solver.Broadcast().TakeWhile(x => x.Lambda <= 1).ToList();

            // ASSERT
            AssertSolutionsAreCorrect(Reaction, force, states);
        }

        [Fact]
        public void SolveQuadraticFunctionArcLength()
        {
            // ARRANGE
            static Vector Reaction(Vector u) => new Vector(2)
            {
                [0] = u[0] * u[0] + 2 * u[1] * u[1],
                [1] = 2 * u[0] * u[0] + u[1] * u[1]
            };

            static ILinearSolver Stiffness(Vector u)
                => new LinearSolverForTesting(new DenseMatrix(2, 2)
                {
                    [0, 0] = 2 * u[0],
                    [0, 1] = 4 * u[1],
                    [1, 0] = 4 * u[0],
                    [1, 1] = 2 * u[1]
                });
            Vector force = new Vector(2) { [0] = 3, [1] = 3 };
            NonLinearSolver Solver = NonLinearSolver.Builder
                .Solve(2, Reaction, Stiffness)
                .Under(force)
                .WithInitialConditions(0.1, new Vector(2, 0), new Vector(2, 1))
                .UsingArcLengthScheme(0.05)
                .NormalizeLoadWith(0.01)
                .Build();

            // ACT
            List<LoadState> states = Solver.Broadcast().TakeWhile(x => x.Lambda <= 10).ToList();

            // ASSERT
            AssertSolutionsAreCorrect(Reaction, force, states);
        }

        void AssertSolutionIsCorrect(Vector solution)
        {
            double first = solution[0];
            foreach (double d in solution)
            {
                Assert.Equal(first, d, decimalsPrecision);
            }
        }

        void AssertSolutionsAreCorrect(
            Func<Vector, Vector> reaction
            , Vector force
            , List<LoadState> states)
        {
            foreach (LoadState state in states)
            {
                AssertSolutionIsCorrect(state.Displacement);
                AssertAreCloseEnough(reaction(state.Displacement), state.Lambda * force, tolerance);
            }
        }

        void AssertAreCloseEnough(
            Vector v1
            , Vector v2
            , double tolerance)
            => Assert.True((v1 - v2).Norm(2) <= tolerance);
    }
}
