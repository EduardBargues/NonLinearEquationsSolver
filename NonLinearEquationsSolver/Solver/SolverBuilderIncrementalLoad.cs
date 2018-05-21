using MathNet.Numerics.LinearAlgebra;

namespace NonLinearEquationsSolver.Solver {
    public partial class Solver {
        public class SolverBuilderIncrementalLoad : SolverBuilder {

            public SolverBuilderIncrementalLoad( Solver solver, Vector<double> referenceLoad ) {
                Solver = solver;
                Solver.Info.ReferenceLoad = referenceLoad;
            }

            public SolverBuilderIncrementalLoad WithInitialLoad( Vector<double> initialLoad ) {
                Solver.Info.InitialLoad = initialLoad;
                return this;
            }
            public SolverBuilderIncrementalLoad WithInitialLoadFactor( double initialLambda ) {
                Solver.State.Lambda = initialLambda;
                return this;
            }
            public SolverBuilderIncrementalLoad WithInitialDisplacement( Vector<double> initialDisplacement ) {
                Solver.State.Displacement = initialDisplacement;
                return this;
            }
        }
    }
}
