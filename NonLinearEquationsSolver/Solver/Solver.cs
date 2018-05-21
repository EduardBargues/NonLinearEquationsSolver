using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using NonLinearEquationsSolver.Common;
using NonLinearEquationsSolver.Correction;
using NonLinearEquationsSolver.Prediction;
using System.Collections.Generic;

namespace NonLinearEquationsSolver.Solver {
    public partial class Solver {

        internal LoadState State { get; set; } = new LoadState ( 0, new DenseVector ( 1 ) );
        internal Predictor Predictor { get; set; } = new Predictor ( );
        internal Corrector Corrector { get; set; } = new Corrector ( );
        internal StructureInfo Info { get; set; } = new StructureInfo ( );

        /// <summary>
        /// Builder to construct a solver.
        /// </summary>
        public static SolverBuilder Builder => new SolverBuilder ( );

        /// <summary>
        /// Broadcasts every LoadState the solver reaches in the equilibrium path.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LoadState> Broadcast() {
            yield return State;

            Matrix<double> mK0 = Info.Stiffness ( State.Displacement );
            Vector<double> Dv0 = mK0.Solve ( Info.ReferenceLoad );
            double k0 = Info.ReferenceLoad.DotProduct ( Dv0 );

            while (true) {
                LoadIncrementalState prediction = Predictor.Predict ( State, k0, Info );
                State = State.Add ( prediction );
                LoadIncrementalState correction = Corrector.Correct ( State, prediction, Info );
                State = State.Add ( correction );
                yield return State;
            }
        }
    }
}
