using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace NonLinearEquationsSolver
{
    public class PredictorInput
    {
        public IFunction Function { get; set; }
        public Vector<double> Force { get; set; }
        public IterationScheme Scheme { get; set; }
        public double LambdaIncrement { get; set; }
        public double Lambda { get; set; }
        public double LastLambda { get; set; }
        public Vector<double> Displacement { get; set; }
        public bool UseArcLength { get; set; }
        public double ReferenceStiffness { get; set; }
        public double ArcLengthRadius { get; set; }
        public double Beta { get; set; }
        public bool DoIterationReport { get; set; }
    }
}
