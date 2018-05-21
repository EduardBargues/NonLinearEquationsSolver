using MathNet.Numerics.LinearAlgebra;

namespace NonLinearEquationsSolver.Common {
    internal class LoadIncrementalState {
        /// <summary>
        /// Increment of load factor.
        /// </summary>
        internal double IncrementLambda { get; set; }
        /// <summary>
        /// Increment of displacement.
        /// </summary>
        internal Vector<double> IncrementDisplacement { get; set; }

        /// <summary>
        /// Creates a LoadIncrementalState.
        /// </summary>
        /// <param name="incrementLambda"></param>
        /// <param name="incrementDisplacement"></param>
        internal LoadIncrementalState( double incrementLambda, Vector<double> incrementDisplacement ) {
            IncrementLambda = incrementLambda;
            IncrementDisplacement = incrementDisplacement;
        }

        internal void Add( LoadIncrementalState inc ) {
            IncrementDisplacement += inc.IncrementDisplacement;
            IncrementLambda += inc.IncrementLambda;
        }
    }
}
