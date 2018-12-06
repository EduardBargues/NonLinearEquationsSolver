using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NonLinearEquationsSolver {
    [TestFixture]
    internal class TestSolverResults {
        readonly double tolerance = 1e-3;
        readonly double numberTolerance = 1e-10;

        [Test]
        public void SolveLinearFunction() {
            Vector<double> Function( Vector<double> u ) {
                return new DenseVector ( 2 ) { [0] = u[0], [1] = u[0] + 2 * u[1] };
            }
            Matrix<double> Stiffness( Vector<double> u ) {
                return new DenseMatrix ( 2, 2 ) { [0, 0] = 1, [1, 0] = 1, [1, 1] = 2 };
            }
            DenseVector force = new DenseVector ( 2 ) { [0] = 1, [1] = 3 };
            Solver solver = Solver.Builder
                .Solve ( 2, Function, Stiffness )
                .Under ( force )
                .Build ( );
            List<LoadState> states = solver.Broadcast ( ).Take ( 2 ).ToList ( );
            AssertSolutionsAreCorrect ( Function, force, states );
        }

        [Test]
        public void SolveLinearFunctionSmallIncrements() {
            Vector<double> Function( Vector<double> u ) {
                return new DenseVector ( 2 ) { [0] = u[0], [1] = u[0] + 2 * u[1] };
            }
            Matrix<double> Stiffness( Vector<double> u ) {
                return new DenseMatrix ( 2, 2 ) { [0, 0] = 1, [1, 0] = 1, [1, 1] = 2 };
            }
            DenseVector force = new DenseVector ( 2 ) { [0] = 1, [1] = 3 };
            double inc = 1e-2;
            Solver solver = Solver.Builder
                .Solve ( 2, Function, Stiffness )
                .Under ( force )
                .UsingStandardNewtonRaphsonScheme ( inc )
                .Build ( );
            List<LoadState> states = solver.Broadcast ( ).TakeWhile ( s => s.Lambda <= 1 ).ToList ( );
            Assert.AreEqual ( (int)(1 / inc) - 1, states.Count );
            AssertSolutionsAreCorrect ( Function, force, states );
        }

        [Test]
        public void SolveLinearFunctionArcLength() {
            Vector<double> Function( Vector<double> u ) {
                return new DenseVector ( 2 ) { [0] = u[0], [1] = u[0] + 2 * u[1] };
            }
            Matrix<double> Stiffness( Vector<double> u ) {
                return new DenseMatrix ( 2, 2 ) { [0, 0] = 1, [1, 0] = 1, [1, 1] = 2 };
            }
            DenseVector force = new DenseVector ( 2 ) { [0] = 1, [1] = 3 };
            double radius = 1e-2;
            Solver solver = Solver.Builder
                .Solve ( 2, Function, Stiffness )
                .Under ( force )
                .UsingArcLengthScheme ( radius )
                .Build ( );
            List<LoadState> states = solver.Broadcast ( ).TakeWhile ( s => s.Lambda <= 1 ).ToList ( );
            AssertSolutionsAreCorrect ( Function, force, states );
        }

        [Test]
        public void SolveQuadraticFunction() {
            Vector<double> Function( Vector<double> u ) {
                return new DenseVector ( 2 ) {
                    [0] = u[0] * u[0] + 2 * u[1] * u[1],
                    [1] = 2 * u[0] * u[0] + u[1] * u[1]
                };
            }
            Matrix<double> Stiffness( Vector<double> u ) {
                return new DenseMatrix ( 2, 2 ) {
                    [0, 0] = 2 * u[0],
                    [0, 1] = 4 * u[1],
                    [1, 0] = 4 * u[0],
                    [1, 1] = 2 * u[1]
                };
            }
            DenseVector force = new DenseVector ( 2 ) { [0] = 3, [1] = 3 };
            Solver solver = Solver.Builder
                .Solve ( 2, Function, Stiffness )
                .Under ( force )
                .WithInitialConditions ( 0.1, DenseVector.Create ( 2, 0 ), DenseVector.Create ( 2, 1 ) )
                .Build ( );
            List<LoadState> states = solver.Broadcast ( ).TakeWhile ( x => x.Lambda <= 1 ).ToList ( );
            AssertSolutionsAreCorrect ( Function, force, states );
        }

        [Test]
        public void SolveQuadraticFunctionSmallIncrements() {
            Vector<double> Function( Vector<double> u ) {
                return new DenseVector ( 2 ) {
                    [0] = u[0] * u[0] + 2 * u[1] * u[1],
                    [1] = 2 * u[0] * u[0] + u[1] * u[1]
                };
            }
            Matrix<double> Stiffness( Vector<double> u ) {
                return new DenseMatrix ( 2, 2 ) {
                    [0, 0] = 2 * u[0],
                    [0, 1] = 4 * u[1],
                    [1, 0] = 4 * u[0],
                    [1, 1] = 2 * u[1]
                };
            }
            DenseVector force = new DenseVector ( 2 ) { [0] = 3, [1] = 3 };
            Solver solver = Solver.Builder
                .Solve ( 2, Function, Stiffness )
                .Under ( force )
                .WithInitialConditions ( 0.1, DenseVector.Create ( 2, 0 ), DenseVector.Create ( 2, 1 ) )
                .UsingStandardNewtonRaphsonScheme ( 0.01 )
                .Build ( );
            List<LoadState> states = solver.Broadcast ( ).TakeWhile ( x => x.Lambda <= 1 ).ToList ( );
            AssertSolutionsAreCorrect ( Function, force, states );
        }

        [Test]
        public void SolveQuadraticFunctionArcLength() {
            Vector<double> Function( Vector<double> u ) {
                return new DenseVector ( 2 ) {
                    [0] = u[0] * u[0] + 2 * u[1] * u[1],
                    [1] = 2 * u[0] * u[0] + u[1] * u[1]
                };
            }
            Matrix<double> Stiffness( Vector<double> u ) {
                return new DenseMatrix ( 2, 2 ) {
                    [0, 0] = 2 * u[0],
                    [0, 1] = 4 * u[1],
                    [1, 0] = 4 * u[0],
                    [1, 1] = 2 * u[1]
                };
            }
            DenseVector force = new DenseVector ( 2 ) { [0] = 3, [1] = 3 };
            Solver solver = Solver.Builder
                .Solve ( 2, Function, Stiffness )
                .Under ( force )
                .WithInitialConditions ( 0.1, DenseVector.Create ( 2, 0 ), DenseVector.Create ( 2, 1 ) )
                .UsingArcLengthScheme ( 0.05 )
                .NormalizeLoadWith ( 0.01 )
                .Build ( );
            List<LoadState> states = solver.Broadcast ( ).TakeWhile ( x => x.Lambda <= 10 ).ToList ( );
            AssertSolutionsAreCorrect ( Function, force, states );
        }

        void AssertSolutionIsCorrect( Vector<double> solution ) {
            double first = solution.First ( );
            foreach (double d in solution) {
                Assert.AreEqual ( first, d, numberTolerance );
            }
        }
        void AssertSolutionsAreCorrect( Func<Vector<double>, Vector<double>> reaction, Vector<double> force, List<LoadState> states ) {
            foreach (LoadState state in states) {
                AssertSolutionIsCorrect ( state.Displacement );
                TestUtils.AssertAreCloseEnough ( reaction ( state.Displacement ), state.Lambda * force, tolerance );
            }
        }
    }
}
