using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;

namespace NonLinearEquationsSolver
{
    public class Corrector
    {
        public bool Convergence { get; set; }
        public event Action ArcLengthRadiusTooBig;
        public Tuple<double, Vector<double>> Correct(IMultiDimensionalFunction function, Vector<double> force, CorrectorInput input)
        {
            int numIter = 0;
            double lambda = input.Lambda;
            var displacement = input.Displacement;
            Vector<double> reaction = function.GetImage(displacement);

            List<double> errors = GetErrors(force, reaction, lambda, displacement, displacement);
            bool keepGoing = numIter <= input.MaximumIterations && !ConvergenceAchieved(errors, input.Tolerances);

            IDisplacementChooser chooser = new RestoringMethod();
            while (keepGoing)
            {
                Vector<double> displacementBefore = displacement;
                Matrix<double> stiffnessMatrix = function.GetTangentMatrix(displacement);
                reaction = function.GetImage(displacement);
                Vector<double> equilibrium = lambda * force - reaction;
                Vector<double> dVr = stiffnessMatrix.Solve(equilibrium);

                if (input.UseArcLength)
                {
                    Vector<double> dVt = stiffnessMatrix.Solve(force);
                    List<Tuple<double, Vector<double>>> candidates = GetCandidates(force, dVt, dVr, input.Dv, input.DLambda, input.Beta, input.ArcLengthRadius);
                    Tuple<double, Vector<double>> bestCandidate = chooser.Choose(function, force, displacement, lambda, candidates);
                    lambda += bestCandidate.Item1;
                    displacement += bestCandidate.Item1 * dVt;
                }
                displacement += dVr;

                errors = GetErrors(force, reaction, input.Lambda, input.Displacement, displacementBefore);
                numIter++;
                Convergence = ConvergenceAchieved(errors, input.Tolerances);
                keepGoing = numIter <= input.MaximumIterations && !Convergence;
            }

            return new Tuple<double, Vector<double>>(lambda, displacement);
        }

        private List<Tuple<double, Vector<double>>> GetCandidates(Vector<double> fr,
            Vector<double> dVt,
            Vector<double> dVr,
            Vector<double> Dv,
            double DLambda,
            double beta,
            double DL)
        {
            double frNormSquare = fr.DotProduct(fr);
            double betaSquare = Math.Pow(beta, 2);

            double a = dVt.DotProduct(dVt) +
                betaSquare * frNormSquare;
            double b = 2 * dVr.DotProduct(dVt) +
                2 * Dv.DotProduct(dVt) +
                2 * DLambda * betaSquare * frNormSquare;
            double c = -Math.Pow(DL, 2) +
                Dv.DotProduct(Dv) +
                dVr.DotProduct(dVr) +
                2 * Dv.DotProduct(dVr) +
                Math.Pow(DLambda, 2) * betaSquare * frNormSquare;
            double control = Math.Pow(b, 2) - 4 * a * c;

            if (control < 0)
                throw new Exception();

            return new List<double>
                {
                (-b + Math.Sqrt(control))/(2*a),
                (-b - Math.Sqrt(control))/(2*a)
            }
            .Select(dlambda => new Tuple<double, Vector<double>>(dlambda, dVr + dlambda * dVt))
            .ToList();
        }

        // HELPER METHODS
        private bool ConvergenceAchieved(List<double> errors, List<double> tolerances)
        {
            bool convergence = true;
            for (int i = 0; i < errors.Count; i++)
            {
                convergence = errors[i] <= tolerances[i];
                if (!convergence)
                    break;
            }
            return convergence;
        }

        private List<double> GetErrors(Vector<double> force, Vector<double> reaction, double lambda, Vector<double> displacement, Vector<double> displacementBefore)
        {
            Vector<double> equilibrium = lambda * force - reaction;
            return new List<double>
            {
                (displacement - displacementBefore).Norm(2) / displacement.Norm(2),
                equilibrium.Norm(2) / force.Norm(2),
                Math.Abs(displacement.DotProduct(equilibrium) / displacement.DotProduct(force))
            };
        }
    }

    public class CorrectorInput
    {
        public double Lambda { get; set; }
        public Vector<double> Displacement { get; set; }
        public double DLambda { get; set; }
        public Vector<double> Dv { get; set; }
        public double Beta { get; set; }
        public bool UseArcLength { get; set; }
        public double ArcLengthRadius { get; set; }
        public int MaxArcLengthIncrements { get; set; }
        public List<double> Tolerances { get; set; }
        public int MaximumIterations { get; set; }
    }
}
