using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace NonLinearEquationsSolver
{
    public class CorrectorIterator : IIterator
    {
        private readonly Vector<double> force;
        private readonly IFunction function;
        private readonly double arcLengthRadius;
        private readonly double beta;
        private readonly Vector<double> predictionPhaseIncrementDisplacement;
        private readonly double predictionPhaseIncrementLambda;
        private readonly IterationScheme scheme;
        private double lambda;
        private Vector<double> displacement;

        public CorrectorIterator(CorrectorInput info, double lambda, Vector<double> displacement)
        {
            force = info.Force;
            function = info.Function;
            arcLengthRadius = info.ArcLengthRadius;
            beta = info.Beta;
            predictionPhaseIncrementDisplacement = info.PredictionPhaseIncrementDisplacement;
            predictionPhaseIncrementLambda = info.PredictionPhaseIncrementLambda;
            scheme = info.Scheme;
            this.lambda = lambda;
            this.displacement = displacement.Clone();
        }

        public IEnumerable<IterationInfo> Iterate(int maxIterations)
        {
            for (int iteration = 1; iteration <= maxIterations; iteration++)
            {
                Matrix<double> stiffnessMatrix = function.GetTangentMatrix(displacement);
                Vector<double> incTangentDisplacement = stiffnessMatrix.Solve(force);
                Vector<double> reaction = function.GetImage(displacement);
                Vector<double> equilibrium = lambda * force - reaction;
                Vector<double> incEquilibriumDisplacement = stiffnessMatrix.Solve(equilibrium);

                IterationInfo iterationInfo = GetCorrection(
                    dut: incTangentDisplacement,
                    dur: incEquilibriumDisplacement);

                iterationInfo.Lambda = lambda;
                iterationInfo.Equilibrium = equilibrium;
                iterationInfo.IncrementDisplacementEquilibrium = incEquilibriumDisplacement;
                iterationInfo.IncrementDisplacementTangent = incTangentDisplacement;
                iterationInfo.Reaction = reaction;
                iterationInfo.TangentMatrix = stiffnessMatrix;

                yield return iterationInfo;

                lambda += iterationInfo.IncrementLambda;
                displacement += iterationInfo.IncrementDisplacement;
            }
        }

        private IterationInfo GetCorrection(
            Vector<double> dut,
            Vector<double> dur)
        {
            IterationInfo info = new IterationInfo();
            IncrementLoadDisplacement bestCandidate = null;
            switch (scheme)
            {
                case IterationScheme.Standard:
                    bestCandidate = new IncrementLoadDisplacement(0, dur);
                    break;
                case IterationScheme.ArcLength:
                    IDisplacementChooser chooser = new RestoringMethod();
                    IEnumerable<IncrementLoadDisplacement> candidates;
                    FailReason failReason = GetCandidates(
                        dut: dut,
                        dur: dur,
                        candidates: out candidates);
                    if (failReason == FailReason.None)
                        bestCandidate = chooser.Choose(function: function,
                            fr: force,
                            displacementAfterPredictionPhase: displacement,
                            lambda: lambda,
                            candidates: candidates);
                    info.FailReason = failReason;
                    break;
                case IterationScheme.WorkControl:
                    double dlambda = force.DotProduct(dur) / force.DotProduct(dut);
                    Vector<double> dv = dur + dlambda * dut;
                    bestCandidate = new IncrementLoadDisplacement(dlambda, dv);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            info.IncrementLambda = bestCandidate?.IncrementLambda ?? 0.0;
            info.IncrementDisplacement = bestCandidate?.IncrementDisplacement;

            return info;
        }
        private FailReason GetCandidates(
            Vector<double> dut,
            Vector<double> dur,
            out IEnumerable<IncrementLoadDisplacement> candidates)
        {
            FailReason output = FailReason.None;
            candidates = null;

            switch (scheme)
            {
                case IterationScheme.ArcLength:
                    break;
                case IterationScheme.WorkControl:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            double frNormSquare = force.DotProduct(force);
            double betaSquare = Math.Pow(beta, 2);

            double a = dut.DotProduct(dut) +
                       betaSquare * frNormSquare;

            double b = 2 * dur.DotProduct(dut) +
                       2 * predictionPhaseIncrementDisplacement.DotProduct(dut) +
                       2 * predictionPhaseIncrementLambda * betaSquare * frNormSquare;

            double c = -Math.Pow(arcLengthRadius, 2) +
                       predictionPhaseIncrementDisplacement.DotProduct(predictionPhaseIncrementDisplacement) +
                       dur.DotProduct(dur) +
                       2 * predictionPhaseIncrementDisplacement.DotProduct(dur) +
                       Math.Pow(predictionPhaseIncrementLambda, 2) * betaSquare * frNormSquare;

            double control = Math.Pow(b, 2) - 4 * a * c;

            if (control < 0)
                output = FailReason.ArcLengthTooLarge;
            else
                candidates = new List<double>
                    {
                        (-b + Math.Sqrt(control))/(2*a),
                        (-b - Math.Sqrt(control))/(2*a)
                    }
                    .Select(dlambda => new IncrementLoadDisplacement(dlambda, dur + dlambda * dut));

            return output;
        }
    }
}
