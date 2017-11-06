using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MoreLinq;

namespace NonLinearEquationsSolver
{
    public class Corrector
    {
        public IterationPhaseReport Correct(CorrectorInput input)
        {
            IterationPhaseReport report = new IterationPhaseReport();
            double firstLambda = input.PredictionPhaseLambda;
            Vector<double> firstDisplacement = input.PredictionPhaseDisplacement;
            double lambda = firstLambda;
            Vector<double> displacement = firstDisplacement;
            Vector<double> reaction = input.Function.GetImage(displacement);
            Vector<double> equilibrium = lambda * input.Force - reaction;
            bool convergence = false;
            for (int iteration = 1; iteration <= input.MaxIterations; iteration++)
            {
                Matrix<double> stiffnessMatrix = input.Function.GetTangentMatrix(displacement);
                Vector<double> incTangentDisplacement = stiffnessMatrix.Solve(input.Force);
                Vector<double> incEquilibriumDisplacement = stiffnessMatrix.Solve(equilibrium);
                IncrementLoadDisplacement increment;
                NonConvergenceReason reason = GetCorrection(input: input,
                    displacement: displacement,
                    lambda: lambda,
                    dut: incTangentDisplacement,
                    dur: incEquilibriumDisplacement,
                    bestCandidate: out increment);
                if (reason == NonConvergenceReason.None)
                {
                    displacement += increment.IncrementDisplacement;
                    lambda += increment.IncrementLambda;
                    reaction = input.Function.GetImage(displacement);
                    equilibrium = lambda * input.Force - reaction;
                    Tolerances errors = GetErrors(force: input.Force,
                        reaction: reaction,
                        lambda: lambda,
                        displacement: displacement,
                        incrementDisplacement: report.IncrementDisplacement);
                    convergence = CheckConvergence(errors, input.Tolerances);
                }
                if (input.DoIterationReport)
                {
                    IterationReport iterationReport = new IterationReport
                    {
                        Lambda = lambda,
                        BergamParameter = 0,
                        Equilibrium = equilibrium,
                        Reaction = reaction,
                        Type = IterationPhaseType.Correction,
                        TangentMatrix = stiffnessMatrix,
                        IncrementDisplacementTangent = incTangentDisplacement,
                        IncrementDisplacementEquilibrium = incEquilibriumDisplacement,
                        IncrementLambda = increment.IncrementLambda,
                        Convergence = convergence,
                        Reason = reason
                    };
                    report.Iterations.Add(iterationReport);
                }
                if (!convergence &&
                    iteration == input.MaxIterations)
                    report.NonConvergenceReason = NonConvergenceReason.MaxIterationsReached;
                if (convergence)
                    break;
            }

            report.Convergence = convergence;
            report.IncrementDisplacement = displacement - firstDisplacement;
            report.IncrementLambda = lambda - firstLambda;
            return report;
        }

        private NonConvergenceReason GetCorrection(CorrectorInput input,
                                                   Vector<double> displacement,
                                                   double lambda,
                                                   Vector<double> dut,
                                                   Vector<double> dur,
                                                   out IncrementLoadDisplacement bestCandidate)
        {
            NonConvergenceReason reason = NonConvergenceReason.None;
            if (input.UseArcLength)
            {
                List<IncrementLoadDisplacement> candidates = GetCandidates(input: input,
                                                                           dut: dut,
                                                                           dur: dur);
                IDisplacementChooser chooser = new RestoringMethod();
                bestCandidate = chooser.Choose(function: input.Function,
                    fr: input.Force,
                    displacementAfterPredictionPhase: displacement,
                    lambda: lambda,
                    candidates: candidates);
            }
            else
                bestCandidate = new IncrementLoadDisplacement(0, dur);
            return reason;
        }

        private List<IncrementLoadDisplacement> GetCandidates(CorrectorInput input,
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
            .Select(dlambda => new IncrementLoadDisplacement(dlambda, dur + dlambda * dut))
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
