using System.Collections.Generic;
using System.Linq;

using MoreLinq;
using NLES.Contracts;

namespace NLES.Correction.Methods
{
    internal class AngleMethod : IDisplacementSelector
    {
        public LoadIncrementalState SelectDisplacement(
            StructureInfo info
            , LoadState state
            , LoadIncrementalState prediction
            , IEnumerable<LoadIncrementalState> candidates)
        {
            double Function(LoadIncrementalState candidate) =>
                prediction.IncrementDisplacement.DotProduct(candidate.IncrementDisplacement) /
                (prediction.IncrementDisplacement.Norm(2) * candidate.IncrementDisplacement.Norm(2));

            return candidates.MinBy(Function).First();
        }
    }
}