using System;
using MathNet.Numerics.LinearAlgebra;

namespace NonLinearEquationsSolver
{
    public class Predictor
    {
        public IterationPhaseReport Predict(PredictorInput input)
        {
            Vector<double> reaction = input.Function.GetImage(input.Displacement);
            Matrix<double> stiffnessMatrix = input.Function.GetTangentMatrix(input.Displacement);
            Vector<double> equilibrium = input.Lambda * input.Force - reaction;

            Vector<double> incDispTangent = stiffnessMatrix.Solve(input.Force);
            double bergam = GetBergamParameter(input.ReferenceStiffness, input.Force, incDispTangent);
            double incLambda = GetLambdaIncrement(input, incDispTangent) * Math.Sign(bergam);

            Vector<double> incDispEquilibrium = stiffnessMatrix.Solve(equilibrium);
            Vector<double> incDisplacement = incLambda * incDispTangent + incDispEquilibrium;

            IterationPhaseReport phaseReport = new IterationPhaseReport(incLambda, incDisplacement);
            if (input.DoIterationReport)
            {
                phaseReport.BergamParameter = bergam;
                phaseReport.Equilibrium = equilibrium;
                phaseReport.IncrementDisplacementTangent = incDispTangent;
                phaseReport.IncrementDisplacementEquilibrium = incDispEquilibrium;
                phaseReport.Lambda = input.Lambda;
                phaseReport.Reaction = reaction;
                phaseReport.Type = IterationPhaseType.Prediction;
            }

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
