using MathNet.Numerics.LinearAlgebra;
using System.Collections.Generic;

namespace NonLinearEquationsSolver {
    public partial class Solver {
        internal LoadState State { get; set; }
        internal Predictor Predictor { get; set; } = new Predictor ( );
        internal Corrector Corrector { get; set; } = new Corrector ( );
        internal StructureInfo Info { get; set; }

        /// <summary>
        /// Builder to construct a solver.
        /// </summary>
        public static SolverBuilder Builder => new SolverBuilder ( );

        /// <summary>
        /// Broadcasts every LoadState the solver reaches in the equilibrium path.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LoadState> Broadcast() {
            Matrix<double> mK0 = Info.Stiffness ( State.Displacement );
            Vector<double> Dv0 = mK0.Solve ( Info.ReferenceLoad );
            double k0 = Info.ReferenceLoad.DotProduct ( Dv0 );
            while (true) {
                LoadIncrementalStateResult prediction = Predictor.Predict ( State, k0, Info );
                State = State.Add ( prediction.IncrementalState );
                LoadIncrementalStateResult correction = Corrector.Correct ( State, prediction.IncrementalState, Info );
                State = State.Add ( correction.IncrementalState );
                yield return State;
            }
        }
    }
}
