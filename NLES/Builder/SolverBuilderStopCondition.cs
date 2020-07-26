using System;

namespace NLES
{
    public partial class Solver
    {
        public class SolverBuilderStopCondition : SolverBuilder
        {
            public SolverBuilderStopCondition(Solver solver, double displacementTolerance, double equilibriumTolerance, double energyTolerance)
            {
                if (displacementTolerance <= 0 || equilibriumTolerance <= 0 || energyTolerance <= 0)
                {
                    throw new InvalidOperationException(Strings.TolerancesLargerThanZero);
                }

                Solver = solver;
                Solver.Corrector.Tolerances = new[]
                    {displacementTolerance, equilibriumTolerance, energyTolerance};
            }
        }
    }
}