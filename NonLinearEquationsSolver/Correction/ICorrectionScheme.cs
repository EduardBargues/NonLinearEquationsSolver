using MathNet.Numerics.LinearAlgebra;
using NonLinearEquationsSolver.Common;

namespace NonLinearEquationsSolver.Correction {
    internal interface ICorrectionScheme {
        LoadIncrementalState GetCorrection( LoadState state,
            LoadIncrementalState prediction,
            StructureInfo info,
            Vector<double> dut,
            Vector<double> dur );
    }
}