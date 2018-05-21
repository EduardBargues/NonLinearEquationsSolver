using NonLinearEquationsSolver.Common;
using System.Collections.Generic;

namespace NonLinearEquationsSolver.Correction {
    internal interface IDisplacementChooser {
        LoadIncrementalState Choose( StructureInfo info,
                                    LoadState state,
                                    LoadIncrementalState prediction,
                                    IEnumerable<LoadIncrementalState> candidates );
    }
}
