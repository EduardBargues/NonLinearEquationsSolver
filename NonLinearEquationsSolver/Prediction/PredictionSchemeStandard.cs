using MathNet.Numerics.LinearAlgebra;

namespace NonLinearEquationsSolver.Prediction {
    internal class PredictionSchemeStandard : IPredictionScheme {

        /// <summary>
        /// Default increment of lambda.
        /// </summary>
        internal double DLambda { get; set; }

        internal PredictionSchemeStandard( double dlambda ) {
            DLambda = dlambda;
        }

        public double GetPrediction( Vector<double> Dvt, Vector<double> fr ) => DLambda;
    }
}
