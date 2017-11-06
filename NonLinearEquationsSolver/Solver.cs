using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace NonLinearEquationsSolver
{
    public class Solver : ISolver
    {
        public IterationPhaseReport Solve(ProblemDefinition problem, out Vector<double> solution)
        {
            solution = null;
            IterationPhaseReport phaseReport = null;

            Vector<double> displacement = problem.InitialApproximation;
            Matrix<double> firstStiffnessMatrix = problem.Function.GetTangentMatrix(displacement);
            Vector<double> firstSolution = firstStiffnessMatrix.Solve(problem.Force);

            Predictor predictor = new Predictor();
            PredictorInput predictorInput = GetPredictorInput(problem, firstSolution);

            Corrector corrector = new Corrector();
            CorrectorInput correctorInput = GetCorrectorInput(problem);
            double lambda = problem.FirstLambdaValue;

            for (int increment = 0; increment < problem.MaxIncrements; increment++)
            {
                // PREDICTION PHASE
                predictorInput.Displacement = displacement;
                predictorInput.Lambda = lambda;
                IterationPhaseReport incPrediction = predictor.Predict(predictorInput);
                displacement += incPrediction.IncrementDisplacement;
                lambda += incPrediction.IncrementLambda;

                // CORRECTION PHASE
                correctorInput.PredictionPhaseLambda = lambda;
                correctorInput.PredictionPhaseDisplacement = displacement;
                correctorInput.PredictionPhaseIncrementLambda = incPrediction.IncrementLambda;
                correctorInput.PredictionPhaseIncrementDisplacement = incPrediction.IncrementDisplacement;

                IterationPhaseReport incCorrection = corrector.Correct(correctorInput);
                if (incCorrection.Convergence)
                {
                    displacement += incCorrection.IncrementDisplacement;
                    lambda += incCorrection.IncrementLambda;
                    // CHECK CONVERGENCE
                    bool convergence = Math.Abs(lambda - problem.LastLambdaValue) <= problem.Tolerances.IncrementalForce;
                    if (convergence)
                    {
                        solution = displacement;
                        phaseReport = new IterationPhaseReport(true, NonConvergenceReason.None);
                        break;
                    }
                    bool maxIncrementsReached = increment <= problem.MaxIncrements;
                    if (maxIncrementsReached)
                    {
                        phaseReport = new IterationPhaseReport(convergence: false,
                            reason: NonConvergenceReason.MaxIncrementsReached);
                        break;
                    }
                }
                else
                {
                    phaseReport = correctionReport;
                    break;
                }
            }

            return phaseReport;
        }

        private static CorrectorInput GetCorrectorInput(ProblemDefinition props)
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
                MaxArcLengthAdjustments = props.MaxArcLengthAdjustments,
                DoIterationReport = props.DoIterationReport,
            };
            return ci;
        }

        private static PredictorInput GetPredictorInput(ProblemDefinition props, Vector<double> firstSolution)
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
                DoIterationReport = props.DoIterationReport,
            };
        }
    }
}
