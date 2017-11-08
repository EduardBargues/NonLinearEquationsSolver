using MathNet.Numerics.LinearAlgebra;

namespace NonLinearEquationsSolver
{
    internal interface IConvergenceChecker
    {
        bool CheckConvergence(
            Vector<double> displacement,
            Vector<double> incrementDisplacement,
            double lambda
            );
    }
}