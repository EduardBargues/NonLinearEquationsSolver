
namespace NLES.Contracts
{
    public struct LoadState
    {
        public double Lambda { get; set; }
        public Vector Displacement { get; set; }

        internal LoadState(double lambda, Vector displacement)
        {
            Lambda = lambda;
            Displacement = displacement;
        }

        internal LoadState Add(LoadIncrementalState increment) =>
            new LoadState(Lambda + increment.IncrementLambda, Displacement + increment.IncrementDisplacement);

        internal LoadIncrementalState Substract(LoadState other) =>
            new LoadIncrementalState(Lambda - other.Lambda, Displacement - other.Displacement);
    }
}
