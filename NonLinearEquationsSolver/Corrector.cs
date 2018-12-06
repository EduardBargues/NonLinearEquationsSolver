using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NonLinearEquationsSolver {
    internal class Corrector {
        internal ICorrectionScheme Scheme { get; set; } = new CorrectionSchemeStandard ( );
        internal double[] Tolerances { get; set; } = { 1e-3, 1e-3, 1e-3 };
        internal int MaximumIterations { get; set; } = 50;

        internal LoadIncrementalStateResult Correct( LoadState state, LoadIncrementalState prediction, StructureInfo info ) {
            LoadIncrementalStateResult result = new LoadIncrementalStateResult ( null, false, "" );
            LoadState initialState = state;
            LoadState currentState = initialState;
            LoadIncrementalState predictionIncrement = prediction;
            for (int iteration = 1; iteration <= MaximumIterations; iteration++) {
                LoadIncrementalStateResult correction = GetCorrection ( currentState, predictionIncrement, info );
                predictionIncrement.Add ( correction.IncrementalState );
                LoadState newState = currentState.Add ( correction.IncrementalState );
                List<double> errors = GetErrors ( newState, currentState, info ).ToList ( );
                bool convergence = CheckConvergence ( errors, Tolerances );
                currentState = newState;
                if (convergence) {
                    break;
                }

                if (iteration >= MaximumIterations) {
                    result.Message = Strings.MaxNumberOfIterationsReached;
                    result.Success = false;
                }
            }

            result.IncrementalState = currentState.Substract ( initialState );

            return result;
        }

        IEnumerable<double> GetErrors( LoadState newState, LoadState oldState, StructureInfo info ) {
            Vector<double> reaction = info.Reaction ( newState.Displacement );
            Vector<double> equilibrium = info.InitialLoad + newState.Lambda * info.ReferenceLoad - reaction;
            Vector<double> incrementDisplacement = newState.Displacement - oldState.Displacement;

            yield return incrementDisplacement.Norm ( 2 ) / newState.Displacement.Norm ( 2 );
            yield return equilibrium.Norm ( 2 ) / info.ReferenceLoad.Norm ( 2 );
            yield return Math.Abs ( newState.Displacement.DotProduct ( equilibrium ) / newState.Displacement.DotProduct ( info.ReferenceLoad ) );
        }

        bool CheckConvergence( IEnumerable<double> errors, IReadOnlyList<double> tolerances ) =>
            errors
                .Select ( ( error, index ) => new { Error = error, Tolerance = tolerances[index] } )
                .All ( couple => couple.Error <= couple.Tolerance );

        LoadIncrementalStateResult GetCorrection( LoadState state, LoadIncrementalState prediction, StructureInfo info ) {
            Matrix<double> stiffnessMatrix = info.Stiffness ( state.Displacement );
            Vector<double> dut = stiffnessMatrix.Solve ( info.ReferenceLoad );
            Vector<double> reaction = info.Reaction ( state.Displacement );
            Vector<double> equilibrium = info.InitialLoad + state.Lambda * info.ReferenceLoad - reaction;
            Vector<double> dur = stiffnessMatrix.Solve ( equilibrium );
            LoadIncrementalStateResult incrementalStateResult = Scheme.GetCorrection ( state, prediction, info, dut, dur );

            return incrementalStateResult;
        }
    }
}
