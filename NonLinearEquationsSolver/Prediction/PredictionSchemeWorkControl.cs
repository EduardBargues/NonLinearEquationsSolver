using MathNet.Numerics.LinearAlgebra;
using System;

namespace NonLinearEquationsSolver.Prediction {
    internal class PredictionSchemeWorkControl : IPredictionScheme {

        /// <summary>
        /// The increment of work allowed by the external forces in the prediction phase.
        /// </summary>
        internal double WorkIncrement { get; set; }

        public PredictionSchemeWorkControl( double work ) => this.WorkIncrement = work;

        public double GetPrediction( Vector<double> Dvt, Vector<double> fr ) =>
            Math.Sqrt ( WorkIncrement / fr.DotProduct ( Dvt ) );
    }
}
