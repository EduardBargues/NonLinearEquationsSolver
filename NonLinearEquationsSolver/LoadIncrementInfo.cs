using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace NonLinearEquationsSolver
{
    public class LoadIncrementInfo
    {
        public Vector<double> Displacement { get; set; }
        public bool Convergence { get; set; }
        public FailReason FailReason { get; set; }
        public double Lambda { get; set; }
        public List<IterationPhaseInfo> Phases { get; set; }=new List<IterationPhaseInfo>();
    }
}