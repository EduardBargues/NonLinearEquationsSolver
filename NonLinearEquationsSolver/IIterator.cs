using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonLinearEquationsSolver
{
    public interface IIterator
    {
        IEnumerable<IterationInfo> Iterate(int maxIterations);
    }
}
