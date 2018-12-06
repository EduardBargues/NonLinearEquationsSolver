namespace NonLinearEquationsSolver {
    public partial class Solver {
        public class SolverBuilderSchemeStandardNewtonRaphson : SolverBuilder {

            public SolverBuilderSchemeStandardNewtonRaphson( Solver solver, double dlambda ) {
                Solver = solver;
                Solver.Predictor.Scheme = new PredictionSchemeStandard ( dlambda );
                Solver.Corrector.Scheme = new CorrectionSchemeStandard ( );
            }
        }
    }
}
