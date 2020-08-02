using System;
using NLES.Correction;
using NLES.Prediction;

namespace NLES
{
    public partial class NonLinearSolver
    {
        public class SolverBuilderSchemeStandardNewtonRaphson : SolverBuilder
        {

            public SolverBuilderSchemeStandardNewtonRaphson(NonLinearSolver solver, double dlambda)
            {
                if (dlambda <= 0)
                {
                    throw new InvalidOperationException(Strings.LoadIncrementLargerThanZero);
                }

                Solver = solver;
                Solver.Predictor.Scheme = new PredictionSchemeStandard(dlambda);
                Solver.Corrector.Scheme = new CorrectionSchemeStandard();
            }
        }
    }
}
