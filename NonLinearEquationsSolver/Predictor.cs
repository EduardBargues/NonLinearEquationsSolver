using MathNet.Numerics.LinearAlgebra;
using System;

namespace NonLinearEquationsSolver {
    /// <summary>
    /// Solve the structure in the prediction phase.
    /// </summary>
    internal class Predictor {
        /// <summary>
        /// Prediction scheme.
        /// </summary>
        internal IPredictionScheme Scheme { get; set; } = new PredictionSchemeStandard ( 0.1 );

        /// <summary>
        /// Predicts a LoadIncrementalState.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="initialStiffness"></param>
        /// <returns></returns>
        internal LoadIncrementalStateResult Predict( LoadState state, double initialStiffness, StructureInfo info ) {
            Vector<double> equilibrium = info.InitialLoad + state.Lambda * info.ReferenceLoad - info.Reaction ( state.Displacement );
            Matrix<double> mK = info.Stiffness ( state.Displacement );
            Vector<double> Dvt = mK.Solve ( info.ReferenceLoad );
            Vector<double> Dvr = mK.Solve ( equilibrium );
            double bergam = GetBergamParameter ( initialStiffness, Dvt, info );
            double DLambda = Scheme.GetPrediction ( Dvt, info.ReferenceLoad ) * Math.Sign ( bergam );
            Vector<double> Dv = DLambda * Dvt + Dvr;

            return new LoadIncrementalStateResult ( new LoadIncrementalState ( DLambda, Dv ), true, "" );
        }

        double GetBergamParameter( double k0, Vector<double> Dvt, StructureInfo info )
            => Math.Abs ( k0 / info.ReferenceLoad.DotProduct ( Dvt ) );
    }
}
