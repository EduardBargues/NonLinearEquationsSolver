using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace NonLinearEquationsSolver
{
    public interface ISolver
    {
        SolverReport Solve(ProblemDefinition problem);
    }
}
