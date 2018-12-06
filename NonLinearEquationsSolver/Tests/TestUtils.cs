using MathNet.Numerics.LinearAlgebra;
using NUnit.Framework;
using System;

namespace NonLinearEquationsSolver.Tests {
    public static class TestUtils {
        public static void AssertAreCloseEnough( double x, double y, double tolerance ) => Assert.IsTrue ( Math.Abs ( x - y ) <= tolerance );
        public static void AssertAreCloseEnough( Vector<double> v1, Vector<double> v2, double tolerance ) => Assert.IsTrue ( (v1 - v2).Norm ( 2 ) <= tolerance );
    }
}
