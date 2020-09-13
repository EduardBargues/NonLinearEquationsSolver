using NLES.Contracts;
using System;

namespace NLES.Prediction
{
    internal class Predictor
    {
        internal IPredictionScheme Scheme { get; set; } = new PredictionSchemeStandard(0.1);

        internal LoadIncrementalState Predict(LoadState state, double initialStiffness, StructureInfo info)
        {
            Vector equilibrium = info.InitialLoad + state.Lambda * info.ReferenceLoad - info.Reaction(state.Displacement);
            ILinearSolver mK = info.Stiffness(state.Displacement);
            Vector Dvt = mK.Solve(info.ReferenceLoad);
            Vector Dvr = mK.Solve(equilibrium);
            double bergam = GetBergamParameter(initialStiffness, Dvt, info);
            double DLambda = Scheme.Predict(Dvt, info.ReferenceLoad) * Math.Sign(bergam);
            Vector Dv = DLambda * Dvt + Dvr;

            return new LoadIncrementalState(DLambda, Dv);
        }

        double GetBergamParameter(double k0, Vector Dvt, StructureInfo info)
            => Math.Abs(k0 / info.ReferenceLoad.DotProduct(Dvt));
    }
}
