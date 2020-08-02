using MathNet.Numerics.LinearAlgebra;

namespace NLES.Prediction
{
    internal interface IPredictionScheme
    {
        double Predict(Vector<double> Dvt, Vector<double> fr);
    }
}