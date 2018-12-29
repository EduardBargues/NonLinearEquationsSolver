using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;

namespace NonLinearEquationsSolver {
    public partial class SolverND {
        public class SolverNdBuilderStructure : SolverNdBuilder {

            public SolverNdBuilderStructure( int degreesOfFreedom, SolverND solverNd, Func<Vector<double>, Vector<double>> structure, Func<Vector<double>, Matrix<double>> stiffness ) {
                SolverNd = solverNd;
                SolverNd.State = new LoadState ( 0, new DenseVector ( degreesOfFreedom ) );
                SolverNd.Info = new StructureInfo {
                    InitialLoad = new DenseVector ( degreesOfFreedom ),
                    Reaction = structure,
                    ReferenceLoad = new DenseVector ( degreesOfFreedom ),
                    Stiffness = stiffness,
                };
                SolverNd.Info.Reaction = structure;
            }
        }
    }
}
