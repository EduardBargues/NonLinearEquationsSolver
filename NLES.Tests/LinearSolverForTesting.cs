using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace NLES.Tests
{
    internal class LinearSolverForTesting : ILinearSolver
    {
        private readonly Matrix<double> matrix;

        public LinearSolverForTesting(Matrix<double> matrix) => this.matrix = matrix;

        public Vector Solve(Vector input)
        {
            Vector<double> v = new DenseVector(input.Dimension);
            for (int i = 0; i < input.Dimension; i++)
            {
                v[i] = input[i];
            }

            Vector<double> sol = matrix.Solve(v);

            Vector solution = new Vector(input.Dimension);
            for (int i = 0; i < input.Dimension; i++)
            {
                solution[i] = sol[i];
            }

            return solution;
        }
    }
}
