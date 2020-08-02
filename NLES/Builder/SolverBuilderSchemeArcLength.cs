using System;
using NLES.Correction;
using NLES.Correction.Methods;
using NLES.Prediction;

namespace NLES
{
    public partial class NonLinearSolver
    {
        public class SolverBuilderSchemeArcLength : SolverBuilder
        {

            public SolverBuilderSchemeArcLength(NonLinearSolver solver, double radius)
            {
                if (radius <= 0)
                {
                    throw new InvalidOperationException(Strings.ArcLengthRadiusLargerThanZero);
                }

                Solver = solver;
                Solver.Predictor.Scheme = new PredictionSchemeArcLength(radius);
                Solver.Corrector.Scheme = new CorrectionSchemeArcLength(radius);
            }

            public SolverBuilderSchemeArcLength NormalizeLoadWith(double beta)
            {
                PredictionSchemeArcLength arcLengthPredictionScheme = (PredictionSchemeArcLength)Solver.Predictor.Scheme;
                arcLengthPredictionScheme.Beta = beta;
                CorrectionSchemeArcLength arcLengthCorrectionScheme = (CorrectionSchemeArcLength)Solver.Corrector.Scheme;
                arcLengthCorrectionScheme.Beta = beta;
                return this;
            }
            public SolverBuilderSchemeArcLength WithRestoringMethodInCorrectionPhase()
            {
                CorrectionSchemeArcLength arcLengthCorrectionScheme = (CorrectionSchemeArcLength)Solver.Corrector.Scheme;
                arcLengthCorrectionScheme.DisplacementSelector = new RestoringMethod();
                return this;
            }
            public SolverBuilderSchemeArcLength WithAngleMethodInCorrectionPhase()
            {
                CorrectionSchemeArcLength arcLengthCorrectionScheme = (CorrectionSchemeArcLength)Solver.Corrector.Scheme;
                arcLengthCorrectionScheme.DisplacementSelector = new AngleMethod();
                return this;
            }
        }
    }
}