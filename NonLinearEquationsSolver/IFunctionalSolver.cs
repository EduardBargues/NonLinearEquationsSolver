using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace NonLinearEquationsSolver
{
    public interface IFunctionalSolver
    {
        Vector<double> Solve(IMultiDimensionalFunction function, Vector<double> forceVector, Dictionary<object,object> props);
    }
}
