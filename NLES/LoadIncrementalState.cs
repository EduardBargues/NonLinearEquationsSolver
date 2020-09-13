
namespace NLES
{
    internal class LoadIncrementalState
    {
        internal double IncrementLambda { get; set; }

        internal Vector IncrementDisplacement { get; set; }

        internal LoadIncrementalState(double incrementLambda, Vector incrementDisplacement)
        {
            IncrementLambda = incrementLambda;
            IncrementDisplacement = incrementDisplacement;
        }

        internal void Add(LoadIncrementalState inc)
        {
            IncrementDisplacement += inc.IncrementDisplacement;
            IncrementLambda += inc.IncrementLambda;
        }
    }
}
