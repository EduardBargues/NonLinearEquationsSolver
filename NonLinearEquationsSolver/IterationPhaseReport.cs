using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex;

namespace NonLinearEquationsSolver
{
    public class IterationPhaseReport
    {
        public double IncrementLambda { get; set; }
        public Vector<double> IncrementDisplacement { get; set; }
        public bool Convergence { get; set; }
        public NonConvergenceReason NonConvergenceReason { get; set; }

        public List<IterationReport> Iterations { get; set; }

        public IterationPhaseReport()
        {
            
        }
        public IterationPhaseReport(double incrementLambda, 
                                    Vector<double> incrementDisplacement)
        {
            IncrementLambda = incrementLambda;
            IncrementDisplacement = incrementDisplacement;
        }
    }
}
