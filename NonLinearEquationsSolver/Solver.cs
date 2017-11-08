using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;

namespace NonLinearEquationsSolver
{
    public class Solver : ISolver
    {
        public SolverReport Solve(ProblemDefinition problem)
        {
            SolverReport report = new SolverReport();

            LoadIncremental loadIncremental = new LoadIncremental(problem);
            foreach (LoadIncrementInfo info in loadIncremental.DoLoadIncrementProcedure(problem.MaxIncrements))
            {
                if (problem.DoIterationReport)
                    report.LoadIncrements.Add(info);
                if (info.Convergence)
                    report.Convergence = Math.Abs(problem.LastLambdaValue - info.Lambda) <= problem.Tolerances.IncrementalForce;
                else
                {
                    report.Reason = FailReason.IncrementLoadFailed;
                    break;
                }
                if (report.Convergence)
                {
                    report.Solution = info.Displacement;
                    break;
                }
            }
            if (!report.Convergence)
                report.Reason = FailReason.MaxIncrementsReached;

            return report;
        }

    }
}
