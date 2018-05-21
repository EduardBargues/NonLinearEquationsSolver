using MathNet.Numerics.LinearAlgebra;
using System;

namespace NonLinearEquationsSolver.Common {
    internal class StructureInfo {
        /// <summary>
        /// Reaction function.
        /// </summary>
        internal Func<Vector<double>, Vector<double>> Reaction { get; set; }
        /// <summary>
        /// Reference load.
        /// </summary>
        internal Vector<double> ReferenceLoad { get; set; }
        /// <summary>
        /// Initial load.
        /// </summary>
        internal Vector<double> InitialLoad { get; set; }
        /// <summary>
        /// Stiffness function.
        /// </summary>
        internal Func<Vector<double>, Matrix<double>> Stiffness { get; set; }
    }
}
