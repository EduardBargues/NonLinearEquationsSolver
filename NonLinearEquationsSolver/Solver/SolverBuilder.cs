using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using NonLinearEquationsSolver.Common;
using NonLinearEquationsSolver.Correction;
using NonLinearEquationsSolver.Prediction;
using System;

namespace NonLinearEquationsSolver.Solver {
    public partial class Solver {
        public class SolverBuilder {

            protected Solver Solver = new Solver ( );

            public SolverBuilderStructure Solve( Func<Vector<double>, Vector<double>> structure ) =>
                new SolverBuilderStructure ( Solver, structure );
            public SolverBuilderIncrementalLoad Under( Vector<double> referenceLoad ) =>
                new SolverBuilderIncrementalLoad ( Solver, referenceLoad );
            public SolverBuilderSchemeStandardNewtonRaphson UsingStandardNewtonRaphsonScheme( double loadFactorIncrement ) =>
                new SolverBuilderSchemeStandardNewtonRaphson ( Solver, loadFactorIncrement );
            public SolverBuilderSchemeArcLength UsingArcLengthScheme( double radius ) =>
                new SolverBuilderSchemeArcLength ( Solver, radius );
            public SolverBuilderSchemeWorkControl UsingWorkControlScheme( double work ) => new SolverBuilderSchemeWorkControl ( Solver, work );
            public SolverBuilderStopCondition UntilTolerancesReached( double displacement, double equilibrium,
                double energy ) => new SolverBuilderStopCondition ( Solver, displacement, equilibrium, energy );
            public SolverBuilder WithMaximumCorrectionIterations( int maximumCorrectionIterations ) {
                Solver.Corrector.MaximumIterations = maximumCorrectionIterations;
                return this;
            }

            public Solver Build() {
                if (Solver.Info.ReferenceLoad == null) {
                    throw new Exception ( Strings.ReferenceLoadMustBeDefined );
                }
                if (Solver.Info.InitialLoad == null) {
                    Solver.Info.InitialLoad = new DenseVector ( Solver.Info.ReferenceLoad.Count );
                }
                if (Solver.Info.Reaction == null) {
                    throw new Exception ( Strings.ReactionMustBeDefined );
                }
                if (Solver.Info.Stiffness == null) {
                    throw new Exception ( Strings.StiffnessLoadMustBeDefined );
                }
                if (Solver.State.Displacement == null) {
                    Solver.State.Displacement = new DenseVector ( Solver.Info.ReferenceLoad.Count );
                }
                if (Solver.Corrector.Tolerances == null) {
                    Solver.Corrector.Tolerances = new[] { 1e-3, 1e-3, 1e-3 };
                }
                if (Solver.Corrector.MaximumIterations == 0) {
                    Solver.Corrector.MaximumIterations = 10;
                }
                if (Solver.Corrector.Scheme == null) {
                    throw new Exception ( Strings.SchemeMustBeDefined );
                }
                if (Solver.Corrector.Scheme is CorrectionSchemeArcLength schemeArcLength) {
                    if (schemeArcLength.Radius == 0) {
                        throw new Exception ( Strings.ArcLengthRadiusMustBeDefined );
                    }
                    if (schemeArcLength.Beta == 0) {
                        schemeArcLength.Beta = 1;
                    }
                    if (schemeArcLength.DisplacementChooser == null) {
                        throw new Exception ( Strings.DisplacementChooserMustBeDefined );
                    }
                }
                if (Solver.Corrector.Scheme is CorrectionSchemeWorkControl schemeWorkControl) {
                    if (schemeWorkControl.WorkIncrement == 0) {
                        throw new Exception ( Strings.WorkIncrementMustBeDefined );
                    }
                }
                if (Solver.Predictor.Scheme == null) {
                    throw new Exception ( Strings.SchemeMustBeDefined );
                }
                if (Solver.Predictor.Scheme is PredictionSchemeStandard scheme) {
                    if (scheme.DLambda == 0) {
                        throw new Exception ( Strings.DefaultLoadFactorIncrementMustBeDefined );
                    }
                }
                if (Solver.Predictor.Scheme is PredictionSchemeWorkControl workScheme) {
                    if (workScheme.WorkIncrement == 0) {
                        throw new Exception ( Strings.WorkIncrementMustBeDefined );
                    }
                }
                if (Solver.Predictor.Scheme is PredictionSchemeArcLength arcLengthScheme) {
                    if (arcLengthScheme.Radius == 0) {
                        throw new Exception ( Strings.ArcLengthRadiusMustBeDefined );
                    }
                    if (arcLengthScheme.Beta == 0) {
                        arcLengthScheme.Beta = 1;
                    }
                }

                return Solver;
            }
        }
    }
}
