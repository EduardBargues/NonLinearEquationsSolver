using MathNet.Numerics.LinearAlgebra;

namespace NLES.Prediction
{
    internal interface IPredictionScheme
    {
        double GetPrediction(Vector<double> Dvt, Vector<double> fr);
    }
}