using System.Collections.Generic;
using NLES.Contracts;

namespace NLES.Correction.Methods
{
    internal interface IDisplacementSelector
    {
        LoadIncrementalState SelectDisplacement(StructureInfo info,
                                    LoadState state,
                                    LoadIncrementalState prediction,
                                    IEnumerable<LoadIncrementalState> candidates);
    }
}
