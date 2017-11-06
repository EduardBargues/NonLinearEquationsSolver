using MathNet.Numerics.LinearAlgebra;

namespace NonLinearEquationsSolver
{
    public class IterationReport
    {
        public IterationPhaseType Type { get; set; }
        public Vector<double> Reaction { get; set; }
        public double Lambda { get; set; }
        public double IncrementLambda { get; set; }
        public Vector<double> Equilibrium { get; set; }
        public Matrix<double> TangentMatrix { get; set; }
        public Vector<double> IncrementDisplacementTangent { get; set; }
        public Vector<double> IncrementDisplacementEquilibrium { get; set; }
        public double BergamParameter { get; set; }
        public bool Convergence { get; set; }
        public NonConvergenceReason Reason { get; set; }
    }
}