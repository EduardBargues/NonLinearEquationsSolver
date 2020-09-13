
namespace NLES
{
    /// <summary>
    /// Represents an interface for a solver to solve A*x = b.
    /// </summary>
    public interface ILinearSolver
    {
        /// <summary>
        /// Solves a linear system Ax=input. The output is x
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Vector Solve(Vector input);
    }
}
