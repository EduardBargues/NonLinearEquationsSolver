using NLES.Contracts;

namespace NLES.Correction
{
    internal interface ICorrectionScheme
    {
        Result<LoadIncrementalState> Correct(LoadState state,
            LoadIncrementalState prediction,
            StructureInfo info,
            Vector dut,
            Vector dur);
    }
}