using MathNet.Numerics.LinearAlgebra;

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
        Vector<double> Solve(Vector<double> input);
        
        
        /// Gets a value indicating whether the solver is initialized.
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// Gets the type of the solver.
        /// </summary>
        //BuiltInSolverType SolverType { get; }

        /// <summary>
        /// Initializes the solver regarding <see cref="A" /> matrix.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Solves a the system of A*x=b and store the x in <see cref="x" />.
        /// </summary>
        /// <param name="b">Right hand side vector.</param>
        /// <param name="x">Solution vector.</param>
        void Solve(double[] b, double[] x);
    }
    
    
}
