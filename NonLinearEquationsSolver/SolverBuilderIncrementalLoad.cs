using MathNet.Numerics.LinearAlgebra;

namespace NonLinearEquationsSolver {
    public partial class Solver {
        public class SolverBuilderIncrementalLoad : SolverBuilder {

            public SolverBuilderIncrementalLoad( Solver solver, Vector<double> referenceLoad ) {
                Solver = solver;
                Solver.Info.ReferenceLoad = referenceLoad;
            }

            public SolverBuilderIncrementalLoad WithInitialConditions( double lambda, Vector<double> load, Vector<double> displacement ) {
                Solver.Info.InitialLoad = load;
                Solver.State = new LoadState ( lambda, displacement );
                return this;
            }
        }
    }
}
