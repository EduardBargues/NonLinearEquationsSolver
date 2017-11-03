using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace NonLinearEquationsSolver
{
    public class Solver : ISolver
    {
        public IterationReport Solve(SolverInputs props, out Vector<double> solution)
        {
            solution = null;
            IterationReport report = null;

            Vector<double> displacement = props.InitialApproximation;
            Matrix<double> firstStiffnessMatrix = props.Function.GetTangentMatrix(displacement);
            Vector<double> firstSolution = firstStiffnessMatrix.Solve(props.Force);

            Predictor predictor = new Predictor();
            PredictorInput predictorInput = GetPredictorInput(props, firstSolution);

            Corrector corrector = new Corrector();
            CorrectorInput correctorInput = GetCorrectorInput(props);
            double lambda = props.FirstLambdaValue;

            for (int increment = 0; increment < props.MaxIncrements; increment++)
            {
                // PREDICTION PHASE
                predictorInput.Displacement = displacement;
                predictorInput.Lambda = lambda;
                IterationPhaseOutput incPrediction = predictor.Predict(predictorInput);
                displacement += incPrediction.IncrementDisplacement;
                lambda += incPrediction.IncrementLambda;

                // CORRECTION PHASE
                correctorInput.PredictionPhaseLambda = lambda;
                correctorInput.PredictionPhaseDisplacement = displacement;
                correctorInput.PredictionPhaseIncrementLambda = incPrediction.IncrementLambda;
                correctorInput.PredictionPhaseIncrementDisplacement = incPrediction.IncrementDisplacement;

                IterationPhaseOutput incCorrection;
                var correctionReport = corrector.Correct(correctorInput, out incCorrection);
                if (correctionReport.Convergence)
                {
                    displacement += incCorrection.IncrementDisplacement;
                    lambda += incCorrection.IncrementLambda;
                    // CHECK CONVERGENCE
                    bool convergence = Math.Abs(lambda - props.LastLambdaValue) <= props.Tolerances.IncrementalForce;
                    if (convergence)
                    {
                        solution = displacement;
                        report = new IterationReport(true, NonConvergenceReason.None);
                        break;
                    }
                    bool maxIncrementsReached = increment <= props.MaxIncrements;
                    if (maxIncrementsReached)
                    {
                        report = new IterationReport(convergence: false,
                            reason: NonConvergenceReason.MaxIncrementsReached);
                        break;
                    }
                }
                else
                {
                    report = correctionReport;
                    break;
                }
            }

            return report;
        }

        private static CorrectorInput GetCorrectorInput(SolverInputs props)
        {
            CorrectorInput ci = new CorrectorInput
            {
                Function = props.Function,
                Force = props.Force,
                MaxIterations = props.MaxIterations,
                UseArcLength = props.UseArcLength,
                ArcLengthRadius = props.ArcLengthRadius,
                Beta = props.Beta,
                Tolerances = props.Tolerances,
                MaxArcLengthAdjustments = props.MaxArcLengthAdjustments
            };
            return ci;
        }

        private static PredictorInput GetPredictorInput(SolverInputs props, Vector<double> firstSolution)
        {
            return new PredictorInput
            {
                Function = props.Function,
                Force = props.Force,
                Beta = props.Beta,
                UseArcLength = props.UseArcLength,
                LambdaIncrement = props.FirstLambdaValue,
                LastLambda = props.LastLambdaValue,
                ArcLengthRadius = props.ArcLengthRadius,
                ReferenceStiffness = props.Force.DotProduct(firstSolution),
            };
        }
    }
}
