using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex;

namespace NonLinearEquationsSolver
{
    public class ProblemDefinition
    {
        public IFunction Function { get; set; }
        public Vector<double> Force { get; set; }
        public double FirstLambdaValue { get; set; }
        public double LastLambdaValue { get; set; }

        public double Beta { get; set; }

        public int MaxIterations { get; set; }
        public int MaxIncrements { get; set; }

        public ErrorTolerancesInfo Tolerances { get; set; }
        public Vector<double> InitialApproximation { get; set; }

        public bool UseArcLength { get; set; }
        public double ArcLengthRadius { get; set; }
        public bool ChangeArcLengthRadius { get; set; }
        public int MaxArcLengthAdjustments { get; set; }
        public bool DoIterationReport { get; set; }
        public IterationScheme Scheme { get; set; }
    }
}
