using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace NLES.Tests
{
    internal class LinearSolverForTesting : ILinearSolver
    {
        private readonly Matrix<double> matrix;

        public LinearSolverForTesting(Matrix<double> matrix) => this.matrix = matrix;

        public Vector<double> Solve(Vector<double> input)
            => matrix.Solve(input);
    }
}
