using MathNet.Numerics.LinearAlgebra;

namespace NonLinearEquationsSolver.Prediction {
    internal interface IPredictionScheme {
        /// <summary>
        /// Get lambda increment prediction based on the tangent displacement increment and the reference load.
        /// </summary>
        /// <param name="Dvt"></param>
        /// <param name="fr"></param>
        /// <returns></returns>
        double GetPrediction( Vector<double> Dvt, Vector<double> fr );
    }
}