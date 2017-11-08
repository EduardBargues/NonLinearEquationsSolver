using System;
using MathNet.Numerics.LinearAlgebra;

namespace NonLinearEquationsSolver
{
    internal class ConvergenceChecker : IConvergenceChecker
    {
        private readonly IFunction function;
        private readonly Vector<double> force;
        private readonly ErrorTolerancesInfo tolerances;
        public ConvergenceChecker(
            IFunction function, 
            Vector<double> force,
            ErrorTolerancesInfo tolerances)
        {
            this.function = function;
            this.force = force;
            this.tolerances = tolerances;
        }

        public bool CheckConvergence(
            Vector<double> displacement,
            Vector<double> incrementDisplacement,
            double lambda
            )
        {
            Vector<double> reaction = function.GetImage(displacement);
            ErrorTolerancesInfo errors = GetErrors(
                lambda: lambda,
                reaction: reaction,
                displacement: displacement,
                incrementDisplacement: incrementDisplacement);

            return CheckConvergence(errors);
        }
        private bool CheckConvergence(ErrorTolerancesInfo errors)
        {
            return errors.Displacement <= tolerances.Displacement &&
                   errors.Equilibrium <= tolerances.Equilibrium &&
                   errors.Work <= tolerances.Work;
        }
        private ErrorTolerancesInfo GetErrors(
            double lambda,
            Vector<double> reaction,
            Vector<double> displacement,
            Vector<double> incrementDisplacement)
        {
            Vector<double> equilibrium = lambda * force - reaction;
            return new ErrorTolerancesInfo(
                displacement: incrementDisplacement.Norm(2) / displacement.Norm(2),
                equilibrium: equilibrium.Norm(2) / force.Norm(2),
                work: Math.Abs(displacement.DotProduct(equilibrium) / displacement.DotProduct(force)),
                incrementalForce: 0);
        }
    }
}