using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace NonLinearEquationsSolver
{
    public class Corrector
    {
        public IterationReport Correct(CorrectorInput input, out IterationPhaseOutput result)
        {
            IterationReport report = null;
            result = null;

            double firstLambda = input.PredictionPhaseLambda;
            Vector<double> firstDisplacement = input.PredictionPhaseDisplacement;
            double lambda = firstLambda;
            Vector<double> displacement = firstDisplacement;

            Vector<double> reaction = input.Function.GetImage(displacement);
            Vector<double> equilibrium = lambda * input.Force - reaction;
            for (int iteration = 0; iteration < input.MaxIterations; iteration++)
            {
                Matrix<double> stiffnessMatrix = input.Function.GetTangentMatrix(displacement);
                Vector<double> incTangentDisplacement = stiffnessMatrix.Solve(input.Force);
                Vector<double> incEquilibriumDisplacement = stiffnessMatrix.Solve(equilibrium);
                IterationPhaseOutput correction;
                NonConvergenceReason reason = GetCorrection(input: input,
                                                                displacement: displacement,
                                                                lambda: lambda,
                                                                dut: incTangentDisplacement,
                                                                dur: incEquilibriumDisplacement,
                                                                result: out correction);
                bool correctionSucceeded = reason == NonConvergenceReason.None;
                if (correctionSucceeded)
                {
                    displacement += correction.IncrementDisplacement;
                    lambda += correction.IncrementLambda;
                    reaction = input.Function.GetImage(displacement);
                    equilibrium = lambda * input.Force - reaction;
                    Tolerances errors = GetErrors(force: input.Force,
                        reaction: reaction,
                        lambda: lambda,
                        displacement: displacement,
                        incrementDisplacement: correction.IncrementDisplacement);
                    bool convergence = CheckConvergence(errors, input.Tolerances);
                    if (convergence)
                    {
                        report = new IterationReport(true, NonConvergenceReason.None);
                        result = new IterationPhaseOutput(incrementLambda: lambda - firstLambda,
                                                          incrementDisplacement: displacement - firstDisplacement);
                        break;
                    }
                }
                else
                {
                    report = new IterationReport(false, reason);
                    break;
                }
            }

            if (report == null ||
                report.Convergence == false)
                report = new IterationReport(false, NonConvergenceReason.MaxIterationsReached);

            return report;
        }

        private NonConvergenceReason GetCorrection(CorrectorInput input,
                                                   Vector<double> displacement,
                                                   double lambda,
                                                   Vector<double> dut,
                                                   Vector<double> dur,
                                                   out IterationPhaseOutput result)
        {
            NonConvergenceReason reason = NonConvergenceReason.None;

            if (input.UseArcLength)
            {
                List<IterationPhaseOutput> candidates = GetCandidates(input, dut, dur);
                IDisplacementChooser chooser = new RestoringMethod();
                result = chooser.Choose(input.Function, input.Force, displacement, lambda, candidates);
            }
            else
                result = new IterationPhaseOutput(0, dur);

            return reason;
        }

        private List<IterationPhaseOutput> GetCandidates(CorrectorInput input,
                                                         Vector<double> dut,
                                                         Vector<double> dur)
        {
            double frNormSquare = input.Force.DotProduct(input.Force);
            double betaSquare = Math.Pow(input.Beta, 2);

            double a = dut.DotProduct(dut) +
                betaSquare * frNormSquare;

            double b = 2 * dur.DotProduct(dut) +
                2 * input.PredictionPhaseIncrementDisplacement.DotProduct(dut) +
                2 * input.PredictionPhaseIncrementLambda * betaSquare * frNormSquare;

            double c = -Math.Pow(input.ArcLengthRadius, 2) +
                       input.PredictionPhaseIncrementDisplacement.DotProduct(input.PredictionPhaseIncrementDisplacement) +
                dur.DotProduct(dur) +
                2 * input.PredictionPhaseIncrementDisplacement.DotProduct(dur) +
                Math.Pow(input.PredictionPhaseIncrementLambda, 2) * betaSquare * frNormSquare;

            double control = Math.Pow(b, 2) - 4 * a * c;

            if (control < 0)
                throw new Exception();

            return new List<double>
                {
                (-b + Math.Sqrt(control))/(2*a),
                (-b - Math.Sqrt(control))/(2*a)
            }
            .Select(dlambda => new IterationPhaseOutput(dlambda, dur + dlambda * dut))
            .ToList();
        }

        // HELPER METHODS
        private bool CheckConvergence(Tolerances errors, Tolerances tolerances)
        {
            return errors.Displacement <= tolerances.Displacement &&
                   errors.Equilibrium <= tolerances.Equilibrium &&
                   errors.Work <= tolerances.Work;
        }
        private Tolerances GetErrors(Vector<double> force,
                                       Vector<double> reaction,
                                       double lambda,
                                       Vector<double> displacement,
                                       Vector<double> incrementDisplacement)
        {
            Vector<double> equilibrium = lambda * force - reaction;
            return new Tolerances(
                displacement: incrementDisplacement.Norm(2) / displacement.Norm(2),
                equilibrium: equilibrium.Norm(2) / force.Norm(2),
                work: Math.Abs(displacement.DotProduct(equilibrium) / displacement.DotProduct(force)),
                incrementalForce: 0);
        }
    }
}
