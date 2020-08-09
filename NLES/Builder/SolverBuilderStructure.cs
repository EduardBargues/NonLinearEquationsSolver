using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using NLES.Contracts;
using System;

namespace NLES
{
    public partial class NonLinearSolver
    {
        public class SolverBuilderStructure : SolverBuilder
        {
            public SolverBuilderStructure(
                int degreesOfFreedom
                , NonLinearSolver solver
                , Func<Vector<double>, Vector<double>> reaction
                , Func<Vector<double>, ILinearSolver> stiffness)
            {
                CheckDregreesOfFreedom(degreesOfFreedom);
                CheckReaction(reaction);
                CheckStiffness(stiffness);

                Solver = solver;
                Solver.State = new LoadState(0, new DenseVector(degreesOfFreedom));
                Solver.Info = new StructureInfo
                {
                    InitialLoad = new DenseVector(degreesOfFreedom),
                    Reaction = reaction,
                    ReferenceLoad = new DenseVector(degreesOfFreedom),
                    Stiffness = stiffness,
                };
                Solver.Info.Reaction = reaction;
            }

            private static void CheckStiffness(Func<Vector<double>, ILinearSolver> stiffness)
            {
                if (stiffness == null)
                {
                    throw new InvalidOperationException(Strings.StiffnessMustBeDefined);
                }
            }

            private static void CheckReaction(Func<Vector<double>, Vector<double>> reaction)
            {
                if (reaction == null)
                {
                    throw new InvalidOperationException(Strings.ReactionMustBeDefined);
                }
            }

            private static void CheckDregreesOfFreedom(int degreesOfFreedom)
            {
                if (degreesOfFreedom <= 0)
                {
                    throw new InvalidOperationException(Strings.DegreesOfFreedomLargerThanZero);
                }
            }
        }
    }
}
