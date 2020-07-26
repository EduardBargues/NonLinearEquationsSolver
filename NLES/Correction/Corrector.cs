using System;
using System.Collections.Generic;
using System.Linq;

using MathNet.Numerics.LinearAlgebra;

using NLES.Contracts;

namespace NLES.Correction
{
    internal class Corrector
    {
        internal ICorrectionScheme Scheme { get; set; } = new CorrectionSchemeStandard();
        internal double[] Tolerances { get; set; } = { 1e-3, 1e-3, 1e-3 };
        internal int MaximumIterations { get; set; } = 50;

        internal Result<LoadIncrementalState> Correct(
            LoadState state
            , LoadIncrementalState prediction
            , StructureInfo info)
        {
            Result<LoadIncrementalState> result = new Result<LoadIncrementalState>();

            LoadState initialState = state;
            LoadState currentState = initialState;
            LoadIncrementalState predictionIncrement = prediction;
            for (int iteration = 1; iteration <= MaximumIterations; iteration++)
            {
                Result<LoadIncrementalState> correction = GetCorrection(currentState, predictionIncrement, info);
                if (correction.IsSuccess)
                {
                    predictionIncrement.Add(correction.Value);
                    LoadState newState = currentState.Add(correction.Value);
                    List<double> errors = GetErrors(newState, currentState, info).ToList();
                    bool convergence = CheckConvergence(errors, Tolerances);
                    currentState = newState;
                    if (convergence)
                    {
                        result.Value = currentState.Substract(initialState);
                        break;
                    }
                }
                else
                {
                    result.Errors.AddRange(correction.Errors);
                }

                if (iteration >= MaximumIterations)
                {
                    result.Errors.Add(new Error(Strings.MaxNumberOfIterationsReached));
                }
            }

            return result;
        }

        IEnumerable<double> GetErrors(LoadState newState, LoadState oldState, StructureInfo info)
        {
            Vector<double> reaction = info.Reaction(newState.Displacement);
            Vector<double> equilibrium = info.InitialLoad + newState.Lambda * info.ReferenceLoad - reaction;
            Vector<double> incrementDisplacement = newState.Displacement - oldState.Displacement;

            yield return incrementDisplacement.Norm(2) / newState.Displacement.Norm(2);
            yield return equilibrium.Norm(2) / info.ReferenceLoad.Norm(2);
            yield return Math.Abs(newState.Displacement.DotProduct(equilibrium) / newState.Displacement.DotProduct(info.ReferenceLoad));
        }

        bool CheckConvergence(IEnumerable<double> errors, IReadOnlyList<double> tolerances) =>
            errors
                .Select((error, index) => new { Error = error, Tolerance = tolerances[index] })
                .All(couple => couple.Error <= couple.Tolerance);

        Result<LoadIncrementalState> GetCorrection(LoadState state, LoadIncrementalState prediction, StructureInfo info)
        {
            Matrix<double> stiffnessMatrix = info.Stiffness(state.Displacement);
            Vector<double> dut = stiffnessMatrix.Solve(info.ReferenceLoad);
            Vector<double> reaction = info.Reaction(state.Displacement);
            Vector<double> equilibrium = info.InitialLoad + state.Lambda * info.ReferenceLoad - reaction;
            Vector<double> dur = stiffnessMatrix.Solve(equilibrium);
            Result<LoadIncrementalState> incrementalStateResult = Scheme.GetCorrection(state, prediction, info, dut, dur);

            return incrementalStateResult;
        }
    }
}
