using MathNet.Numerics.LinearAlgebra;

namespace NonLinearEquationsSolver {
    internal class CorrectionSchemeStandard : ICorrectionScheme {
        public LoadIncrementalStateResult GetCorrection( LoadState state, LoadIncrementalState prediction, StructureInfo info, Vector<double> dut, Vector<double> dur ) =>
            new LoadIncrementalStateResult ( new LoadIncrementalState ( 0, dur ), true, "" );
    }
}
