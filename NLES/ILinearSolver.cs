using MathNet.Numerics.LinearAlgebra;

namespace NLES
{
    public interface ILinearSolver
    {
        /// <summary>
        /// Solves a linear system Ax=input. The output is x
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Vector<double> Solve(Vector<double> input);
    }
}
