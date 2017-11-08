using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace NonLinearEquationsSolver
{
    public class Predictor
    {
        public IterationPhaseInfo Predict(PredictorInput input)
        {
            Vector<double> reaction = input.Function.GetImage(input.Displacement);
            Matrix<double> stiffnessMatrix = input.Function.GetTangentMatrix(input.Displacement);
            Vector<double> equilibrium = input.Lambda * input.Force - reaction;

            Vector<double> incDispTangent = stiffnessMatrix.Solve(input.Force);
            double bergam = GetBergamParameter(input.ReferenceStiffness, input.Force, incDispTangent);
            double incLambda = GetLambdaIncrement(input, incDispTangent) * Math.Sign(bergam);

            Vector<double> incDispEquilibrium = stiffnessMatrix.Solve(equilibrium);
            Vector<double> incDisplacement = incLambda * incDispTangent + incDispEquilibrium;

            IterationPhaseInfo phaseReport = new IterationPhaseInfo(
                incrementLambda: incLambda,
                incrementDisplacement: incDisplacement,
                type: IterationPhaseType.Prediction,
                convergence: true)
            { BergamParameter = bergam };
            if (input.DoIterationReport)
                phaseReport.Iterations = new List<IterationInfo>
                {
                    new IterationInfo
                    {
                        Equilibrium = equilibrium,
                        IncrementDisplacementTangent = incDispTangent,
                        IncrementDisplacementEquilibrium = incDispEquilibrium,
                        IncrementDisplacement = incDisplacement,
                        Lambda = input.Lambda,
                        Reaction = reaction,
                        Success = true,
                        FailReason = FailReason.None,
                        IncrementLambda = incLambda,
                        TangentMatrix = stiffnessMatrix,
                    }
                };

            return phaseReport;
        }
        private double GetBergamParameter(double referenceStiffness,
                                          Vector<double> forceVector,
                                          Vector<double> displacement)
        {
            return Math.Abs(referenceStiffness / forceVector.DotProduct(displacement));
        }
        public double GetLambdaIncrement(PredictorInput input,
                                         Vector<double> incrementDisplacement)
        {
            double increment;
            switch (input.Scheme)
            {
                case IterationScheme.Standard:
                    increment = input.LambdaIncrement;
                    break;
                case IterationScheme.ArcLength:
                    double dispDotProduct = incrementDisplacement.DotProduct(incrementDisplacement);
                    double forceDotProduct = input.Force.DotProduct(input.Force);
                    increment = input.ArcLengthRadius / Math.Sqrt(dispDotProduct + Math.Pow(input.Beta, 2) * forceDotProduct);
                    break;
                case IterationScheme.WorkControl:
                    double dispForceDotProduct = input.Force.DotProduct(incrementDisplacement);
                    increment = Math.Sqrt(input.ArcLengthRadius / dispForceDotProduct);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return Math.Min(increment, Math.Abs(input.LastLambda - input.Lambda));
        }
    }
}
