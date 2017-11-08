using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace NonLinearEquationsSolver
{
    public class SolverReport
    {
        public bool Convergence { get; set; }
        public FailReason Reason { get; set; }
        public List<LoadIncrementInfo> LoadIncrements { get; set; } = new List<LoadIncrementInfo>();
        public Vector<double> Solution { get; set; }
    }
}
