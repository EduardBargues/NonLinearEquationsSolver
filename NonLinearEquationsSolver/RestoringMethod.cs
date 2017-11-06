using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using MoreLinq;

namespace NonLinearEquationsSolver
{
    public class RestoringMethod : IDisplacementChooser
    {
        public IterationPhaseReport Choose(IFunction function,
            Vector<double> fr,
            Vector<double> displacementAfterPredictionPhase,
            double lambda,
            List<IterationPhaseReport> candidates)
        {
            return candidates.Any()
                ? candidates.MinBy(candidate =>
                    {
                        Vector<double> image =
                            function.GetImage(displacementAfterPredictionPhase + candidate.IncrementDisplacement);
                        Vector<double> equilibriumVector = lambda * fr - image;
                        return equilibriumVector.Norm(2);
                    })
                : null;
        }
    }
}
