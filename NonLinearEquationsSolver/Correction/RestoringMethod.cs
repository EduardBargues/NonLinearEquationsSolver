using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MoreLinq;
using NonLinearEquationsSolver.Common;
using System.Collections.Generic;
using System.Linq;

namespace NonLinearEquationsSolver.Correction {
    internal class RestoringMethod : IDisplacementChooser {
        public LoadIncrementalState Choose( StructureInfo info, LoadState state, LoadIncrementalState prediction, IEnumerable<LoadIncrementalState> candidates ) {
            List<LoadIncrementalState> candidateList = candidates.ToList ( );

            if (candidateList.Any ( )) {
                double Function( LoadIncrementalState candidate ) {
                    Vector<double> displacement = state.Displacement + candidate.IncrementDisplacement;
                    Vector<double> reaction = info.Reaction ( displacement );
                    double lambda = state.Lambda + candidate.IncrementLambda;
                    Vector<double> equilibriumVector = info.InitialLoad + lambda * info.ReferenceLoad - reaction;
                    return equilibriumVector.Norm ( 2 );
                }

                return candidateList.MinBy ( Function );
            } else {
                return new LoadIncrementalState ( 0, new DenseVector ( state.Displacement.Count ( ) ) );
            }
        }
    }
}
