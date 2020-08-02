using MathNet.Numerics.LinearAlgebra;
using NLES.Contracts;

namespace NLES.Correction
{
    internal interface ICorrectionScheme
    {
        Result<LoadIncrementalState> Correct(LoadState state,
            LoadIncrementalState prediction,
            StructureInfo info,
            Vector<double> dut,
            Vector<double> dur);
    }
}