using MathNet.Numerics.LinearAlgebra;

namespace NonLinearEquationsSolver
{
    public interface IMultiDimensionalFunction
    {
        Matrix<double> GetTangentMatrix(Vector<double> u);
        Vector<double> GetImage(Vector<double> u);
    }
}
