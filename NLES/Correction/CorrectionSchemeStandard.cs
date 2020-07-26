using MathNet.Numerics.LinearAlgebra;

using NLES.Contracts;

namespace NLES.Correction
{
    internal class CorrectionSchemeStandard : ICorrectionScheme
    {
        public Result<LoadIncrementalState> GetCorrection(
            LoadState state
            , LoadIncrementalState prediction
            , StructureInfo info
            , Vector<double> dut
            , Vector<double> dur) => new Result<LoadIncrementalState>()
            {
                Value = new LoadIncrementalState(0, dur)
            };
    }
}
