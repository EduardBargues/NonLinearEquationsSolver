namespace NonLinearEquationsSolver {
    public partial class SolverND {
        public class SolverNdBuilderSchemeWorkControl : SolverNdBuilder {

            public SolverNdBuilderSchemeWorkControl( SolverND solverNd, double work ) {
                SolverNd = solverNd;
                SolverNd.Predictor.Scheme = new PredictionSchemeWorkControl ( work );
                SolverNd.Corrector.Scheme = new CorrectionSchemeWorkControl ( work );
            }

            public SolverNdBuilderSchemeWorkControl WithIncrementWorkControl( double workIncrement ) {
                PredictionSchemeWorkControl workControlPredictionScheme = (PredictionSchemeWorkControl)SolverNd.Predictor.Scheme;
                workControlPredictionScheme.WorkIncrement = workIncrement;
                CorrectionSchemeWorkControl workControlCorectionScheme = (CorrectionSchemeWorkControl)SolverNd.Corrector.Scheme;
                workControlCorectionScheme.WorkIncrement = workIncrement;
                return this;
            }
        }
    }
}