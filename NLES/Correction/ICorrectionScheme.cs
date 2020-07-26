using MathNet.Numerics.LinearAlgebra;
using NLES.Contracts;

namespace NLES.Correction
{
    internal interface ICorrectionScheme
    {
        Result<LoadIncrementalState> GetCorrection(LoadState state,
            LoadIncrementalState prediction,
            StructureInfo info,
            Vector<double> dut,
            Vector<double> dur);
    }
}