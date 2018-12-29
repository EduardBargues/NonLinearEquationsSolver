namespace NonLinearEquationsSolver {
    public partial class SolverND {
        public class SolverNdBuilderStopCondition : SolverNdBuilder {
            public SolverNdBuilderStopCondition( SolverND solverNd, double displacementTolerance, double equilibriumTolerance, double energyTolerance ) {
                SolverNd = solverNd;
                SolverNd.Corrector.Tolerances = new[]
                    {displacementTolerance, equilibriumTolerance, energyTolerance};
            }
        }
    }
}