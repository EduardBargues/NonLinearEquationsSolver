using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using MoreLinq;

namespace NonLinearEquationsSolver
{
    public class RestoringMethod : IDisplacementChooser
    {
        public Tuple<double, Vector<double>> Choose(IMultiDimensionalFunction function,
            Vector<double> fr,
            Vector<double> displacementAfterPredictionPhase,
            double lambda,
            List<Tuple<double, Vector<double>>> candidates)
        {
            return candidates
                .MinBy(candidate => (lambda * fr - function.GetImage(displacementAfterPredictionPhase + candidate.Item2)).Norm(2));
        }
    }
}
