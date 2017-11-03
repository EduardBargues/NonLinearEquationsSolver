namespace NonLinearEquationsSolver
{
    public class IterationReport
    {

        

        public bool Convergence { get; set; }
        public NonConvergenceReason NonConvergenceReason {get; set; }
        public IterationReport(bool convergence, NonConvergenceReason reason)
        {
            Convergence = convergence;
            NonConvergenceReason = reason;
        }
    }
}