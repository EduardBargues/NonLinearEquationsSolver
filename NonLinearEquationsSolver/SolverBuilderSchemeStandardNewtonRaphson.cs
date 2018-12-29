namespace NonLinearEquationsSolver {
    public partial class SolverND {
        public class SolverNdBuilderSchemeStandardNewtonRaphson : SolverNdBuilder {

            public SolverNdBuilderSchemeStandardNewtonRaphson( SolverND solverNd, double dlambda ) {
                SolverNd = solverNd;
                SolverNd.Predictor.Scheme = new PredictionSchemeStandard ( dlambda );
                SolverNd.Corrector.Scheme = new CorrectionSchemeStandard ( );
            }
        }
    }
}
