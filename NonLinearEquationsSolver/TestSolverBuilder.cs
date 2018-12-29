using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using NUnit.Framework;
using System;
using System.Linq;

namespace NonLinearEquationsSolver {
    [TestFixture]
    internal class TestSolverBuilder {

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
            SolverND solverNd = SolverND.NdBuilder
                .Solve ( 2, Reaction, Stiffness )
                .Under ( referenceLoad )
                .WithInitialConditions ( initialLoadFactor, initialLoad, initialDisplacement )
                .UntilTolerancesReached ( dispTol, eqTol, enTol )
                .WithMaximumCorrectionIterations ( maxIter )
                .UsingArcLengthScheme ( radius )
                .NormalizeLoadWith ( beta )
                .WithRestoringMethodInCorrectionPhase ( )
                .Build ( );

            // Then
            Assert.AreEqual ( referenceLoad, solverNd.Info.ReferenceLoad );
            Assert.AreEqual ( initialLoad, solverNd.Info.InitialLoad );
            Assert.AreEqual ( (Func<Vector<double>, Matrix<double>>)Stiffness, solverNd.Info.Stiffness );
            Assert.AreEqual ( (Func<Vector<double>, Vector<double>>)Reaction, solverNd.Info.Reaction );
            Assert.AreEqual ( initialLoadFactor, solverNd.State.Lambda );
            Assert.AreEqual ( initialDisplacement, solverNd.State.Displacement );
            Assert.AreEqual ( typeof ( CorrectionSchemeArcLength ), solverNd.Corrector.Scheme.GetType ( ) );
            CorrectionSchemeArcLength scheme = (CorrectionSchemeArcLength)solverNd.Corrector.Scheme;
            Assert.AreEqual ( typeof ( RestoringMethod ), scheme.DisplacementChooser.GetType ( ) );
            Assert.AreEqual ( maxIter, solverNd.Corrector.MaximumIterations );
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
            SolverND solverNd = SolverND.NdBuilder
                .Solve ( 2, Reaction, Stiffness )
                .Under ( referenceLoad )
                .WithInitialConditions ( initialLoadFactor, initialLoad, initialDisplacement )
                .UntilTolerancesReached ( dispTol, eqTol, enTol )
                .WithMaximumCorrectionIterations ( maxIter )
                .UsingArcLengthScheme ( radius )
                .NormalizeLoadWith ( beta )
                .WithAngleMethodInCorrectionPhase ( )
                .Build ( );

            // Then
            Assert.AreEqual ( referenceLoad, solverNd.Info.ReferenceLoad );
            Assert.AreEqual ( initialLoad, solverNd.Info.InitialLoad );
            Assert.AreEqual ( (Func<Vector<double>, Matrix<double>>)Stiffness, solverNd.Info.Stiffness );
            Assert.AreEqual ( (Func<Vector<double>, Vector<double>>)Reaction, solverNd.Info.Reaction );
            Assert.AreEqual ( initialLoadFactor, solverNd.State.Lambda );
            Assert.AreEqual ( initialDisplacement, solverNd.State.Displacement );
            Assert.AreEqual ( typeof ( CorrectionSchemeArcLength ), solverNd.Corrector.Scheme.GetType ( ) );
            CorrectionSchemeArcLength scheme = (CorrectionSchemeArcLength)solverNd.Corrector.Scheme;
            Assert.AreEqual ( typeof ( AngleMethod ), scheme.DisplacementChooser.GetType ( ) );
            Assert.AreEqual ( maxIter, solverNd.Corrector.MaximumIterations );
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
            SolverND solverNd = SolverND.NdBuilder
                .Solve ( 2, Reaction, Stiffness )
                .Under ( referenceLoad )
                .WithInitialConditions ( initialLoadFactor, initialLoad, initialDisplacement )
                .UntilTolerancesReached ( dispTol, eqTol, enTol )
                .WithMaximumCorrectionIterations ( maxIter )
                .UsingStandardNewtonRaphsonScheme ( dlambda )
                .Build ( );

