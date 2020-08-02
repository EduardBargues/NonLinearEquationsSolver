using MathNet.Numerics.LinearAlgebra;

namespace NLES.Prediction
{
    internal class PredictionSchemeStandard : IPredictionScheme
    {
        public double DLambda { get; set; }

        internal PredictionSchemeStandard(double dlambda) => DLambda = dlambda;

        public double Predict(Vector<double> Dvt, Vector<double> fr) => DLambda;
    }
}
