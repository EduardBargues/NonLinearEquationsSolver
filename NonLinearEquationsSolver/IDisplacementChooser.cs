using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace NonLinearEquationsSolver
{
    public interface IDisplacementChooser
    {
        IterationPhaseOutput Choose(IFunction function,
                                             Vector<double> fr,
                                             Vector<double> displacementAfterPredictionPhase,
                                             double lambda,
                                             List<IterationPhaseOutput> candidates);
    }
}
