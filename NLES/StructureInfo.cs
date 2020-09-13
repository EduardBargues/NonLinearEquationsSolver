using System;

namespace NLES
{
    internal class StructureInfo
    {
        internal Func<Vector, Vector> Reaction { get; set; }
        internal Vector ReferenceLoad { get; set; }
        internal Vector InitialLoad { get; set; }
        internal Func<Vector, ILinearSolver> Stiffness { get; set; }
    }
}
