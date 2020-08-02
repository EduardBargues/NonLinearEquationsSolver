using System;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace NLES
{
    internal class StructureInfo
    {
        internal Func<Vector<double>, Vector<double>> Reaction { get; set; }
        internal Vector<double> ReferenceLoad { get; set; }
        internal Vector<double> InitialLoad { get; set; }
        internal Func<Vector<double>, Matrix<double>> Stiffness { get; set; }
    }
}
