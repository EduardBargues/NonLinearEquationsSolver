using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonLinearEquationsSolver
{
    public class Tolerances
    {
        public double Displacement { get; set; }
        public double Equilibrium { get; set; }
        public double Work { get; set; }
        public double IncrementalForce { get; set; }

        public Tolerances(double displacement, double equilibrium, double work, double incrementalForce)
        {
            Displacement = displacement;
            Equilibrium = equilibrium;
            Work = work;
            IncrementalForce = incrementalForce;
        }
    }
}
