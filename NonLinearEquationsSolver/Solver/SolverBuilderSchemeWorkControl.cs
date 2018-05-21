using NonLinearEquationsSolver.Correction;
using NonLinearEquationsSolver.Prediction;

namespace NonLinearEquationsSolver.Solver {
    public partial class Solver {
        public class SolverBuilderSchemeWorkControl : SolverBuilder {

            public SolverBuilderSchemeWorkControl( Solver solver, double work ) {
                Solver = solver;
                Solver.Predictor.Scheme = new PredictionSchemeWorkControl ( work );
                Solver.Corrector.Scheme = new CorrectionSchemeWorkControl ( work );
            }

            public SolverBuilderSchemeWorkControl WithIncrementWorkControl( double workIncrement ) {
                PredictionSchemeWorkControl workControlPredictionScheme = (PredictionSchemeWorkControl)Solver.Predictor.Scheme;
                workControlPredictionScheme.WorkIncrement = workIncrement;
                CorrectionSchemeWorkControl workControlCorectionScheme = (CorrectionSchemeWorkControl)Solver.Corrector.Scheme;
                workControlCorectionScheme.WorkIncrement = workIncrement;
                return this;
            }
        }
    }
}