            // Then
            Assert.AreEqual ( referenceLoad, solverNd.Info.ReferenceLoad );
            Assert.AreEqual ( initialLoad, solverNd.Info.InitialLoad );
            Assert.AreEqual ( (Func<Vector<double>, Matrix<double>>)Stiffness, solverNd.Info.Stiffness );
            Assert.AreEqual ( (Func<Vector<double>, Vector<double>>)Reaction, solverNd.Info.Reaction );
            Assert.AreEqual ( initialLoadFactor, solverNd.State.Lambda );
            Assert.AreEqual ( initialDisplacement, solverNd.State.Displacement );
            Assert.AreEqual ( typeof ( CorrectionSchemeStandard ), solverNd.Corrector.Scheme.GetType ( ) );
            Assert.AreEqual ( maxIter, solverNd.Corrector.MaximumIterations );
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
            SolverND solverNd = SolverND.NdBuilder
                .Solve ( 2, Reaction, Stiffness )
                .Under ( referenceLoad )
                .WithInitialConditions ( initialLoadFactor, initialLoad, initialDisplacement )
                .UntilTolerancesReached ( dispTol, eqTol, enTol )
                .UsingWorkControlScheme ( w )
                .WithMaximumCorrectionIterations ( maxIter )
                .Build ( );

            // Then
            Assert.AreEqual ( referenceLoad, solverNd.Info.ReferenceLoad );
            Assert.AreEqual ( initialLoad, solverNd.Info.InitialLoad );
            Assert.AreEqual ( (Func<Vector<double>, Matrix<double>>)Stiffness, solverNd.Info.Stiffness );
            Assert.AreEqual ( (Func<Vector<double>, Vector<double>>)Reaction, solverNd.Info.Reaction );
            Assert.AreEqual ( initialLoadFactor, solverNd.State.Lambda );
            Assert.AreEqual ( initialDisplacement, solverNd.State.Displacement );
            Assert.AreEqual ( typeof ( CorrectionSchemeWorkControl ), solverNd.Corrector.Scheme.GetType ( ) );
            CorrectionSchemeWorkControl scheme = (CorrectionSchemeWorkControl)solverNd.Corrector.Scheme;
            Assert.AreEqual ( w, scheme.WorkIncrement );
            Assert.AreEqual ( maxIter, solverNd.Corrector.MaximumIterations );
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
                SolverND solverNd = SolverND.NdBuilder
                    .Under ( referenceLoad )
                    .WithInitialConditions ( initialLoadFactor, initialLoad, initialDisplacement )
                    .UntilTolerancesReached ( dispTol, eqTol, enTol )
                    .UsingStandardNewtonRaphsonScheme ( dlambda )
                    .Build ( );
            } catch (Exception) {
                // Then
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
            SolverND solverNd = SolverND.NdBuilder
                .Solve ( 2, Reaction, Stiffness )
                .Under ( referenceLoad )
                .WithInitialConditions ( initialLoadFactor, initialLoad, initialDisplacement )
                .UsingStandardNewtonRaphsonScheme ( dlambda )
                .Build ( );

            // Then
            Assert.AreEqual ( 50, solverNd.Corrector.MaximumIterations );
            bool allVerify = new[] { 1e-3, 1e-3, 1e-3 }
                .Select ( ( tol, index ) => new {
                    ExpectedTolerance = tol,
                    Tolerance = solverNd.Corrector.Tolerances[index]
                } )
                .All ( tol => tol.ExpectedTolerance == tol.Tolerance );
            Assert.IsTrue ( allVerify );
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
                SolverND solverNd = SolverND.NdBuilder
                    .Solve ( 2, Reaction, Stiffness )
                    .Under ( referenceLoad )
                    .WithInitialConditions ( initialLoadFactor, initialLoad, initialDisplacement )
                    .UsingStandardNewtonRaphsonScheme ( 0 )
                    .Build ( );
            } catch (Exception) {
                // Then
                //Assert.AreEqual ( Strings.DefaultLoadFactorIncrementMustBeDefined, e.Message );
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
                SolverND solverNd = SolverND.NdBuilder
                    .Solve ( 2, Reaction, Stiffness )
                    .Under ( referenceLoad )
                    .WithInitialConditions ( initialLoadFactor, initialLoad, initialDisplacement )
                    .UsingArcLengthScheme ( 0 )
                    .Build ( );
            } catch (Exception) {
                // Then
                //Assert.AreEqual ( Strings.ArcLengthRadiusMustBeDefined, e.Message );
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
                SolverND solverNd = SolverND.NdBuilder
                    .Solve ( 2, Reaction, Stiffness )
                    .Under ( referenceLoad )
                    .WithInitialConditions ( initialLoadFactor, initialLoad, initialDisplacement )
                    .UsingWorkControlScheme ( 0 )
                    .Build ( );
            } catch (Exception) {
                // Then
                //Assert.AreEqual ( Strings.WorkIncrementMustBeDefined, e.Message );
                exceptionLaunched = true;
            }
            Assert.IsTrue ( exceptionLaunched );
        }
    }
}
