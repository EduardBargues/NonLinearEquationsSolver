using System;
using NLES.Correction;
using NLES.Prediction;

namespace NLES
{
    public partial class Solver
    {
        public class SolverBuilderSchemeStandardNewtonRaphson : SolverBuilder
        {

            public SolverBuilderSchemeStandardNewtonRaphson(Solver solver, double dlambda)
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
