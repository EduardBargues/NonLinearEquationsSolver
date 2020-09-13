
namespace NLES.Prediction
{
    internal interface IPredictionScheme
    {
        double Predict(Vector Dvt, Vector fr);
    }
}