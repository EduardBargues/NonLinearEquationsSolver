using System;
using System.Linq;

using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

using NLES;
using NLES.Correction;
using NLES.Correction.Methods;
using NLES.Tests;

using Xunit;

namespace NonLinearEquationsSolver
{
    public class Builder
    {
        [Fact]
        public void RestoringArcLengthSolverIsCorrectlyBuild()
        {
            //Given 
            static Vector<double> Reaction(Vector<double> u) => new DenseVector(2);

            static ILinearSolver Stiffness(Vector<double> u)
                => new LinearSolverForTesting(new DenseMatrix(2, 2));
            Vector<double> initialLoad = new DenseVector(2) { [0] = 1, [1] = 1 };
            Vector<double> initialDisplacement = new DenseVector(2);
            double initialLoadFactor = 1;
            Vector<double> referenceLoad = new DenseVector(2) { [0] = 1, [1] = 1 };
            double dispTol = 1e-3;
            double eqTol = 1e-3;
            double enTol = 1e-3;
            double radius = 1;
            double beta = 100;
            int maxIter = 10;

            // When
            NonLinearSolver solver = NonLinearSolver.Builder
                .Solve(2, Reaction, Stiffness)
                .Under(referenceLoad)
                .WithInitialConditions(initialLoadFactor, initialLoad, initialDisplacement)
                .UntilTolerancesReached(dispTol, eqTol, enTol)
                .WithMaximumCorrectionIterations(maxIter)
                .UsingArcLengthScheme(radius)
                .NormalizeLoadWith(beta)
                .WithRestoringMethodInCorrectionPhase()
                .Build();

            // Then
            Assert.Equal(referenceLoad, solver.Info.ReferenceLoad);
            Assert.Equal(initialLoad, solver.Info.InitialLoad);
            Assert.Equal(Stiffness, solver.Info.Stiffness);
            Assert.Equal(Reaction, solver.Info.Reaction);
            Assert.Equal(initialLoadFactor, solver.State.Lambda);
            Assert.Equal(initialDisplacement, solver.State.Displacement);
            Assert.Equal(typeof(CorrectionSchemeArcLength), solver.Corrector.Scheme.GetType());
            CorrectionSchemeArcLength scheme = (CorrectionSchemeArcLength)solver.Corrector.Scheme;
            Assert.Equal(typeof(RestoringMethod), scheme.DisplacementSelector.GetType());
            Assert.Equal(maxIter, solver.Corrector.MaximumIterations);
        }
        [Fact]
        public void AngleArcLengthSolverIsCorrectlyBuild()
        {
            //Given 
            static Vector<double> Reaction(Vector<double> u) => new DenseVector(2);

            static ILinearSolver Stiffness(Vector<double> u)
                => new LinearSolverForTesting(new DenseMatrix(2, 2));
            Vector<double> initialLoad = new DenseVector(2) { [0] = 1, [1] = 1 };
            Vector<double> initialDisplacement = new DenseVector(2);
            double initialLoadFactor = 1;
            Vector<double> referenceLoad = new DenseVector(2) { [0] = 1, [1] = 1 };
            double dispTol = 1e-3;
            double eqTol = 1e-3;
            double enTol = 1e-3;
            double radius = 1;
            double beta = 100;
            int maxIter = 10;

            // When
            NonLinearSolver Solver = NonLinearSolver.Builder
                .Solve(2, Reaction, Stiffness)
                .Under(referenceLoad)
                .WithInitialConditions(initialLoadFactor, initialLoad, initialDisplacement)
                .UntilTolerancesReached(dispTol, eqTol, enTol)
                .WithMaximumCorrectionIterations(maxIter)
                .UsingArcLengthScheme(radius)
                .NormalizeLoadWith(beta)
                .WithAngleMethodInCorrectionPhase()
                .Build();

            // Then
            Assert.Equal(referenceLoad, Solver.Info.ReferenceLoad);
            Assert.Equal(initialLoad, Solver.Info.InitialLoad);
            Assert.Equal(Stiffness, Solver.Info.Stiffness);
            Assert.Equal(Reaction, Solver.Info.Reaction);
            Assert.Equal(initialLoadFactor, Solver.State.Lambda);
            Assert.Equal(initialDisplacement, Solver.State.Displacement);
            Assert.Equal(typeof(CorrectionSchemeArcLength), Solver.Corrector.Scheme.GetType());
            CorrectionSchemeArcLength scheme = (CorrectionSchemeArcLength)Solver.Corrector.Scheme;
            Assert.Equal(typeof(AngleMethod), scheme.DisplacementSelector.GetType());
            Assert.Equal(maxIter, Solver.Corrector.MaximumIterations);
        }
        [Fact]
        public void StandardSolverIsCorrectlyBuild()
        {
            //Given 
            static Vector<double> Reaction(Vector<double> u) => new DenseVector(2);

            static ILinearSolver Stiffness(Vector<double> u)
                => new LinearSolverForTesting(new DenseMatrix(2, 2));
            Vector<double> initialLoad = new DenseVector(2) { [0] = 1, [1] = 1 };
            Vector<double> initialDisplacement = new DenseVector(2);
            double initialLoadFactor = 1;
            Vector<double> referenceLoad = new DenseVector(2) { [0] = 1, [1] = 1 };
            double dispTol = 1e-3;
            double eqTol = 1e-3;
            double enTol = 1e-3;
            int maxIter = 10;
            double dlambda = 0.1;

            // When
            NonLinearSolver Solver = NonLinearSolver.Builder
                .Solve(2, Reaction, Stiffness)
                .Under(referenceLoad)
                .WithInitialConditions(initialLoadFactor, initialLoad, initialDisplacement)
                .UntilTolerancesReached(dispTol, eqTol, enTol)
                .WithMaximumCorrectionIterations(maxIter)
                .UsingStandardNewtonRaphsonScheme(dlambda)
                .Build();

            // Then
            Assert.Equal(referenceLoad, Solver.Info.ReferenceLoad);
            Assert.Equal(initialLoad, Solver.Info.InitialLoad);
            Assert.Equal(Stiffness, Solver.Info.Stiffness);
            Assert.Equal(Reaction, Solver.Info.Reaction);
            Assert.Equal(initialLoadFactor, Solver.State.Lambda);
            Assert.Equal(initialDisplacement, Solver.State.Displacement);
            Assert.Equal(typeof(CorrectionSchemeStandard), Solver.Corrector.Scheme.GetType());
            Assert.Equal(maxIter, Solver.Corrector.MaximumIterations);
        }

