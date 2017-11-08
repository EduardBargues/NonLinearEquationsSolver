using MathNet.Numerics.LinearAlgebra;

namespace NonLinearEquationsSolver
{
    public class IterationInfo
    {
        public double Lambda { get; set; }
        public Vector<double> Reaction { get; set; }
        public Vector<double> Equilibrium { get; set; }
        public Matrix<double> TangentMatrix { get; set; }
        public Vector<double> IncrementDisplacementTangent { get; set; }
        public Vector<double> IncrementDisplacementEquilibrium { get; set; }
        public double IncrementLambda { get; set; }
        public Vector<double> IncrementDisplacement { get; set; }
        public bool Success { get; set; }
        public FailReason FailReason { get; set; }
    }
}