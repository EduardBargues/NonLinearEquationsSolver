namespace NonLinearEquationsSolver {
    public partial class Solver {
        public class SolverBuilderSchemeArcLength : SolverBuilder {

            public SolverBuilderSchemeArcLength( Solver solver, double radius ) {
                Solver = solver;
                Solver.Predictor.Scheme = new PredictionSchemeArcLength ( radius );
                Solver.Corrector.Scheme = new CorrectionSchemeArcLength ( radius );
            }

            public SolverBuilderSchemeArcLength NormalizeLoadWith( double beta ) {
                PredictionSchemeArcLength arcLengthPredictionScheme = (PredictionSchemeArcLength)Solver.Predictor.Scheme;
                arcLengthPredictionScheme.Beta = beta;
                CorrectionSchemeArcLength arcLengthCorrectionScheme = (CorrectionSchemeArcLength)Solver.Corrector.Scheme;
                arcLengthCorrectionScheme.Beta = beta;
                return this;
            }
            public SolverBuilderSchemeArcLength WithRestoringMethodInCorrectionPhase() {
                CorrectionSchemeArcLength arcLengthCorrectionScheme = (CorrectionSchemeArcLength)Solver.Corrector.Scheme;
                arcLengthCorrectionScheme.DisplacementChooser = new RestoringMethod ( );
                return this;
            }
            public SolverBuilderSchemeArcLength WithAngleMethodInCorrectionPhase() {
                CorrectionSchemeArcLength arcLengthCorrectionScheme = (CorrectionSchemeArcLength)Solver.Corrector.Scheme;
                arcLengthCorrectionScheme.DisplacementChooser = new AngleMethod ( );
                return this;
            }
            SolverBuilderSchemeArcLength AllowRadiusToChange( int maximumNumberOfCorrections ) =>
                // TODO: allow arc length radius to change in both the prediction and correction phase.
                this;
        }
    }
}