        [Fact]
        public void StandardSolverWithoutReactionDoesNotLaunchException()
        {
            //Given 
            Vector<double> initialLoad = new DenseVector(2) { [0] = 1, [1] = 1 };
            Vector<double> initialDisplacement = new DenseVector(2);
            double initialLoadFactor = 1;
            Vector<double> referenceLoad = new DenseVector(2) { [0] = 1, [1] = 1 };
            double dispTol = 1e-3;
            double eqTol = 1e-3;
            double enTol = 1e-3;
            double dlambda = 0.1;

            bool exceptionLaunched = false;
            try
            {
                // When
                NonLinearSolver Solver = NonLinearSolver.Builder
                    .Solve(2, v => v, v => new LinearSolverForTesting(new DenseMatrix(2)))
                    .Under(referenceLoad)
                    .WithInitialConditions(initialLoadFactor, initialLoad, initialDisplacement)
                    .UntilTolerancesReached(dispTol, eqTol, enTol)
                    .UsingStandardNewtonRaphsonScheme(dlambda)
                    .Build();
            }
            catch (Exception)
            {
                // Then
                exceptionLaunched = true;
            }
            Assert.False(exceptionLaunched);
        }

        [Fact]
        public void StandardSolverWithoutDefinedStopConditionsHasDefaultStopConditions()
        {
            //Given 
            static Vector<double> Reaction(Vector<double> u) => new DenseVector(2);

            static ILinearSolver Stiffness(Vector<double> u)
                => new LinearSolverForTesting(new DenseMatrix(2, 2));
            Vector<double> initialLoad = new DenseVector(2) { [0] = 1, [1] = 1 };
            Vector<double> initialDisplacement = new DenseVector(2);
            double initialLoadFactor = 1;
            Vector<double> referenceLoad = new DenseVector(2) { [0] = 1, [1] = 1 };
            double dlambda = 0.1;

            // When
            NonLinearSolver Solver = NonLinearSolver.Builder
                .Solve(2, Reaction, Stiffness)
                .Under(referenceLoad)
                .WithInitialConditions(initialLoadFactor, initialLoad, initialDisplacement)
                .UsingStandardNewtonRaphsonScheme(dlambda)
                .Build();

            // Then
            Assert.Equal(50, Solver.Corrector.MaximumIterations);
            bool allVerify = new[] { 1e-3, 1e-3, 1e-3 }
                .Select((tol, index) => new
                {
                    ExpectedTolerance = tol,
                    Tolerance = Solver.Corrector.Tolerances[index]
                })
                .All(tol => tol.ExpectedTolerance == tol.Tolerance);
            Assert.True(allVerify);
        }
        [Fact]
        public void StandardSolverWithoutDefinedLoadFactorIncrementLaunchesException()
        {
            //Given 
            static Vector<double> Reaction(Vector<double> u) => new DenseVector(2);

            static ILinearSolver Stiffness(Vector<double> u)
                => new LinearSolverForTesting(new DenseMatrix(2, 2));
            Vector<double> initialLoad = new DenseVector(2) { [0] = 1, [1] = 1 };
            Vector<double> initialDisplacement = new DenseVector(2);
            double initialLoadFactor = 1;
            Vector<double> referenceLoad = new DenseVector(2) { [0] = 1, [1] = 1 };

            bool exceptionLaunched = false;
            try
            {
                // When
                NonLinearSolver Solver = NonLinearSolver.Builder
                    .Solve(2, Reaction, Stiffness)
                    .Under(referenceLoad)
                    .WithInitialConditions(initialLoadFactor, initialLoad, initialDisplacement)
                    .UsingStandardNewtonRaphsonScheme(0)
                    .Build();
            }
            catch (Exception)
            {
                // Then
                exceptionLaunched = true;
            }
            Assert.True(exceptionLaunched);
        }
        [Fact]
        public void ArcLengthSolverWithoutDefinedRadiusLaunchesException()
        {
            //Given 
            static Vector<double> Reaction(Vector<double> u) => new DenseVector(2);

            static ILinearSolver Stiffness(Vector<double> u)
                => new LinearSolverForTesting(new DenseMatrix(2, 2));
            Vector<double> initialLoad = new DenseVector(2) { [0] = 1, [1] = 1 };
            Vector<double> initialDisplacement = new DenseVector(2);
            double initialLoadFactor = 1;
            Vector<double> referenceLoad = new DenseVector(2) { [0] = 1, [1] = 1 };

            bool exceptionLaunched = false;
            try
            {
                // When
                NonLinearSolver Solver = NonLinearSolver.Builder
                    .Solve(2, Reaction, Stiffness)
                    .Under(referenceLoad)
                    .WithInitialConditions(initialLoadFactor, initialLoad, initialDisplacement)
                    .UsingArcLengthScheme(0)
                    .Build();
            }
            catch (Exception)
            {
                // Then
                exceptionLaunched = true;
            }
            Assert.True(exceptionLaunched);
        }
    }
}
