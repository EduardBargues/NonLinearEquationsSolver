using MathNet.Numerics.LinearAlgebra;

namespace NonLinearEquationsSolver {
    internal class PredictionSchemeStandard : IPredictionScheme {
        /// <inheritdoc />
        public double DLambda { get; set; }

        internal PredictionSchemeStandard( double dlambda ) => DLambda = dlambda;

        public double GetPrediction( Vector<double> Dvt, Vector<double> fr ) => DLambda;
    }
}
