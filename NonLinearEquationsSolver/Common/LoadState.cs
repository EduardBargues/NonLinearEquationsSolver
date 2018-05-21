using MathNet.Numerics.LinearAlgebra;

namespace NonLinearEquationsSolver.Common {
    public class LoadState {
        /// <summary>
        /// Load factor.
        /// </summary>
        public double Lambda { get; set; }
        /// <summary>
        /// Displacement.
        /// </summary>
        public Vector<double> Displacement { get; set; }

        /// <summary>
        /// Constructs a LoadState.
        /// </summary>
        /// <param name="lambda"></param>
        /// <param name="displacement"></param>
        internal LoadState( double lambda, Vector<double> displacement ) {
            Lambda = lambda;
            Displacement = displacement;
        }
        /// <summary>
        /// Adds a LoadIncrementalState to the current LoadState and returns a new reference.
        /// </summary>
        /// <param name="increment"></param>
        /// <returns></returns>
        internal LoadState Add( LoadIncrementalState increment ) =>
            new LoadState ( Lambda + increment.IncrementLambda,
                            Displacement + increment.IncrementDisplacement );
        /// <summary>
        /// Obtain the LoadIncrementalState between two states.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        internal LoadIncrementalState Substract( LoadState other ) =>
            new LoadIncrementalState ( Lambda - other.Lambda, Displacement - other.Displacement );
    }
}
