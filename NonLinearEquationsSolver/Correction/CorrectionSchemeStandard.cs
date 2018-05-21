using MathNet.Numerics.LinearAlgebra;
using NonLinearEquationsSolver.Common;

namespace NonLinearEquationsSolver.Correction {
    internal class CorrectionSchemeStandard : ICorrectionScheme {
        public LoadIncrementalState GetCorrection( LoadState state, LoadIncrementalState prediction, StructureInfo info, Vector<double> dut, Vector<double> dur ) =>
            new LoadIncrementalState ( 0, dur );
    }
}
