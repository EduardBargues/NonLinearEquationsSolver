using NLES.Contracts;

namespace NLES
{
    public partial class NonLinearSolver
    {
        public class SolverBuilderIncrementalLoad : SolverBuilder
        {
            public SolverBuilderIncrementalLoad(NonLinearSolver solver, Vector referenceLoad)
            {
                Solver = solver;
                Solver.Info.ReferenceLoad = referenceLoad;
            }

            public SolverBuilderIncrementalLoad WithInitialConditions(double lambda, Vector load, Vector displacement)
            {
                Solver.Info.InitialLoad = load;
                Solver.State = new LoadState(lambda, displacement);
                return this;
            }
        }
    }
}
