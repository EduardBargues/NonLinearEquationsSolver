using MoreLinq;
using System.Collections.Generic;
using System.Linq;

namespace NonLinearEquationsSolver {
    internal class AngleMethod : IDisplacementChooser {
        public LoadIncrementalStateResult Choose( StructureInfo info, LoadState state, LoadIncrementalState prediction, IEnumerable<LoadIncrementalState> candidates ) {
            List<LoadIncrementalState> candidateList = candidates.ToList ( );

            if (candidateList.Any ( )) {
                double Function( LoadIncrementalState candidate ) =>
                    prediction.IncrementDisplacement.DotProduct ( candidate.IncrementDisplacement ) /
                    (prediction.IncrementDisplacement.Norm ( 2 ) * candidate.IncrementDisplacement.Norm ( 2 ));

                LoadIncrementalState result = candidateList.MinBy ( Function ).First ( );
                return new LoadIncrementalStateResult ( result, true, "" );
            }
            return new LoadIncrementalStateResult ( null, false, "" );
        }
    }
}