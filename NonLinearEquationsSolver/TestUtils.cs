using MathNet.Numerics.LinearAlgebra;
using NUnit.Framework;

namespace NonLinearEquationsSolver {
    internal static class TestUtils {
        public static void AssertAreCloseEnough( Vector<double> v1, Vector<double> v2, double tolerance ) => Assert.IsTrue ( (v1 - v2).Norm ( 2 ) <= tolerance );
    }
}
