using System;
using MathNet.Numerics.LinearAlgebra;

namespace NonLinearEquationsSolver {
    internal class PredictionSchemeArcLength : IPredictionScheme {
        /// <summary>
        /// Arc length radius
        /// </summary>
        public double Radius { get; set; }

        /// <summary>
        /// Normalization factor between forces and displacements.
        /// </summary>
        public double Beta { get; set; } = 1;

        internal PredictionSchemeArcLength( double radius ) {
            Radius = radius;
        }

        public double GetPrediction( Vector<double> Dvt, Vector<double> fr ) {
            double dispDotProduct = Dvt.DotProduct ( Dvt );
            double forceDotProduct = fr.DotProduct ( fr );
            double deltaLambda = Radius / Math.Sqrt ( dispDotProduct + Math.Pow ( Beta, 2 ) * forceDotProduct );

            return deltaLambda;
        }
    }
}
