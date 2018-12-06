using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;

namespace NonLinearEquationsSolver {
    public partial class Solver {
        public class SolverBuilderStructure : SolverBuilder {

            public SolverBuilderStructure( int degreesOfFreedom, Solver solver, Func<Vector<double>, Vector<double>> structure, Func<Vector<double>, Matrix<double>> stiffness ) {
                Solver = solver;
                Solver.State = new LoadState ( 0, new DenseVector ( degreesOfFreedom ) );
                Solver.Info = new StructureInfo {
                    InitialLoad = new DenseVector ( degreesOfFreedom ),
                    Reaction = structure,
                    ReferenceLoad = new DenseVector ( degreesOfFreedom ),
                    Stiffness = stiffness,
                };
                Solver.Info.Reaction = structure;
            }
        }
    }
}
