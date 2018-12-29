namespace NonLinearEquationsSolver {
    public partial class SolverND {
        public class SolverNdBuilderSchemeArcLength : SolverNdBuilder {

            public SolverNdBuilderSchemeArcLength( SolverND solverNd, double radius ) {
                SolverNd = solverNd;
                SolverNd.Predictor.Scheme = new PredictionSchemeArcLength ( radius );
                SolverNd.Corrector.Scheme = new CorrectionSchemeArcLength ( radius );
            }

            public SolverNdBuilderSchemeArcLength NormalizeLoadWith( double beta ) {
                PredictionSchemeArcLength arcLengthPredictionScheme = (PredictionSchemeArcLength)SolverNd.Predictor.Scheme;
                arcLengthPredictionScheme.Beta = beta;
                CorrectionSchemeArcLength arcLengthCorrectionScheme = (CorrectionSchemeArcLength)SolverNd.Corrector.Scheme;
                arcLengthCorrectionScheme.Beta = beta;
                return this;
            }
            public SolverNdBuilderSchemeArcLength WithRestoringMethodInCorrectionPhase() {
                CorrectionSchemeArcLength arcLengthCorrectionScheme = (CorrectionSchemeArcLength)SolverNd.Corrector.Scheme;
                arcLengthCorrectionScheme.DisplacementChooser = new RestoringMethod ( );
                return this;
            }
            public SolverNdBuilderSchemeArcLength WithAngleMethodInCorrectionPhase() {
                CorrectionSchemeArcLength arcLengthCorrectionScheme = (CorrectionSchemeArcLength)SolverNd.Corrector.Scheme;
                arcLengthCorrectionScheme.DisplacementChooser = new AngleMethod ( );
                return this;
            }
            SolverNdBuilderSchemeArcLength AllowRadiusToChange( int maximumNumberOfCorrections ) =>
                // TODO: allow arc length radius to change in both the prediction and correction phase.
                this;
        }
    }
}