﻿using System;
using MathNet.Numerics.LinearAlgebra;

namespace NLES.Prediction
{
    internal class PredictionSchemeArcLength : IPredictionScheme
    {
        public double Radius { get; set; }

        public double Beta { get; set; } = 1;

        internal PredictionSchemeArcLength(double radius) => Radius = radius;

        public double Predict(Vector<double> Dvt, Vector<double> fr)
        {
            double dispDotProduct = Dvt.DotProduct(Dvt);
            double forceDotProduct = fr.DotProduct(fr);
            double deltaLambda = Radius / Math.Sqrt(dispDotProduct + Math.Pow(Beta, 2) * forceDotProduct);

            return deltaLambda;
        }
    }
}
