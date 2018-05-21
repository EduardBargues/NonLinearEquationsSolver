namespace NonLinearEquationsSolver.Solver {
    public partial class Solver {
        public class SolverBuilderStopCondition : SolverBuilder {
            public SolverBuilderStopCondition( Solver solver, double displacementTolerance, double equilibriumTolerance, double energyTolerance ) {
                Solver = solver;
                Solver.Corrector.Tolerances = new[]
                    {displacementTolerance, equilibriumTolerance, energyTolerance};
            }
        }
    }
}