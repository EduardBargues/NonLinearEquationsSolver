using MathNet.Numerics.LinearAlgebra;
using MoreLinq;
using System.Collections.Generic;
using System.Linq;

namespace NonLinearEquationsSolver {
    internal class RestoringMethod : IDisplacementChooser {
        public LoadIncrementalStateResult Choose( StructureInfo info, LoadState state, LoadIncrementalState prediction, IEnumerable<LoadIncrementalState> candidates ) {
            List<LoadIncrementalState> candidateList = candidates.ToList ( );

            if (candidateList.Any ( )) {
                double Function( LoadIncrementalState candidate ) {
                    Vector<double> displacement = state.Displacement + candidate.IncrementDisplacement;
                    Vector<double> reaction = info.Reaction ( displacement );
                    double lambda = state.Lambda + candidate.IncrementLambda;
                    Vector<double> equilibriumVector = info.InitialLoad + lambda * info.ReferenceLoad - reaction;
                    return equilibriumVector.Norm ( 2 );
                }

                LoadIncrementalState result = candidateList.MinBy ( Function ).First ( );
                return new LoadIncrementalStateResult ( result, true, "" );
            }
            return new LoadIncrementalStateResult ( null, false, "" );
        }
    }
}
