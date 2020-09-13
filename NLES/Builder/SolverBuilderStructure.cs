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
                , Func<Vector, Vector> reaction
                , Func<Vector, ILinearSolver> stiffness)
            {
                CheckDregreesOfFreedom(degreesOfFreedom);
                CheckReaction(reaction);
                CheckStiffness(stiffness);

                Solver = solver;
                Solver.State = new LoadState(0, new Vector(degreesOfFreedom));
                Solver.Info = new StructureInfo
                {
                    InitialLoad = new Vector(degreesOfFreedom),
                    Reaction = reaction,
                    ReferenceLoad = new Vector(degreesOfFreedom),
                    Stiffness = stiffness,
                };
                Solver.Info.Reaction = reaction;
            }

            private static void CheckStiffness(Func<Vector, ILinearSolver> stiffness)
            {
                if (stiffness == null)
                {
                    throw new InvalidOperationException(Strings.StiffnessMustBeDefined);
                }
            }

            private static void CheckReaction(Func<Vector, Vector> reaction)
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
