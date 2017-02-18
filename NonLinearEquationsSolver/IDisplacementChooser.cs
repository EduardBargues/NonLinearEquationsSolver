using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace NonLinearEquationsSolver
{
    public interface IDisplacementChooser
    {
        Tuple<double, Vector<double>> Choose(IMultiDimensionalFunction function,
            Vector<double> fr,
            Vector<double> displacementAfterPredictionPhase,
            double lambda,
            List<Tuple<double, Vector<double>>> candidates);
    }
}
