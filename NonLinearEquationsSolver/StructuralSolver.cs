using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace NonLinearEquationsSolver
{
    public class StructuralSolver : IFunctionalSolver
    {
        public Vector<double> Solve(IMultiDimensionalFunction f, Vector<double> force, Dictionary<object, object> props)
        {
            Vector<double> displacement = (Vector<double>)props[SolverInputs.InitialApproximation];
            int maximumNumberOfIncrements = (int)props[SolverInputs.MaximumNumberOfIncrements];
            Predictor predictor = new Predictor();
            PredictorInput pi = new PredictorInput
            {
                Beta = (double)props[SolverInputs.Beta],
                UseArcLength = (bool)props[SolverInputs.UseArcLength],
                Lambda = (double)props[SolverInputs.FirstLambdaValue]
            };
            pi.ArcLengthRadius = pi.UseArcLength
                ? (double)props[SolverInputs.ArcLengthRadius]
                : 0;
            Matrix<double> firstStiffnessMatrix = f.GetTangentMatrix(displacement);
            Vector<double> firstSolution = firstStiffnessMatrix.Solve(force);
            pi.ReferenceStiffness = force.DotProduct(firstSolution);

            Corrector corrector = new Corrector();
            CorrectorInput ci = new CorrectorInput
            {
                MaximumIterations = (int)props[SolverInputs.MaximumNumberOfIterations],
                UseArcLength = pi.UseArcLength,
                ArcLengthRadius = pi.ArcLengthRadius,
                Beta = pi.Beta,
                Tolerances = (List<double>)props[SolverInputs.Tolerances],
                MaxArcLengthIncrements = pi.UseArcLength
                    ? (int)props[SolverInputs.MaximumNumberOfArcLengthAdjustments]
                    : 0
            };
            double lastLambdaValue = (double)props[SolverInputs.LastLambdaValue];
            int numIncrements = 0;
            bool keepIncrementing = true;
            while (keepIncrementing)
            {
                pi.Displacement = displacement;
                Tuple<double, Vector<double>> prediction = predictor.Predict(f, force, pi);
                double lambda = prediction.Item1;
                displacement = prediction.Item2;
                double DLambda = lambda-pi.Lambda;
                Vector<double> Dv = displacement - pi.Displacement;
                ci.Lambda = lambda;
                ci.Displacement = displacement;
                ci.DLambda = DLambda;
                ci.Dv = Dv;
                Tuple<double, Vector<double>> correction = corrector.Correct(f, force, ci);
                lambda = correction.Item1;
                pi.Lambda = lambda;
                displacement = correction.Item2;
                numIncrements++;
                keepIncrementing = lambda < lastLambdaValue &&
                    numIncrements <= maximumNumberOfIncrements;
            }

            return displacement;
        }
    }
}
