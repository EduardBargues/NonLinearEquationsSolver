using NLES.Contracts;

namespace NLES.Correction
{
    internal class CorrectionSchemeStandard : ICorrectionScheme
    {
        public Result<LoadIncrementalState> Correct(
            LoadState state
            , LoadIncrementalState prediction
            , StructureInfo info
            , Vector dut
            , Vector dur) => new Result<LoadIncrementalState>()
            {
                Value = new LoadIncrementalState(0, dur)
            };
    }
}
