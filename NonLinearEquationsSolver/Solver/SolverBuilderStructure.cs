using MathNet.Numerics.LinearAlgebra;
using System;

namespace NonLinearEquationsSolver.Solver {
    public partial class Solver {
        public class SolverBuilderStructure : SolverBuilder {

            public SolverBuilderStructure( Solver solver, Func<Vector<double>, Vector<double>> structure ) {
                Solver = solver;
                Solver.Info.Reaction = structure;
            }

            public SolverBuilderStructure WithStiffnessMatrix( Func<Vector<double>, Matrix<double>> stiffnessMatrixFunc ) {
                Solver.Info.Stiffness = stiffnessMatrixFunc;
                return this;
            }
        }
    }
}
