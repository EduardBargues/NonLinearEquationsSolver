using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using NonLinearEquationsSolver.Common;
using NonLinearEquationsSolver.Correction;
using NUnit.Framework;
using System;
using System.Linq;

namespace NonLinearEquationsSolver.Tests {
    class BuildSolver {

        [Test]
        public void RestoringArcLengthSolverIsCorrectlyBuild() {
            //Given 
            Vector<double> Reaction( Vector<double> u ) => new DenseVector ( 2 );

            Matrix<double> Stiffness( Vector<double> u ) => new DenseMatrix ( 2, 2 );
            Vector<double> initialLoad = new DenseVector ( 2 ) { [0] = 1, [1] = 1 };
            Vector<double> initialDisplacement = new DenseVector ( 2 );
            double initialLoadFactor = 1;
            Vector<double> referenceLoad = new DenseVector ( 2 ) { [0] = 1, [1] = 1 };
            double dispTol = 1e-3;
            double eqTol = 1e-3;
            double enTol = 1e-3;
            double radius = 1;
            double beta = 100;
            int maxIter = 10;

            // When
            Solver.Solver solver = Solver.Solver.Builder
                .Solve ( Reaction )
                .WithStiffnessMatrix ( Stiffness )
                .Under ( referenceLoad )
                .WithInitialLoad ( initialLoad )
                .WithInitialLoadFactor ( initialLoadFactor )
                .WithInitialDisplacement ( initialDisplacement )
                .UntilTolerancesReached ( dispTol, eqTol, enTol )
                .WithMaximumCorrectionIterations ( maxIter )
                .UsingArcLengthScheme ( radius )
                .NormalizeLoadWith ( beta )
                .WithRestoringMethodInCorrectionPhase ( )
                .Build ( );

            // Then
            Assert.AreEqual ( referenceLoad, solver.Info.ReferenceLoad );
            Assert.AreEqual ( initialLoad, solver.Info.InitialLoad );
            Assert.AreEqual ( (Func<Vector<double>, Matrix<double>>)Stiffness, solver.Info.Stiffness );
            Assert.AreEqual ( (Func<Vector<double>, Vector<double>>)Reaction, solver.Info.Reaction );
            Assert.AreEqual ( initialLoadFactor, solver.State.Lambda );
            Assert.AreEqual ( initialDisplacement, solver.State.Displacement );
            Assert.AreEqual ( typeof ( CorrectionSchemeArcLength ), solver.Corrector.Scheme.GetType ( ) );
            CorrectionSchemeArcLength scheme = (CorrectionSchemeArcLength)solver.Corrector.Scheme;
            Assert.AreEqual ( typeof ( RestoringMethod ), scheme.DisplacementChooser.GetType ( ) );
            Assert.AreEqual ( maxIter, solver.Corrector.MaximumIterations );
        }
        [Test]
        public void AngleArcLengthSolverIsCorrectlyBuild() {
            //Given 
            Vector<double> Reaction( Vector<double> u ) => new DenseVector ( 2 );

            Matrix<double> Stiffness( Vector<double> u ) => new DenseMatrix ( 2, 2 );
            Vector<double> initialLoad = new DenseVector ( 2 ) { [0] = 1, [1] = 1 };
            Vector<double> initialDisplacement = new DenseVector ( 2 );
            double initialLoadFactor = 1;
            Vector<double> referenceLoad = new DenseVector ( 2 ) { [0] = 1, [1] = 1 };
            double dispTol = 1e-3;
            double eqTol = 1e-3;
            double enTol = 1e-3;
            double radius = 1;
            double beta = 100;
            int maxIter = 10;

            // When
            Solver.Solver solver = Solver.Solver.Builder
                .Solve ( Reaction )
                .WithStiffnessMatrix ( Stiffness )
                .Under ( referenceLoad )
                .WithInitialLoad ( initialLoad )
                .WithInitialLoadFactor ( initialLoadFactor )
                .WithInitialDisplacement ( initialDisplacement )
                .UntilTolerancesReached ( dispTol, eqTol, enTol )
                .WithMaximumCorrectionIterations ( maxIter )
                .UsingArcLengthScheme ( radius )
                .NormalizeLoadWith ( beta )
                .WithAngleMethodInCorrectionPhase ( )
                .Build ( );

            // Then
            Assert.AreEqual ( referenceLoad, solver.Info.ReferenceLoad );
            Assert.AreEqual ( initialLoad, solver.Info.InitialLoad );
            Assert.AreEqual ( (Func<Vector<double>, Matrix<double>>)Stiffness, solver.Info.Stiffness );
            Assert.AreEqual ( (Func<Vector<double>, Vector<double>>)Reaction, solver.Info.Reaction );
            Assert.AreEqual ( initialLoadFactor, solver.State.Lambda );
            Assert.AreEqual ( initialDisplacement, solver.State.Displacement );
            Assert.AreEqual ( typeof ( CorrectionSchemeArcLength ), solver.Corrector.Scheme.GetType ( ) );
            CorrectionSchemeArcLength scheme = (CorrectionSchemeArcLength)solver.Corrector.Scheme;
            Assert.AreEqual ( typeof ( AngleMethod ), scheme.DisplacementChooser.GetType ( ) );
            Assert.AreEqual ( maxIter, solver.Corrector.MaximumIterations );
        }
        [Test]
        public void StandardSolverIsCorrectlyBuild() {
            //Given 
            Vector<double> Reaction( Vector<double> u ) => new DenseVector ( 2 );

            Matrix<double> Stiffness( Vector<double> u ) => new DenseMatrix ( 2, 2 );
            Vector<double> initialLoad = new DenseVector ( 2 ) { [0] = 1, [1] = 1 };
            Vector<double> initialDisplacement = new DenseVector ( 2 );
            double initialLoadFactor = 1;
            Vector<double> referenceLoad = new DenseVector ( 2 ) { [0] = 1, [1] = 1 };
            double dispTol = 1e-3;
            double eqTol = 1e-3;
            double enTol = 1e-3;
            int maxIter = 10;
            double dlambda = 0.1;

            // When
            Solver.Solver solver = Solver.Solver.Builder
                .Solve ( Reaction )
                .WithStiffnessMatrix ( Stiffness )
                .Under ( referenceLoad )
                .WithInitialLoad ( initialLoad )
                .WithInitialLoadFactor ( initialLoadFactor )
                .WithInitialDisplacement ( initialDisplacement )
                .UntilTolerancesReached ( dispTol, eqTol, enTol )
                .WithMaximumCorrectionIterations ( maxIter )
                .UsingStandardNewtonRaphsonScheme ( dlambda )
                .Build ( );

            // Then
            Assert.AreEqual ( referenceLoad, solver.Info.ReferenceLoad );
            Assert.AreEqual ( initialLoad, solver.Info.InitialLoad );
            Assert.AreEqual ( (Func<Vector<double>, Matrix<double>>)Stiffness, solver.Info.Stiffness );
            Assert.AreEqual ( (Func<Vector<double>, Vector<double>>)Reaction, solver.Info.Reaction );
            Assert.AreEqual ( initialLoadFactor, solver.State.Lambda );
            Assert.AreEqual ( initialDisplacement, solver.State.Displacement );
            Assert.AreEqual ( typeof ( CorrectionSchemeStandard ), solver.Corrector.Scheme.GetType ( ) );
            Assert.AreEqual ( maxIter, solver.Corrector.MaximumIterations );
        }
        [Test]
        public void WorkControlSolverIsCorrectlyBuild() {
            //Given 
            Vector<double> Reaction( Vector<double> u ) => new DenseVector ( 2 );

            Matrix<double> Stiffness( Vector<double> u ) => new DenseMatrix ( 2, 2 );
            Vector<double> initialLoad = new DenseVector ( 2 ) { [0] = 1, [1] = 1 };
            Vector<double> initialDisplacement = new DenseVector ( 2 );
            double initialLoadFactor = 1;
            Vector<double> referenceLoad = new DenseVector ( 2 ) { [0] = 1, [1] = 1 };
            double dispTol = 1e-3;
            double eqTol = 1e-3;
            double enTol = 1e-3;
            double w = 1;
            int maxIter = 10;

            // When
            Solver.Solver solver = Solver.Solver.Builder
                .Solve ( Reaction )
                .WithStiffnessMatrix ( Stiffness )
                .Under ( referenceLoad )
                .WithInitialLoad ( initialLoad )
                .WithInitialLoadFactor ( initialLoadFactor )
                .WithInitialDisplacement ( initialDisplacement )
                .UntilTolerancesReached ( dispTol, eqTol, enTol )
                .UsingWorkControlScheme ( w )
                .WithMaximumCorrectionIterations ( maxIter )
                .Build ( );

            // Then
            Assert.AreEqual ( referenceLoad, solver.Info.ReferenceLoad );
            Assert.AreEqual ( initialLoad, solver.Info.InitialLoad );
            Assert.AreEqual ( (Func<Vector<double>, Matrix<double>>)Stiffness, solver.Info.Stiffness );
            Assert.AreEqual ( (Func<Vector<double>, Vector<double>>)Reaction, solver.Info.Reaction );
            Assert.AreEqual ( initialLoadFactor, solver.State.Lambda );
            Assert.AreEqual ( initialDisplacement, solver.State.Displacement );
            Assert.AreEqual ( typeof ( CorrectionSchemeWorkControl ), solver.Corrector.Scheme.GetType ( ) );
            CorrectionSchemeWorkControl scheme = (CorrectionSchemeWorkControl)solver.Corrector.Scheme;
            Assert.AreEqual ( w, scheme.WorkIncrement );
            Assert.AreEqual ( maxIter, solver.Corrector.MaximumIterations );
        }
        [Test]
        public void StandardSolverWithoutReactionLaunchesException() {
            //Given 
            Vector<double> initialLoad = new DenseVector ( 2 ) { [0] = 1, [1] = 1 };
            Vector<double> initialDisplacement = new DenseVector ( 2 );
            double initialLoadFactor = 1;
            Vector<double> referenceLoad = new DenseVector ( 2 ) { [0] = 1, [1] = 1 };
            double dispTol = 1e-3;
            double eqTol = 1e-3;
            double enTol = 1e-3;
            double dlambda = 0.1;

            bool exceptionLaunched = false;
            try {
                // When
                Solver.Solver solver = Solver.Solver.Builder
                    .Under ( referenceLoad )
                    .WithInitialLoad ( initialLoad )
                    .WithInitialLoadFactor ( initialLoadFactor )
                    .WithInitialDisplacement ( initialDisplacement )
                    .UntilTolerancesReached ( dispTol, eqTol, enTol )
                    .UsingStandardNewtonRaphsonScheme ( dlambda )
                    .Build ( );
            } catch (Exception e) {
                // Then
                Assert.AreEqual ( Strings.ReactionMustBeDefined, e.Message );
                exceptionLaunched = true;
            }
            Assert.IsTrue ( exceptionLaunched );
        }
        [Test]
        public void StandardSolverWithoutStiffnessLaunchesException() {
            //Given 
            Vector<double> Reaction( Vector<double> u ) => new DenseVector ( 2 );
            Vector<double> initialLoad = new DenseVector ( 2 ) { [0] = 1, [1] = 1 };
            Vector<double> initialDisplacement = new DenseVector ( 2 );
            double initialLoadFactor = 1;
            Vector<double> referenceLoad = new DenseVector ( 2 ) { [0] = 1, [1] = 1 };
            double dispTol = 1e-3;
            double eqTol = 1e-3;
            double enTol = 1e-3;
            double dlambda = 0.1;

            bool exceptionLaunched = false;
            try {
                // When
                Solver.Solver solver = Solver.Solver.Builder
                    .Solve ( Reaction )
                    .Under ( referenceLoad )
                    .WithInitialLoad ( initialLoad )
                    .WithInitialLoadFactor ( initialLoadFactor )
                    .WithInitialDisplacement ( initialDisplacement )
                    .UntilTolerancesReached ( dispTol, eqTol, enTol )
                    .UsingStandardNewtonRaphsonScheme ( dlambda )
                    .Build ( );
            } catch (Exception e) {
                // Then
                Assert.AreEqual ( Strings.StiffnessLoadMustBeDefined, e.Message );
                exceptionLaunched = true;
            }
            Assert.IsTrue ( exceptionLaunched );
        }
        [Test]
        public void StandardSolverWithoutReferenceLoadLaunchesException() {
            //Given 
            Vector<double> Reaction( Vector<double> u ) => new DenseVector ( 2 );
            Matrix<double> Stiffness( Vector<double> u ) => new DenseMatrix ( 2, 2 );
            double dispTol = 1e-3;
            double eqTol = 1e-3;
            double enTol = 1e-3;
            double dlambda = 0.1;

            bool exceptionLaunched = false;
            try {
                // When
                Solver.Solver solver = Solver.Solver.Builder
                    .Solve ( Reaction )
                    .WithStiffnessMatrix ( Stiffness )
                    .UntilTolerancesReached ( dispTol, eqTol, enTol )
                    .UsingStandardNewtonRaphsonScheme ( dlambda )
                    .Build ( );
            } catch (Exception e) {
                // Then
                Assert.AreEqual ( Strings.ReferenceLoadMustBeDefined, e.Message );
                exceptionLaunched = true;
            }
            Assert.IsTrue ( exceptionLaunched );
        }
        [Test]
        public void StandardSolverWithoutDefinedStopConditionsHasDefaultStopConditions() {
            //Given 
            Vector<double> Reaction( Vector<double> u ) => new DenseVector ( 2 );
            Matrix<double> Stiffness( Vector<double> u ) => new DenseMatrix ( 2, 2 );
            Vector<double> initialLoad = new DenseVector ( 2 ) { [0] = 1, [1] = 1 };
            Vector<double> initialDisplacement = new DenseVector ( 2 );
            double initialLoadFactor = 1;
            Vector<double> referenceLoad = new DenseVector ( 2 ) { [0] = 1, [1] = 1 };
            double dlambda = 0.1;

            // When
            Solver.Solver solver = Solver.Solver.Builder
                .Solve ( Reaction )
                .WithStiffnessMatrix ( Stiffness )
                .Under ( referenceLoad )
                .WithInitialLoad ( initialLoad )
                .WithInitialLoadFactor ( initialLoadFactor )
                .WithInitialDisplacement ( initialDisplacement )
                .UsingStandardNewtonRaphsonScheme ( dlambda )
                .Build ( );

            // Then
            Assert.AreEqual ( 10, solver.Corrector.MaximumIterations );
            bool allVerify = new[] { 1e-3, 1e-3, 1e-3 }
                .Select ( ( tol, index ) => new {
                    ExpectedTolerance = tol,
                    Tolerance = solver.Corrector.Tolerances[index]
                } )
                .All ( tol => tol.ExpectedTolerance == tol.Tolerance );
            Assert.IsTrue ( allVerify );
        }
        [Test]
        public void SolverWithoutDefinedIterationSchemeLaunchesException() {
            //Given 
            Vector<double> Reaction( Vector<double> u ) => new DenseVector ( 2 );
            Matrix<double> Stiffness( Vector<double> u ) => new DenseMatrix ( 2, 2 );
            Vector<double> initialLoad = new DenseVector ( 2 ) { [0] = 1, [1] = 1 };
            Vector<double> initialDisplacement = new DenseVector ( 2 );
            double initialLoadFactor = 1;
            Vector<double> referenceLoad = new DenseVector ( 2 ) { [0] = 1, [1] = 1 };

            bool exceptionLaunched = false;
            try {
                // When
                Solver.Solver solver = Solver.Solver.Builder
                    .Solve ( Reaction )
                    .WithStiffnessMatrix ( Stiffness )
                    .Under ( referenceLoad )
                    .WithInitialLoad ( initialLoad )
                    .WithInitialLoadFactor ( initialLoadFactor )
                    .WithInitialDisplacement ( initialDisplacement )
                    .Build ( );
            } catch (Exception e) {
                // Then
                Assert.AreEqual ( Strings.SchemeMustBeDefined, e.Message );
                exceptionLaunched = true;
            }
            Assert.IsTrue ( exceptionLaunched );
        }
        [Test]
        public void StandardSolverWithoutDefinedLoadFactorIncrementLaunchesException() {
            //Given 
            Vector<double> Reaction( Vector<double> u ) => new DenseVector ( 2 );
            Matrix<double> Stiffness( Vector<double> u ) => new DenseMatrix ( 2, 2 );
            Vector<double> initialLoad = new DenseVector ( 2 ) { [0] = 1, [1] = 1 };
            Vector<double> initialDisplacement = new DenseVector ( 2 );
            double initialLoadFactor = 1;
            Vector<double> referenceLoad = new DenseVector ( 2 ) { [0] = 1, [1] = 1 };

            bool exceptionLaunched = false;
            try {
                // When
                Solver.Solver solver = Solver.Solver.Builder
                    .Solve ( Reaction )
                    .WithStiffnessMatrix ( Stiffness )
                    .Under ( referenceLoad )
                    .WithInitialLoad ( initialLoad )
                    .WithInitialLoadFactor ( initialLoadFactor )
                    .WithInitialDisplacement ( initialDisplacement )
                    .UsingStandardNewtonRaphsonScheme ( 0 )
                    .Build ( );
            } catch (Exception e) {
                // Then
                Assert.AreEqual ( Strings.DefaultLoadFactorIncrementMustBeDefined, e.Message );
                exceptionLaunched = true;
            }
            Assert.IsTrue ( exceptionLaunched );
        }
        [Test]
        public void ArcLengthSolverWithoutDefinedRadiusLaunchesException() {
            //Given 
            Vector<double> Reaction( Vector<double> u ) => new DenseVector ( 2 );
            Matrix<double> Stiffness( Vector<double> u ) => new DenseMatrix ( 2, 2 );
            Vector<double> initialLoad = new DenseVector ( 2 ) { [0] = 1, [1] = 1 };
            Vector<double> initialDisplacement = new DenseVector ( 2 );
            double initialLoadFactor = 1;
            Vector<double> referenceLoad = new DenseVector ( 2 ) { [0] = 1, [1] = 1 };

            bool exceptionLaunched = false;
            try {
                // When
                Solver.Solver solver = Solver.Solver.Builder
                    .Solve ( Reaction )
                    .WithStiffnessMatrix ( Stiffness )
                    .Under ( referenceLoad )
                    .WithInitialLoad ( initialLoad )
                    .WithInitialLoadFactor ( initialLoadFactor )
                    .WithInitialDisplacement ( initialDisplacement )
                    .UsingArcLengthScheme ( 0 )
                    .Build ( );
            } catch (Exception e) {
                // Then
                Assert.AreEqual ( Strings.ArcLengthRadiusMustBeDefined, e.Message );
                exceptionLaunched = true;
            }
            Assert.IsTrue ( exceptionLaunched );
        }
        [Test]
        public void WorkControlSolverWithoutDefinedWorkLaunchesException() {
            //Given 
            Vector<double> Reaction( Vector<double> u ) => new DenseVector ( 2 );
            Matrix<double> Stiffness( Vector<double> u ) => new DenseMatrix ( 2, 2 );
            Vector<double> initialLoad = new DenseVector ( 2 ) { [0] = 1, [1] = 1 };
            Vector<double> initialDisplacement = new DenseVector ( 2 );
            double initialLoadFactor = 1;
            Vector<double> referenceLoad = new DenseVector ( 2 ) { [0] = 1, [1] = 1 };

            bool exceptionLaunched = false;
            try {
                // When
                Solver.Solver solver = Solver.Solver.Builder
                    .Solve ( Reaction )
                    .WithStiffnessMatrix ( Stiffness )
                    .Under ( referenceLoad )
                    .WithInitialLoad ( initialLoad )
                    .WithInitialLoadFactor ( initialLoadFactor )
                    .WithInitialDisplacement ( initialDisplacement )
                    .UsingWorkControlScheme ( 0 )
                    .Build ( );
            } catch (Exception e) {
                // Then
                Assert.AreEqual ( Strings.WorkIncrementMustBeDefined, e.Message );
                exceptionLaunched = true;
            }
            Assert.IsTrue ( exceptionLaunched );
        }
    }
}