using MathNet.Numerics.LinearAlgebra;

namespace NonLinearEquationsSolver {
    internal interface ICorrectionScheme {
        LoadIncrementalStateResult GetCorrection( LoadState state,
            LoadIncrementalState prediction,
            StructureInfo info,
            Vector<double> dut,
            Vector<double> dur );
    }
}