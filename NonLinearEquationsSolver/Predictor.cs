using System;
using MathNet.Numerics.LinearAlgebra;

namespace NonLinearEquationsSolver
{
    public class Predictor
    {
        public bool Convergence { get; set; } = true;

        public Tuple<double, Vector<double>> Predict(IMultiDimensionalFunction function,
            Vector<double> force,
            PredictorInput input)
        {
            Vector<double> reaction = function.GetImage(input.Displacement);
            Matrix<double> stiffnessMatrix = function.GetTangentMatrix(input.Displacement);
            Vector<double> equilibrium = input.Lambda * force - reaction;
            Vector<double> incDisplacement = stiffnessMatrix.Solve(equilibrium);

            double lambda = input.Lambda;
            if (input.UseArcLength)
            {
                Vector<double> incDisplacementTangent = stiffnessMatrix.Solve(force);
                double bergam = GetBergamParameter(input.ReferenceStiffness, force, incDisplacementTangent);
                double incLambda = Math.Sign(bergam) *
                    GetLambdaIncrement(force, input.ArcLengthRadius, input.Beta, incDisplacementTangent);
                lambda += incLambda;
                incDisplacement = incLambda * incDisplacementTangent;
            }

            return new Tuple<double, Vector<double>>(lambda, input.Displacement + incDisplacement);
        }

        private double GetBergamParameter(double referenceStiffness, Vector<double> forceVector, Vector<double> displacement)
        {
            return Math.Abs(referenceStiffness / forceVector.DotProduct(displacement));
        }
        private double GetLambdaIncrement(Vector<double> force, double radius, double beta, Vector<double> displacement)
        {
            return radius / Math.Sqrt(Math.Pow(displacement.Norm(2), 2) + Math.Pow(beta, 2) * Math.Pow(force.Norm(2), 2));
        }
    }

    public class PredictorInput
    {
        public double Lambda { get; set; }
        public Vector<double> Displacement { get; set; }
        public bool UseArcLength { get; set; }
        public double ReferenceStiffness { get; set; }
        public double ArcLengthRadius { get; set; }
        public double Beta { get; set; }
    }
}
