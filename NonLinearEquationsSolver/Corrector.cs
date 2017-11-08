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
        public IterationPhaseInfo Correct(CorrectorInput input)
        {
            IterationPhaseInfo report = new IterationPhaseInfo();

            double firstLambda = input.PredictionPhaseLambda;
            Vector<double> firstDisplacement = input.PredictionPhaseDisplacement;
            double lambda = firstLambda;
            Vector<double> displacement = firstDisplacement;

            IIterator iterator = new CorrectorIterator(input, lambda, displacement);
            IConvergenceChecker checker = new ConvergenceChecker(input.Function,
                input.Force,
                input.Tolerances);
            bool convergence = false;
            foreach (IterationInfo info in iterator.Iterate(input.MaxIterations))
            {
                if (info.Success)
                {
                    displacement += info.IncrementDisplacement;
                    lambda += info.IncrementLambda;
                    convergence = checker.CheckConvergence(
                        displacement: displacement,
                        incrementDisplacement: info.IncrementDisplacement,
                        lambda: lambda);
                }
                if (input.DoIterationReport)
                    report.Iterations.Add(info);
                if (convergence)
                    break;
            }

            report.Convergence = convergence;
            report.FailReason = convergence
                ? FailReason.None
                : FailReason.MaxIterationsReached;
            report.IncrementLambda = lambda - firstLambda;
            report.IncrementDisplacement = displacement - firstDisplacement;

            return report;
        }
    }
}
