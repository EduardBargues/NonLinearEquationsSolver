using MathNet.Numerics.LinearAlgebra;

namespace NonLinearEquationsSolver {
    public partial class SolverND {
        public class SolverNdBuilderIncrementalLoad : SolverNdBuilder {

            public SolverNdBuilderIncrementalLoad( SolverND solverNd, Vector<double> referenceLoad ) {
                SolverNd = solverNd;
                SolverNd.Info.ReferenceLoad = referenceLoad;
            }

            public SolverNdBuilderIncrementalLoad WithInitialConditions( double lambda, Vector<double> load, Vector<double> displacement ) {
                SolverNd.Info.InitialLoad = load;
                SolverNd.State = new LoadState ( lambda, displacement );
                return this;
            }
        }
    }
}
