using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace NonLinearEquationsSolver
{
    public interface ISolver
    {
        IterationReport Solve(SolverInputs props, out Vector<double> solution);
    }
}
