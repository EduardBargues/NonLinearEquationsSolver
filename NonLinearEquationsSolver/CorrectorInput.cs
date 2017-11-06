using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace NonLinearEquationsSolver
{
    public class CorrectorInput
    {
        public IFunction Function { get; set; }
        public Vector<double> Force { get; set; }
        public double PredictionPhaseLambda { get; set; }
        public Vector<double> PredictionPhaseDisplacement { get; set; }
        public double PredictionPhaseIncrementLambda { get; set; }
        public Vector<double> PredictionPhaseIncrementDisplacement { get; set; }
        public double Beta { get; set; }
        public bool UseArcLength { get; set; }
        public double ArcLengthRadius { get; set; }
        public int MaxArcLengthAdjustments { get; set; }
        public Tolerances Tolerances { get; set; }
        public int MaxIterations { get; set; }
        public bool DoIterationReport { get; set; }
    }
}
