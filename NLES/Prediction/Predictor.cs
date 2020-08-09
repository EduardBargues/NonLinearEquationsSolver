using MathNet.Numerics.LinearAlgebra;
using NLES.Contracts;
using System;

namespace NLES.Prediction
{
    internal class Predictor
    {
        internal IPredictionScheme Scheme { get; set; } = new PredictionSchemeStandard(0.1);

        internal LoadIncrementalState Predict(LoadState state, double initialStiffness, StructureInfo info)
        {
            Vector<double> equilibrium = info.InitialLoad + state.Lambda * info.ReferenceLoad - info.Reaction(state.Displacement);
            ILinearSolver mK = info.Stiffness(state.Displacement);
            Vector<double> Dvt = mK.Solve(info.ReferenceLoad);
            Vector<double> Dvr = mK.Solve(equilibrium);
            double bergam = GetBergamParameter(initialStiffness, Dvt, info);
            double DLambda = Scheme.Predict(Dvt, info.ReferenceLoad) * Math.Sign(bergam);
            Vector<double> Dv = DLambda * Dvt + Dvr;

            return new LoadIncrementalState(DLambda, Dv);
        }

        double GetBergamParameter(double k0, Vector<double> Dvt, StructureInfo info)
            => Math.Abs(k0 / info.ReferenceLoad.DotProduct(Dvt));
    }
}
