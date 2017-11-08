using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace NonLinearEquationsSolver
{
    internal class LoadIncremental
    {
        private readonly ProblemDefinition problem;
        public LoadIncremental(ProblemDefinition problem)
        {
            this.problem = problem;
        }
        public IEnumerable<LoadIncrementInfo> DoLoadIncrementProcedure(int maxIncrements)
        {
            Vector<double> displacement = problem.InitialApproximation;
            Matrix<double> firstStiffnessMatrix = problem.Function.GetTangentMatrix(displacement);
            Vector<double> firstSolution = firstStiffnessMatrix.Solve(problem.Force);

            Predictor predictor = new Predictor();
            PredictorInput predictorInput = GetPredictorInput(firstSolution);
            Corrector corrector = new Corrector();
            CorrectorInput correctorInput = GetCorrectorInput();
            double lambda = problem.FirstLambdaValue;

            for (int increment = 0; increment < maxIncrements; increment++)
            {
                LoadIncrementInfo info = new LoadIncrementInfo();

                // PREDICTION PHASE
                predictorInput.Displacement = displacement;
                predictorInput.Lambda = lambda;
                IterationPhaseInfo incPrediction = predictor.Predict(predictorInput);
                displacement += incPrediction.IncrementDisplacement;
                lambda += incPrediction.IncrementLambda;
                if (problem.DoIterationReport)
                    info.Phases.Add(incPrediction);

                // CORRECTION PHASE
                correctorInput.PredictionPhaseLambda = lambda;
                correctorInput.PredictionPhaseDisplacement = displacement;
                correctorInput.PredictionPhaseIncrementLambda = incPrediction.IncrementLambda;
                correctorInput.PredictionPhaseIncrementDisplacement = incPrediction.IncrementDisplacement;

                IterationPhaseInfo incCorrection = corrector.Correct(correctorInput);
                if (problem.DoIterationReport)
                    info.Phases.Add(incCorrection);

                info.Convergence = incCorrection.Convergence;
                info.Displacement = displacement;
                info.FailReason = incCorrection.FailReason;
                info.Lambda = lambda;

                yield return info;
            }
        }


        private CorrectorInput GetCorrectorInput()
        {
            return new CorrectorInput
            {
                Function = problem.Function,
                Force = problem.Force,
                MaxIterations = problem.MaxIterations,
                ArcLengthRadius = problem.ArcLengthRadius,
                Beta = problem.Beta,
                Tolerances = problem.Tolerances,
                MaxArcLengthAdjustments = problem.MaxArcLengthAdjustments,
                DoIterationReport = problem.DoIterationReport,
                Scheme = problem.Scheme
            };
        }

        private PredictorInput GetPredictorInput(Vector<double> firstSolution)
        {
            return new PredictorInput
            {
                Function = problem.Function,
                Force = problem.Force,
                Beta = problem.Beta,
                UseArcLength = problem.UseArcLength,
                LambdaIncrement = problem.FirstLambdaValue,
                LastLambda = problem.LastLambdaValue,
                ArcLengthRadius = problem.ArcLengthRadius,
                ReferenceStiffness = problem.Force.DotProduct(firstSolution),
                DoIterationReport = problem.DoIterationReport,
                Scheme = problem.Scheme,
            };
        }
    }
}