using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex;

namespace NonLinearEquationsSolver
{
    public class IterationPhaseOutput
    {
        public double IncrementLambda { get; set; }
        public Vector<double> IncrementDisplacement { get; set; }

        public IterationPhaseOutput(double incrementLambda, 
                                    Vector<double> incrementDisplacement)
        {
            IncrementLambda = incrementLambda;
            IncrementDisplacement = incrementDisplacement;
        }
    }
}
