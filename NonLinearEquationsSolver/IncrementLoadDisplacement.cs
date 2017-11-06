using MathNet.Numerics.LinearAlgebra;

namespace NonLinearEquationsSolver
{
    public class IncrementLoadDisplacement
    {
        public IncrementLoadDisplacement(double dlambda, Vector<double> vector)
        {
            IncrementDisplacement = vector;
            IncrementLambda = dlambda;
        }

        public double IncrementLambda { get; set; }
        public Vector<double> IncrementDisplacement { get; set; }
    }
}