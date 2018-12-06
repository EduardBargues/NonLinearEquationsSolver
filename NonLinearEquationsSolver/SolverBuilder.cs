using MathNet.Numerics.LinearAlgebra;
using System;

namespace NonLinearEquationsSolver {
    public partial class Solver {
        public class SolverBuilder {

            protected Solver Solver = new Solver ( );

            /// <summary>
            /// First method to be called while constructing a solver.
            /// </summary>
            /// <param name="degreesOfFreedom"></param>
            /// <param name="structure"></param>
            /// <param name="stiffness"></param>
            /// <returns></returns>
            public SolverBuilderStructure Solve( int degreesOfFreedom, Func<Vector<double>, Vector<double>> structure, Func<Vector<double>, Matrix<double>> stiffness ) =>
                new SolverBuilderStructure ( degreesOfFreedom, Solver, structure, stiffness );
            public SolverBuilderIncrementalLoad Under( Vector<double> referenceLoad ) =>
                new SolverBuilderIncrementalLoad ( Solver, referenceLoad );
            public SolverBuilderSchemeStandardNewtonRaphson UsingStandardNewtonRaphsonScheme( double loadFactorIncrement ) {
                if (loadFactorIncrement <= 0) {
                    throw new InvalidOperationException ( Strings.LoadIncrementLargerThanZero );
                }
                return new SolverBuilderSchemeStandardNewtonRaphson ( Solver, loadFactorIncrement );
            }
            public SolverBuilderSchemeArcLength UsingArcLengthScheme( double radius ) {
                if (radius <= 0) {
                    throw new InvalidOperationException ( Strings.ArcLengthRadiusLargerThanZero );
                }
                return new SolverBuilderSchemeArcLength ( Solver, radius );
            }
            public SolverBuilderSchemeWorkControl UsingWorkControlScheme( double work ) {
                if (work <= 0) {
                    throw new InvalidOperationException ( Strings.WorkControlValueLargerThanZero );
                }
                return new SolverBuilderSchemeWorkControl ( Solver, work );
            }
            public SolverBuilderStopCondition UntilTolerancesReached( double displacement, double equilibrium,
                double energy ) => new SolverBuilderStopCondition ( Solver, displacement, equilibrium, energy );
            public SolverBuilder WithMaximumCorrectionIterations( int maximumCorrectionIterations ) {
                if (maximumCorrectionIterations <= 0) {
                    throw new InvalidOperationException ( Strings.MaximumNumberOfIterationsLargerThanZero );
                }
                Solver.Corrector.MaximumIterations = maximumCorrectionIterations;
                return this;
            }

            public Solver Build() => Solver;
        }
    }
}
