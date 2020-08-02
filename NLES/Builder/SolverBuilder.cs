using System;

using MathNet.Numerics.LinearAlgebra;

namespace NLES
{
    public partial class NonLinearSolver
    {
        public class SolverBuilder
        {
            protected NonLinearSolver Solver = new NonLinearSolver();

            public SolverBuilderStructure Solve(
                int degreesOfFreedom
                , Func<Vector<double>, Vector<double>> reaction
                , Func<Vector<double>, ILinearSolver> stiffness) => new SolverBuilderStructure(degreesOfFreedom, Solver, reaction, stiffness);

            public SolverBuilderIncrementalLoad Under(Vector<double> referenceLoad) => new SolverBuilderIncrementalLoad(Solver, referenceLoad);

            public SolverBuilderSchemeStandardNewtonRaphson UsingStandardNewtonRaphsonScheme(double loadFactorIncrement)
                => new SolverBuilderSchemeStandardNewtonRaphson(Solver, loadFactorIncrement);

            public SolverBuilderSchemeArcLength UsingArcLengthScheme(double radius) => new SolverBuilderSchemeArcLength(Solver, radius);

            public SolverBuilderStopCondition UntilTolerancesReached(
                double displacement
                , double equilibrium
                , double energy) => new SolverBuilderStopCondition(Solver, displacement, equilibrium, energy);

            public SolverBuilder WithMaximumCorrectionIterations(int maximumCorrectionIterations)
            {
                if (maximumCorrectionIterations <= 0)
                {
                    throw new InvalidOperationException(Strings.MaximumNumberOfIterationsLargerThanZero);
                }
                Solver.Corrector.MaximumIterations = maximumCorrectionIterations;
                return this;
            }

            public NonLinearSolver Build() => Solver;
        }
    }
}
