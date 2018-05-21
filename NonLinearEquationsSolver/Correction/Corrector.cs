using MathNet.Numerics.LinearAlgebra;
using NonLinearEquationsSolver.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NonLinearEquationsSolver.Correction {
    internal class Corrector {
        internal ICorrectionScheme Scheme { get; set; }
        internal double[] Tolerances { get; set; }
        internal int MaximumIterations { get; set; }

        internal LoadIncrementalState Correct( LoadState state, LoadIncrementalState prediction, StructureInfo info ) {
            LoadState initialState = state;
            LoadState currentState = initialState;
            LoadIncrementalState predictionIncrement = prediction;
            for (int iteration = 1; iteration <= MaximumIterations; iteration++) {
                LoadIncrementalState correction = GetCorrection ( state, predictionIncrement, info );
                predictionIncrement.Add ( correction );
                LoadState newState = currentState.Add ( correction );
                Vector<double> reaction = info.Reaction ( newState.Displacement );
                IEnumerable<double> errors = GetErrors ( newState, reaction, correction.IncrementDisplacement, info );
                bool convergence = CheckConvergence ( errors, Tolerances );
                currentState = newState;
                if (convergence)
                    break;
                if (iteration >= MaximumIterations) {
                    throw new Exception ( "Maximum number of iterations reached in correction phase." );
                }
            }
            return state.Substract ( initialState );
        }

        IEnumerable<double> GetErrors( LoadState state,
            Vector<double> reaction,
            Vector<double> incrementDisplacement,
            StructureInfo info ) {
            Vector<double> equilibrium = info.InitialLoad + state.Lambda * info.ReferenceLoad - reaction;
            yield return incrementDisplacement.Norm ( 2 ) / state.Displacement.Norm ( 2 );
            yield return equilibrium.Norm ( 2 ) / info.ReferenceLoad.Norm ( 2 );
            yield return Math.Abs ( state.Displacement.DotProduct ( equilibrium ) / state.Displacement.DotProduct ( info.ReferenceLoad ) );
        }

        bool CheckConvergence( IEnumerable<double> errors, IReadOnlyList<double> tolerances ) =>
            errors
                .Select ( ( error, index ) => new { Error = error, Tolerance = tolerances[index] } )
                .All ( couple => couple.Error <= couple.Tolerance );

        LoadIncrementalState GetCorrection( LoadState state, LoadIncrementalState prediction, StructureInfo info ) {
            Matrix<double> stiffnessMatrix = info.Stiffness ( state.Displacement );
            Vector<double> dut = stiffnessMatrix.Solve ( info.ReferenceLoad );
            Vector<double> reaction = info.Reaction ( state.Displacement );
            Vector<double> equilibrium = info.InitialLoad + state.Lambda * info.ReferenceLoad - reaction;
            Vector<double> dur = stiffnessMatrix.Solve ( equilibrium );

            LoadIncrementalState incrementalState = Scheme.GetCorrection ( state, prediction, info, dut, dur );

            return incrementalState;
        }
    }
}
