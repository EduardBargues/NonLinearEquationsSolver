using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex;

namespace NonLinearEquationsSolver
{
    public class IterationPhaseInfo
    {
        public double IncrementLambda { get; set; }
        public Vector<double> IncrementDisplacement { get; set; }
        public bool Convergence { get; set; }
        public FailReason FailReason { get; set; }
        public IterationPhaseType Type { get; set; }
        public List<IterationInfo> Iterations { get; set; }
        public double BergamParameter { get; set; }

        public IterationPhaseInfo()
        {
            
        }
        public IterationPhaseInfo(double incrementLambda, 
                                    Vector<double> incrementDisplacement,
                                    IterationPhaseType type,
                                    bool convergence)
        {
            IncrementLambda = incrementLambda;
            IncrementDisplacement = incrementDisplacement;
            Type = type;
            Convergence = convergence;
        }
    }
}
