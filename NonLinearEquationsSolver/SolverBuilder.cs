using MathNet.Numerics.LinearAlgebra;
using System;

namespace NonLinearEquationsSolver {
    public partial class SolverND {
        public class SolverNdBuilder {

            protected SolverND SolverNd = new SolverND ( );

            /// <summary>
            /// First method to be called while constructing a solverNd.
            /// </summary>
            /// <param name="degreesOfFreedom"></param>
            /// <param name="structure"></param>
            /// <param name="stiffness"></param>
            /// <returns></returns>
            public SolverNdBuilderStructure Solve( int degreesOfFreedom, Func<Vector<double>, Vector<double>> structure, Func<Vector<double>, Matrix<double>> stiffness ) =>
                new SolverNdBuilderStructure ( degreesOfFreedom, SolverNd, structure, stiffness );
            public SolverNdBuilderIncrementalLoad Under( Vector<double> referenceLoad ) =>
                new SolverNdBuilderIncrementalLoad ( SolverNd, referenceLoad );
            public SolverNdBuilderSchemeStandardNewtonRaphson UsingStandardNewtonRaphsonScheme( double loadFactorIncrement ) {
                if (loadFactorIncrement <= 0) {
                    throw new InvalidOperationException ( Strings.LoadIncrementLargerThanZero );
                }
                return new SolverNdBuilderSchemeStandardNewtonRaphson ( SolverNd, loadFactorIncrement );
            }
            public SolverNdBuilderSchemeArcLength UsingArcLengthScheme( double radius ) {
                if (radius <= 0) {
                    throw new InvalidOperationException ( Strings.ArcLengthRadiusLargerThanZero );
                }
                return new SolverNdBuilderSchemeArcLength ( SolverNd, radius );
            }
            public SolverNdBuilderSchemeWorkControl UsingWorkControlScheme( double work ) {
                if (work <= 0) {
                    throw new InvalidOperationException ( Strings.WorkControlValueLargerThanZero );
                }
                return new SolverNdBuilderSchemeWorkControl ( SolverNd, work );
            }
            public SolverNdBuilderStopCondition UntilTolerancesReached( double displacement, double equilibrium,
                double energy ) => new SolverNdBuilderStopCondition ( SolverNd, displacement, equilibrium, energy );
            public SolverNdBuilder WithMaximumCorrectionIterations( int maximumCorrectionIterations ) {
                if (maximumCorrectionIterations <= 0) {
                    throw new InvalidOperationException ( Strings.MaximumNumberOfIterationsLargerThanZero );
                }
                SolverNd.Corrector.MaximumIterations = maximumCorrectionIterations;
                return this;
            }

            public SolverND Build() => SolverNd;
        }
    }
}
