using System.Collections.Generic;

namespace NonLinearEquationsSolver {
    internal interface IDisplacementChooser {
        LoadIncrementalStateResult Choose( StructureInfo info,
                                    LoadState state,
                                    LoadIncrementalState prediction,
                                    IEnumerable<LoadIncrementalState> candidates );
    }
}
