using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace NonLinearEquationsSolver
{
    public interface ISolver
    {
        IterationPhaseReport Solve(ProblemDefinition problem, out Vector<double> solution);
    }
}
