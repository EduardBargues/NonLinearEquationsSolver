using MathNet.Numerics.LinearAlgebra.Double;
using MoreLinq;
using NonLinearEquationsSolver.Common;
using System.Collections.Generic;
using System.Linq;

namespace NonLinearEquationsSolver.Correction {
    internal class AngleMethod : IDisplacementChooser {
        public LoadIncrementalState Choose( StructureInfo info, LoadState state, LoadIncrementalState prediction, IEnumerable<LoadIncrementalState> candidates ) {
            List<LoadIncrementalState> candidateList = candidates.ToList ( );

            if (candidateList.Any ( )) {
                double Function( LoadIncrementalState candidate ) =>
                    prediction.IncrementDisplacement.DotProduct ( candidate.IncrementDisplacement ) /
                    (prediction.IncrementDisplacement.Norm ( 2 ) * candidate.IncrementDisplacement.Norm ( 2 ));
                return candidateList.MinBy ( Function );
            } else {
                return new LoadIncrementalState ( 0, new DenseVector ( state.Displacement.Count ) );
            }
        }
    }
}